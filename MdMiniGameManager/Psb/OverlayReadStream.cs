using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MArchiveBatchTool.Psb
{
    class OverlayReadStream : Stream
    {
        Stream baseStream;
        byte[] decryptedData;
        long overlayStart;
        long overlayEnd;
        bool isDisposed;

        public OverlayReadStream(Stream baseStream, uint overlayStart, uint overlayEnd, IPsbFilter filter)
        {
            this.baseStream = baseStream;
            this.overlayStart = overlayStart;
            this.overlayEnd = overlayEnd;
            long origBasePos = baseStream.Position;
            baseStream.Seek(overlayStart, SeekOrigin.Begin);
            decryptedData = new byte[(int)(overlayEnd - overlayStart)];
            if (baseStream.Read(decryptedData, 0, decryptedData.Length) != decryptedData.Length)
                throw new IOException("Could not read all bytes in overlay region.");
            filter.Filter(decryptedData);
            baseStream.Position = origBasePos;
        }

        public override bool CanRead => baseStream.CanRead;

        public override bool CanSeek => baseStream.CanSeek;

        public override bool CanWrite => false;

        public override long Length => baseStream.Length;

        public override long Position { get => baseStream.Position; set => baseStream.Position = value; }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset is negative.");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count is negative.");
            if (offset > buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset), "Offset is past the end of buffer.");
            if (offset + count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count), "Offset and count exceeds end of buffer.");

            if (isDisposed) throw new ObjectDisposedException(GetType().FullName);

            int totalRead = 0;
            while (count > 0)
            {
                if (Position < overlayStart)
                {
                    int bytesInOverlay = (int)(Position + count - overlayStart);
                    if (bytesInOverlay < 0) bytesInOverlay = 0;
                    int bytesToRead = count - bytesInOverlay;
                    int read = baseStream.Read(buffer, offset, bytesToRead);
                    totalRead += read;
                    offset += read;
                    count -= read;
                    if (read != bytesToRead) break;
                }
                else if (Position >= overlayEnd)
                {
                    totalRead += baseStream.Read(buffer, offset, count);
                    break;
                }
                else
                {
                    int bytesOutsideOverlay = (int)(Position + count - overlayEnd);
                    if (bytesOutsideOverlay < 0) bytesOutsideOverlay = 0;
                    int bytesToRead = count - bytesOutsideOverlay;
                    Buffer.BlockCopy(decryptedData, (int)(Position - overlayStart), buffer, offset, bytesToRead);
                    totalRead += bytesToRead;
                    offset += bytesToRead;
                    count -= bytesToRead;
                    baseStream.Position += bytesToRead;
                }
            }
            return totalRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                isDisposed = true;
                baseStream.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
