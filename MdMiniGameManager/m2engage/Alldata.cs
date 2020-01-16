using Force.Crc32;
using MArchiveBatchTool;
using MArchiveBatchTool.MArchive;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectLunarUI.M2engage
{
    public enum SpecialLunarRom
    {
        BootMenu,
        RetroArch
    }

    public static class Alldata
    {
        public static string AddGameToRomsObject(string romCode, JObject romsObject, string romPath, string basePath, string sysRegion, string country, bool use6ButtonHack)
        {
            int gameVersionValue = romsObject["root"]["game_versions"].Children().Children().Max(c => (int)c[0]);

            if (gameVersionValue.Equals(255))
            {
                throw new InvalidDataException("Cannot add more games. Maximum limit supported by the stock UI has been reached.");
            }

            string gameVersionJson = "[\r\n  " + (gameVersionValue + 1).ToString() + ",\r\n  0,\r\n  \"MenuItemText__TITLE_GAME_VERSION_SNOW_BROS\"\r\n]";
            JToken gameVersion = JToken.Parse(gameVersionJson);

            string regionTag = (romCode.Contains("BOOTMENU__") || romCode.Contains("RETROARCH__") ? romCode : $"{sysRegion.ToUpper()}_EN_{romCode}");

            ((JObject)romsObject["root"]["game_versions"]).Add(regionTag, gameVersion);

            string RomName = Path.GetFileNameWithoutExtension(romPath).Replace(" ", "_").Replace("'", "").Replace("\"", "").Replace(".", "");
            if (RomName.Contains("[") || RomName.Contains("("))
            {
                RomName = RomName.Substring(0, RomName.IndexOfAny(new char[] { '[', '(' }) - 1);
            }

            //In case of duplicate rom...
            if (File.Exists($@"{basePath}\system\roms\{sysRegion}_en_{RomName}.bin.m") || File.Exists($@"{basePath}\system\roms\{sysRegion}_en_{RomName}.bin"))
            {
                int i = 0;
                while (File.Exists($@"{basePath}\system\roms\{sysRegion}_en_{RomName}{i}.bin.m") || 
                       File.Exists($@"{basePath}\system\roms\{sysRegion}_en_{RomName}{i}.bin"))
                {
                    i++;
                }
                RomName = $"{RomName}{i}";
            }

            string versionJson = "{\r\n  \"arch\": \"md\",\r\n  \"country\": \"" + country + "\",\r\n  \"preamp\": 1.00,\r\n  \"rom\": \"roms/" + sysRegion + "_en_" + RomName + ".bin\",\r\n  \"stringGlobal\": {\r\n    \"mdz80.overclock\": 1.0\r\n  }\r\n}";
            JToken versionObj = JToken.Parse(versionJson);
            if (use6ButtonHack)
            {
                ((JObject)versionObj["stringGlobal"]).Add("dev.mdpad.enable_6b_toejam2_hack", "1");
            }

            ((JObject)romsObject["root"]["m2epi"]["version"]).Add(regionTag, versionObj);

            return RomName;
        }

        public static string AddGameToTitlesObject(int newGameTime, string specialRomCode, JObject titlesObject, JObject texturesObject, string titleName, string description, string copyright, string dev_name, string sysRegion, int demoTime, ScraperData gameData = null, string romPath = null)
        {
            int gamesCount = titlesObject["items"].Count() / 8;
            int newSortMax = titlesObject["items"].Where(c => !(c["tname"].ToString().Trim().Equals(""))).Max(c => (int)c["sor_name"]);
            string dummyGameJson = "{\r\n  \"action\": \"Boot\",\r\n  \"copy\": \"\",\r\n  \"csize\": 0,\r\n  \"demo_time\": 6216,\r\n  \"desc\": \"\",\r\n  \"dev_name\": \"MegaDriveMiniUSA\",\r\n  \"image\": 49,\r\n  \"name\": \"ダミー\",\r\n  \"regionTag\": \"DUMMY\",\r\n  \"sor_date\": 49,\r\n  \"sor_demo\": 49,\r\n  \"sor_genr\": 49,\r\n  \"sor_name\": 49,\r\n  \"sor_pnum\": 49,\r\n  \"tname\": \"\"\r\n}";
            JToken newGame = JToken.Parse(dummyGameJson);
            newGame["desc"] = description.Replace("\r", " ");
            newGame["image"] = newGameTime;
            newGame["name"] = "プロジェクト・ルナーの新しいゲーム";
            newGame["copy"] = $"┏{copyright}";
            newGame["dev_name"] = dev_name;
            newGame["demo_time"] = (demoTime > 0 ? demoTime : 6216);
            newGame["sor_date"] = newSortMax + 1;
            newGame["sor_demo"] = gamesCount; //let's not mess with the demo settings
            newGame["sor_genr"] = newSortMax + 1;
            newGame["sor_name"] = newSortMax + 1;
            newGame["sor_pnum"] = newSortMax + 1;
            newGame["tname"] = titleName;

            string romCode = string.Empty;          
            if (string.IsNullOrEmpty(specialRomCode))
            {
                uint crc = Crc32Algorithm.Compute(File.ReadAllBytes(romPath));
                romCode = crc.ToString("X8");
            }
            else
            {
                romCode = specialRomCode;
            }
            newGame["regionTag"] = (string.IsNullOrEmpty(specialRomCode) ? $"{sysRegion.ToUpper()}_EN_{romCode}" : romCode);

            titlesObject["items"][(gamesCount * 8) - 1].AddAfterSelf(GetGameTextForLanguage(newGame, "kr", gameData));
            titlesObject["items"][(gamesCount * 7) - 1].AddAfterSelf(GetGameTextForLanguage(newGame, "cn", gameData));
            titlesObject["items"][(gamesCount * 6) - 1].AddAfterSelf(GetGameTextForLanguage(newGame, "de", gameData));
            titlesObject["items"][(gamesCount * 5) - 1].AddAfterSelf(GetGameTextForLanguage(newGame, "it", gameData));
            titlesObject["items"][(gamesCount * 4) - 1].AddAfterSelf(GetGameTextForLanguage(newGame, "fr", gameData));
            titlesObject["items"][(gamesCount * 3) - 1].AddAfterSelf(GetGameTextForLanguage(newGame, "en", gameData));
            titlesObject["items"][(gamesCount * 2) - 1].AddAfterSelf(GetGameTextForLanguage(newGame, "en", gameData));
            titlesObject["items"][(gamesCount * 1) - 1].AddAfterSelf(GetGameTextForLanguage(newGame, "jp", gameData));

            //Fix sorting for all 8 languages 😩
            for (int lang = 0; lang < 8; lang++)
            {
                //Get sublist of that language only
                List<JToken> games = new List<JToken>();
                for (int i = (lang * (gamesCount + 1)); i < ((lang + 1) * (gamesCount + 1)); i++)
                {
                    games.Add(titlesObject["items"][i]);
                }


                //Sort by name
                int sortNum = 0;
                games.Where(c => !c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                             .OrderBy(c => c["tname"].ToString()).ToList()
                                             .ForEach(c => c["sor_name"] = sortNum++);


                //Sort by release date, name
                sortNum = 0;
                games.Where(c => !c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                             .OrderBy(c => GetReleaseDate(c["desc"].ToString()))
                                             .ThenBy(c => c["tname"].ToString()).ToList()
                                             .ForEach(c => c["sor_date"] = sortNum++);

                //Sort by genre, name
                sortNum = 0;
                games.Where(c => !c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                             .OrderBy(c => GetGenre((int)c["image"], texturesObject))
                                             .ThenBy(c => c["tname"].ToString()).ToList()
                                             .ForEach(c => c["sor_genr"] = sortNum++);

                //Sort by player number, name
                sortNum = 0;
                games.Where(c => !c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                             .OrderBy(c => GetPlayerNum((int)c["image"], texturesObject))
                                             .ThenBy(c => c["tname"].ToString()).ToList()
                                             .ForEach(c => c["sor_pnum"] = sortNum++);
            }

            //set all other empty title games to the highest sort number.
            titlesObject["items"].Where(c => c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                 .ToList().ForEach(c => c["sor_date"] =
                                                        c["sor_demo"] =
                                                        c["sor_genr"] =
                                                        c["sor_name"] =
                                                        c["sor_pnum"] = gamesCount);

            return romCode;
        }

        private static JToken GetGameTextForLanguage(JToken newGame, string language, ScraperData gameData)
        {
            if (gameData == null)
            {
                return newGame;
            }

            JToken newGameLanguage = newGame.DeepClone();
            newGameLanguage["tname"] = gameData.Name[language];

            DateTime releaseDate = gameData.ReleaseDate;
            string overview = gameData.Description[language];

            string releaseYear = $"Release Year: {releaseDate.Year}";
            if (overview.ToLower().Contains("Release Year:"))
            {
                int releaseStart = overview.IndexOf("Release Year", StringComparison.InvariantCultureIgnoreCase);
                int releaseEnd = overview.IndexOf("\r\n", releaseStart);
                releaseYear = overview.Substring(releaseStart, releaseEnd - releaseStart);
                overview = overview.Replace(releaseYear, "");
                overview = overview.Replace("\r\n\r\n", "");
            }

            List<string> formattedDescriptionLines = Regex.Split($"{releaseYear}\r\n\r\n{Assign<string>(overview)}",
                                                                 @"(.{1,40})(?:\s|$)").Where(x => x.Length > 0)
                                                                                      .Select(x => x.Trim())
                                                                                      .ToList();
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < 9; i++)
            {
                try
                {
                    strBuilder.AppendLine(formattedDescriptionLines[i]);
                }
                catch
                {
                    break;
                }
            }

            newGameLanguage["desc"] = strBuilder.ToString().Replace("\r", " ");
            return newGameLanguage;
        }

        private static int GetPlayerNum(int image, JObject texturesObject)
        {
            int pnum = 0;
            try
            {
                pnum = Convert.ToInt32(texturesObject["object"]["pkgData"]["motion"]["big_pnum"]["layer"][0]["frameList"]
                                                     .Where(c => ((int)c["time"]).Equals(image)).FirstOrDefault()
                                                     ["content"]["icon"].ToString().Substring(1, 1));
            }
            catch
            {
                //Image code outside the icon time range... o_O
                //Spanish and French Light Cruzader, French and German Landstalker... all single player
                return 0;
            }

            return pnum;
        }

        private static int GetGenre(int image, JObject texturesObject)
        {
            int genre = 0;
            try
            {
                genre = Convert.ToInt32(texturesObject["object"]["pkgData"]["motion"]["big_genre"]["layer"][0]["frameList"]
                                                      .Where(c => ((int)c["time"]).Equals(image)).FirstOrDefault()
                                                      ["content"]["icon"].ToString().Substring(1, 1));
            }
            catch
            {
                //Image code outside the icon time range... o_O
                //Spanish and French Light Cruzader, French and German Landstalker... all RPGs
                return 6;
            }
            return genre;
        }

        private static DateTime GetReleaseDate(string description)
        {
            DateTime releaseDate;
            string firstLine = new StringReader(description).ReadLine().Trim();

            try
            {
                if (firstLine.Contains("発"))
                {
                    releaseDate = Convert.ToDateTime(firstLine.Substring(0, firstLine.LastIndexOf("発")));
                }
                else
                {
                    if (firstLine.Contains("年"))
                    {
                        releaseDate = new DateTime(Convert.ToInt32(firstLine.Substring(firstLine.Length - 5, 4)), 1, 1);
                    }
                    else
                    {
                        releaseDate = new DateTime(Convert.ToInt32(firstLine.Substring(firstLine.Length - 4, 4)), 1, 1);
                    }
                }

                return releaseDate;
            }
            catch
            {
                return new DateTime(1988, 10, 29);
            }
        }

        public static void AddGameToTextureObject(int newGameTime, JObject texturesObject, string artRegion,
                                          int genreIndex, int pnumIndex, Image boxArt, Image spineArt, ImageAttributes imageAttributes,
                                          Dictionary<string, int> streamNumber, Dictionary<string, Dictionary<string, Bitmap>> textureSources)
        {
            AddGameToTextureObject(newGameTime, texturesObject, artRegion, genreIndex,
                                   pnumIndex, boxArt, spineArt, imageAttributes, streamNumber, textureSources,
                                   null, new Point(0, 0), new Point(0, 0));
        }

        public static void AddGameToTextureObject(int newGameTime, JObject texturesObject, string artRegion,
                                                  int genreIndex, int pnumIndex, Image boxArt, Image spineArt, ImageAttributes imageAttributes,
                                                  Dictionary<string, int> streamNumber, Dictionary<string, Dictionary<string, Bitmap>> textureSources,
                                                  Bitmap dest_texture, Point dest_coords_box, Point dest_coords_spine)
        {
            //Update mystery *big* list

            string bigName = (artRegion.Equals("jp") ? "big" : $"big_{artRegion}");
            if (texturesObject["object"]["pkgData"]["motion"][bigName] != null)
            {
                JToken big = texturesObject["object"]["pkgData"]["motion"][bigName];
                JToken bigFramelist = big["layer"][0]["frameList"];
                int bigCount = bigFramelist.Count();
                string bigIconJson = "{\r\n  \"content\": {\r\n    \"icon\": \"big_" + bigCount.ToString() + "\",\r\n    \"mask\": 0,\r\n    \"src\": \"big\"\r\n  },\r\n  \"time\": " + newGameTime.ToString() + ",\r\n  \"type\": 2\r\n}";

                JToken newBigIcon = JToken.Parse(bigIconJson);
                bigFramelist[bigFramelist.Count() - 1].AddBeforeSelf(newBigIcon);
                if (newGameTime >= (int)big["lastTime"])
                {
                    big["lastTime"] = newGameTime + 1;
                    bigFramelist[bigFramelist.Count() - 1]["time"] = newGameTime + 1;
                }
                texturesObject["object"]["pkgData"]["motion"][bigName]["parameter"][0]["division"] = newGameTime;
                texturesObject["object"]["pkgData"]["motion"][bigName]["parameter"][0]["rangeEnd"] = newGameTime;
                texturesObject["object"]["pkgData"]["motion"][bigName]["priority"][1]["time"] = newGameTime + 1;
            }

            //Update Genre icon list
            JToken bigGenre = texturesObject["object"]["pkgData"]["motion"]["big_genre"];
            JToken bigGenreFramelist = bigGenre["layer"][0]["frameList"];
            string genreIcon = $"g{(genreIndex >= 0 ? genreIndex : 0)}";
            string genreIconJson = "{\r\n  \"content\": {\r\n    \"icon\": \"" + genreIcon + "\",\r\n    \"mask\": 524288,\r\n    \"motion\": {\r\n      \"mask\": 0,\r\n      \"timeOffset\": 0\r\n    },\r\n    \"src\": \"icon\"\r\n  },\r\n  \"time\": " + newGameTime.ToString() + ",\r\n  \"type\": 2\r\n}";

            JToken newGenreIcon = JToken.Parse(genreIconJson);
            bigGenreFramelist[bigGenreFramelist.Count() - 1].AddBeforeSelf(newGenreIcon);
            if (newGameTime >= (int)bigGenre["lastTime"])
            {
                bigGenre["lastTime"] = newGameTime + 1;
                bigGenreFramelist[bigGenreFramelist.Count() - 1]["time"] = newGameTime + 1;
            }
            texturesObject["object"]["pkgData"]["motion"]["big_genre"]["parameter"][0]["division"] = newGameTime;
            texturesObject["object"]["pkgData"]["motion"]["big_genre"]["parameter"][0]["rangeEnd"] = newGameTime;
            texturesObject["object"]["pkgData"]["motion"]["big_genre"]["priority"][1]["time"] = newGameTime + 1;

            //Update pnum icon list
            JToken bigpPnum = texturesObject["object"]["pkgData"]["motion"]["big_pnum"];
            JToken bigpPnumFramelist = bigpPnum["layer"][0]["frameList"];
            string pnumIcon = $"p{(pnumIndex >= 0 ? pnumIndex : 0)}";
            string pnumIconJson = "{\r\n  \"content\": {\r\n    \"icon\": \"" + pnumIcon + "\",\r\n    \"mask\": 524288,\r\n    \"motion\": {\r\n      \"mask\": 0,\r\n      \"timeOffset\": 0\r\n    },\r\n    \"src\": \"icon\"\r\n  },\r\n  \"time\": " + newGameTime.ToString() + ",\r\n  \"type\": 2\r\n}";

            JToken newPnumIcon = JToken.Parse(pnumIconJson);
            bigpPnumFramelist[bigpPnumFramelist.Count() - 1].AddBeforeSelf(newPnumIcon);
            if (newGameTime >= (int)bigpPnum["lastTime"])
            {
                bigpPnum["lastTime"] = newGameTime + 1;
                bigpPnumFramelist[bigpPnumFramelist.Count() - 1]["time"] = newGameTime + 1;
            }

            texturesObject["object"]["pkgData"]["motion"]["big_pnum"]["parameter"][0]["division"] = newGameTime;
            texturesObject["object"]["pkgData"]["motion"]["big_pnum"]["parameter"][0]["rangeEnd"] = newGameTime;
            texturesObject["object"]["pkgData"]["motion"]["big_pnum"]["priority"][1]["time"] = newGameTime + 1;

            //Write boxes graphics
            Bitmap streamTexture;
            Rectangle destRectangleBox;
            Rectangle destRectangleSpine;
            if (dest_texture == null)
            {
                streamTexture = new Bitmap(152 + 30, 216);
                destRectangleSpine = new Rectangle(0, 0, 30, 216);
                destRectangleBox = new Rectangle(30, 0, 152, 216);
            }
            else
            {
                streamTexture = dest_texture;
                destRectangleSpine = new Rectangle(dest_coords_spine, new Size(30, 216));
                destRectangleBox = new Rectangle(dest_coords_box, new Size(152, 216));
            }

            using (Graphics gfx = Graphics.FromImage(streamTexture))
            {
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.DrawImage(spineArt, destRectangleSpine,
                                        0, 0, 30, 216,
                                        GraphicsUnit.Pixel, imageAttributes);
                gfx.DrawImage(boxArt, destRectangleBox,
                                      0, 0, 152, 216,
                                      GraphicsUnit.Pixel, imageAttributes);
            }

            //New texture object
            string lastTexture = ((JProperty)texturesObject["source"].Children().OrderBy(c => int.Parse(((JProperty)c).Name.Split('#')[1])).Last()).Name;
            streamNumber[artRegion] = texturesObject["source"].Children().Children().Max(c => int.Parse(c["texture"]["pixel"].ToString().Split(':')[1])) + 1;
            string textureName = $"{lastTexture.Substring(0, 4)}{(int.Parse(lastTexture.Substring(4)) + 1).ToString("000")}";


            List<int> lstMax = new List<int>(); //scan the highest icon number
            foreach (JToken texture in texturesObject["source"].Children().Children())
            {
                lstMax.Add(texture["icon"].Max(c => Convert.ToInt32(((JProperty)c).Name)));
            }
            int newIconNum = lstMax.Max() + 1;

            string textureJson = "{\r\n  \"icon\": {\r\n    \"" + newIconNum.ToString("0000") + "\": {\r\n      \"attr\": 0,\r\n      \"height\": 214,\r\n      \"left\": 1.0,\r\n      \"metadata\": null,\r\n      \"originX\": 14,\r\n      \"originY\": 107,\r\n      \"top\": 1.0,\r\n      \"width\": 28\r\n    },\r\n    \"" + (newIconNum + 1).ToString("0000") + "\": {\r\n      \"attr\": 0,\r\n      \"height\": 214,\r\n      \"left\": 31.0,\r\n      \"metadata\": null,\r\n      \"originX\": 75,\r\n      \"originY\": 107,\r\n      \"top\": 1.0,\r\n      \"width\": 150\r\n    }\r\n      },\r\n  \"metadata\": null,\r\n  \"texture\": {\r\n    \"ast\": 0,\r\n    \"height\": 216,\r\n    \"pixel\": \"_stream:" + streamNumber[artRegion].ToString() + "\",\r\n    \"truncated_height\": 216,\r\n    \"truncated_width\": 182,\r\n    \"type\": \"RGBA8\",\r\n    \"width\": 182\r\n  },\r\n  \"type\": 0\r\n }";
            JToken newTexture = JToken.Parse(textureJson);
            newTexture["texture"]["pixel"] = $"_stream:{streamNumber[artRegion]}";

            ((JObject)texturesObject["source"]).Add(textureName, newTexture);
            textureSources[artRegion].Add(textureName, streamTexture);

            //Update Front icon list
            string frontTag = $"front{(artRegion == "jp" ? "" : $"_{artRegion}")}";
            JToken boxFront = texturesObject["object"]["pkgData"]["motion"][frontTag];
            JToken boxFrontFramelist = boxFront["layer"][0]["frameList"];
            if (newGameTime >= (int)boxFront["lastTime"])
            {
                boxFront["lastTime"] = newGameTime + 1;
                boxFrontFramelist[boxFrontFramelist.Count() - 1]["time"] = newGameTime + 1;
            }

            texturesObject["object"]["pkgData"]["motion"][frontTag]["parameter"][0]["division"] = newGameTime;
            texturesObject["object"]["pkgData"]["motion"][frontTag]["parameter"][0]["rangeEnd"] = newGameTime;
            texturesObject["object"]["pkgData"]["motion"][frontTag]["priority"][1]["time"] = newGameTime + 1;

            string boxFrontIconJson = "{\r\n  \"content\": {\r\n    \"icon\": \"" + (newIconNum + 1).ToString("0000") + "\",\r\n    \"mask\": 0,\r\n    \"src\": \"" + textureName + "\"\r\n  },\r\n  \"time\": " + newGameTime.ToString() + ",\r\n  \"type\": 2\r\n}";
            JToken newboxFrontIcon = JToken.Parse(boxFrontIconJson);

            JToken boxFrontChildrenFrameList = boxFront["layer"][0]["children"][1]["frameList"];
            boxFrontChildrenFrameList[boxFrontChildrenFrameList.Count() - 1].AddBeforeSelf(newboxFrontIcon);

            if (newGameTime >= (int)boxFrontChildrenFrameList.Last()["time"])
            {
                boxFrontChildrenFrameList[boxFrontChildrenFrameList.Count() - 1]["time"] = newGameTime + 1;
            }

            //Update Side icon list
            string sideTag = $"side{(artRegion == "jp" ? "" : $"_{artRegion}")}";
            JToken boxside = texturesObject["object"]["pkgData"]["motion"][sideTag];
            JToken boxsideFramelist = boxside["layer"][0]["frameList"];
            if (newGameTime >= (int)boxside["lastTime"])
            {
                boxside["lastTime"] = newGameTime + 1;
                boxsideFramelist[boxsideFramelist.Count() - 1]["time"] = newGameTime + 1;
            }

            texturesObject["object"]["pkgData"]["motion"][sideTag]["parameter"][0]["division"] = newGameTime;
            texturesObject["object"]["pkgData"]["motion"][sideTag]["parameter"][0]["rangeEnd"] = newGameTime;
            texturesObject["object"]["pkgData"]["motion"][sideTag]["priority"][1]["time"] = newGameTime + 1;

            string boxsideIconJson = "{\r\n  \"content\": {\r\n    \"icon\": \"" + newIconNum.ToString("0000") + "\",\r\n    \"mask\": 0,\r\n    \"src\": \"" + textureName + "\"\r\n  },\r\n  \"time\": " + newGameTime.ToString() + ",\r\n  \"type\": 2\r\n}";
            JToken newboxsideIcon = JToken.Parse(boxsideIconJson);

            JToken boxsideChildrenFrameList = boxside["layer"][0]["children"][1]["frameList"];
            boxsideChildrenFrameList[boxsideChildrenFrameList.Count() - 1].AddBeforeSelf(newboxsideIcon);

            if (newGameTime >= (int)boxsideChildrenFrameList.Last()["time"])
            {
                boxsideChildrenFrameList[boxsideChildrenFrameList.Count() - 1]["time"] = newGameTime + 1;
            }
        }

        public static void DeleteGameFromTextureObject(int gameTime, JObject texturesObject, string artRegion,
                                                       string basePath, string dev_id, string sysRegion, MArchivePacker packer, List<string> touchedFiles)
        {
            //Update Genre icon list
            JToken bigGenre = texturesObject["object"]["pkgData"]["motion"]["big_genre"];
            JToken bigGenreFramelist = bigGenre["layer"][0]["frameList"];
            JToken bigGenreEntry = bigGenreFramelist.Where(c => c["time"].ToString().Equals(gameTime.ToString())).FirstOrDefault();
            bigGenreEntry.Remove();

            //Update pnum icon list
            JToken bigpPnum = texturesObject["object"]["pkgData"]["motion"]["big_pnum"];
            JToken bigpPnumFramelist = bigpPnum["layer"][0]["frameList"];
            JToken bigPnumEntry = bigpPnumFramelist.Where(c => c["time"].ToString().Equals(gameTime.ToString())).FirstOrDefault();
            bigPnumEntry.Remove();

            //Update Front icon list
            string frontTag = $"front{(artRegion == "jp" ? "" : $"_{artRegion}")}";
            JToken boxFront = texturesObject["object"]["pkgData"]["motion"][frontTag];
            JToken boxFrontFramelist = boxFront["layer"][0]["frameList"];
            JToken boxFrontChildrenFrameList = boxFront["layer"][0]["children"][1]["frameList"];
            JToken boxFrontEntry = boxFrontChildrenFrameList.Where(c => c["time"].ToString().Equals(gameTime.ToString())).FirstOrDefault();
            boxFrontEntry.Remove();

            //Update Side icon list
            string sideTag = $"side{(artRegion == "jp" ? "" : $"_{artRegion}")}";
            JToken boxside = texturesObject["object"]["pkgData"]["motion"][sideTag];
            JToken boxsideFramelist = boxside["layer"][0]["frameList"];
            JToken boxsideChildrenFrameList = boxside["layer"][0]["children"][1]["frameList"];
            JToken boxsideEntry = boxsideChildrenFrameList.Where(c => c["time"].ToString().Equals(gameTime.ToString())).FirstOrDefault();
            boxsideEntry.Remove();

            //Remove texture object
            string boxTextureName = boxFrontEntry["content"]["src"].ToString();
            JToken boxTexture = texturesObject["source"][boxTextureName];
            string streamName = boxTexture["texture"]["pixel"].ToString();
            string streamFileName = streamName.Replace("_", "").Replace(":", "_");
            bool boxTextureRemoved = ((JObject)texturesObject["source"]).Remove(boxTextureName);

            string texturesJsonPath = PrepareAccess($@"{basePath}\{dev_id}\motion\{sysRegion}_titleselect_{artRegion}.psb.m", packer, touchedFiles);
            if (texturesObject["source"].Children().Children().Where(c => c["texture"]["pixel"].ToString().Equals(streamName)).Count().Equals(0))
            {
                File.Delete($@"{basePath}\{dev_id}\motion\{sysRegion}_titleselect_{artRegion}.psb.streams\{streamFileName}");
            }
            File.WriteAllText(texturesJsonPath, texturesObject.ToString());
        }

        public static string DeleteGameFromTitlesObject(int gameIndex, int gamesCount,
                                                        JObject texturesObject, JObject titlesObject, string titlesJsonPath,
                                                        MArchivePacker packer, List<string> touchedFiles)
        {
            JToken game = titlesObject["items"][gameIndex];

            string romCode = game["regionTag"].ToString();

            titlesObject["items"][(gamesCount * 7) + gameIndex].Remove();
            titlesObject["items"][(gamesCount * 6) + gameIndex].Remove();
            titlesObject["items"][(gamesCount * 5) + gameIndex].Remove();
            titlesObject["items"][(gamesCount * 4) + gameIndex].Remove();
            titlesObject["items"][(gamesCount * 3) + gameIndex].Remove();
            titlesObject["items"][(gamesCount * 2) + gameIndex].Remove();
            titlesObject["items"][(gamesCount * 1) + gameIndex].Remove();
            titlesObject["items"][(gamesCount * 0) + gameIndex].Remove();

            //Fix sorting for all 8 languages 😩
            for (int lang = 0; lang < 8; lang++)
            {
                //Get sublist of that language only
                List<JToken> games = new List<JToken>();
                for (int i = (lang * (gamesCount - 1)); i < ((lang + 1) * (gamesCount - 1)); i++)
                {
                    games.Add(titlesObject["items"][i]);
                }


                //Sort by name
                int sortNum = 0;
                games.Where(c => !c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                             .OrderBy(c => c["tname"].ToString()).ToList()
                                             .ForEach(c => c["sor_name"] = sortNum++);


                //Sort by release date, name
                sortNum = 0;
                games.Where(c => !c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                             .OrderBy(c => GetReleaseDate(c["desc"].ToString()))
                                             .ThenBy(c => c["tname"].ToString()).ToList()
                                             .ForEach(c => c["sor_date"] = sortNum++);

                //Sort by genre, name
                sortNum = 0;
                games.Where(c => !c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                             .OrderBy(c => GetGenre((int)c["image"], texturesObject))
                                             .ThenBy(c => c["tname"].ToString()).ToList()
                                             .ForEach(c => c["sor_genr"] = sortNum++);

                //Sort by player number, name
                sortNum = 0;
                games.Where(c => !c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                             .OrderBy(c => GetPlayerNum((int)c["image"], texturesObject))
                                             .ThenBy(c => c["tname"].ToString()).ToList()
                                             .ForEach(c => c["sor_pnum"] = sortNum++);
            }

            //set all other empty title games to the highest sort number.
            titlesObject["items"].Where(c => c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                 .ToList().ForEach(c => c["sor_date"] =
                                                        c["sor_demo"] =
                                                        c["sor_genr"] =
                                                        c["sor_name"] =
                                                        c["sor_pnum"] = (gamesCount - 2));

            PrepareAccess($@"{Path.GetDirectoryName(titlesJsonPath)}\{Path.GetFileNameWithoutExtension(titlesJsonPath)}.m", packer, touchedFiles);
            File.WriteAllText(titlesJsonPath, titlesObject.ToString());

            return romCode;
        }

        public static void DeleteGameFromRomsObject(string romCode, JObject romsObject, string basePath, string romsJsonPath,
                                              MArchivePacker packer, List<string> touchedFiles)
        {
            JToken romEntry = romsObject["root"]["m2epi"]["version"][romCode];
            string romName = romEntry["rom"].ToString();
            ((JObject)romsObject["root"]["game_versions"]).Remove(romCode);
            ((JObject)romsObject["root"]["m2epi"]["version"]).Remove(romCode);

            List<JToken> gvs = new List<JToken>();
            foreach (var gv in romsObject["root"]["game_versions"].Children().Children())
            {
                gvs.Add(gv);
            }
            int sortNum = 0;
            gvs.OrderBy(c => (int)c[0]).ToList().ForEach(c => c[0] = sortNum++);


            if (File.Exists($@"{basePath}\system\{romName.Replace("/", "\\")}"))
            {
                File.Delete($@"{basePath}\system\{romName.Replace("/", "\\")}");
            }
            else
            {
                File.Delete($@"{basePath}\system\{romName.Replace("/", "\\")}.m");
            }
            PrepareAccess($@"{Path.GetDirectoryName(romsJsonPath)}\{Path.GetFileNameWithoutExtension(romsJsonPath)}.m", packer, touchedFiles);
            File.WriteAllText(romsJsonPath, romsObject.ToString());
        }

        public static void UpdateTitlesObject(int gamesCount, JObject titlesObject, JObject texturesObject, string titlesJsonPath,
                                              MArchivePacker packer, List<string> touchedFiles)
        {
            //Fix sorting for all 8 languages 😩
            for (int lang = 0; lang < 8; lang++)
            {
                //Get sublist of that language only
                List<JToken> games = new List<JToken>();
                for (int i = (lang * gamesCount); i < ((lang + 1) * gamesCount); i++)
                {
                    games.Add(titlesObject["items"][i]);
                }


                //Sort by name
                int sortNum = 0;
                games.Where(c => !c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                             .OrderBy(c => c["tname"].ToString()).ToList()
                                             .ForEach(c => c["sor_name"] = sortNum++);


                //Sort by release date, name
                sortNum = 0;
                games.Where(c => !c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                             .OrderBy(c => GetReleaseDate(c["desc"].ToString()))
                                             .ThenBy(c => c["tname"].ToString()).ToList()
                                             .ForEach(c => c["sor_date"] = sortNum++);

                //Sort by genre, name
                sortNum = 0;
                games.Where(c => !c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                             .OrderBy(c => GetGenre((int)c["image"], texturesObject))
                                             .ThenBy(c => c["tname"].ToString()).ToList()
                                             .ForEach(c => c["sor_genr"] = sortNum++);

                //Sort by player number, name
                sortNum = 0;
                games.Where(c => !c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                             .OrderBy(c => GetPlayerNum((int)c["image"], texturesObject))
                                             .ThenBy(c => c["tname"].ToString()).ToList()
                                             .ForEach(c => c["sor_pnum"] = sortNum++);
            }

            //set all other empty title games to the highest sort number.
            titlesObject["items"].Where(c => c["regionTag"].ToString().Trim().ToUpper().Equals("DUMMY"))
                                 .ToList().ForEach(c => c["sor_date"] =
                                                        c["sor_demo"] =
                                                        c["sor_genr"] =
                                                        c["sor_name"] =
                                                        c["sor_pnum"] = (gamesCount - 1));

            PrepareAccess($@"{Path.GetDirectoryName(titlesJsonPath)}\{Path.GetFileNameWithoutExtension(titlesJsonPath)}.m", packer, touchedFiles);
            File.WriteAllText(titlesJsonPath, titlesObject.ToString());
        }

        public static void ModeTitleSelectNutPatch(string basePath, MArchivePacker packer, List<string> touchedFiles)
        {
            string modeTitleSelectNutPath = PrepareAccess($@"{basePath}\system\script\mode_title_select.nut.m", packer, touchedFiles);
            File.WriteAllBytes(modeTitleSelectNutPath, Properties.Resources.script_mode_title_select);
            StringReader modeTitleSelectNut = new StringReader(File.ReadAllText(modeTitleSelectNutPath));
            StringBuilder newNutScript = new StringBuilder();

            //skip first 46 lines
            for (int i = 1; i < 47; i++)
            {
                newNutScript.AppendLine(modeTitleSelectNut.ReadLine());
            }

            newNutScript.AppendLine("const MAX_TITLES = 50;");
            newNutScript.AppendLine();

            //skip another block
            for (int i = 47; i < 149; i++)
            {
                newNutScript.AppendLine(modeTitleSelectNut.ReadLine());
            }

            newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace("50", "MAX_TITLES"));
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine());
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace("49", "MAX_TITLES - 1"));

            //skip another block
            for (int i = 152; i < 496; i++)
            {
                newNutScript.AppendLine(modeTitleSelectNut.ReadLine());
            }

            for (int i = 496; i < 518; i++)
            {
                newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace("50", "MAX_TITLES"));
            }

            //skip another block
            for (int i = 518; i < 774; i++)
            {
                newNutScript.AppendLine(modeTitleSelectNut.ReadLine());
            }

            newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace("50", "MAX_TITLES"));

            //skip another block
            for (int i = 775; i < 799; i++)
            {
                newNutScript.AppendLine(modeTitleSelectNut.ReadLine());
            }


            //Read until the end
            while (true)
            {
                string nextLine = modeTitleSelectNut.ReadLine();
                if (nextLine == null)
                {
                    break;
                }

                newNutScript.AppendLine(nextLine);
            }

            string updatedNutScript = newNutScript.ToString().Replace("\r\n", "\n");
            File.WriteAllText(modeTitleSelectNutPath, updatedNutScript);
        }

        public static void SysDataStructPatch(int fromValue, int toValue, string basePath, MArchivePacker packer, List<string> touchedFiles)
        {
            string sysDataPath = PrepareAccess($@"{basePath}\system\config\struct_systemdata.psb.m", packer, touchedFiles);
            string sysData = File.ReadAllText(sysDataPath);
            sysData = sysData.Replace($"{fromValue}", $"{toValue}");
            File.WriteAllText(sysDataPath, sysData);

            string sysDataTitlePath = PrepareAccess($@"{basePath}\system\config\struct_systemdata_title.psb.m", packer, touchedFiles);
            string sysDataTitle = File.ReadAllText(sysDataTitlePath);
            sysDataTitle = sysDataTitle.Replace($"{fromValue}", $"{toValue}");
            File.WriteAllText(sysDataTitlePath, sysDataTitle);

            string sysDataTitlesPath = PrepareAccess($@"{basePath}\system\config\struct_systemdata_titles.psb.m", packer, touchedFiles);
            string sysDataTitles = File.ReadAllText(sysDataTitlesPath);
            sysDataTitles = sysDataTitles.Replace($"{fromValue}", $"{toValue}");
            File.WriteAllText(sysDataTitlesPath, sysDataTitles);
        }

        public static void PlayStandalonePatch(string basePath, MArchivePacker packer, List<string> touchedFiles)
        {
            string scriptPath = PrepareAccess($@"{basePath}\system\script\play_standalone.nut.m", packer, touchedFiles);
            StreamReader scriptText = new StreamReader(File.OpenRead(scriptPath));

            StringBuilder newText = new StringBuilder();
            string line = string.Empty;
            while (!line.Trim().Equals("::g_emu_task.setStateCheckControl(network_replay_control);"))
            {
                line = scriptText.ReadLine();
                newText.AppendLine(line);
            }
            newText.AppendLine(scriptText.ReadLine()); //read }
            newText.AppendLine(scriptText.ReadLine()); //read empty line
            newText.AppendLine("  // backupのstructと処理を流用するよ");
            newText.AppendLine("  local backupStatedata = BackupStatedata(null, false); // エラー表示なし");
            newText.AppendLine("");
            newText.AppendLine("  // バイナリ書き出し（ファイル出力）");
            newText.AppendLine("  local struct_state = backupStatedata.get_struct();");
            newText.AppendLine("  struct_state.clear();");
            newText.AppendLine("");
            newText.AppendLine("  // struct内のheaderを設定する");
            newText.AppendLine("  backupStatedata.set_statedata_struct(struct_state);");
            newText.AppendLine("");
            newText.AppendLine("  save_state_data(99999);");
            newText.AppendLine("");

            newText.AppendLine(scriptText.ReadToEnd());
            scriptText.Close();

            File.WriteAllText(scriptPath, newText.ToString());
        }

        public static void ModeTitleSelectNutAddUpdate(string basePath, MArchivePacker packer, List<string> touchedFiles, bool addGame = true)
        {
            int modifier = (addGame ? 1 : -1);
            string modeTitleSelectNutPath = PrepareAccess($@"{basePath}\system\script\mode_title_select.nut.m", packer, touchedFiles);
            StringReader modeTitleSelectNut = new StringReader(File.ReadAllText(modeTitleSelectNutPath));
            StringBuilder newNutScript = new StringBuilder();

            //skip first 46 lines
            for (int i = 1; i < 47; i++)
            {
                newNutScript.AppendLine(modeTitleSelectNut.ReadLine());
            }

            string maxTitlesConst = modeTitleSelectNut.ReadLine(); //47
            int maxTitles = Convert.ToInt32(maxTitlesConst.Substring(maxTitlesConst.LastIndexOf(" "),
                                                                     maxTitlesConst.LastIndexOf(";") - maxTitlesConst.LastIndexOf(" ")));

            newNutScript.AppendLine(maxTitlesConst.Replace(maxTitles.ToString(), (maxTitles + modifier).ToString()));

            for (int i = 48; i < 526; i++)
            {
                newNutScript.AppendLine(modeTitleSelectNut.ReadLine());
            }

            string titleNumJp = modeTitleSelectNut.ReadLine(); //526
            int titleNum = Convert.ToInt32(titleNumJp.Substring(titleNumJp.LastIndexOf(" "),
                                                                titleNumJp.LastIndexOf(";") - titleNumJp.LastIndexOf(" ")));

            newNutScript.AppendLine(titleNumJp.Replace(titleNum.ToString(), (titleNum + modifier).ToString()));
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine()); //527
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine()); //528

            string frontNumYjp = modeTitleSelectNut.ReadLine(); //529
            int frontNumY = Convert.ToInt32(frontNumYjp.Substring(frontNumYjp.LastIndexOf(" "),
                                                                  frontNumYjp.LastIndexOf(";") - frontNumYjp.LastIndexOf(" ")));

            int rowCountFront = ((titleNum + modifier) / 6) + (((titleNum + modifier) % 6) > 0 ? 1 : 0);
            newNutScript.AppendLine(frontNumYjp.Replace(frontNumY.ToString(), (rowCountFront).ToString()));
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine()); //530

            string sideNumYjp = modeTitleSelectNut.ReadLine(); //531
            int sideNumY = Convert.ToInt32(sideNumYjp.Substring(sideNumYjp.LastIndexOf(" "),
                                                                sideNumYjp.LastIndexOf(";") - sideNumYjp.LastIndexOf(" ")));

            int rowCountSide = ((titleNum + modifier) / 21) + (((titleNum + modifier) % 21) > 0 ? 1 : 0);
            newNutScript.AppendLine(sideNumYjp.Replace(sideNumY.ToString(), (rowCountSide).ToString()));

            newNutScript.AppendLine(modeTitleSelectNut.ReadLine()); //533
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace(titleNum.ToString(), (titleNum + modifier).ToString()));
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace(frontNumY.ToString(), (rowCountFront).ToString()));
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace(sideNumY.ToString(), (rowCountSide).ToString()));

            newNutScript.AppendLine(modeTitleSelectNut.ReadLine()); //537
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace(titleNum.ToString(), (titleNum + modifier).ToString()));
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace(frontNumY.ToString(), (rowCountFront).ToString()));
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace(sideNumY.ToString(), (rowCountSide).ToString()));

            newNutScript.AppendLine(modeTitleSelectNut.ReadLine()); //541
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace(titleNum.ToString(), (titleNum + modifier).ToString()));
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace(frontNumY.ToString(), (rowCountFront).ToString()));
            newNutScript.AppendLine(modeTitleSelectNut.ReadLine().Replace(sideNumY.ToString(), (rowCountSide).ToString()));

            //Read until the end
            while (true)
            {
                string nextLine = modeTitleSelectNut.ReadLine();
                if (nextLine == null)
                {
                    break;
                }

                newNutScript.AppendLine(nextLine);
            }

            string updatedNutScript = newNutScript.ToString().Replace("\r\n", "\n");
            File.WriteAllText(modeTitleSelectNutPath, updatedNutScript);
        }

        public static T Assign<T>(JToken source)
        {
            try
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType((source == null ? string.Empty : source.ToString()), typeof(T));
                }
                else if (typeof(T) == typeof(int))
                {
                    return (T)Convert.ChangeType((source == null ? 0 : (int)source), typeof(T));
                }
                else if (typeof(T) == typeof(DateTime))
                {
                    try
                    {
                        return (T)Convert.ChangeType((source == null ? DateTime.Today : (DateTime)source), typeof(T));
                    }
                    catch
                    {
                        return (T)Convert.ChangeType(DateTime.Today, typeof(T));
                    }
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        private static string PrepareAccess(string path, MArchivePacker packer, List<string> touchedFiles)
        {
            string clearPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
            if (!File.Exists(clearPath))
            {
                packer.DecompressFile(path, false);
            }

            if (clearPath.EndsWith(".psb"))
            {
                string jsonClearPath = Path.ChangeExtension(path, "json");
                if (!File.Exists(jsonClearPath))
                {
                    Utils.DumpPsb(clearPath, false);
                }

                if (!touchedFiles.Contains(jsonClearPath))
                {
                    touchedFiles.Add(jsonClearPath);
                }
                return jsonClearPath;
            }

            if (!touchedFiles.Contains(clearPath))
            {
                touchedFiles.Add(clearPath);
            }
            return clearPath;
        }
    }
}
