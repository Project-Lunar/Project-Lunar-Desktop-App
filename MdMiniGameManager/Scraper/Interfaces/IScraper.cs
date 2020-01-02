using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLunarUI
{
    public enum GameSystems
    {
        SegaMegaDrive,
        SegaMasterSystem,
        Sega32X,
        SegaCD
    }

    public enum GameImageType
    {
        CoverFront,
        CoverSpine,
        CoverBack,
        ClearLogo,
        TitleScreen
    }

    public interface IScraper
    {
        bool CanUseHash
        {
            get;
        }

        string ProviderName
        {
            get;
        }

        List<ScraperData> GetGameInformation(string gameName, GameSystems system);

        List<ScraperData> GetGameInformation(FileInfo romFile, GameSystems system, string gameTitle = null);

        List<ScraperData> GetGameInformation(byte[] MD5, GameSystems system);

        List<ScraperData> GetGameInformation(string MD5, string SHA1, string romName, GameSystems system, string gameTitle = null);

        Bitmap GetGameImage(JObject gameData, string region, GameImageType imageType);
    }
}
