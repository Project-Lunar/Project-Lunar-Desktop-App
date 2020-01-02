using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLunarUI
{
    public class ScraperData
    {
        public ScraperData()
        {
            Name = new Dictionary<string, string>();
            Description = new Dictionary<string, string>();
        }

        public Dictionary<string,string> Name
        {
            get; set;
        }

        public Dictionary<string,string> Description
        {
            get; set;
        }

        public DateTime ReleaseDate
        {
            get; set;
        }

        public string Copyright
        {
            get; set;
        }

        public int Genre
        {
            get; set;
        }

        public int PlayerNum
        {
            get; set;
        }

        public Bitmap CoverFront
        {
            get; set;
        }

        public Bitmap CoverSpine
        {
            get; set;
        }

        public Bitmap ClearLogo
        {
            get; set;
        }

        public string System
        {
            get; set;
        }

        public string Region
        {
            get; set;
        }

        public JObject ScrapedGameData
        {
            get; set;
        }

        public override string ToString() => $"{Name["en"]} ({(string.IsNullOrEmpty(Region) ? string.Empty : $"{Region.ToUpper()} ")}{System})";

    }
}
