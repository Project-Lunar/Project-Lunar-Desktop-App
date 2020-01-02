using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using MArchiveBatchTool.Psb;
using MArchiveBatchTool.Models;

namespace MArchiveBatchTool.MArchive
{
    public static class AllDataPacker
    {
        static readonly int ALIGNMENT = 2048;

        public static void UnpackFiles(string psbPath, string outputPath, MArchivePacker maPacker = null)
        {
            // Figure out what file we've been given
            if (Path.GetExtension(psbPath).ToLower() == ".bin")
                psbPath = Path.ChangeExtension(psbPath, ".psb");

            // No PSB, try .psb.m
            if (!File.Exists(psbPath)) psbPath += ".m";

            if (Path.GetExtension(psbPath).ToLower() == ".m")
            {
                // Decompress .m file
                if (maPacker == null) throw new ArgumentNullException(nameof(maPacker));
                maPacker.DecompressFile(psbPath, true);
                psbPath = Path.ChangeExtension(psbPath, null);
            }

            ArchiveV1 arch;
            using (FileStream fs = File.OpenRead(psbPath))
            using (PsbReader reader = new PsbReader(fs))
            {
                var root = reader.Root;
                arch = root.ToObject<ArchiveV1>();
            }

            if (arch.ObjectType != "archive" || arch.Version != 1.0)
                throw new InvalidDataException("PSB file is not an archive.");

            using (FileStream fs = File.OpenRead(Path.ChangeExtension(psbPath, ".bin")))
            {
                foreach (var file in arch.FileInfo)
                {
                    Console.WriteLine($"Extracting {file.Key}");
                    string outPath = Path.Combine(outputPath, file.Key);
                    Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                    using (FileStream ofs = File.Create(outPath))
                    {
                        fs.Seek(file.Value[0], SeekOrigin.Begin);
                        Utils.CopyStream(fs, ofs, file.Value[1]);
                    }
                }
            }
        }

        public static void Build(string folderPath, string outputPath, MArchivePacker maPacker = null, IPsbFilter filter = null)
        {
            using (FileStream packStream = File.Create(outputPath + ".bin"))
            using (FileStream psbStream = File.Create(outputPath + ".psb"))
            {
                ArchiveV1 archive = new ArchiveV1();
                archive.ObjectType = "archive";
                archive.Version = 1.0f;
                archive.FileInfo = new Dictionary<string, List<int>>();
                folderPath = Path.GetFullPath(folderPath);

                foreach (var file in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
                {
                    string key = file.Replace(folderPath, string.Empty).TrimStart('/', '\\');
                    Console.WriteLine($"Packing {key}");
                    var targetLength = (packStream.Position + ALIGNMENT - 1) / ALIGNMENT * ALIGNMENT;
                    byte[] alignBytes = new byte[targetLength - packStream.Length];
                    packStream.Write(alignBytes, 0, alignBytes.Length);
                    var currPos = packStream.Position;
                    using (FileStream fs = File.OpenRead(file))
                    {
                        fs.CopyTo(packStream);
                        archive.FileInfo.Add(key.Replace('\\', '/'), new List<int>() { (int)currPos, (int)fs.Length });
                    }
                }

                JToken root = JToken.FromObject(archive);
                PsbWriter writer = new PsbWriter(root, null) { Version = 3 };
                writer.Write(psbStream, filter);
            }

            if (maPacker != null)
            {
                maPacker.CompressFile(outputPath + ".psb");
            }
        }
    }
}
