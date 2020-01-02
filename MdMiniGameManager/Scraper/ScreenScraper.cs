using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProjectLunarUI
{
    public class ScreenScraper : IScraper
    {
        Dictionary<string, int> genreMapping = new Dictionary<string, int>();
        Dictionary<string, int> playerNumMapping = new Dictionary<string, int>();

        public ScreenScraper()
        {
            genreMapping.Add("shooter", 0);
            genreMapping.Add("action", 1);
            genreMapping.Add("platform", 1);
            genreMapping.Add("action / adventure", 2);
            genreMapping.Add("adventure", 2);
            genreMapping.Add("fight", 3);
            genreMapping.Add("beat'em up", 3);
            genreMapping.Add("puzzle", 4);
            genreMapping.Add("puzzle-game", 4);
            genreMapping.Add("race, driving", 5);
            genreMapping.Add("sports", 5);
            genreMapping.Add("role playing games", 6);
            genreMapping.Add("strategy", 7);
            genreMapping.Add("shoot'em up", 8);
            genreMapping.Add("simulation", 8);
            genreMapping.Add("casino", 9);

            playerNumMapping.Add("1", 0);
            playerNumMapping.Add("1-2", 1);
            playerNumMapping.Add("1-3", 2);
            playerNumMapping.Add("1-4", 3);
            playerNumMapping.Add("1-5", 4);
            playerNumMapping.Add("4", 5);
            playerNumMapping.Add("8+", 5);
        }

        public bool CanUseHash => true;

        public string ProviderName => "ScreenScraper";

        public List<ScraperData> GetGameInformation(string gameName, GameSystems system)
        {
            throw new NotImplementedException();
        }

        public List<ScraperData> GetGameInformation(FileInfo romFile, GameSystems system, string romTitle = null)
        {
            string MD5 = GetMd5Hash(romFile.FullName);
            string SHA1 = GetSHA1(romFile.FullName);
            return GetGameInformation(MD5, SHA1, romFile.Name, system, romTitle);
        }

        public List<ScraperData> GetGameInformation(byte[] MD5, GameSystems system)
        {
            throw new NotImplementedException();
        }

        public List<ScraperData> GetGameInformation(string MD5, string SHA1, string romName, GameSystems system, string romTitle = null)
        {
            List<ScraperData> gamesList = new List<ScraperData>();
            string responseJson = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                string platform = string.Empty;
                switch (system)
                {
                    case GameSystems.SegaMegaDrive:
                        platform = "1";
                        break;
                    case GameSystems.SegaMasterSystem:
                        platform = "2";
                        break;
                    case GameSystems.Sega32X:
                        platform = "19";
                        break;
                    case GameSystems.SegaCD:
                        platform = "20";
                        break;
                }

                StringReader cdata = new StringReader(Encoding.Default.GetString(Properties.Resources.ss));
                string devId = cdata.ReadLine();
                string devPwd = cdata.ReadLine();
                string gamesDbRequest = $"https://www.screenscraper.fr/api/jeuInfos.php?devid={devId}&devpassword={devPwd}&softname=ProjectLunar1.0&output=json&md5={MD5}&sha1={SHA1}&systemeid={platform}";
                responseJson = httpClient.GetStringAsync(new Uri(gamesDbRequest)).Result;

                if (responseJson.ToLower().Contains("erreur") && !responseJson.ToLower().Contains("header") && !responseJson.ToLower().Contains("response"))
                {
                    //attempt to get by rom name
                    gamesDbRequest = $"https://www.screenscraper.fr/api/jeuInfos.php?devid={devId}&devpassword={devPwd}&softname=ProjectLunar1.0&output=json&romnom={romName}&systemeid={platform}";
                    responseJson = httpClient.GetStringAsync(new Uri(gamesDbRequest)).Result;
                }
                string gameName = string.Empty;
                if (responseJson.ToLower().Contains("erreur") && !responseJson.ToLower().Contains("header") && !responseJson.ToLower().Contains("response"))
                {
                    //attempt to distill game name
                    gameName = Path.GetFileNameWithoutExtension(romName).Replace("_", " ");
                    if (gameName.Contains("(") || gameName.Contains("("))
                    {
                        gameName = gameName.Substring(0, gameName.IndexOfAny(new char[] { '[', '(' }) - 1);
                    }

                    gamesDbRequest = $"https://www.screenscraper.fr/api/jeuInfos.php?devid={devId}&devpassword={devPwd}&softname=ProjectLunar1.0&output=json&romnom={gameName}&systemeid={platform}";
                    responseJson = httpClient.GetStringAsync(new Uri(gamesDbRequest)).Result;
                }
                if (responseJson.ToLower().Contains("erreur") && !responseJson.ToLower().Contains("header") && !responseJson.ToLower().Contains("response"))
                {
                    //Try using the typed title as a ROM name
                    gamesDbRequest = $"https://www.screenscraper.fr/api/jeuInfos.php?devid={devId}&devpassword={devPwd}&softname=ProjectLunar1.0&output=json&romnom={romTitle}&systemeid={platform}";
                    responseJson = httpClient.GetStringAsync(new Uri(gamesDbRequest)).Result;
                }

                if (responseJson.ToLower().Contains("erreur") && !responseJson.ToLower().Contains("header") && !responseJson.ToLower().Contains("response"))
                {
                    throw new AggregateException($"Could not find game information in the ScreenScraper database. \r\nYour ROM hash {MD5}, the ROM name \"{romName}\", the distilled ROM name \"{gameName}\", and ROM title \"{romTitle}\" don't match any records.", new Exception(responseJson));
                }

            }

            if (responseJson.Contains("The maximum threads allowed to leecher users is already used"))
            {
                throw new Exception($"ScreenScraper did not respond with game information.\r\nDetails: {responseJson}");
            }

            JObject scrapedGames = null;
            try
            {
                scrapedGames = JObject.Parse(responseJson);
            }
            catch (Exception ex)
            {
                throw new AggregateException("Could not find game information. Your ROM hash is not in the ScreenScraper database, or the ROM name doesn't match any records.", new Exception(responseJson), ex);
            }

            ScraperData gameEntry = new ScraperData();
            gameEntry.ScrapedGameData = scrapedGames;

            JToken game = scrapedGames["response"]["jeu"];
            string some = game.ToString();
            gameEntry.System = game["systemenom"].ToString();

            string name_Def = "nom_us";
            if (game["noms"]["nom_us"] == null)
            {
                if (game["noms"]["nom_wor"] == null)
                {
                    name_Def = "nom_ss";
                }
                else
                {
                    name_Def = "nom_wor";
                }
            }

            gameEntry.Name.Add("jp", (game["noms"]["nom_jp"] == null ? game["noms"][name_Def].ToString()
                                                                     : game["noms"]["nom_jp"].ToString()));
            gameEntry.Name.Add("en", (game["noms"]["nom_us"] == null ? game["noms"][name_Def].ToString()
                                                                     : game["noms"]["nom_us"].ToString()));
            gameEntry.Name.Add("fr", (game["noms"]["nom_eu"] == null ? game["noms"][name_Def].ToString()
                                                                     : game["noms"]["nom_eu"].ToString()));
            gameEntry.Name.Add("it", (game["noms"]["nom_eu"] == null ? game["noms"][name_Def].ToString()
                                                                     : game["noms"]["nom_eu"].ToString()));
            gameEntry.Name.Add("de", (game["noms"]["nom_eu"] == null ? game["noms"][name_Def].ToString()
                                                                     : game["noms"]["nom_eu"].ToString()));
            gameEntry.Name.Add("es", (game["noms"]["nom_eu"] == null ? game["noms"][name_Def].ToString()
                                                                     : game["noms"]["nom_eu"].ToString()));
            gameEntry.Name.Add("cn", (game["noms"]["nom_cn"] == null ? game["noms"][name_Def].ToString()
                                                                     : game["noms"]["nom_cn"].ToString()));
            gameEntry.Name.Add("kr", (game["noms"]["nom_kr"] == null ? game["noms"][name_Def].ToString()
                                                                     : game["noms"]["nom_kr"].ToString()));

            string default_descr = "synopsis_en";
            if (game["synopsis"]["synopsis_en"]==null)
            {
                default_descr = ((JProperty)game["synopsis"].First()).Name;
            }

            if (game["synopsis"] != null)
            {
                gameEntry.Description.Add("jp", (game["synopsis"]["synopsis_jp"] == null ? HttpUtility.HtmlDecode(game["synopsis"][default_descr].ToString())
                                                                                         : HttpUtility.HtmlDecode(game["synopsis"]["synopsis_jp"].ToString())));
                gameEntry.Description.Add("en", (game["synopsis"]["synopsis_en"] == null ? HttpUtility.HtmlDecode(game["synopsis"][default_descr].ToString())
                                                                                         : HttpUtility.HtmlDecode(game["synopsis"]["synopsis_en"].ToString())));
                gameEntry.Description.Add("fr", (game["synopsis"]["synopsis_fr"] == null ? HttpUtility.HtmlDecode(game["synopsis"][default_descr].ToString())
                                                                                         : HttpUtility.HtmlDecode(game["synopsis"]["synopsis_fr"].ToString())));
                gameEntry.Description.Add("it", (game["synopsis"]["synopsis_it"] == null ? HttpUtility.HtmlDecode(game["synopsis"][default_descr].ToString())
                                                                                         : HttpUtility.HtmlDecode(game["synopsis"]["synopsis_it"].ToString())));
                gameEntry.Description.Add("de", (game["synopsis"]["synopsis_de"] == null ? HttpUtility.HtmlDecode(game["synopsis"][default_descr].ToString())
                                                                                         : HttpUtility.HtmlDecode(game["synopsis"]["synopsis_de"].ToString())));
                gameEntry.Description.Add("es", (game["synopsis"]["synopsis_es"] == null ? HttpUtility.HtmlDecode(game["synopsis"][default_descr].ToString())
                                                                                         : HttpUtility.HtmlDecode(game["synopsis"]["synopsis_es"].ToString())));
                gameEntry.Description.Add("cn", (game["synopsis"]["synopsis_cn"] == null ? HttpUtility.HtmlDecode(game["synopsis"][default_descr].ToString())
                                                                                         : HttpUtility.HtmlDecode(game["synopsis"]["synopsis_cn"].ToString())));
                gameEntry.Description.Add("kr", (game["synopsis"]["synopsis_kr"] == null ? HttpUtility.HtmlDecode(game["synopsis"][default_descr].ToString())
                                                                                         : HttpUtility.HtmlDecode(game["synopsis"]["synopsis_kr"].ToString())));
            }
            else
            {
                gameEntry.Description.Add("jp", "No description available.");
                gameEntry.Description.Add("en", "No description available.");
                gameEntry.Description.Add("fr", "No description available.");
                gameEntry.Description.Add("it", "No description available.");
                gameEntry.Description.Add("de", "No description available.");
                gameEntry.Description.Add("es", "No description available.");
                gameEntry.Description.Add("cn", "No description available.");
                gameEntry.Description.Add("kr", "No description available.");
            }

            gameEntry.Copyright = (game["editeur"] == null ? string.Empty : game["editeur"].ToString());
            try
            {
                gameEntry.PlayerNum = playerNumMapping[Assign<string>(game["joueurs"])];
            }
            catch
            {
                gameEntry.PlayerNum = 0;
            }
            if (game["genres"] != null)
            {
                try
                {
                    gameEntry.Genre = genreMapping[Assign<string>(game["genres"]["genres_en"][0]).ToLower()];
                }
                catch
                {
                    gameEntry.Genre = 0;
                }
            }

            gameEntry.Region = "ss";
            gameEntry.ReleaseDate = GetReleaseDate(game, "jp");
            gamesList.Add(gameEntry);

            if (game["medias"]["media_boxs"] == null || game["medias"]["media_boxs"]["media_boxs2d"] == null)
            {
                return gamesList;
            }

            List<JToken> covers = game["medias"]["media_boxs"]["media_boxs2d"].Children()
                     .Where(c => ((JProperty)c).Name.Split('_').Count().Equals(3)).ToList();

            if (covers.Count > 1)
            {
                foreach (var cover in covers)
                {
                    string coverName = ((JProperty)cover).Name;
                    string coverRegion = coverName.Substring(coverName.LastIndexOf("_") + 1);
                    if (coverRegion.Equals("ss"))
                    {
                        continue;
                    }

                    gameEntry.ReleaseDate = GetReleaseDate(game, coverRegion);

                    gamesList.Add(new ScraperData()
                    {
                        Name = gameEntry.Name,
                        Description = gameEntry.Description,
                        Copyright = gameEntry.Copyright,
                        Genre = gameEntry.Genre,
                        PlayerNum = gameEntry.PlayerNum,
                        ReleaseDate = gameEntry.ReleaseDate,
                        ScrapedGameData = gameEntry.ScrapedGameData,
                        System = gameEntry.System,
                        Region = coverRegion
                    });
                }
            }

            return gamesList;
        }

        private static DateTime GetReleaseDate(JToken game, string region)
        {
            if (game["dates"] == null)
            {
                return DateTime.Now;
            }

            List<JToken> dates = game["dates"].Children().ToList();
            Dictionary<string, string> regionDate = new Dictionary<string, string>();
            foreach (var date in dates)
            {
                JProperty dateProp = (JProperty)date;
                regionDate.Add(dateProp.Name.Substring(dateProp.Name.LastIndexOf("_") + 1), dateProp.Value.ToString());
            }

            DateTime gameReleaseDate = new DateTime();
            string releaseDate = string.Empty;
            if (regionDate.Keys.Count > 0)
            {
                if (regionDate.ContainsKey(region))
                {
                    releaseDate = regionDate[region];
                }
                else
                {
                    releaseDate = regionDate.Values.First();
                }

                try
                {
                    gameReleaseDate = DateTime.Parse(releaseDate);
                }
                catch
                {
                    gameReleaseDate = new DateTime(int.Parse(releaseDate), 1, 1);
                }
            }
            else
            {
                gameReleaseDate = DateTime.Now;
            }

            return gameReleaseDate;
        }

        private T Assign<T>(JToken source)
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

        public override string ToString() => ProviderName;

        private string GetMd5Hash(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                return GetMd5Hash(stream);
            }
        }

        private string GetMd5Hash(Stream input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(input);

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        private string GetSHA1(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                return GetSHA1(stream);
            }
        }

        private string GetSHA1(Stream input)
        {
            SHA1 shaHash = new SHA1CryptoServiceProvider();
            byte[] data = shaHash.ComputeHash(input);

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public Bitmap GetGameImage(JObject gameData, string region, GameImageType imageType)
        {
            JToken game = gameData["response"]["jeu"];

            switch (imageType)
            {
                case GameImageType.CoverFront:
                    {
                        string boxRegion = GetBoxRegion(game, region, "media_boxs2d");
                        string coverURL = game["medias"]["media_boxs"]["media_boxs2d"][boxRegion]?.ToString();
                        return DownloadImage(coverURL);
                    }
                case GameImageType.CoverSpine:
                    {
                        string boxRegion = GetBoxRegion(game, region, "media_boxs2d-side");
                        string spineURL = game["medias"]["media_boxs"]["media_boxs2d-side"][boxRegion]?.ToString();
                        return DownloadImage(spineURL);
                    }
                //case GameImageType.CoverBack:
                //    string backCoverUrl = game["medias"]["media_wheels"]["media_wheel_wor"]?.ToString();
                //    return DownloadImage(backCoverUrl);
                case GameImageType.ClearLogo:
                    string logoURL = game["medias"]["media_wheels"]["media_wheel_wor"]?.ToString();
                    return DownloadImage(logoURL);
                //case GameImageType.TitleScreen:
                //    string titleURL = game["medias"]["media_wheels"]["media_wheel_wor"]?.ToString();
                //    return DownloadImage(titleURL);
                default:
                    return null;
            }
        }

        private static string GetBoxRegion(JToken game, string boxRegion, string boxImageType)
        {
            if (game["medias"]["media_boxs"][boxImageType][$"{boxImageType.Replace("boxs2d","box2d")}_{boxRegion}"] == null)
            {
                if (game["medias"]["media_boxs"][boxImageType].Count() > 0)
                {
                    boxRegion = ((JProperty)game["medias"]["media_boxs"][boxImageType].First()).Name;
                }
            }
            else
            {
                boxRegion = $"{boxImageType.Replace("boxs2d", "box2d")}_{boxRegion}";
            }

            return boxRegion;
        }

        private Bitmap DownloadImage(string url)
        {
            if (url != null)
            {
                try
                {
                    Stream imageData = null;
                    using (HttpClient httpClient = new HttpClient())
                    {
                        imageData = httpClient.GetStreamAsync(url).Result;
                    }

                    return new Bitmap(imageData);
                }
                catch { }
            }
            return null;
        }
    }
}
