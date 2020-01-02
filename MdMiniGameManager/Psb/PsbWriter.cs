using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using ICSharpCode.SharpZipLib.Checksum;
using MArchiveBatchTool.Psb.Writing;

namespace MArchiveBatchTool.Psb
{
    public class PsbWriter
    {
        static readonly int STREAM_ALIGNMENT = 64;

        class StreamCacheEntry
        {
            public string OrigValue
            {
                get; set;
            }
            public List<JStream> Streams { get; set; } = new List<JStream>();
            public long Length
            {
                get; set;
            }
        }

        KeyNamesGenerator keyNamesGen;
        List<string> stringsCache = new List<string>();
        Dictionary<string, StreamCacheEntry> streamsCache = new Dictionary<string, StreamCacheEntry>();
        Dictionary<string, StreamCacheEntry> bStreamsCache = new Dictionary<string, StreamCacheEntry>();
        List<StreamCacheEntry> sortedStreams;
        List<StreamCacheEntry> sortedBStreams;
        JToken root;
        IPsbStreamSource streamSource;
        HashAlgorithm hasher;

        // Header values
        uint keysOffsetsOffset;
        uint keysBlobOffset;
        uint stringsOffsetsOffset;
        uint stringsBlobOffset;
        uint streamsOffsetsOffset;
        uint streamsSizesOffset;
        uint streamsBlobOffset;
        uint rootOffset;
        uint bStreamsOffsetsOffset;
        uint bStreamsSizesOffset;
        uint bStreamsBlobOffset;

        public ushort Version
        {
            get; set;
        }
        public PsbFlags Flags
        {
            get; set;
        }
        public bool Optimize { get; set; } = true;

        public PsbWriter(JToken root, IPsbStreamSource streamSource)
        {
            this.root = root;
            this.streamSource = streamSource;
            hasher = SHA1.Create();
        }

        public void Write(Stream stream, IPsbFilter filter = null)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanSeek)
                throw new ArgumentException("Stream not seekable", nameof(stream));
            if (Version < 1 || Version > 4)
                throw new NotSupportedException("PSB version requested not supported.");
            Prepare();
            BinaryWriter bw = new BinaryWriter(stream);
            WritePrelimHeader(bw);
            WriteKeyNames(bw);
            WriteRoot(bw);
            WriteStrings(bw);
            if (Version >= 4)
            {
                WriteStreamsMeta(bw, true);
                WriteStreams(bw, true);
            }
            WriteStreamsMeta(bw, false);
            WriteStreams(bw, false);
            UpdateHeader(bw);
            if (filter != null)
                ApplyFilter(stream, filter);
        }

        void WritePrelimHeader(BinaryWriter bw)
        {
            bw.Write("PSB\0".ToCharArray());
            bw.Write(Version);
            bw.Write((ushort)Flags);
            bw.Write(0u);
            bw.Write(0u);
            bw.Write(0u);
            bw.Write(0u);
            bw.Write(0u);
            bw.Write(0u);
            bw.Write(0u);
            bw.Write(0u);
            if (Version >= 3)
            {
                bw.Write(0u);
                if (Version >= 4)
                {
                    bw.Write(0u);
                    bw.Write(0u);
                    bw.Write(0u);
                }
            }
        }

        void WriteKeyNames(BinaryWriter bw)
        {
            keysOffsetsOffset = (uint)bw.BaseStream.Position;
            if (Version == 1)
            {
                List<uint> offsets = new List<uint>();
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryWriter innerBw = new BinaryWriter(ms);
                    for (int i = 0; i < keyNamesGen.Count; ++i)
                    {
                        offsets.Add((uint)ms.Length);
                        WriteStringZ(innerBw, keyNamesGen[i]);
                    }
                    ms.Flush();

                    Write(bw, offsets.ToArray());
                    keysBlobOffset = (uint)bw.BaseStream.Position;
                    bw.Write(ms.ToArray());
                }
            }
            else
            {
                keysBlobOffset = (uint)bw.BaseStream.Position;
                Write(bw, keyNamesGen.ValueOffsets);
                Write(bw, keyNamesGen.Tree);
                Write(bw, keyNamesGen.Tails);
            }
        }

        void WriteRoot(BinaryWriter bw)
        {
            rootOffset = (uint)bw.BaseStream.Position;
            WriteTokenValue(bw, root);
        }

        void WriteStrings(BinaryWriter bw)
        {
            stringsOffsetsOffset = (uint)bw.BaseStream.Position;
            List<uint> offsets = new List<uint>();
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter innerBw = new BinaryWriter(ms);
                foreach (var s in stringsCache)
                {
                    offsets.Add((uint)ms.Position);
                    WriteStringZ(innerBw, s);
                }
                ms.Flush();
                Write(bw, offsets.ToArray());
                stringsBlobOffset = (uint)bw.BaseStream.Length;
                bw.Write(ms.ToArray());
            }
        }

        void WriteStreamsMeta(BinaryWriter bw, bool isBStream)
        {
            var cache = isBStream ? sortedBStreams : sortedStreams;
            if (isBStream)
                bStreamsOffsetsOffset = (uint)bw.BaseStream.Position;
            else
                streamsOffsetsOffset = (uint)bw.BaseStream.Position;

            List<uint> offsets = new List<uint>();
            List<uint> sizes = new List<uint>();
            uint lastOffset = 0;

            foreach (var entry in cache)
            {
                offsets.Add(lastOffset);
                var length = entry.Length;
                sizes.Add((uint)length);
                // Add memory alignment
                uint targetLength = (uint)((length + STREAM_ALIGNMENT - 1) / STREAM_ALIGNMENT * STREAM_ALIGNMENT);
                lastOffset += targetLength;
            }

            Write(bw, offsets.ToArray());
            if (isBStream)
                bStreamsSizesOffset = (uint)bw.BaseStream.Position;
            else
                streamsSizesOffset = (uint)bw.BaseStream.Position;
            Write(bw, sizes.ToArray());
        }

        void WriteStreams(BinaryWriter bw, bool isBStream)
        {
            var cache = isBStream ? sortedBStreams : sortedStreams;
            if (cache.Count > 0)
            {
                // Align stream
                var targetStart = (bw.BaseStream.Position + STREAM_ALIGNMENT - 1) / STREAM_ALIGNMENT * STREAM_ALIGNMENT;
                bw.Write(new byte[targetStart - bw.BaseStream.Position]);
            }

            if (isBStream)
                bStreamsBlobOffset = (uint)bw.BaseStream.Position;
            else
                streamsBlobOffset = (uint)bw.BaseStream.Position;

            foreach (var entry in cache)
            {
                var stream = entry.Streams[0];
                uint length;
                if (stream.BinaryData != null)
                {
                    length = (uint)stream.BinaryData.Length;
                    bw.Write(stream.BinaryData);
                }
                else if (streamSource != null)
                {
                    using (Stream fs = streamSource.GetStream(entry.OrigValue))
                    {
                        length = (uint)fs.Length;
                        fs.CopyTo(bw.BaseStream);
                    }
                }
                else
                {
                    throw new Exception("No binary data for stream");
                }

                // Add memory alignment
                uint targetLength = (uint)((length + STREAM_ALIGNMENT - 1) / STREAM_ALIGNMENT * STREAM_ALIGNMENT);
                bw.Write(new byte[targetLength - length]);
            }
        }

        void UpdateHeader(BinaryWriter bw)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter innerBw = new BinaryWriter(ms);
                innerBw.Write(keysOffsetsOffset);
                innerBw.Write(keysBlobOffset);
                innerBw.Write(stringsOffsetsOffset);
                innerBw.Write(stringsBlobOffset);
                innerBw.Write(streamsOffsetsOffset);
                innerBw.Write(streamsSizesOffset);
                innerBw.Write(streamsBlobOffset);
                innerBw.Write(rootOffset);
                innerBw.Flush();

                if (Version >= 3)
                {
                    Adler32 adler = new Adler32();
                    adler.Update(ms.ToArray());

                    if (Version >= 4)
                    {
                        using (MemoryStream secondMs = new MemoryStream())
                        {
                            BinaryWriter secondBw = new BinaryWriter(secondMs);
                            secondBw.Write(bStreamsOffsetsOffset);
                            secondBw.Write(bStreamsSizesOffset);
                            secondBw.Write(bStreamsBlobOffset);
                            secondBw.Flush();

                            adler.Update(secondMs.ToArray());

                            innerBw.Write((uint)adler.Value);
                            innerBw.Write(secondMs.ToArray());
                        }
                    }
                    else
                    {
                        innerBw.Write((uint)adler.Value);
                    }

                    ms.Flush();
                }

                bw.BaseStream.Seek(8, SeekOrigin.Begin);
                bw.Write(ms.ToArray());
            }
        }

        void WriteStringZ(BinaryWriter bw, string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s + '\0');
            bw.Write(bytes);
        }

        void ApplyFilter(Stream stream, IPsbFilter filter)
        {
            BinaryReader br = new BinaryReader(stream);
            BinaryWriter bw = new BinaryWriter(stream);
            stream.Seek(8, SeekOrigin.Begin);
            if (Version >= 3 && (Flags & PsbFlags.HeaderFiltered) != 0)
            {
                // Filter header
                int headerLength = (8 + 1) * 4;
                if (Version >= 4)
                    headerLength += 3 * 4;
                byte[] headerBytes = br.ReadBytes(headerLength);
                filter.Filter(headerBytes);
                stream.Seek(8, SeekOrigin.Begin);
                bw.Write(headerBytes);
            }

            if (Version <= 2 || (Flags & PsbFlags.BodyFiltered) != 0)
            {
                uint filterStart = keysOffsetsOffset;
                uint filterEnd = Version >= 4 ? bStreamsOffsetsOffset : streamsOffsetsOffset;
                stream.Seek(filterStart, SeekOrigin.Begin);
                byte[] data = br.ReadBytes((int)(filterEnd - filterStart));
                filter.Filter(data);
                stream.Seek(filterStart, SeekOrigin.Begin);
                bw.Write(data);
            }
        }

        void Prepare()
        {
            // Reset build objects
            keyNamesGen = new KeyNamesGenerator(new StandardKeyNamesEncoder(), Version == 1);
            stringsCache.Clear();
            streamsCache.Clear();
            bStreamsCache.Clear();

            PrepareNode(root);
            keyNamesGen.Generate();
            // Sort strings
            stringsCache.Sort((x, y) => string.CompareOrdinal(x, y));
            SortStreamNodes(false);
            if (Version >= 4)
                SortStreamNodes(true);
        }

        void PrepareNode(JToken node)
        {
            if (node.Type == JTokenType.String)
            {
                var stringValue = (string)node;
                if ((stringValue.StartsWith("_stream:") || stringValue.StartsWith("_bstream:"))
                    && !(node is JStream))
                {
                    // Replace stream string representation with a JStream
                    var newNode = JStream.CreateFromStringRepresentation(stringValue);
                    ((JValue)node).Value = string.Empty; // Invalidate old node so replacement goes through
                    node.Replace(newNode);
                    node = newNode;
                }

                if (node is JStream stream)
                {
                    PrepareStream(stream);
                }
                else
                {
                    PrepareString(stringValue);
                }
            }
            else if (node is JArray array)
            {
                foreach (var token in array)
                {
                    PrepareNode(token);
                }
            }
            else if (node is JObject obj)
            {
                foreach (var prop in obj)
                {
                    keyNamesGen.AddString(prop.Key);
                    PrepareNode(prop.Value);
                }
            }
        }

        void PrepareStream(JStream stream)
        {
            if (stream.IsBStream && Version < 4)
                stream.IsBStream = false;
            var cache = stream.IsBStream ? bStreamsCache : streamsCache;
            bool doOptimize = Optimize;
            byte[] hash = null;
            long length;

            if (stream.BinaryData != null)
            {
                if (doOptimize)
                    hash = hasher.ComputeHash(stream.BinaryData);
                length = stream.BinaryData.Length;
            }
            else if (streamSource != null)
            {
                using (Stream dataStream = streamSource.GetStream((string)stream))
                {
                    if (doOptimize)
                        hash = hasher.ComputeHash(dataStream);
                    length = dataStream.Length;
                }
            }
            else
            {
                throw new Exception("No binary data for stream");
            }

            if (doOptimize)
            {
                string hashString = BitConverter.ToString(hash).Replace("-", string.Empty);
                if (cache.TryGetValue(hashString, out var entry))
                {
                    entry.Streams.Add(stream);
                }
                else
                {
                    cache.Add(hashString, new StreamCacheEntry
                    {
                        OrigValue = (string)stream,
                        Streams = new List<JStream> { stream },
                        Length = length
                    });
                }
            }
            else
            {
                cache.Add(cache.Count.ToString(), new StreamCacheEntry
                {
                    OrigValue = (string)stream,
                    Streams = new List<JStream> { stream },
                    Length = length
                });
            }
        }

        void PrepareString(string s)
        {
            if (!stringsCache.Contains(s))
                stringsCache.Add(s);
        }

        void SortStreamNodes(bool isBStream)
        {
            var cache = isBStream ? bStreamsCache : streamsCache;
            var sorted = cache.Values.ToList();
            sorted.ForEach(s => s.Streams.Sort((x, y) => x.Index.CompareTo(y.Index)));
            sorted = sorted.OrderBy(x => x.Length).ThenBy(x => x.Streams[0].Index).ToList();
            for (int i = 0; i < cache.Count; ++i)
            {
                foreach (var stream in sorted[i].Streams)
                {
                    stream.Index = (uint)i;
                }
            }
            if (isBStream)
                sortedBStreams = sorted;
            else
                sortedStreams = sorted;
        }

        #region Tree writing functions
        void WriteTokenValue(BinaryWriter bw, JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Array:
                    Write(bw, (JArray)token);
                    break;
                case JTokenType.Boolean:
                    Write(bw, (bool)token);
                    break;
                case JTokenType.Comment:
                    break;
                case JTokenType.Float:
                    if (((JValue)token).Value is decimal d)
                    {
                        var decString = ((decimal)token).ToString();
                        var decFloatString = ((decimal)(float)token).ToString();
                        if (d - decimal.Truncate(d) == 0 || decFloatString == decString)
                            Write(bw, (float)token);
                        else
                            Write(bw, (double)token);
                    }
                    else
                    {
                        if ((float)token == (double)token)
                            Write(bw, (float)token);
                        else
                            Write(bw, (double)token);
                    }
                    break;
                case JTokenType.Integer:
                    var intValue = (long)token;
                    if (intValue > int.MaxValue || intValue < int.MinValue)
                        Write(bw, (long)token);
                    else
                        Write(bw, (int)token);
                    break;
                case JTokenType.Null:
                    WriteNull(bw);
                    break;
                case JTokenType.Object:
                    Write(bw, (JObject)token);
                    break;
                case JTokenType.String:
                    if (token is JStream stream)
                        Write(bw, stream);
                    else
                        Write(bw, (string)token);
                    break;
                default:
                    throw new ArgumentException("Unknown token type", nameof(token));
            }
        }

        void WriteNull(BinaryWriter bw)
        {
            bw.Write((byte)1);
        }

        void Write(BinaryWriter bw, bool value)
        {
            bw.Write((byte)(value ? 2 : 3));
        }

        void Write(BinaryWriter bw, int value)
        {
            if (value == 0)
            {
                bw.Write((byte)4);
                return;
            }

            // Find out number of bytes we need
            int bytesNeeded = CalcBytesNeeded(value);

            bw.Write((byte)(4 + bytesNeeded));
            byte[] buf = BitConverter.GetBytes(value);
            for (int i = 0; i < bytesNeeded; ++i)
            {
                bw.Write(buf[i]);
            }
        }

        void Write(BinaryWriter bw, long value)
        {
            // Find out number of bytes we need
            int bytesNeeded = CalcBytesNeeded(value);
            if (bytesNeeded == -1)
            {
                Write(bw, (int)value);
                return;
            }

            bw.Write((byte)(9 + bytesNeeded - 5));
            byte[] buf = BitConverter.GetBytes(value);
            for (int i = 0; i < bytesNeeded; ++i)
            {
                bw.Write(buf[i]);
            }
        }

        int CalcBytesNeeded(uint value)
        {
            int bytesNeeded = 0;
            while (value != 0)
            {
                ++bytesNeeded;
                value >>= 8;
            }

            // Need min 1 byte even if all zeroes because there's no separate encoding for UInt zero
            if (bytesNeeded == 0)
                bytesNeeded = 1;
            return bytesNeeded;
        }

        int CalcBytesNeeded(int value)
        {
            if (value < -8388608 || value > 8388607) // Range of 24-bit int
                return 4;
            else if (value < short.MinValue || value > short.MaxValue)
                return 3;
            else if (value < sbyte.MinValue || value > sbyte.MaxValue)
                return 2;
            else
                return 1;
        }

        int CalcBytesNeeded(long value)
        {
            if (value < int.MinValue || value > int.MaxValue)
                return CalcBytesNeeded((int)(value >> 32)) + 4;
            else
                return -1;
        }

        void Write(BinaryWriter bw, uint value, int baseId)
        {
            // Find out number of bytes we need
            int bytesNeeded = CalcBytesNeeded(value);

            bw.Write((byte)(baseId + bytesNeeded - 1));
            for (int i = 0; i < bytesNeeded; ++i)
            {
                bw.Write((byte)value);
                value >>= 8;
            }
        }

        void Write(BinaryWriter bw, uint[] value)
        {
            int maxBytesNeeded = 1;
            foreach (var v in value)
            {
                int currBytesNeeded = CalcBytesNeeded(v);
                if (currBytesNeeded > maxBytesNeeded)
                {
                    maxBytesNeeded = currBytesNeeded;
                    if (maxBytesNeeded == 4)
                        break;
                }
            }

            // Write count
            Write(bw, (uint)value.Length, 13);
            // Write value type ID
            bw.Write((byte)(13 + maxBytesNeeded - 1));
            // Write values
            foreach (var v in value)
            {
                uint vv = v;
                for (int i = 0; i < maxBytesNeeded; ++i)
                {
                    bw.Write((byte)vv);
                    vv >>= 8;
                }
            }
        }

        void WriteKey(BinaryWriter bw, string key)
        {
            Write(bw, keyNamesGen[key], 17);
        }

        void Write(BinaryWriter bw, string value)
        {
            int index = stringsCache.IndexOf(value);
            if (index == -1)
                throw new Exception("Could not find string");
            Write(bw, (uint)index, 21);
        }

        void Write(BinaryWriter bw, JStream stream)
        {
            Write(bw, stream.Index, stream.IsBStream ? 34 : 25);
        }

        void Write(BinaryWriter bw, float value)
        {
            if (value == 0)
                bw.Write((byte)29);
            else
            {
                bw.Write((byte)30);
                bw.Write(value);
            }
        }

        void Write(BinaryWriter bw, double value)
        {
            if (value == 0)
                bw.Write((byte)29);
            else
            {
                bw.Write((byte)31);
                bw.Write(value);
            }
        }

        void Write(BinaryWriter bw, JArray value)
        {
            bw.Write((byte)32);
            List<uint> offsets = new List<uint>();
            List<Tuple<uint, int, JToken>> tokenCache = new List<Tuple<uint, int, JToken>>();
            JTokenEqualityComparer comparer = new JTokenEqualityComparer();
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter innerBw = new BinaryWriter(ms);
                foreach (JToken item in value)
                {
                    if (Optimize)
                    {
                        bool found = false;
                        int hashCode = comparer.GetHashCode(item);
                        foreach (var potentialEntry in tokenCache.Where(x => x.Item2 == hashCode))
                        {
                            if (comparer.Equals(item, potentialEntry.Item3))
                            {
                                found = true;
                                offsets.Add(potentialEntry.Item1);
                                break;
                            }
                        }
                        if (found)
                            continue;
                        else
                            tokenCache.Add(new Tuple<uint, int, JToken>((uint)ms.Length, hashCode, item));
                    }
                    offsets.Add((uint)ms.Length);
                    WriteTokenValue(innerBw, item);
                }
                innerBw.Flush();

                Write(bw, offsets.ToArray());
                bw.Write(ms.ToArray());
            }
        }

        void Write(BinaryWriter bw, JObject value)
        {
            bw.Write((byte)33);
            List<uint> offsets = new List<uint>();
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter innerBw = new BinaryWriter(ms);
                if (Version == 1)
                {
                    // Note: Optimization is not applicable here because key is stored with value,
                    // and all keys are unique
                    foreach (var entry in value.Values<JProperty>().OrderBy(x => x.Name, StringComparer.Ordinal))
                    {
                        offsets.Add((uint)ms.Length);
                        WriteKey(innerBw, entry.Name);
                        WriteTokenValue(innerBw, entry.Value);
                    }
                }
                else
                {
                    List<uint> keyIndexes = new List<uint>();
                    List<Tuple<uint, int, JToken>> tokenCache = new List<Tuple<uint, int, JToken>>();
                    JTokenEqualityComparer comparer = new JTokenEqualityComparer();

                    foreach (var entry in value.Values<JProperty>().OrderBy(x => x.Name, StringComparer.Ordinal))
                    {
                        if (Optimize)
                        {
                            bool found = false;
                            int hashCode = comparer.GetHashCode(entry.Value);
                            foreach (var potentialEntry in tokenCache.Where(x => x.Item2 == hashCode))
                            {
                                if (comparer.Equals(entry.Value, potentialEntry.Item3))
                                {
                                    found = true;
                                    offsets.Add(potentialEntry.Item1);
                                    keyIndexes.Add(keyNamesGen[entry.Name]);
                                    break;
                                }
                            }
                            if (found)
                                continue;
                            else
                                tokenCache.Add(new Tuple<uint, int, JToken>((uint)ms.Length, hashCode, entry.Value));
                        }
                        offsets.Add((uint)ms.Length);
                        keyIndexes.Add(keyNamesGen[entry.Name]);
                        WriteTokenValue(innerBw, entry.Value);
                    }
                    Write(bw, keyIndexes.ToArray());
                }
                ms.Flush();
                Write(bw, offsets.ToArray());
                bw.Write(ms.ToArray());
            }
        }
        #endregion
    }
}
