using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MArchiveBatchTool.Models
{
    public class ArchiveV1
    {
        [JsonProperty("id")]
        public string ObjectType { get; set; }
        [JsonProperty("version")]
        public float Version { get; set; }
        [JsonProperty("file_info")]
        public Dictionary<string, List<int>> FileInfo { get; set; }
        [JsonProperty("expire_suffix_list")]
        public List<string> ExpireSuffixList { get; set; }
    }
}
