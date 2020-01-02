using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace MArchiveBatchTool.MArchive
{
    public class ZlibCodec : IMArchiveCodec
    {
        public uint Magic => 0x0066646d; // "mdf\0"

        public Stream GetCompressionStream(Stream inStream)
        {
            return new DeflaterOutputStream(inStream);
        }

        public Stream GetDecompressionStream(Stream inStream)
        {
            return new InflaterInputStream(inStream);
        }
    }
}
