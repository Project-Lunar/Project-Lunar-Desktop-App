using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MArchiveBatchTool.MArchive
{
    public class MArchivePacker
    {
        IMArchiveCodec codec;
        string seed;
        int keyLength;
        List<string> noCompressionFilters = new List<string>() { "sound" };

        public MArchivePacker(IMArchiveCodec codec, string seed, int keyLength)
        {
            if (codec == null) throw new ArgumentNullException(nameof(codec));
            if (string.IsNullOrEmpty(seed)) throw new ArgumentNullException(nameof(seed));

            this.codec = codec;
            this.seed = seed;
            this.keyLength = keyLength;
        }

        public List<string> NoCompressionFilters
        {
            get => noCompressionFilters;
            set => noCompressionFilters = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void DecompressFile(string path, bool keepOrig = false)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (Path.GetExtension(path).ToLower() != ".m")
                throw new ArgumentException("File is not compressed.", nameof(path));

            using (FileStream fs = File.OpenRead(path))
            {
                BinaryReader br = new BinaryReader(fs);
                uint magic = br.ReadUInt32();
                // TODO: dynamically grab the right codec
                if (magic != codec.Magic) throw new ArgumentException("Codec mismatch", nameof(path));
                int decompressedLength = br.ReadInt32();

                using (FileStream ofs = File.Create(Path.ChangeExtension(path, null)))
                using (MArchiveCryptoStream cs = new MArchiveCryptoStream(fs, path, seed, keyLength))
                using (Stream decompStream = codec.GetDecompressionStream(cs))
                {
                    decompStream.CopyTo(ofs);
                    ofs.Flush();
                    if (ofs.Length != decompressedLength)
                        throw new InvalidDataException("Decompressed stream length is not same as expected.");
                }
            }

            if (!keepOrig) File.Delete(path);
        }

        public void CompressFile(string path, bool keepOrig = false)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            string destPath = path + ".m";
            using (FileStream fs = File.OpenRead(path))
            using (FileStream ofs = File.Create(destPath))
            {
                BinaryWriter bw = new BinaryWriter(ofs);
                bw.Write(codec.Magic);
                bw.Write((int)fs.Length);

                using (MArchiveCryptoStream cs = new MArchiveCryptoStream(ofs, destPath, seed, keyLength))
                using (Stream compStream = codec.GetCompressionStream(cs))
                {
                    fs.CopyTo(compStream);
                }
            }

            if (!keepOrig) File.Delete(path);
        }

        public void CompressDirectory(string path, bool keepOrig = false, bool forceCompress = false)
        {
            foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
            {
                if (Path.GetExtension(file).ToLower() == ".m") continue;
                string containingDir = Path.GetFileName(Path.GetDirectoryName(file));
                if (forceCompress || !noCompressionFilters.Contains(containingDir))
                {
                    Console.WriteLine($"Compressing {file}");
                    CompressFile(file, keepOrig);
                }
            }
        }

        public void DecompressDirectory(string path, bool keepOrig = false)
        {
            foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
            {
                if (Path.GetExtension(file).ToLower() == ".m")
                {
                    Console.WriteLine($"Decompressing {file}");
                    DecompressFile(file, keepOrig);
                }
            }
        }
    }
}
