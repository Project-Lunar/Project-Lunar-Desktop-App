using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using Newtonsoft.Json.Linq;
using ICSharpCode.SharpZipLib.Checksum;
using MArchiveBatchTool.Psb.Writing;

namespace MArchiveBatchTool.Psb
{
    public class PsbReader : IDisposable
    {
        internal static readonly PsbTokenType[] IdToTypeMapping =
        {
            PsbTokenType.Invalid,
            PsbTokenType.Null,
            // true and false
            PsbTokenType.Bool, PsbTokenType.Bool,
            // 32-bit signed integer: value 0, 8 to 32 bits
            PsbTokenType.Int, PsbTokenType.Int, PsbTokenType.Int, PsbTokenType.Int, PsbTokenType.Int,
            // 64-bit signed integer: 40 to 64 buts
            PsbTokenType.Long, PsbTokenType.Long, PsbTokenType.Long, PsbTokenType.Long,
            // 32-bit unsigned integer, for use with uint array decoding and not usually encountered
            // 8 to 32 bits
            PsbTokenType.UInt, PsbTokenType.UInt, PsbTokenType.UInt, PsbTokenType.UInt,
            // Keys index -> 32-bit unsigned integer, 8 to 32 bits
            PsbTokenType.Key, PsbTokenType.Key, PsbTokenType.Key, PsbTokenType.Key,
            // String index: 8 to 32 bits
            PsbTokenType.String, PsbTokenType.String, PsbTokenType.String, PsbTokenType.String,
            // Stream index: 8 to 32 bits
            PsbTokenType.Stream, PsbTokenType.Stream, PsbTokenType.Stream, PsbTokenType.Stream,
            // Single precision float: 0 or value
            PsbTokenType.Float, PsbTokenType.Float,
            PsbTokenType.Double,
            PsbTokenType.TokenArray,
            PsbTokenType.Object,
            // BStream index: 8 to 32 bits
            PsbTokenType.BStream, PsbTokenType.BStream, PsbTokenType.BStream, PsbTokenType.BStream,
        };

        Stream stream;
        BinaryReader br;
        IPsbFilter filter;
        KeyNamesReader keyNames;
        IndentedTextWriter debugWriter;
        bool lazyStreamLoading;
        bool disposed;

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

        uint[] stringsOffsets;
        uint[] streamsOffsets;
        uint[] streamsSizes;
        uint[] bStreamsOffsets;
        uint[] bStreamsSizes;

        JToken root;

        public ushort Version { get; private set; }
        public PsbFlags Flags { get; private set; }
        public JToken Root
        {
            get
            {
                CheckDisposed();
                if (root == null)
                {
                    if (debugWriter != null)
                    {
                        debugWriter.WriteLine();
                        debugWriter.WriteLine("# Root");
                        debugWriter.WriteLine();
                    }
                    stream.Seek(rootOffset, SeekOrigin.Begin);
                    root = ReadTokenValue();
                }
                return root;
            }
        }
        public Dictionary<uint, JStream> StreamCache { get; } = new Dictionary<uint, JStream>();
        public Dictionary<uint, JStream> BStreamCache { get; } = new Dictionary<uint, JStream>();
        internal KeyNamesReader KeyNames => keyNames;

        public PsbReader(Stream stream, IPsbFilter filter = null, TextWriter debugWriter = null,
            bool lazyStreamLoading = true)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanSeek) throw new ArgumentException("Stream cannot be seeked.", nameof(stream));
            this.stream = stream;
            br = new BinaryReader(stream);
            this.filter = filter;
            this.lazyStreamLoading = lazyStreamLoading;

            if (new string(br.ReadChars(4)) != "PSB\0") throw new InvalidDataException("File is not a PSB");
            Version = br.ReadUInt16();
            Flags = (PsbFlags)br.ReadUInt16();

            byte[] headerBytes = VerifyHeader();
            ReadHeader(headerBytes);
            SetupUnfiltering();
            LoadKeyNames();
            LoadStringsOffsets();
            LoadStreamsInfo();
            if (Version >= 4) LoadBStreamsInfo();
            // Move this down here so I don't get output while arrays are being read
            if (debugWriter != null)
                this.debugWriter = new IndentedTextWriter(debugWriter);
            DebugWriteHeader();
        }

        byte[] VerifyHeader()
        {
            int headerLength = 8 * 4;
            if (Version >= 3)
            {
                headerLength += 4;
                if (Version >= 4) headerLength += 3 * 4;
            }
            byte[] headerBytes = br.ReadBytes(headerLength);

            if (Version >= 3)
            {
                if (filter != null && (Flags & PsbFlags.HeaderFiltered) != 0) filter.Filter(headerBytes);
                uint headerChecksum = BitConverter.ToUInt32(headerBytes, 0x20);

                // This is probably actually for checking that the header was decrypted successfully
                Adler32 adler = new Adler32();
                adler.Update(new ArraySegment<byte>(headerBytes, 0, 0x20));
                if (Version >= 4) adler.Update(new ArraySegment<byte>(headerBytes, 0x24, 0x0c));
                if (adler.Value != headerChecksum) throw new InvalidDataException("Header checksum mismatch");
            }

            return headerBytes;
        }

        void ReadHeader(byte[] headerBytes)
        {
            using (MemoryStream ms = new MemoryStream(headerBytes))
            {
                BinaryReader hbr = new BinaryReader(ms);
                keysOffsetsOffset = hbr.ReadUInt32();
                keysBlobOffset = hbr.ReadUInt32();
                stringsOffsetsOffset = hbr.ReadUInt32();
                stringsBlobOffset = hbr.ReadUInt32();
                streamsOffsetsOffset = hbr.ReadUInt32();
                streamsSizesOffset = hbr.ReadUInt32();
                streamsBlobOffset = hbr.ReadUInt32();
                rootOffset = hbr.ReadUInt32();
                if (Version >= 3)
                {
                    hbr.ReadUInt32(); // Skip checksum
                    if (Version >= 4)
                    {
                        bStreamsOffsetsOffset = hbr.ReadUInt32();
                        bStreamsSizesOffset = hbr.ReadUInt32();
                        bStreamsBlobOffset = hbr.ReadUInt32();
                    }
                }
            }
        }

        void SetupUnfiltering()
        {
            if (filter != null && (Version < 3 || (Flags & PsbFlags.BodyFiltered) != 0))
            {
                stream = new OverlayReadStream(
                    stream,
                    keysOffsetsOffset,
                    Version >= 4 ? bStreamsOffsetsOffset : streamsOffsetsOffset,
                    filter
                );
                br = new BinaryReader(stream);
            }
        }

        void LoadKeyNames()
        {
            keyNames = new KeyNamesReader(this);
        }

        void LoadStringsOffsets()
        {
            stream.Seek(stringsOffsetsOffset, SeekOrigin.Begin);
            stringsOffsets = ParseUIntArray();
        }

        void LoadStreamsInfo()
        {
            stream.Seek(streamsOffsetsOffset, SeekOrigin.Begin);
            streamsOffsets = ParseUIntArray();
            stream.Seek(streamsSizesOffset, SeekOrigin.Begin);
            streamsSizes = ParseUIntArray();
        }

        void LoadBStreamsInfo()
        {
            stream.Seek(bStreamsOffsetsOffset, SeekOrigin.Begin);
            bStreamsOffsets = ParseUIntArray();
            stream.Seek(bStreamsSizesOffset, SeekOrigin.Begin);
            bStreamsSizes = ParseUIntArray();
        }

        void DebugWriteHeader()
        {
            if (debugWriter == null) return;
            debugWriter.WriteLine("PSB");
            debugWriter.WriteLine($"{nameof(Version)}: {Version}");
            debugWriter.WriteLine($"{nameof(Flags)}: {Flags}");
            debugWriter.WriteLine($"{nameof(keysOffsetsOffset)}: {keysOffsetsOffset}");
            debugWriter.WriteLine($"{nameof(keysBlobOffset)}: {keysBlobOffset}");
            debugWriter.WriteLine($"{nameof(stringsOffsetsOffset)}: {stringsOffsetsOffset}");
            debugWriter.WriteLine($"{nameof(stringsBlobOffset)}: {stringsBlobOffset}");
            debugWriter.WriteLine($"{nameof(streamsOffsetsOffset)}: {streamsOffsetsOffset}");
            debugWriter.WriteLine($"{nameof(streamsSizesOffset)}: {streamsSizesOffset}");
            debugWriter.WriteLine($"{nameof(streamsBlobOffset)}: {streamsBlobOffset}");
            debugWriter.WriteLine($"{nameof(rootOffset)}: {rootOffset}");
            if (Version >= 4)
            {
                debugWriter.WriteLine($"{nameof(bStreamsOffsetsOffset)}: {bStreamsOffsetsOffset}");
                debugWriter.WriteLine($"{nameof(bStreamsSizesOffset)}: {bStreamsSizesOffset}");
                debugWriter.WriteLine($"{nameof(bStreamsBlobOffset)}: {bStreamsBlobOffset}");
            }
            debugWriter.WriteLine();
            debugWriter.WriteLine("# Blob Tables");
            debugWriter.WriteLine();
            debugWriter.WriteLine($"{nameof(stringsOffsets)}:");
            foreach (var o in stringsOffsets)
                debugWriter.WriteLine("{0:x8}", o);
            debugWriter.WriteLine();
            debugWriter.WriteLine($"{nameof(streamsOffsets)}:");
            foreach (var o in streamsOffsets)
                debugWriter.WriteLine("{0:x8}", o);
            debugWriter.WriteLine();
            debugWriter.WriteLine($"{nameof(streamsSizes)}:");
            foreach (var o in streamsSizes)
                debugWriter.WriteLine("{0:x8}", o);
            debugWriter.WriteLine();
            if (Version >= 4)
            {
                debugWriter.WriteLine($"{nameof(bStreamsOffsets)}:");
                foreach (var o in bStreamsOffsets)
                    debugWriter.WriteLine("{0:x8}", o);
                debugWriter.WriteLine();
                debugWriter.WriteLine($"{nameof(bStreamsSizes)}:");
                foreach (var o in bStreamsSizes)
                    debugWriter.WriteLine("{0:x8}", o);
                debugWriter.WriteLine();
            }
        }

        void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        public void DumpDecryptedStream(Stream stream)
        {
            CheckDisposed();
            this.stream.Seek(0, SeekOrigin.Begin);
            this.stream.CopyTo(stream);
        }

        public Dictionary<uint, NameNode> GenerateNameNodes()
        {
            return keyNames.GenerateNodes();
        }

        public void Close()
        {
            disposed = true;
            stream.Close();
            root = null;
            StreamCache.Clear();
            BStreamCache.Clear();
        }

        public void Dispose()
        {
            Close();
        }

        #region Stream decoding functions
        JToken ReadTokenValue()
        {
            byte typeId = br.ReadByte();
            if (typeId < 0 || typeId >= IdToTypeMapping.Length)
                throw new InvalidDataException("Unknown type ID");
            switch (IdToTypeMapping[typeId])
            {
                case PsbTokenType.Null:
                    if (debugWriter != null)
                        debugWriter.WriteLine($"{PsbTokenType.Null} {typeId}: null");
                    return JValue.CreateNull();
                case PsbTokenType.Bool:
                    return ParseBool(typeId);
                case PsbTokenType.Int:
                    return ParseInt(typeId);
                case PsbTokenType.Long:
                    return ParseLong(typeId);
                case PsbTokenType.String:
                    return ParseString(typeId);
                case PsbTokenType.Stream:
                    return ParseStream(typeId);
                case PsbTokenType.Float:
                    return (decimal)ParseFloat(typeId);
                case PsbTokenType.Double:
                    return (decimal)ParseDouble(typeId);
                case PsbTokenType.TokenArray:
                    return ParseTokenArray(typeId);
                case PsbTokenType.Object:
                    return ParseObject(typeId);
                case PsbTokenType.BStream:
                    return ParseBStream(typeId);
                case PsbTokenType.Invalid:
                case PsbTokenType.UInt:
                case PsbTokenType.Key:
                default:
                    throw new InvalidDataException("Invalid token type");
            }
        }

        bool ParseBool(byte typeId)
        {
            bool value = typeId == 2;
            if (debugWriter != null)
                debugWriter.WriteLine($"{PsbTokenType.Bool} {typeId}: {value}");
            return value;
        }

        int ParseInt(byte typeId)
        {
            int value;
            switch (typeId)
            {
                case 4:
                    value = 0;
                    break;
                case 5:
                    value = br.ReadSByte();
                    break;
                case 6:
                    value = br.ReadInt16();
                    break;
                case 7:
                    value = br.ReadUInt16() | (br.ReadSByte() << 16);
                    break;
                case 8:
                    value = br.ReadInt32();
                    break;
                default:
                    throw new ArgumentException("Dispatched to incorrect parser.", nameof(typeId));
            }

            if (debugWriter != null)
                debugWriter.WriteLine($"{PsbTokenType.Int} {typeId}: {value}");
            return value;
        }

        long ParseLong(byte typeId)
        {
            long value;
            switch (typeId)
            {
                case 9:
                    value = br.ReadUInt32() | (br.ReadSByte() << 32);
                    break;
                case 10:
                    value = br.ReadUInt32() | (br.ReadInt16() << 32);
                    break;
                case 11:
                    value = br.ReadUInt32() | (br.ReadUInt16() << 32) | (br.ReadSByte() << 48);
                    break;
                case 12:
                    value = br.ReadInt64();
                    break;
                default:
                    throw new ArgumentException("Dispatched to incorrect parser.", nameof(typeId));
            }

            if (debugWriter != null)
                debugWriter.WriteLine($"{PsbTokenType.Long} {typeId}: {value}");
            return value;
        }

        uint ParseUInt(byte typeId, bool debugWriteType = false)
        {
            uint value;
            switch (typeId)
            {
                case 13:
                    value = br.ReadByte();
                    break;
                case 14:
                    value = br.ReadUInt16();
                    break;
                case 15:
                    value = (uint)(br.ReadUInt16() | (br.ReadByte() << 16));
                    break;
                case 16:
                    value = br.ReadUInt32();
                    break;
                default:
                    throw new ArgumentException("Dispatched to incorrect parser.", nameof(typeId));
            }

            if (debugWriter != null)
            {
                if (debugWriteType)
                    debugWriter.WriteLine($"{PsbTokenType.UInt} {typeId}: {value}");
                else
                    debugWriter.WriteLine(value);
            }
            return value;
        }

        uint[] ParseUIntArray()
        {
            byte typeId = br.ReadByte();
            if (debugWriter != null)
            {
                debugWriter.WriteLine($"{PsbTokenType.UInt}[] <");
                ++debugWriter.Indent;
                debugWriter.Write("count - ");
            }
            uint count = ParseUInt(typeId, true);
            uint[] arr = new uint[count];
            byte valTypeId = br.ReadByte();
            if (debugWriter != null)
            {
                debugWriter.WriteLine($"valTypeId - {valTypeId}");
                --debugWriter.Indent;
                debugWriter.WriteLine(">");
                ++debugWriter.Indent;
            }
            for (int i = 0; i < arr.Length; ++i)
            {
                arr[i] = ParseUInt(valTypeId);
            }
            if (debugWriter != null)
            {
                --debugWriter.Indent;
            }

            return arr;
        }

        string ParseKey()
        {
            byte typeId = br.ReadByte();
            if (debugWriter != null)
            {
                debugWriter.WriteLine($"{PsbTokenType.Key} {typeId} <");
                ++debugWriter.Indent;
                debugWriter.Write("index - ");
            }
            uint keyIndex = ParseUInt((byte)(typeId - 4));
            if (debugWriter != null)
            {
                --debugWriter.Indent;
                debugWriter.WriteLine(">");
                ++debugWriter.Indent;
            }
            string value = keyNames[keyIndex];
            if (debugWriter != null)
            {
                debugWriter.WriteLine(value);
                --debugWriter.Indent;
            }
            return value;
        }

        string ParseString(byte typeId)
        {
            if (debugWriter != null)
            {
                debugWriter.WriteLine($"{PsbTokenType.String} {typeId} <");
                ++debugWriter.Indent;
                debugWriter.Write("index - ");
            }
            uint stringIndex = ParseUInt((byte)(typeId - 8));
            if (debugWriter != null)
            {
                --debugWriter.Indent;
                debugWriter.WriteLine(">");
                ++debugWriter.Indent;
            }
            long oldPos = stream.Position;
            stream.Seek(stringsBlobOffset + stringsOffsets[stringIndex], SeekOrigin.Begin);
            string s = ReadStringZ();
            stream.Position = oldPos;
            if (debugWriter != null)
            {
                debugWriter.WriteLine(s);
                --debugWriter.Indent;
            }
            return s;
        }

        JStream ParseStream(byte typeId)
        {
            if (debugWriter != null)
            {
                debugWriter.WriteLine($"{PsbTokenType.Stream} {typeId} <");
                ++debugWriter.Indent;
                debugWriter.Write("index - ");
            }
            uint index = ParseUInt((byte)(typeId - 12));
            if (debugWriter != null)
            {
                debugWriter.WriteLine("* offset - 0x{0:x8}", streamsOffsets[index]);
                debugWriter.WriteLine("* length - 0x{0:x8}", streamsSizes[index]);
                --debugWriter.Indent;
                debugWriter.WriteLine(">");
            }
            var js = new JStream(index, false, this);
            if (!lazyStreamLoading)
            {
                long oldPos = stream.Position;
                stream.Seek(streamsBlobOffset + streamsOffsets[index], SeekOrigin.Begin);
                byte[] data = br.ReadBytes((int)streamsSizes[index]);
                stream.Position = oldPos;
                js.BinaryData = data;
                js.Reader = null;
            }
            StreamCache[index] = js;
            return js;
        }

        float ParseFloat(byte typeId)
        {
            float value;
            switch (typeId)
            {
                case 29:
                    value = 0f;
                    break;
                case 30:
                    value = br.ReadSingle();
                    break;
                default:
                    throw new ArgumentException("Dispatched to incorrect parser.", nameof(typeId));
            }

            if (debugWriter != null)
                debugWriter.WriteLine($"{PsbTokenType.Float} {typeId}: {value}");
            return value;
        }

        double ParseDouble(byte typeId)
        {
            double value;
            switch (typeId)
            {
                case 31:
                    value = br.ReadDouble();
                    break;
                default:
                    throw new ArgumentException("Dispatched to incorrect parser.", nameof(typeId));
            }

            if (debugWriter != null)
                debugWriter.WriteLine($"{PsbTokenType.Double} {typeId}: {value}");
            return value;
        }

        JArray ParseTokenArray(byte typeId)
        {
            if (typeId != 32)
                throw new ArgumentException("Dispatched to incorrect parser.", nameof(typeId));

            if (debugWriter != null)
            {
                debugWriter.WriteLine($"{PsbTokenType.TokenArray} {typeId} <");
                ++debugWriter.Indent;
                debugWriter.Write("offsets - ");
            }
            uint[] offsets = ParseUIntArray();
            if (debugWriter != null)
            {
                --debugWriter.Indent;
                debugWriter.WriteLine(">");
                ++debugWriter.Indent;
            }
            long seekBase = stream.Position;
            JArray jarr = new JArray();
            for (int i = 0; i < offsets.Length; ++i)
            {
                stream.Seek(seekBase + offsets[i], SeekOrigin.Begin);
                jarr.Add(ReadTokenValue());
            }
            if (debugWriter != null)
                --debugWriter.Indent;
            return jarr;
        }

        JObject ParseObject(byte typeId)
        {
            if (typeId != 33)
                throw new ArgumentException("Dispatched to incorrect parser.", nameof(typeId));

            if (debugWriter != null)
            {
                debugWriter.WriteLine($"{PsbTokenType.Object} {typeId} <");
                ++debugWriter.Indent;
            }
            JObject obj = new JObject();
            if (Version == 1)
            {
                // Array of offsets, then key and value blob at each offset
                if (debugWriter != null)
                    debugWriter.Write("offsets - ");
                uint[] offsets = ParseUIntArray();
                if (debugWriter != null)
                {
                    --debugWriter.Indent;
                    debugWriter.WriteLine(">");
                    ++debugWriter.Indent;
                }
                long seekBase = stream.Position;

                for (int i = 0; i < offsets.Length; ++i)
                {
                    stream.Seek(seekBase + offsets[i], SeekOrigin.Begin);
                    if (debugWriter != null)
                    {
                        debugWriter.WriteLine("~ <");
                        ++debugWriter.Indent;
                    }
                    string key = ParseKey();
                    if (debugWriter != null)
                    {
                        --debugWriter.Indent;
                        debugWriter.WriteLine("> {");
                        ++debugWriter.Indent;
                    }
                    obj.Add(key, ReadTokenValue());
                    if (debugWriter != null)
                    {
                        --debugWriter.Indent;
                        debugWriter.WriteLine("}");
                    }
                }
            }
            else
            {
                // Array of key indexes, array of offsets, and token blobs
                if (debugWriter != null)
                    debugWriter.Write("keyIndexes - ");
                uint[] keyIndexes = ParseUIntArray();
                if (debugWriter != null)
                    debugWriter.Write("offsets - ");
                uint[] offsets = ParseUIntArray();
                if (debugWriter != null)
                {
                    --debugWriter.Indent;
                    debugWriter.WriteLine(">");
                    ++debugWriter.Indent;
                }
                long seekBase = stream.Position;

                for (int i = 0; i < keyIndexes.Length; ++i)
                {
                    string keyName = keyNames[keyIndexes[i]];
                    if (debugWriter != null)
                    {
                        debugWriter.WriteLine("~ {0} ({1}) 0x{2:x8} {{", keyName, keyIndexes[i], offsets[i]);
                        ++debugWriter.Indent;
                    }
                    stream.Seek(seekBase + offsets[i], SeekOrigin.Begin);
                    obj.Add(keyName, ReadTokenValue());
                    if (debugWriter != null)
                    {
                        --debugWriter.Indent;
                        debugWriter.WriteLine("}");
                    }
                }
            }
            if (debugWriter != null)
                --debugWriter.Indent;

            return obj;
        }

        JStream ParseBStream(byte typeId)
        {
            if (debugWriter != null)
            {
                debugWriter.WriteLine($"{PsbTokenType.BStream} {typeId} <");
                ++debugWriter.Indent;
                debugWriter.Write("index - ");
            }
            uint index = ParseUInt((byte)(typeId - 21));
            if (debugWriter != null)
            {
                debugWriter.WriteLine("* offset - 0x{0:x8}", bStreamsOffsets[index]);
                debugWriter.WriteLine("* length - 0x{0:x8}", bStreamsSizes[index]);
                --debugWriter.Indent;
                debugWriter.WriteLine(">");
            }

            var js = new JStream(index, true, this);
            if (!lazyStreamLoading)
            {
                long oldPos = stream.Position;
                stream.Seek(bStreamsBlobOffset + bStreamsOffsets[index], SeekOrigin.Begin);
                byte[] data = br.ReadBytes((int)bStreamsSizes[index]);
                stream.Position = oldPos;
                js.BinaryData = data;
                js.Reader = null;
            }
            BStreamCache[index] = js;
            return js;
        }

        string ReadStringZ()
        {
            List<byte> buffer = new List<byte>();
            byte b = br.ReadByte();
            while (b != 0)
            {
                buffer.Add(b);
                b = br.ReadByte();
            }
            return Encoding.UTF8.GetString(buffer.ToArray());
        }
        #endregion

        internal byte[] GetStreamData(JStream js)
        {
            if (js.Reader != this) throw new ArgumentException("Stream does not belong to this reader.", nameof(js));
            CheckDisposed();
            if (js.IsBStream)
            {
                stream.Seek(bStreamsBlobOffset + bStreamsOffsets[js.Index], SeekOrigin.Begin);
                return br.ReadBytes((int)bStreamsSizes[js.Index]);
            }
            else
            {
                stream.Seek(streamsBlobOffset + streamsOffsets[js.Index], SeekOrigin.Begin);
                return br.ReadBytes((int)streamsSizes[js.Index]);
            }
        }

        // Decouples JStreams from the reader
        public void LoadAllStreamData()
        {
            foreach (var js in BStreamCache.Values)
            {
                js.BinaryData = GetStreamData(js);
            }
            foreach (var js in StreamCache.Values)
            {
                js.BinaryData = GetStreamData(js);
            }
        }

        internal class KeyNamesReader
        {
            uint[] valueOffsets;
            uint[] tree;
            uint[] tails;
            Dictionary<uint, string> cache = new Dictionary<uint, string>();

            public KeyNamesReader(PsbReader reader)
            {
                if (reader.Version == 1)
                {
                    reader.stream.Seek(reader.keysOffsetsOffset, SeekOrigin.Begin);
                    uint[] offsets = reader.ParseUIntArray();
                    for (uint i = 0; i < offsets.Length; ++i)
                    {
                        reader.stream.Seek(reader.keysBlobOffset + offsets[i], SeekOrigin.Begin);
                        cache.Add(i, reader.ReadStringZ());
                    }

                }
                else
                {
                    reader.stream.Seek(reader.keysBlobOffset, SeekOrigin.Begin);
                    valueOffsets = reader.ParseUIntArray();
                    tree = reader.ParseUIntArray();
                    tails = reader.ParseUIntArray();
                }
            }

            internal KeyNamesReader(uint[] valueOffsets, uint[] tree, uint[] tails)
            {
                this.valueOffsets = valueOffsets ?? throw new ArgumentNullException(nameof(valueOffsets));
                this.tree = tree ?? throw new ArgumentNullException(nameof(tree));
                this.tails = tails ?? throw new ArgumentNullException(nameof(tails));
            }

            public int Count => tails?.Length ?? cache.Count;

            public string this[uint index]
            {
                get
                {
                    if (cache.TryGetValue(index, out string v))
                    {
                        return v;
                    }

                    List<byte> buffer = new List<byte>();
                    uint current = tree[tails[index]]; // Reading from tree skips the terminating null char
                    while (current != 0)
                    {
                        uint parent = tree[current];
                        buffer.Add((byte)(current - valueOffsets[parent]));
                        current = parent;
                    }

                    buffer.Reverse();
                    string s = Encoding.UTF8.GetString(buffer.ToArray());
                    cache.Add(index, s);
                    return s;
                }
            }

            public Dictionary<uint, NameNode> GenerateNodes()
            {
                if (tree == null) throw new InvalidOperationException("Names are stored flat, not in a tree.");
                Dictionary<uint, NameNode> nodes = new Dictionary<uint, NameNode>();
                for (uint i = 0; i < tails.Length; ++i)
                {
                    TerminalNameNode tailNode = new TerminalNameNode();
                    tailNode.Index = tails[i];
                    tailNode.TailIndex = i;
                    // Quick check to ensure
                    if (valueOffsets[tailNode.Index] != i) throw new Exception();
                    tailNode.ParentIndex = tree[tailNode.Index];
                    tailNode.Character = (byte)(tailNode.Index - valueOffsets[tailNode.ParentIndex]);
                    nodes.Add(tailNode.Index, tailNode);

                    uint parentIndex = tailNode.ParentIndex;
                    while (!nodes.ContainsKey(parentIndex))
                    {
                        RegularNameNode regularNode = new RegularNameNode();
                        regularNode.Index = parentIndex;
                        regularNode.ParentIndex = tree[regularNode.Index];
                        regularNode.ValueOffset = valueOffsets[regularNode.Index];
                        if (regularNode.Index != 0)
                            regularNode.Character = (byte)(regularNode.Index - valueOffsets[regularNode.ParentIndex]);
                        nodes.Add(regularNode.Index, regularNode);
                        parentIndex = regularNode.ParentIndex;
                    }
                }

                // Link it up
                foreach (var node in nodes.Values)
                {
                    if (node.Index != 0)
                    {
                        node.Parent = nodes[node.ParentIndex];
                        var parent = node.Parent as RegularNameNode;
                        parent.Children.Add(node.Character, node);
                    }
                }

                if (nodes.Count == 0)
                {
                    // Create root node if we don't have any strings
                    RegularNameNode regularNode = new RegularNameNode();
                    regularNode.Index = 0;
                    regularNode.ParentIndex = tree[regularNode.Index];
                    regularNode.ValueOffset = valueOffsets[regularNode.Index];
                    nodes.Add(regularNode.Index, regularNode);
                }

                return nodes;
            }
        }
    }
}
