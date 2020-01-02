using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Meisui.Random;

namespace MArchiveBatchTool.MArchive
{
    public class MArchiveCryptoStream : Stream
    {
        Stream stream;
        byte[] keyBuffer;

        public MArchiveCryptoStream(Stream inStream, string fileName, string seed, int keyLength)
        {
            if (inStream == null) throw new ArgumentNullException(nameof(inStream));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));
            if (string.IsNullOrEmpty(seed)) throw new ArgumentNullException(nameof(seed));

            if (!inStream.CanSeek) throw new ArgumentException("Stream is not seekable.", nameof(inStream));
            stream = inStream;

            // Generate key buffer
            // Loosely based off of https://github.com/ajd4096/inject_gba
            string hashSeed = seed + Path.GetFileName(fileName).ToLower();
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(hashSeed));
            }
            uint[] twisterSeed = new uint[4];
            for (int i = 0; i < twisterSeed.Length; ++i)
            {
                twisterSeed[i] = BitConverter.ToUInt32(hash, i * 4);
            }
            MersenneTwister twister = new MersenneTwister(twisterSeed);
            List<byte> keyBytes = new List<byte>();
            while (keyBytes.Count < keyLength)
            {
                keyBytes.AddRange(BitConverter.GetBytes(twister.genrand_Int32()));
            }
            keyBuffer = keyBytes.ToArray();
            if (keyBuffer.Length > keyLength)
            {
                Array.Resize(ref keyBuffer, keyLength);
            }
        }

        public override bool CanRead => stream.CanRead;

        public override bool CanSeek => stream.CanSeek;

        public override bool CanWrite => stream.CanWrite;

        public override long Length => stream.Length;

        public override long Position { get => stream.Position; set => stream.Position = value; }

        public override void Flush()
        {
            stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            long startPos = stream.Position;
            int read = stream.Read(buffer, offset, count);
            ProcessBuffer(buffer, offset, read, startPos);
            return read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // We have to check arguments here because the stream only gets to
            // them after there's a chance for something to go wrong.
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset is negative.");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count is negative.");
            if (offset > buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset), "Offset is past the end of buffer.");
            if (offset + count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count), "Offset and count exceeds end of buffer.");

            byte[] dup = (byte[])buffer.Clone();
            ProcessBuffer(dup, offset, count, stream.Position);
            stream.Write(dup, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                stream.Dispose();
            }
            base.Dispose(disposing);
        }

        void ProcessBuffer(byte[] buffer, int offset, int count, long streamPosition)
        {
            for (int i = 0; i < count; ++i)
            {
                // Don't process first 8 bytes, which is the MArchive header
                if (streamPosition + i >= 8)
                {
                    buffer[offset + i] ^= keyBuffer[(streamPosition + i - 8) % keyBuffer.Length];
                }
            }
        }
    }
}
