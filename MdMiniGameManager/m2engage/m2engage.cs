using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLunarUI.M2engage
{
    public static class m2engage
    {
        public static string FindPsbKey(string filePath)
        {
            string key = string.Empty;
            BinaryReader reader = new BinaryReader(File.OpenRead(filePath));

            for (int i = 0; i < reader.BaseStream.Length; i += 1024)
            {
                byte[] dataBlock = new byte[1024];
                reader.Read(dataBlock, 0, 1024);

                string blockText = Encoding.Default.GetString(dataBlock);
                if (blockText.Contains("getExistFileDirInMountArchive"))
                {
                    key = blockText.Substring(blockText.IndexOf("getExistFileDirInMountArchive") + 32, 13);
                    break;
                }
            }
            return key;
        }
    }
}
