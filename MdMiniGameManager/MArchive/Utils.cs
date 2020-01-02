using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MArchiveBatchTool.Psb;

namespace MArchiveBatchTool
{
    public static class Utils
    {
        // Modified from https://stackoverflow.com/a/230141/1180879
        public static void CopyStream(Stream input, Stream output, int count)
        {
            byte[] buffer = new byte[81920];
            int read;
            while (count > 0 && (read = input.Read(buffer, 0, Math.Min(buffer.Length, count))) > 0)
            {
                output.Write(buffer, 0, read);
                count -= read;
            }
        }

        public static void SerializePsb(string path, ushort version, PsbFlags flags, IPsbFilter filter, bool optimize, bool readAsFloat)
        {
            string psbPath = Path.ChangeExtension(path, null);
            if (!psbPath.ToLower().EndsWith(".psb"))
                psbPath += ".psb";
            using (StreamReader reader = File.OpenText(path))
            using (Stream writer = File.Create(psbPath))
            {
                JsonTextReader jReader = new JsonTextReader(reader)
                {
                    FloatParseHandling = readAsFloat ? FloatParseHandling.Double : FloatParseHandling.Decimal
                };
                JToken root = JToken.ReadFrom(jReader);
                IPsbStreamSource streamSource = new CliStreamSource(Path.ChangeExtension(path, ".streams"));
                PsbWriter psbWriter = new PsbWriter(root, streamSource);
                psbWriter.Version = version;
                psbWriter.Flags = flags;
                psbWriter.Optimize = optimize;
                psbWriter.Write(writer, filter);
            }
        }

        public static void DumpPsb(string psbPath, bool writeDebug, IPsbFilter filter = null)
        {
            using (FileStream fs = File.OpenRead(psbPath))
            using (StreamWriter debugWriter = writeDebug ? File.CreateText(psbPath + ".debug.txt") : null)
            using (PsbReader psbReader = new PsbReader(fs, filter, debugWriter))
            {
                Newtonsoft.Json.Linq.JToken root;
                try
                {
                    root = psbReader.Root;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error parsing PSB");
                    Console.WriteLine("Current position: 0x{0:x8}", fs.Position);
                    Console.WriteLine(ex);

                    if (writeDebug && filter != null)
                    {
                        using (FileStream dcStream = File.Create(psbPath + ".decrypted"))
                        {
                            psbReader.DumpDecryptedStream(dcStream);
                        }
                    }
                    return;
                }
                using (StreamWriter sw = File.CreateText(psbPath + ".json"))
                using (JsonTextWriter jtw = new JsonTextWriter(sw) { Formatting = Formatting.Indented })
                {
                    root.WriteTo(jtw);
                }
                if (psbReader.StreamCache.Count > 0 || psbReader.BStreamCache.Count > 0)
                {
                    string streamsDirPath = psbPath + ".streams";
                    Directory.CreateDirectory(streamsDirPath);
                    foreach (var js in psbReader.StreamCache)
                    {
                        File.WriteAllBytes(Path.Combine(streamsDirPath, "stream_" + js.Key), js.Value.BinaryData);
                    }
                    foreach (var js in psbReader.BStreamCache)
                    {
                        File.WriteAllBytes(Path.Combine(streamsDirPath, "bstream_" + js.Key), js.Value.BinaryData);
                    }
                }
                // if (writeDebug)
                // {
                //     using (StreamWriter sw = File.CreateText(psbPath + ".keys.gv"))
                //     {
                //         Analysis.GenerateNameGraphDot(sw, psbReader);
                //     }
                //     using (StreamWriter sw = File.CreateText(psbPath + ".keyranges.txt"))
                //     {
                //         Analysis.GenerateNameRanges(sw, psbReader);
                //     }
                //     using (StreamWriter sw = File.CreateText(psbPath + ".rangevis.txt"))
                //     {
                //         Analysis.GenerateRangeUsageVisualization(sw, psbReader);
                //     }
                //     using (StreamWriter sw = File.CreateText(psbPath + ".keygen.txt"))
                //     {
                //         Analysis.TestKeyNamesGeneration(sw, psbReader);
                //     }
                // }
            }
        }


    }
}
