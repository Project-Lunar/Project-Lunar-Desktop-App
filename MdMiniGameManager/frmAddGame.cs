using DarkUI.Forms;
using Newtonsoft.Json.Linq;
using ProjectLunarUI.M2engage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectLunarUI
{
    public partial class frmAddGame : DarkForm
    {
        JObject scrapedGames;
        JObject allPublishers;
        private string romPath;
        string romRegion = "U";
        private string sysRegion;
        private string initialScrapedDescription;
        GameSystems romSystem;

        public JObject RomsObject
        {
            get; set;
        }

        public JObject TitlesObject
        {
            get; set;
        }

        public JObject TexturesObject
        {
            get; set;
        }

        public Dictionary<string, JObject> OtherTexturesObject
        {
            get; set;
        }

        public Dictionary<string, Dictionary<string, Bitmap>> textureSources
        {
            get; set;
        }

        public ImageAttributes ImageAttributes
        {
            get; set;
        }

        public string SpecialRomCode
        {
            get; set;
        }

        public int DemoTime
        {
            get; set;
        }

        public string RomPath
        {
            get => romPath;
            set
            {
                romPath = value;
                txtTname.Text = Path.GetFileNameWithoutExtension(RomPath).Replace("_", " ");
                if (txtTname.Text.Contains("(") || txtTname.Text.Contains("("))
                {
                    txtTname.Text = txtTname.Text.Substring(0, txtTname.Text.IndexOfAny(new char[] { '[', '(' }) - 1);
                }

                byte[] romData = File.ReadAllBytes(RomPath);
                string systemName = Encoding.Default.GetString(romData, 0x100, 16);
                string regionCodes = Encoding.Default.GetString(romData, 0x1F0, 3);
                string smsHeader = Encoding.Default.GetString(romData, 0x7FF0, 8);
                bool smsGame = smsHeader.Equals("TMR SEGA");

                if (smsGame)
                {
                    romSystem = GameSystems.SegaMasterSystem;
                }
                else if (systemName.Contains("32X"))
                {
                    romSystem = GameSystems.Sega32X;
                }
                else
                {
                    romSystem = GameSystems.SegaMegaDrive;
                }
                
                

                if (regionCodes.Contains(SysRegion.ToUpper().Substring(0, 1)))
                {
                    romRegion = SysRegion.ToUpper().Substring(0, 1);
                }
                else
                {
                    if (regionCodes.Contains("J") || regionCodes.Contains("U") || regionCodes.Contains("E"))
                    {
                        romRegion = regionCodes.Substring(0, 1);
                    }
                }

                switch (romRegion)
                {
                    case "J":
                        rdoDetected.Text = $"Detected: Japan ({regionCodes.Trim()})";
                        break;
                    case "U":
                        rdoDetected.Text = $"Detected: USA ({regionCodes.Trim()})";
                        break;
                    case "E":
                        rdoDetected.Text = $"Detected: Europe ({regionCodes.Trim()})";
                        break;
                }
            }
        }

        public string RomName
        {
            get; set;
        }

        public Dictionary<string, int> StreamNumber
        {
            get; set;
        }

        public string ArtRegion
        {
            get; set;
        }

        public string SysRegion
        {
            get => sysRegion;
            set 
            {
                sysRegion = value;
                switch (sysRegion)
                {
                    case "as":
                    case "jp":
                        rdoJapan.Checked = true;
                        break;
                    case "eu":
                        rdoEuClassic.Checked = true;
                        break;
                    case "us":
                        rdoUsClassic.Checked = true;
                        break;
                }
            } 
        }

        public string dev_name
        {
            get; set;
        }

        public string basePath
        {
            get;
            set;
        }

        public string lunarPath
        {
            get;
            set;
        }

        public string sysPath
        {
            get;
            set;
        }

        public frmAddGame()
        {
            InitializeComponent();
            picBoxArt.Image = new Bitmap(152, 216);
            picSpine.Image = new Bitmap(30, 216);
            picNumPlayers.Image = new Bitmap(61, 48);
            picGenre.Image = new Bitmap(61, 48);
            allPublishers = JObject.Parse(Properties.Resources.PublisherJson);

            StreamNumber = new Dictionary<string, int>();
            StreamNumber.Add("eu", 0);
            StreamNumber.Add("jp", 0);
            StreamNumber.Add("us", 0);
        }

        private void FrmAddGame_Load(object sender, EventArgs e)
        {
            var type = typeof(IScraper);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                                               .SelectMany(s => s.GetTypes())
                                               .Where(p => type.IsAssignableFrom(p));

            foreach (Type scraperType in types)
            {
                if (!scraperType.IsAbstract)
                {
                    cboScraper.Items.Add(Activator.CreateInstance(scraperType));
                }
            }
            cboScraper.SelectedIndex = 0;

            string romName = Path.GetFileNameWithoutExtension(RomPath);
            if (File.Exists($@"{lunarPath}\IPS\{romName}.ips"))
            {
                chkApplyIPS.Enabled = true;
                chkApplyIPS.Checked = true;
            }

            CmdModMyClassicArt_Click(sender, e);
        }

        private void CmdScrape_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            IScraper scraper = (IScraper)cboScraper.SelectedItem;

            try
            {
                List<ScraperData> result = null;
                if (scraper.CanUseHash)
                {
                    result = scraper.GetGameInformation(new FileInfo(romPath), romSystem, txtTname.Text);
                }
                else
                {
                    result = scraper.GetGameInformation(txtTname.Text, romSystem);
                }
                lstScrapeItems.Items.Clear();
                lstScrapeItems.Items.AddRange(result.ToArray());
            }
            catch (KeyNotFoundException ex) 
            {
                SwingMessageBox.Show($"Error retrieving game data.\n{ex.Message}", "Scraper", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (AggregateException ex)
            {
                string allMessages = GetAllAggregateExceptionMessages(ex);
                SwingMessageBox.Show($"Error retrieving game data.\n{allMessages}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                SwingMessageBox.Show($"Error retrieving game data.\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
            

            this.Cursor = Cursors.Default;
        }

        private string GetAllAggregateExceptionMessages(AggregateException agEx)
        {
            StringBuilder allMessages = new StringBuilder();
            allMessages.AppendLine($"{agEx.Message}");
            foreach (var subEx in agEx.InnerExceptions)
            {
                allMessages.AppendLine($"{subEx.Message}");
                Exception innerEx = subEx.InnerException;
                while (innerEx != null)
                {
                    allMessages.AppendLine($"{innerEx.Message}");
                    innerEx = innerEx.InnerException;
                }
            }

            return allMessages.ToString();
        }

        private void LstScrapeItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstScrapeItems.SelectedIndex < 0)
            {
                return;
            }
            this.Cursor = Cursors.WaitCursor;

            ScraperData selectedGame = lstScrapeItems.SelectedItem as ScraperData;

            txtTname.Text = selectedGame.Name["en"];

            DateTime releaseDate = selectedGame.ReleaseDate;
            string overview = selectedGame.Description["en"];
            List<string> formattedDescriptionLines = Regex.Split($"Release Year: {releaseDate.Year}\r\n\r\n{Alldata.Assign<string>(overview)}",
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
            txtDescription.Text = initialScrapedDescription = strBuilder.ToString();
            txtCopyright.Text = selectedGame.Copyright;
            cboPlayerNum.SelectedIndex = selectedGame.PlayerNum;
            cboGenre.SelectedIndex = selectedGame.Genre;

            //if (selectedGame.CoverFront == null)
            //{
            //    this.Cursor = Cursors.Default;
            //    return;
            //}

            try
            {
                Bitmap boxArtBmp = null;
                if (selectedGame.CoverFront != null)
                {
                    boxArtBmp = selectedGame.CoverFront;
                }
                else
                {
                    IScraper scraper = (IScraper)cboScraper.SelectedItem;
                    boxArtBmp = scraper.GetGameImage(selectedGame.ScrapedGameData, selectedGame.Region, GameImageType.CoverFront);
                }
                if (boxArtBmp != null)
                {
                    using (Graphics gfx = Graphics.FromImage(picBoxArt.Image))
                    {
                        gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gfx.DrawImage(boxArtBmp, new Rectangle(0, 0, 152, 216), new Rectangle(0, 0, boxArtBmp.Width, boxArtBmp.Height), GraphicsUnit.Pixel);
                    }
                }

                Bitmap spineBmp = null;
                if (selectedGame.CoverSpine != null)
                {
                    spineBmp = selectedGame.CoverSpine;
                }
                else
                {
                    IScraper scraper = (IScraper)cboScraper.SelectedItem;
                    spineBmp = scraper.GetGameImage(selectedGame.ScrapedGameData, selectedGame.Region, GameImageType.CoverSpine);
                }
                if (spineBmp != null)
                {
                    using (Graphics gfx = Graphics.FromImage(picSpine.Image))
                    {
                        gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gfx.DrawImage(spineBmp, new Rectangle(0, 0, 30, 216), new Rectangle(0, 0, spineBmp.Width, spineBmp.Height), GraphicsUnit.Pixel);
                    }
                }
                else
                {
                    //attempt at building a spine :P
                    Bitmap spineTemplate = null;
                    if (rdoEuClassic.Checked)
                    {
                        spineTemplate = Properties.Resources.EUspine_Blank;
                    }
                    else if (rdoEuModern.Checked)
                    {
                        spineTemplate = Properties.Resources.EUspine2_Blank;
                    }
                    else if (rdoJapan.Checked)
                    {
                        spineTemplate = Properties.Resources.JPspine_Blank;
                    }
                    else if (rdoUsClassic.Checked)
                    {
                        spineTemplate = Properties.Resources.spineTemplateClassic;
                    }
                    else if (rdoUsModern.Checked)
                    {
                        spineTemplate = Properties.Resources.spineTemplate;
                    }

                    using (Graphics gfx = Graphics.FromImage(picSpine.Image))
                    {
                        gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gfx.DrawImage(spineTemplate, new Rectangle(0, 0, 30, 216), new Rectangle(0, 0, 30, 216), GraphicsUnit.Pixel);
                    }

                    Bitmap logoBmp = null;
                    if (selectedGame.ClearLogo != null)
                    {
                        logoBmp = selectedGame.ClearLogo;
                    }
                    else
                    {
                        IScraper scraper = (IScraper)cboScraper.SelectedItem;
                        logoBmp = scraper.GetGameImage(selectedGame.ScrapedGameData, selectedGame.Region, GameImageType.ClearLogo);
                    }
                    if (logoBmp !=null)
                    {
                        using (Graphics gfx = Graphics.FromImage(picSpine.Image))
                        {
                            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

                            double baseWidth = 26d;
                            double baseHeight = 95d;

                            logoBmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            int newWidth = (int)baseWidth;
                            int newHeight = (int)Math.Round((baseWidth / logoBmp.Width) * logoBmp.Height);
                            int posX = (picSpine.Image.Width - (int)baseWidth) / 2;
                            if (newHeight > baseHeight) //limiter to avoid overlapping logos
                            {
                                newWidth = (int)Math.Round((baseHeight / logoBmp.Height) * logoBmp.Width);
                                newHeight = (int)baseHeight;

                                posX = (int)(baseWidth - newWidth) / 2;
                            }
                            gfx.DrawImage(logoBmp, new Rectangle(posX, 25, newWidth, newHeight), new Rectangle(0, 0, logoBmp.Width, logoBmp.Height), GraphicsUnit.Pixel);
                        }
                    }
                    //else try writing something manually
                }
            }
            catch { }
            picBoxArt.Refresh();
            picSpine.Refresh();
            this.Cursor = Cursors.Default;
        }

        private void CmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CboPlayerNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSpecialIcon($"p{cboPlayerNum.SelectedIndex}", picNumPlayers);
        }

        private void LoadSpecialIcon(string iconCode, PictureBox pixtureBox)
        {
            string iconnNme = TexturesObject["object"]["icon"]["motion"]
                                        [iconCode]["layer"][0]
                                        ["frameList"][0]["content"]["icon"].ToString();
            string textureName = TexturesObject["object"]["icon"]["motion"]
                                               [iconCode]["layer"][0]
                                               ["frameList"][0]["content"]["src"].ToString();

            JToken icon = TexturesObject["source"][textureName]["icon"][iconnNme];

            using (Graphics gfx = Graphics.FromImage(pixtureBox.Image))
            {
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.DrawImage(textureSources[ArtRegion][textureName], new Rectangle(0, 0, 61, 48),
                                                           (int)icon["left"], (int)icon["top"], (int)icon["width"], (int)icon["height"],
                                                           GraphicsUnit.Pixel, ImageAttributes);

            }

            pixtureBox.Refresh();
        }

        private void CboGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGenre.SelectedIndex < 10)
            {
                LoadSpecialIcon($"g{cboGenre.SelectedIndex}", picGenre);
            }
            else
            {
                LoadSpecialIcon($"d0", picGenre);
            }
        }

        private void CmdLoadBox_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog().Equals(DialogResult.Cancel))
            {
                return;
            }

            Bitmap boxArtBmp = new Bitmap(openFileDialog.OpenFile());
            using (Graphics gfx = Graphics.FromImage(picBoxArt.Image))
            {
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.DrawImage(boxArtBmp, new Rectangle(0, 0, 152, 216), new Rectangle(0, 0, boxArtBmp.Width, boxArtBmp.Height), GraphicsUnit.Pixel);
            }
            picBoxArt.Refresh();

        }

        private void CmdLoadSpine_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog().Equals(DialogResult.Cancel))
            {
                return;
            }

            Bitmap spineArtBmp = new Bitmap(openFileDialog.OpenFile());
            using (Graphics gfx = Graphics.FromImage(picSpine.Image))
            {
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.DrawImage(spineArtBmp, new Rectangle(0, 0, 30, 216), new Rectangle(0, 0, spineArtBmp.Width, spineArtBmp.Height), GraphicsUnit.Pixel);
            }
            picSpine.Refresh();

        }

        public void CmdSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            //Lol. This bug.
            this.Enabled = false;

            string romName = Path.GetFileNameWithoutExtension(RomPath);
            if (File.Exists($@"{lunarPath}\IPS\{romName}.ips") && chkApplyIPS.Checked)
            {
                string newRomPath = $@"{lunarPath}\IPS\{romName}.bin";
                File.Copy(RomPath, newRomPath, true);
                ApplyIPS(newRomPath, File.ReadAllBytes($@"{lunarPath}\IPS\{romName}.ips"));
                romPath = newRomPath;
            }

            this.Cursor = Cursors.WaitCursor;
            int newGameTime = Math.Max(((int)TitlesObject["items"].Max(c => (int)c["image"])) + 1, 60);

            //Add to textures object
            Alldata.AddGameToTextureObject(newGameTime, TexturesObject, ArtRegion, cboGenre.SelectedIndex, cboPlayerNum.SelectedIndex,
                                           picBoxArt.Image, picSpine.Image, ImageAttributes, StreamNumber, textureSources);
            foreach (var otherTexture in OtherTexturesObject)
            {
                Alldata.AddGameToTextureObject(newGameTime, otherTexture.Value, otherTexture.Key, cboGenre.SelectedIndex, cboPlayerNum.SelectedIndex,
                                               picBoxArt.Image, picSpine.Image, ImageAttributes, StreamNumber, textureSources);
            }

            //Add game profile
            ScraperData gameData = (ScraperData)lstScrapeItems.SelectedItem;
            string romCode = string.Empty;
            romCode = Alldata.AddGameToTitlesObject(newGameTime, SpecialRomCode, TitlesObject, TexturesObject, txtTname.Text, txtDescription.Text, txtCopyright.Text, dev_name, SysRegion, DemoTime, gameData, romPath);

            //Add rom profile
            try
            {
                RomName = Alldata.AddGameToRomsObject(romCode, RomsObject, romPath, basePath, SysRegion, GetCountry(), chk6ButtonHack.Checked);
            }
            catch (ArgumentException)
            {
                SwingMessageBox.Show("Cannot add game. This version of the ROM already exists.", "Add new game",
                                     MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Abort;
                this.Close();
                return;
            }
            catch (InvalidDataException)
            {
                SwingMessageBox.Show("Cannot add more games. Maximum limit supported by the stock UI has been reached.", "Add new game",
                                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.DialogResult = DialogResult.Abort;
                this.Close();
                return;
            }
            catch { throw; }

            //All done here save all the stuff on the game manager Form
            this.Cursor = Cursors.Default;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private string GetCountry()
        {
            if (rdoDetected.Checked)
            {
                switch (romRegion.ToUpper())
                {
                    case "E":
                        return "eu";
                    case "J":
                        return "jp";
                    case "U":
                    default:
                        return "us";
                }
            }
            else if (rdoJ.Checked)
            {
                return "jp";
            }
            else if (rdoU.Checked)
            {
                return "us";
            }
            else
            {
                return "eu";
            }
        }

        private bool ValidateInput()
        {
            try
            {
                if (txtTname.Text.Trim().Equals(string.Empty))
                {
                    SwingMessageBox.Show("Game name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (lstScrapeItems.SelectedIndex >= 0)
                    {
                        ScraperData gameInfo = lstScrapeItems.SelectedItem as ScraperData;
                        if (!txtTname.Text.Equals(gameInfo.Name["en"]))
                        {
                            var result = SwingMessageBox.Show("You have edited the game title. Do you want to override all title entries for this game?",
                                               "Game Title Validation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                            if (result.Equals(DialogResult.Yes))
                            {
                                gameInfo.Name["jp"] = 
                                gameInfo.Name["en"] = 
                                gameInfo.Name["es"] = 
                                gameInfo.Name["fr"] = 
                                gameInfo.Name["it"] = 
                                gameInfo.Name["de"] = 
                                gameInfo.Name["cn"] = 
                                gameInfo.Name["kr"] = txtTname.Text;
                            }
                            else if (result.Equals(DialogResult.Cancel))
                            {
                                return false;
                            }
                        }
                    }
                }
                if (txtDescription.Text.Trim().Equals(string.Empty) || txtCopyright.Text.Equals(string.Empty)
                    || cboPlayerNum.SelectedIndex.Equals(-1) || cboGenre.SelectedIndex.Equals(-1))
                {
                    if (SwingMessageBox.Show("Some information is missing and will be set with default values, are you sure you want to continue?",
                                        "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
                    {
                        return false;
                    }

                    if (txtDescription.Text.Trim().Equals(string.Empty))
                    {
                        txtDescription.Text = "Release Year: 1988\r\n\r\nDescription not available.";
                    }
                    if (txtCopyright.Text.Equals(string.Empty))
                    {
                        txtCopyright.Text = "None";
                    }
                    if (cboPlayerNum.SelectedIndex.Equals(-1))
                    {
                        cboPlayerNum.SelectedIndex = 0;
                    }
                    if (cboGenre.SelectedIndex.Equals(-1))
                    {
                        cboGenre.SelectedIndex = 0;
                    }
                }
                if (lstScrapeItems.SelectedIndex >= 0)
                {
                    ScraperData gameInfo = lstScrapeItems.SelectedItem as ScraperData;
                    if (!txtDescription.Text.Equals(initialScrapedDescription))
                    {
                        var result = SwingMessageBox.Show("You have edited the game description. Do you want to override all description entries for this game?",
                                           "Game Description Validation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                        if (result.Equals(DialogResult.Yes))
                        {
                            gameInfo.Description["jp"] =
                            gameInfo.Description["en"] =
                            gameInfo.Description["es"] =
                            gameInfo.Description["fr"] =
                            gameInfo.Description["it"] =
                            gameInfo.Description["de"] =
                            gameInfo.Description["cn"] =
                            gameInfo.Description["kr"] = txtDescription.Text;
                        }
                        else if (result.Equals(DialogResult.Cancel))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CmdModMyClassicArt_Click(object sender, EventArgs e)
        {
            switch (ArtRegion)
            {
                case "eu":
                    picBoxArt.Image = Properties.Resources.cover_MMC_EU;
                    picSpine.Image = Properties.Resources.spine_MMC_EU;
                    break;
                case "jp":
                    picBoxArt.Image = Properties.Resources.cover_MMC_JP;
                    picSpine.Image = Properties.Resources.spine_MMC_JP;
                    break;
                case "us":
                    picBoxArt.Image = Properties.Resources.cover_MMC_US;
                    picSpine.Image = Properties.Resources.spine_MMC_US;
                    break;
            }
        }

        private void rdoEuClassic_CheckedChanged(object sender, EventArgs e)
        {
            if (lstScrapeItems.SelectedIndex >= 0)
            {
                LstScrapeItems_SelectedIndexChanged(sender, e);
            }
        }

        private void cboScraper_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstScrapeItems.Items.Clear();
        }

        private void txtDescription_Leave(object sender, EventArgs e)
        {
            List<string> formattedDescriptionLines = Regex.Split($"{Alldata.Assign<string>(txtDescription.Text)}",
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
            txtDescription.Text = strBuilder.ToString();
        }

        private void ApplyIPS(string romPath, byte[] ipsPatch)
        {
            BinaryWriter writer = new BinaryWriter(File.OpenWrite(romPath));
            BinaryReader patchReader = new BinaryReader(new MemoryStream(ipsPatch));

            List<IpsPatch> patchPackets = new List<IpsPatch>();
            patchReader.BaseStream.Position = 5;
            while (patchReader.BaseStream.Position < ipsPatch.Length - 3)
            {
                int offset = patchReader.ReadByte();
                offset = (offset << 8) | patchReader.ReadByte();
                offset = (offset << 8) | patchReader.ReadByte();

                int packetSize = patchReader.ReadByte();
                packetSize = (packetSize << 8) | patchReader.ReadByte();

                if (packetSize > 0)
                {
                    byte[] packetData = patchReader.ReadBytes(packetSize);

                    patchPackets.Add(new IpsPatch() { Offset = offset, Data = packetData });
                }
                else
                {
                    //Packet is RLE
                    int rleLength = patchReader.ReadByte();
                    rleLength = (rleLength << 8) | patchReader.ReadByte();

                    byte rleByte = patchReader.ReadByte();

                    byte[] packetData = Enumerable.Repeat(rleByte, rleLength).ToArray();

                    patchPackets.Add(new IpsPatch() { Offset = offset, Data = packetData });
                }
            }
            patchReader.Close();

            foreach (IpsPatch packet in patchPackets)
            {
                writer.Seek(packet.Offset, SeekOrigin.Begin);
                writer.Write(packet.Data);
            }

            writer.Close();

        }
    }

    public class IpsPatch
    {
        public int Offset
        {
            get; set;
        }

        public byte[] Data
        {
            get; set;
        }
    }
}
