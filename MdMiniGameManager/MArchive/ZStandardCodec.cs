using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Zstandard.Net;

namespace MArchiveBatchTool.MArchive
{
    public class ZStandardCodec : IMArchiveCodec
    {
        public uint Magic => 0x00737a6d; // "mzs\0"

        public Stream GetCompressionStream(Stream inStream)
        {
            return new ZstandardStream(inStream, CompressionMode.Compress, true);
        }

        public Stream GetDecompressionStream(Stream inStream)
        {
            return new ZstandardStream(inStream, CompressionMode.Decompress, true);
        }
    }
}
