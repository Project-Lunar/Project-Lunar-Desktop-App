using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectLunarUI
{
    public class TheGamesDB : IScraper
    {
        JObject allPublishers;
        Dictionary<int, int> genreMapping = new Dictionary<int, int>();

        public TheGamesDB()
        {
            allPublishers = JObject.Parse(Properties.Resources.PublisherJson);
            genreMapping.Add(1, 1);
            genreMapping.Add(2, 2);
            genreMapping.Add(3, 9);
            genreMapping.Add(4, 6);
            genreMapping.Add(5, 4);
            genreMapping.Add(6, 7);
            genreMapping.Add(7, 5);
            genreMapping.Add(8, 8);
            genreMapping.Add(9, 9);
            genreMapping.Add(10, 3);
            genreMapping.Add(11, 5);
            genreMapping.Add(12, 9);
            genreMapping.Add(13, 8);
            genreMapping.Add(14, 6);
            genreMapping.Add(15, 1);
            genreMapping.Add(16, 9);
            genreMapping.Add(17, 9);
            genreMapping.Add(18, 9);
            genreMapping.Add(19, 5);
        }

        public bool CanUseHash => false;

        public string ProviderName => "TheGamesDB";

        public List<ScraperData> GetGameInformation(string gameName, GameSystems system)
        {
            string apiKey = Encoding.ASCII.GetString(Properties.Resources.tgdb);
            List<ScraperData> gamesList = new List<ScraperData>();
            string responseJson = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                gameName = Uri.EscapeUriString(gameName);
                string platform = (system.Equals(GameSystems.SegaMasterSystem) ? "35" : "18%2C36");
                string gamesDbRequest = $"https://api.thegamesdb.net/Games/ByGameName?apikey={apiKey}&name={gameName}&fields=players%2Cpublishers%2Cgenres%2Coverview%2Cplatform&filter%5Bplatform%5D={platform}&include=boxart%2Cplatform";

                responseJson = httpClient.GetStringAsync(new Uri(gamesDbRequest)).Result;
            }
            //string responseJson = "{\"code\":200,\"status\":\"Success\",\"data\":{\"count\":7,\"games\":[{\"id\":691,\"game_title\":\"Sonic the Hedgehog 3\",\"release_date\":\"1994-02-02\",\"platform\":18,\"players\":2,\"overview\":\"Swing from vines, launch new attacks, survive deadly traps and summon Tails to airlift Sonic out of danger. Discover hidden rooms and passageways in the mega-sized Zones. Transform into Super Sonic and experience the ultimate in speed and ultra-sonic power. Save your progress using the new Game Save Feature.\",\"developers\":[7979],\"genres\":[2],\"publishers\":[15]},{\"id\":5569,\"game_title\":\"Sonic the Hedgehog 3\",\"release_date\":\"1994-02-02\",\"platform\":36,\"players\":2,\"overview\":\"In single player mode, the player can choose to play solo, as either Sonic or Tails, or as a team, controlling Sonic, with the AI controlling Tails, which is the default configuration. Another player may take control of Tails at any time by using a controller plugged into port 2. The object of the game is to progress through the levels. In order to completely finish the game, seven Chaos Emeralds must also be collected from the special stages.\",\"developers\":[7979],\"genres\":[15],\"publishers\":[15]},{\"id\":39170,\"game_title\":\"Sonic & Knuckles + Sonic the Hedgehog 3\",\"release_date\":\"1994-10-18\",\"platform\":36,\"players\":1,\"overview\":\"Dr. Eggmans (AKA Dr. Robotniks) Death Egg was once again blasted by Sonic, crash-landing on the peak of a volcano on the Floating Island.\\r\\n\\r\\nDr. Eggman is still at large, and Sonic cant allow him to get his hands on the Master Emerald and repair the Death Egg. Sonic must also keep Knuckles off his back but Knuckles has problems too. As guardian of the Floating Island and all the Emeralds, Knuckles must do his part to keep the island safe. While theyre going the rounds with each other, who will stop Dr. Eggman?\",\"developers\":[7549],\"genres\":[15],\"publishers\":[15]},{\"id\":114,\"game_title\":\"Sonic the Hedgehog\",\"release_date\":\"1991-06-23\",\"platform\":18,\"players\":1,\"overview\":\"Super Speed! Bust the video game speed barrier wide open with Sonic the Hedgehog. Blaze by in a blur using the super sonic spin attack. Loop the loop by defying gravity. Plummet down tunnels. Then dash to safety with Sonic's power sneakers. All at a frenzied pace. Super Graphics! Help Sonic escape bubbling molten lava. Swim through turbulent waterfalls. Scale glistening green mountains. And soar past shimmering city lights. There's even a 360 degree rotating maze. You've never seen anything like it. Supper Attitude! Sonic has an attitude that just won't quit. He's flip and funny, yet tough as nails as he fights to free his friends from evil. So just wait. Sonic may be the world's next SUPER hero...\",\"developers\":[7979],\"genres\":[1,15],\"publishers\":[15]},{\"id\":5544,\"game_title\":\"Sonic the Hedgehog\",\"release_date\":\"1991-06-23\",\"platform\":36,\"players\":1,\"overview\":\"When Dr. Eggman was hatching his plans for a global takeover, there was one little thing he didn't count on - Sonic The Hedgehog! Our blue hero zips, flips, and spins through the levels at lightning speed to collect the Chaos Emerald and restore World Order.\",\"developers\":[7549],\"genres\":[1,15],\"publishers\":[15]},{\"id\":142,\"game_title\":\"Sonic the Hedgehog 2\",\"release_date\":\"1992-11-21\",\"platform\":18,\"players\":2,\"overview\":\"Super Speed! Sonic's back and better than ever. He's a blur in blue! A blaze of action! With his new Super Spin Dash. And a new, fabulous friend, \\\"Tails\\\" the Fox. You won't believe it 'til you see it. And when you play, you won't stop. Super Play! Defy gravity in hair-raising loop-de-loops. Grab Power Sneakers and race like lightning through the mazes. Dash in a dizzying whirl across corkscrew speedway. Bounce like a pinball through the bumpers and springs of the amazing Zones. All at break-neck speed! Super Power! Sonic's attitude is can-do. The mad scientist Dr. Robotnik is planning a world takeover. Sonic gets tough in the fight to save his friends and squash Robotnik for good!\",\"developers\":[7979],\"genres\":[1,15],\"publishers\":[15]},{\"id\":7504,\"game_title\":\"Sonic the Hedgehog 2\",\"release_date\":\"1992-11-20\",\"platform\":36,\"players\":2,\"overview\":\"The gameplay of Sonic the Hedgehog 2 builds upon the basic set-up of the original Sonic the Hedgehog game. The player finishes each level, generally moving from left to right, within a time limit of 10 minutes.  Along the way, rings are collected and Badniks are defeated. Star posts serve as checkpoints, where if the player was to lose a life then he or she would return to one.  When the player has collected at least 50 rings, star posts can be run past for an optional Special Stage.  At the end of the last act of each zone (with the exception of Sky Chase Zone which does not have a boss), Sonic confronts Dr. Robotnik.\",\"developers\":[7549],\"genres\":[1,15],\"publishers\":[15]}]},\"include\":{\"boxart\":{\"base_url\":{\"original\":\"https:\\/\\/cdn.thegamesdb.net\\/images\\/original\\/\",\"small\":\"https:\\/\\/cdn.thegamesdb.net\\/images\\/small\\/\",\"thumb\":\"https:\\/\\/cdn.thegamesdb.net\\/images\\/thumb\\/\",\"cropped_center_thumb\":\"https:\\/\\/cdn.thegamesdb.net\\/images\\/cropped_center_thumb\\/\",\"medium\":\"https:\\/\\/cdn.thegamesdb.net\\/images\\/medium\\/\",\"large\":\"https:\\/\\/cdn.thegamesdb.net\\/images\\/large\\/\"},\"data\":{\"114\":[{\"id\":1447,\"type\":\"boxart\",\"side\":\"back\",\"filename\":\"boxart\\/back\\/114-1.jpg\",\"resolution\":\"903x1271\"},{\"id\":211263,\"type\":\"boxart\",\"side\":\"front\",\"filename\":\"boxart\\/front\\/114-1.jpg\",\"resolution\":null}],\"142\":[{\"id\":19386,\"type\":\"boxart\",\"side\":\"front\",\"filename\":\"boxart\\/front\\/142-2.jpg\",\"resolution\":\"1529x2100\"},{\"id\":19387,\"type\":\"boxart\",\"side\":\"back\",\"filename\":\"boxart\\/back\\/142-1.jpg\",\"resolution\":\"1530x2100\"}],\"691\":[{\"id\":8174,\"type\":\"boxart\",\"side\":\"front\",\"filename\":\"boxart\\/front\\/691-2.jpg\",\"resolution\":\"1443x2036\"},{\"id\":8177,\"type\":\"boxart\",\"side\":\"back\",\"filename\":\"boxart\\/back\\/691-2.jpg\",\"resolution\":\"1416x2036\"}],\"5544\":[{\"id\":14405,\"type\":\"boxart\",\"side\":\"back\",\"filename\":\"boxart\\/back\\/5544-1.jpg\",\"resolution\":\"741x1008\"},{\"id\":132939,\"type\":\"boxart\",\"side\":\"front\",\"filename\":\"boxart\\/front\\/5544-1.jpg\",\"resolution\":\"1445x2035\"}],\"5569\":[{\"id\":14402,\"type\":\"boxart\",\"side\":\"back\",\"filename\":\"boxart\\/back\\/5569-1.jpg\",\"resolution\":\"1354x1872\"},{\"id\":14404,\"type\":\"boxart\",\"side\":\"front\",\"filename\":\"boxart\\/front\\/5569-1.jpg\",\"resolution\":\"1353x1872\"}],\"7504\":[{\"id\":19389,\"type\":\"boxart\",\"side\":\"front\",\"filename\":\"boxart\\/front\\/7504-1.jpg\",\"resolution\":\"1529x2085\"},{\"id\":19391,\"type\":\"boxart\",\"side\":\"back\",\"filename\":\"boxart\\/back\\/7504-1.jpg\",\"resolution\":\"1530x2085\"}],\"39170\":[{\"id\":108372,\"type\":\"boxart\",\"side\":\"front\",\"filename\":\"boxart\\/front\\/39170-1.jpg\",\"resolution\":\"1435x1990\"}]}},\"platform\":{\"18\":{\"id\":18,\"name\":\"Sega Genesis\",\"alias\":\"sega-genesis\"},\"36\":{\"id\":36,\"name\":\"Sega Mega Drive\",\"alias\":\"sega-mega-drive\"}}},\"pages\":{\"previous\":null,\"current\":\"https:\\/\\/api.thegamesdb.net\\/Games\\/ByGameName?apikey={apiKey}&name=Sonic+the+Hedgehog+3&fields=players%2Cpublishers%2Cgenres%2Coverview%2Cplatform&filter%5Bplatform%5D=18%2C36&include=boxart%2Cplatform&page=1\",\"next\":null},\"remaining_monthly_allowance\":1496,\"extra_allowance\":0,\"allowance_refresh_timer\":2536674}";
            JObject scrapedGames = JObject.Parse(responseJson);

            List<string> lstScrapeItems = new List<string>();
            foreach (var game in scrapedGames["data"]["games"])
            {
                ScraperData gameEntry = new ScraperData();

                int platformNum = (int)game["platform"];
                string platformName = (platformNum == 18 ? "Genesis" : (platformNum == 36 ? "Mega Drive" : (platformNum == 35 ? "Master System" : "unknown")));
                lstScrapeItems.Add($"{game["game_title"].ToString()} ({platformName})");

                gameEntry.System = platformName;

                string title = game["game_title"].ToString();
                gameEntry.Name.Add("jp", title);
                gameEntry.Name.Add("en", title);
                gameEntry.Name.Add("fr", title);
                gameEntry.Name.Add("it", title);
                gameEntry.Name.Add("de", title);
                gameEntry.Name.Add("es", title);
                gameEntry.Name.Add("cn", title);
                gameEntry.Name.Add("kr", title);

                gameEntry.ReleaseDate = Assign<DateTime>(game["release_date"]);

                string desc = game["overview"].ToString();
                gameEntry.Description.Add("jp", desc);
                gameEntry.Description.Add("en", desc);
                gameEntry.Description.Add("fr", desc);
                gameEntry.Description.Add("it", desc);
                gameEntry.Description.Add("de", desc);
                gameEntry.Description.Add("es", desc);
                gameEntry.Description.Add("cn", desc);
                gameEntry.Description.Add("kr", desc);

                List<string> publisherNames = new List<string>();
                if (game["publishers"].Count() > 0)
                {
                    foreach (var publisher in game["publishers"])
                    {
                        publisherNames.Add(allPublishers["data"]["publishers"][publisher.ToString()]["name"].ToString().ToUpper());
                    }
                }
                gameEntry.Copyright = string.Join(",", publisherNames);

                string boxBaseUrl = Assign<string>(scrapedGames["include"]["boxart"]["base_url"]["thumb"]);
                string boxName = string.Empty;
                string gameId = Assign<string>(game["id"]);
                if (scrapedGames["include"]["boxart"]["data"][gameId] != null)
                {
                    boxName = Assign<string>(scrapedGames["include"]["boxart"]["data"][gameId]
                                                                .Where(c => c["side"].ToString().Equals("front"))
                                                                .FirstOrDefault()["filename"]);
                }

                int numPlayers = Assign<int>(game["players"]);
                gameEntry.PlayerNum = (numPlayers > 0 ? numPlayers - 1 : 0);
                if (!game["genres"].ToString().Equals(string.Empty))
                {
                    gameEntry.Genre = genreMapping[Assign<int>(game["genres"][0])];
                }

                if (boxName.Equals(string.Empty))
                {
                    continue;
                }

                try
                {
                    Stream boxArtData = null;
                    using (HttpClient httpClient = new HttpClient())
                    {
                        boxArtData = httpClient.GetStreamAsync($"{boxBaseUrl}{boxName}").Result;
                    }

                    gameEntry.CoverFront = new Bitmap(boxArtData);


                    Stream clearLogoData = null;
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string logoJson = httpClient.GetStringAsync($"https://api.thegamesdb.net/Games/Images?apikey={apiKey}&games_id={gameId}&filter%5Btype%5D=clearlogo").Result;
                        //string logoJson = "{\"code\":200,\"status\":\"Success\",\"data\":{\"count\":1,\"base_url\":{\"original\":\"https://cdn.thegamesdb.net/images/original/\",\"small\":\"https://cdn.thegamesdb.net/images/small/\",\"thumb\":\"https://cdn.thegamesdb.net/images/thumb/\",\"cropped_center_thumb\":\"https://cdn.thegamesdb.net/images/cropped_center_thumb/\",\"medium\":\"https://cdn.thegamesdb.net/images/medium/\",\"large\":\"https://cdn.thegamesdb.net/images/large/\"},\"images\":{\"691\":[{\"id\":45225,\"type\":\"clearlogo\",\"side\":null,\"filename\":\"clearlogo/691.png\",\"resolution\":\"400x159\"}]}},\"pages\":{\"previous\":null,\"current\":\"https://api.thegamesdb.net/Games/Images?apikey={apiKey}&games_id=691&filter%5Btype%5D=clearlogo&page=1\",\"next\":null},\"remaining_monthly_allowance\":1473,\"extra_allowance\":0,\"allowance_refresh_timer\":2441138}";

                        JObject logoObject = JObject.Parse(logoJson);

                        string spineBaseUrl = logoObject["data"]["base_url"]["original"].ToString();

                        if (logoObject["data"]["images"].Count() > 0)
                        {
                            string spineName = logoObject["data"]["images"][gameId][0]["filename"].ToString();
                            clearLogoData = httpClient.GetStreamAsync($"{spineBaseUrl}{spineName}").Result;
                        }
                    }

                    if (clearLogoData != null)
                    {
                        Bitmap logoBmp = new Bitmap(clearLogoData);
                    }
                }
                catch { }

                gamesList.Add(gameEntry);
            }

            if (gamesList.Count == 0)
            {
                throw new KeyNotFoundException("No games found. Try using less keywords.");
            }

            return gamesList;
        }

        public List<ScraperData> GetGameInformation(FileInfo romFile, GameSystems system, string romTitle = null)
        {
            return GetGameInformation(romFile.Name, system);
        }

        public List<ScraperData> GetGameInformation(byte[] MD5, GameSystems system)
        {
            throw new NotImplementedException();
        }

        public List<ScraperData> GetGameInformation(string MD5, string SHA1, string romName, GameSystems system, string romTitle = null)
        {
            throw new NotImplementedException();
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

        public Bitmap GetGameImage(JObject gameData, string region, GameImageType imageType)
        {
            throw new NotImplementedException();
        }
    }
}
