using MArchiveBatchTool;
using MArchiveBatchTool.MArchive;
using MArchiveBatchTool.Psb;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Collections;
using DarkUI.Config;
using DarkUI.Controls;
using DarkUI.Docking;
using DarkUI.Forms;
using DarkUI.Renderers;
using ProjectLunarUI.M2engage;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace ProjectLunarUI
{
    public partial class frmGameManager : DarkForm
    {
        ImageAttributes imageAttr;
        JObject titlesObject;
        JObject romsObject;
        JObject texturesObject;
        Dictionary<string, Dictionary<string, Bitmap>> textureSources = new Dictionary<string, Dictionary<string, Bitmap>>();
        MArchivePacker packer = null;
        ConnectionInfo connInfo = null;
        string basePath;
        string lunarPath;
        private string sysPath;
        string artRegion;
        string sysRegion;
        string name_usa;
        string dev_name;
        string dev_id;
        bool dataLoadaed = false;
        bool delayedFSW = false;
        bool downloadFromConsole = false;
        bool updateChecked = false;
        bool warnedUsbUnmount = false;
        bool usbConnected = false;
        string titlesJsonPath;
        string romsJsonPath;
        string texturesJsonPath;

        string tnameState = string.Empty;
        string descState = string.Empty;
        string copyState = string.Empty;
        int playerNumState = -1;
        int genreState = -1;
        bool inLockOutEvent = false;

        private bool editLocked = true;

        FileSystemWatcher fsw = new FileSystemWatcher();
        List<string> lstChanged = new List<string>();
        List<string> lstCreated = new List<string>();
        List<string> lstDeleted = new List<string>();
        List<string> lstRenamed = new List<string>();

        List<string> touchedFiles = new List<string>();
        System.Timers.Timer timer = new System.Timers.Timer();

        public static readonly object tLock = new object();
        Dictionary<string, string> regionNames = new Dictionary<string, string>();

        frmLoading loadingForm = new frmLoading("");

        enum ControlLockType
        {
            ActionButtons,
            GameFields,
            AllControls
        }

        public frmGameManager()
        {
            InitializeComponent();
        }

        private void FrmGameManager_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " - Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // No game selected so lock controls
            LockControls(true, ControlLockType.GameFields);
            LockControls(true, cmdRemoveGame);

            picBoxArt.Image = new Bitmap(150, 214);
            picSpine.Image = new Bitmap(28, 214);
            picNumPlayers.Image = new Bitmap(61, 48);
            picGenre.Image = new Bitmap(61, 48);

            float[][] colorMatrix =
                {
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
                };
            imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(new ColorMatrix(colorMatrix));

            regionNames.Add("030", "System: Japan");
            regionNames.Add("031", "System: USA");
            regionNames.Add("032", "System: Europe");
            regionNames.Add("033", "System: Asia");

            lunarPath = Path.Combine(Application.StartupPath, @"lunar_data");

            dev_id = GetDevId();
            if (string.IsNullOrEmpty(dev_id))
            {
                if (Directory.Exists($@"{lunarPath}\console"))
                {
                    dev_id = new DirectoryInfo($@"{lunarPath}\console").GetDirectories().FirstOrDefault()?.Name;
                }
            }

            sysPath = $@"{lunarPath}\console\{dev_id}";
            basePath = $@"{sysPath}\alldata.psb_extracted";

            if (!string.IsNullOrEmpty(dev_id) && !Directory.Exists($@"{basePath}") && File.Exists($@"{sysPath}\alldata.bin") && File.Exists($@"{sysPath}\alldata.psb.m"))
            {
                delayedFSW = true;
            }
            else if (!Directory.Exists($@"{basePath}"))
            {
                using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
                {
                    try
                    {
                        ssh.Connect();
                    }
                    catch
                    {
                        SwingMessageBox.Show("This program hasn't performed an installation on a Mini yet. Please install the mod using this program before using the game manager.",
                                        "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                        return;
                    }

                    dev_id = GetDevId();
                    if (!Directory.Exists($@"{lunarPath}\Backup\{dev_id}"))
                    {
                        Directory.CreateDirectory($@"{lunarPath}\Backup\{dev_id}");
                    }
                    if (!Directory.Exists(sysPath))
                    {
                        Directory.CreateDirectory(sysPath);
                    }
                    using (ScpClient scp = new ScpClient("169.254.215.100", "root", "5A7213"))
                    {
                        scp.Connect();
                        scp.Download("/usr/game/m2engage", new FileInfo($@"{sysPath}\m2engage"));
                    }
                    delayedFSW = true;
                    downloadFromConsole = true;
                }
            }
            else
            {
                try
                {
                    SetupFSW();
                }
                catch
                {
                    SwingMessageBox.Show("This program hasn't performed an installation on a Mini yet. Please install the mod using this program before using the game manager.",
                                    "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }
            }

            lblImage.Text = string.Empty;
            cboRomSet.SelectedIndex = 0;

            if (!File.Exists($@"{sysPath}\m2engage"))
            {
                if (!Directory.Exists($@"{lunarPath}\Backup"))
                {
                    if (Directory.EnumerateDirectories($@"{lunarPath}\Backup\{dev_id}").Count() > 0)
                    {
                        string tempDevId = Path.GetFileName(Directory.EnumerateDirectories($@"{lunarPath}\Backup\{dev_id}").First());
                        if (!File.Exists($@"{sysPath}\m2engage") && File.Exists($@"{lunarPath}\Backup\{tempDevId}\m2engage"))
                        {
                            File.Copy($@"{lunarPath}\Backup\{tempDevId}\m2engage", $@"{sysPath}\m2engage");
                        }
                    }
                }
                else
                {
                    SwingMessageBox.Show("Essential data is missing. Please contact the support forums for assistance.",
                                         "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }
            }
            string psbKey = m2engage.FindPsbKey($@"{sysPath}\m2engage");
            packer = new MArchivePacker(new ZStandardCodec(), psbKey, 0x40);

            toolStripStatusLabel.Alignment = ToolStripItemAlignment.Right;

            KeyboardInteractiveAuthenticationMethod keyAuth = new KeyboardInteractiveAuthenticationMethod("root");
            keyAuth.AuthenticationPrompt += KeyAuth_AuthenticationPrompt;
            connInfo = new ConnectionInfo("169.254.215.100", "root", keyAuth);

            // Assume console not connected at this instance.
            cmdSaveChanges.Enabled = false;

            System.Timers.Timer tehTimer = new System.Timers.Timer();

            timer.Interval = 2000;
            timer.Elapsed += Timer_Elapsed;
        }

        private void SetupFSW()
        {
            fsw.Path = basePath;
            fsw.IncludeSubdirectories = true;
            fsw.Changed += Fsw_Changed;
            fsw.Created += Fsw_Created;
            fsw.Deleted += Fsw_Deleted;
            fsw.Renamed += Fsw_Renamed;
        }

        private void Fsw_Renamed(object sender, RenamedEventArgs e)
        {
            lstRenamed.Add(e.FullPath);
        }

        private void Fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            lstDeleted.Add(e.FullPath);
        }

        private void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            lstCreated.Add(e.FullPath);
        }

        private void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            lstChanged.Add(e.FullPath);
        }

        private void KeyAuth_AuthenticationPrompt(object sender, Renci.SshNet.Common.AuthenticationPromptEventArgs e)
        {
            foreach (var prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = "5A7213";
                }
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;
            try
            {
                using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
                {
                    ssh.Connect();
                    int freeSpace = Convert.ToInt32(ssh.RunCommand("df -m /rootfs_data | awk '{ print $3; }' | tail -n1").Result);
                    int maxSpace = Convert.ToInt32(ssh.RunCommand("df -m /rootfs_data | awk '{ print $1; }' | tail -n1").Result);

                    freeSpace -= 1;

                    string result = ssh.RunCommand("ls /media").Result;

                    if (result.Equals(string.Empty))
                    {
                        if (usbConnected && !warnedUsbUnmount)
                        {
                            Thread.Sleep(2000);
                            if (ssh.IsConnected)
                            {
                                SwingMessageBox.Show("Removing USB with the system connected is not recommended and can provoke loss of data. Initiating shutdown request to prevent data corruption!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                warnedUsbUnmount = true;
                                ssh.RunCommand("kill_ui_programs");
                                Thread.Sleep(500);
                                ssh.RunCommand("kill_ui_programs");
                                ssh.RunCommand("touch /tmp/.dontrestart");
                                ssh.RunCommand("shutdown-detection 2");
                            }
                            else
                            {
                                return;
                            }
                        }

                        lblMediaType.Invoke((MethodInvoker)delegate
                        {
                            lblMediaType.Text = "Media Type: NAND";
                        });
                        lblVolume.Invoke((MethodInvoker)delegate
                        {
                            lblVolume.Text = "Volume: DATA";
                        });
                    }
                    else
                    {
                        usbConnected = true;
                        lblMediaType.Invoke((MethodInvoker)delegate
                        {
                            lblMediaType.Text = "Media Type: USB";
                        });
                        lblVolume.Invoke((MethodInvoker)delegate
                        {
                            lblVolume.Text = "Volume: MEDIA";
                        });

                        ////Check if the USB drive is empty
                        //result = ssh.RunCommand("ls /media/project_lunar/m2engage/system/config/").Result;
                        //if (result.Equals(string.Empty))
                        //{
                        //    SwingMessageBox.Show("Inserting a new USB drive requires a full sync. Please wait while the USB drive is prepared.",
                        //                    "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    SyncGames(true);
                        //}
                    }

                    pbFreeSpace.Invoke((MethodInvoker)delegate
                    {
                        pbFreeSpace.Maximum = maxSpace;
                    });
                    pbFreeSpace.Invoke((MethodInvoker)delegate
                    {
                        pbFreeSpace.Value = maxSpace - freeSpace;
                    });

                    string freeUnit = "MB";
                    string maxUnit = "MB";
                    if (freeSpace >= 1024)
                    {
                        freeSpace /= 1024;
                        freeUnit = "GB";
                    }
                    if (maxSpace >= 1024)
                    {
                        maxSpace /= 1024;
                        maxUnit = "GB";
                    }

                    lblFreeSpace.Invoke((MethodInvoker)delegate
                    {
                        lblFreeSpace.Text = $@"Estimated free space: {freeSpace.ToString("N0")} {freeUnit} of {maxSpace.ToString("N0")} {maxUnit}";
                    });

                    if (inLockOutEvent == false)
                    {
                        cmdSaveChanges.Invoke((MethodInvoker)delegate
                        {
                            try
                            {
                                cboSystemRegion.Enabled = false;
                                if (cmdSaveChanges.Enabled == false)
                                {
                                    UpdateStatus("Console region changed. Reloading data...");
                                    string consoleDevId = GetDevId();
                                    if (!consoleDevId.Equals(dev_id))
                                    {
                                        ReloadRegion(consoleDevId);
                                    }
                                }
                                cmdSaveChanges.Enabled = true;
                                manageModsToolStripMenuItem.Enabled = true;
                                UpdateStatus("Connected");
                            }
                            catch
                            {
                                Console.WriteLine("Multi threading bug that I can't reproduce for the life of me just happened...");
                            }
                        });
                    }
                }
            }
            catch
            {
                usbConnected = false;
                if (lblMediaType.InvokeRequired)
                    lblMediaType.Invoke((MethodInvoker)delegate
                    {
                        lblMediaType.Text = "Media Type: Unknown";
                    });

                if (lblVolume.InvokeRequired)
                    lblVolume.Invoke((MethodInvoker)delegate
                    {
                        lblVolume.Text = "Volume: Unknown";
                    });

                if (lblVolume.InvokeRequired)
                    pbFreeSpace.Invoke((MethodInvoker)delegate
                    {
                        pbFreeSpace.Value = 0;
                    });

                if (lblVolume.InvokeRequired)
                    lblFreeSpace.Invoke((MethodInvoker)delegate
                    {
                        lblFreeSpace.Text = $@"Estimated free space: Disconnected";
                    });
                if (cmdSaveChanges.InvokeRequired)
                    cmdSaveChanges.Invoke((MethodInvoker)delegate
                    {
                        try
                        {
                            cmdSaveChanges.Enabled = false;
                            manageModsToolStripMenuItem.Enabled = false;
                            UpdateStatus("Disconnected");
                            cboSystemRegion.Enabled = true;
                        }
                        catch
                        {
                            Console.WriteLine("Multi threading bug that I can't reproduce for the life of me just happened...");
                        }
                    });
            }

            if (!updateChecked)
            {
                try
                {
                    CheckOnlineUpdates(false);
                    updateChecked = true;
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.ToString());
                }
            }

            timer.Enabled = true;
        }

        private void ReloadRegion(string consoleDevId)
        {
            dataLoadaed = false;
            dev_id = consoleDevId;
            sysPath = $@"{lunarPath}\console\{dev_id}";
            basePath = $@"{sysPath}\alldata.psb_extracted";
            if (!Directory.Exists(sysPath))
            {
                downloadFromConsole = true;
            }
            FrmGameManager_Shown(this, new EventArgs());
        }

        private void LoadRawBitmap(Bitmap texture, string imageFile)
        {
            byte[] imageData = File.ReadAllBytes(imageFile);
            BitmapData bmpData = texture.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), ImageLockMode.WriteOnly, texture.PixelFormat);
            Marshal.Copy(imageData, 0, bmpData.Scan0, imageData.Length);
            texture.UnlockBits(bmpData);
        }

        private void TreeGames_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode treeNode = e.Node;
            treeGames.SelectedNode = treeNode;
            if (treeNode.Nodes.Count > 0)
            {
                treeNode = treeNode.Nodes[1];
            }

            LoadGameInformation(treeNode);
        }

        private void LoadGameInformation(TreeNode treeNode)
        {
            int gamesCount = titlesObject["items"].Count() / 8;
            int gameNumber = (int)treeNode.Parent.Tag;
            var game = titlesObject["items"][((int)treeNode.Parent.Tag) + (treeNode.Index * gamesCount)];
            txtDescription.Text = game["desc"].ToString().Replace("\n", "\r\n");
            txtTname.Text = game["tname"].ToString().Replace("┓", "®");
            txtAction.Text = game["action"].ToString();
            txtCopy.Text = game["copy"]?.ToString().Replace("┏", "©").Replace("┓", "®");
            txtCsize.Text = game["csize"].ToString();
            txtDemoTime.Text = game["demo_time"].ToString();
            txtDevName.Text = game["dev_name"].ToString();
            txtName.Text = game["name"].ToString();
            txtRegionTag.Text = game["regionTag"].ToString();
            txtSorDate.Text = game["sor_date"].ToString();
            txtSorDemo.Text = game["sor_demo"].ToString();
            txtSorGenr.Text = game["sor_genr"].ToString();
            txtSorName.Text = game["sor_name"].ToString();
            txtSorPnum.Text = game["sor_pnum"].ToString();
            lblImage.Text = $"Debug: game.Image = {game["image"]}";
            cboGenre.SelectedIndex = GetGenre((int)game["image"]);
            cboPlayerNum.SelectedIndex = GetPlayerNum((int)game["image"]);

            if (gameNumber < 50)
            {
                // Don't enable edit controls on stock games for now
                editLocked = true;
                LockControls(false, ControlLockType.AllControls);
                LockControls(true, ControlLockType.GameFields);
                LockControls(true, cmdRemoveGame);
            }
            else
            {
                //Unlock the controls as it's a custom game
                editLocked = false;
                LockControls(false, ControlLockType.AllControls);
            }

            string frontTagName = string.Empty;
            string sideTagName = string.Empty;
            switch (artRegion)
            {
                case "us":
                case "eu":
                    frontTagName = $"front_{artRegion}";
                    sideTagName = $"side_{artRegion}";
                    break;
                case "jp":
                    frontTagName = "front";
                    sideTagName = "side";
                    break;
            }

            //Box Art front
            JToken icon = texturesObject["object"]["pkgData"]["motion"][frontTagName]["layer"][0]["children"][1]["frameList"]
                         .Where(c => c["time"].ToString().Equals(game["image"].ToString())).FirstOrDefault();
            if (!(icon == null) && !(icon["content"] == null))
            {
                try
                {
                    string textureKey = icon["content"]["src"].ToString();
                    Bitmap sourceTexture = textureSources[artRegion][textureKey];
                    JToken imageIcon = texturesObject["source"][textureKey]["icon"][icon["content"]["icon"].ToString()];

                    Graphics gfx = Graphics.FromImage(picBoxArt.Image);
                    gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gfx.DrawImage(sourceTexture, new Rectangle(0, 0, 150, 214), (int)imageIcon["left"], (int)imageIcon["top"], (int)imageIcon["width"], (int)imageIcon["height"], GraphicsUnit.Pixel, imageAttr);
                    gfx.Dispose();
                }
                catch
                {
                    picBoxArt.Image = new Bitmap(150, 214);
                }
            }
            else
            {
                picBoxArt.Image = new Bitmap(150, 214);
            }

            //spine
            JToken spineIcon = texturesObject["object"]["pkgData"]["motion"][sideTagName]["layer"][0]["children"][1]["frameList"]
                         .Where(c => c["time"].ToString().Equals(game["image"].ToString())).FirstOrDefault();
            if (!(spineIcon == null) && !(spineIcon["content"] == null))
            {
                string textureKey = spineIcon["content"]["src"].ToString();
                Bitmap sourceTexture = textureSources[artRegion][textureKey];
                JToken imageIcon = texturesObject["source"][textureKey]["icon"][spineIcon["content"]["icon"].ToString()];

                int SpineTop = (int)imageIcon["top"];
                int SpineLeft = (int)imageIcon["left"];
                int SpineWidth = (int)imageIcon["width"];
                int SpineHeight = (int)imageIcon["height"];

                Graphics gfx = Graphics.FromImage(picSpine.Image);
                if (SpineHeight > SpineWidth)
                {
                    gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gfx.DrawImage(sourceTexture, new Rectangle(0, 0, 28, 214), SpineLeft, SpineTop, SpineWidth, SpineHeight, GraphicsUnit.Pixel, imageAttr);
                }
                else
                {
                    Bitmap tempSpine = new Bitmap((int)SpineWidth, (int)SpineHeight);
                    Graphics tempGfx = Graphics.FromImage(tempSpine);
                    tempGfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    tempGfx.DrawImage(sourceTexture, new Rectangle(0, 0, SpineWidth, SpineHeight), SpineLeft, SpineTop, SpineWidth, SpineHeight, GraphicsUnit.Pixel, imageAttr);
                    tempGfx.Dispose();

                    tempSpine.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    gfx.DrawImage(tempSpine, new Rectangle(0, 0, 28, 214), 0, 0, SpineHeight, SpineWidth, GraphicsUnit.Pixel, new ImageAttributes());
                }
                gfx.Dispose();
            }
            else
            {
                picSpine.Image = new Bitmap(28, 214);
            }
            picBoxArt.Refresh();
            picSpine.Refresh();

            try
            {
                txtRom.Text = (string)romsObject["root"]["m2epi"]["version"][game["regionTag"].ToString()]["rom"];
            }
            catch
            {
                txtRom.Text = string.Empty;
            }

            //cmdRemoveGame.Enabled = true;
        }

        private int GetPlayerNum(int image)
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

        private int GetGenre(int image)
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

        private void TreeGames_KeyUp(object sender, KeyEventArgs e)
        {
            if (treeGames.SelectedNode == null)
            {
                return;
            }
            TreeNode treeNode = treeGames.SelectedNode;
            treeGames.SelectedNode = treeNode;
            if (treeNode.Nodes.Count > 0)
            {
                treeNode = treeNode.Nodes[1];
            }

            LoadGameInformation(treeNode);

            if (e.KeyCode.Equals(Keys.Delete))
            {
                if (cmdRemoveGame.Enabled)
                {
                    cmdRemoveGame_Click(sender, e);
                }
            }
        }

        private void CmdBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtDirectoryPath.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtDirectoryPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void CmdLoad_Click(object sender, EventArgs e)
        {
            artRegion = string.Empty;
            switch (cboRomSet.SelectedIndex)
            {
                case 0:
                    artRegion = "eu";
                    break;
                case 1:
                    artRegion = "jp";
                    break;
                case 2:
                    artRegion = "us";
                    break;
            }

            string systemJson = File.ReadAllText(PrepareAccess($@"{basePath}\system\config\system_prof.psb.m"));
            JObject systemObject = JObject.Parse(systemJson);
            dev_name = systemObject["root"]["dev_name"].ToString();
            JToken systemEntry = systemObject["root"]["title_list"]
                                                .Where(c => c["dev_name"].ToString().Equals(dev_name))
                                                .FirstOrDefault();
            name_usa = systemEntry["name"]["usa"].ToString();
            dev_id = ((int)systemEntry["dev_id"]).ToString("000");

            sysRegion = "jp";
            if (name_usa.Contains("USA"))
            {
                sysRegion = "us";
            }
            else if (name_usa.Contains("EUR"))
            {
                sysRegion = "eu";
            }
            else if (name_usa.Contains("ASIA"))
            {
                sysRegion = "as";
            }


            textureSources.Clear();
            string[] regionList = new string[] { "eu", "jp", "us" };
            foreach (string region in regionList)
            {
                string textJsonPath = PrepareAccess($@"{basePath}\{dev_id}\motion\{sysRegion}_titleselect_{region}.psb.m");
                string textJson = File.ReadAllText(textJsonPath);
                JObject textObject = JObject.Parse(textJson);

                Dictionary<string, Bitmap> textureSrc = new Dictionary<string, Bitmap>();
                foreach (var textureSource in textObject["source"])
                {
                    var textureData = textureSource.Children().FirstOrDefault();
                    Bitmap bmp = new Bitmap((int)textureData["texture"]["width"], (int)textureData["texture"]["height"]);
                    string streamName = textureData["texture"]["pixel"].ToString();
                    LoadRawBitmap(bmp, $@"{basePath}\{dev_id}\motion\{sysRegion}_titleselect_{region}.psb.streams\stream_{streamName.Substring(streamName.LastIndexOf(":") + 1)}");
                    string textureName = textureData.Path.Substring(textureData.Path.LastIndexOf(".") + 1);
                    textureSrc.Add(textureName, bmp);
                }
                textureSources.Add(region, textureSrc);
            }


            titlesJsonPath = PrepareAccess($@"{basePath}\{dev_id}\config\title_mode_top.psb.m");
            texturesJsonPath = PrepareAccess($@"{basePath}\{dev_id}\motion\{sysRegion}_titleselect_{artRegion}.psb.m");
            romsJsonPath = PrepareAccess($@"{basePath}\{dev_id}\config\title_prof.psb.m");
            string titlesJson = File.ReadAllText(titlesJsonPath);
            string texturesJson = File.ReadAllText(texturesJsonPath);
            string romsJson = File.ReadAllText(romsJsonPath);
            titlesObject = JObject.Parse(titlesJson);
            romsObject = JObject.Parse(romsJson);
            texturesObject = JObject.Parse(texturesJson);

            lblSystem.Text = $"Console Name: {name_usa}";

            if (!dataLoadaed)
            {
                treeGames.Nodes.Clear();
                List<TreeNode> allTitles = new List<TreeNode>();
                int gamesCount = titlesObject["items"].Count() / 8;
                for (int i = 0; i < gamesCount; i++)
                {
                    TreeNode gameNode = new TreeNode(GetGameName(titlesObject["items"][i + gamesCount]["tname"].ToString()));
                    gameNode.Nodes.Add($"(JP) {GetGameName(titlesObject["items"][i]["tname"].ToString())}");
                    gameNode.Nodes.Add($"(EN) {GetGameName(titlesObject["items"][i + (gamesCount * 1)]["tname"].ToString())}");
                    gameNode.Nodes.Add($"(ES) {GetGameName(titlesObject["items"][i + (gamesCount * 2)]["tname"].ToString())}");
                    gameNode.Nodes.Add($"(FR) {GetGameName(titlesObject["items"][i + (gamesCount * 3)]["tname"].ToString())}");
                    gameNode.Nodes.Add($"(IT) {GetGameName(titlesObject["items"][i + (gamesCount * 4)]["tname"].ToString())}");
                    gameNode.Nodes.Add($"(DE) {GetGameName(titlesObject["items"][i + (gamesCount * 5)]["tname"].ToString())}");
                    gameNode.Nodes.Add($"(CN) {GetGameName(titlesObject["items"][i + (gamesCount * 6)]["tname"].ToString())}");
                    gameNode.Nodes.Add($"(KO) {GetGameName(titlesObject["items"][i + (gamesCount * 7)]["tname"].ToString())}");
                    if (i >= 50)
                    {
                        gameNode.ForeColor = Color.FromArgb(0, 176, 244);
                    }
                    if (!gameNode.Text.Equals("SYSTEM RESERVED") && !titlesObject["items"][i]["regionTag"].ToString().Equals("RETROARCH____")
                                                                 && !titlesObject["items"][i]["regionTag"].ToString().Equals("BOOTMENU_____")
                                                                 && !titlesObject["items"][i]["regionTag"].ToString().Equals("DUMMY"))
                    {
                        gameNode.Tag = i;
                        allTitles.Add(gameNode);
                    }
                }
                treeGames.Nodes.AddRange(allTitles.OrderBy(c => c.Text).ToArray());
            }

            string utilsScriptPath = PrepareAccess($@"{basePath}\system\script\utils.nut.m");
            string utilsScript = File.ReadAllText(utilsScriptPath);

            if (utilsScript.Contains("::g_emu_task.smoothing  = scale_tbl[num][2] == 1 ? true : false;"))
            {
                restoreDefaultToolStripMenuItem.Checked = true;
                disableForScanlinesToolStripMenuItem.Checked = false;
                enableAlwaysToolStripMenuItem.Checked = false;
            }
            else if (utilsScript.Contains("::g_emu_task.smoothing  = true;"))
            {
                restoreDefaultToolStripMenuItem.Checked = false;
                enableAlwaysToolStripMenuItem.Checked = true;
                disableForScanlinesToolStripMenuItem.Checked = false;
            }
            else
            {
                restoreDefaultToolStripMenuItem.Checked = false;
                enableAlwaysToolStripMenuItem.Checked = false;
                disableForScanlinesToolStripMenuItem.Checked = true;
            }

            ReloadRegionsDropDown();

            PrepareAccess($@"{basePath}\system\config\mode_staff_text_as.psb.m");
            PrepareAccess($@"{basePath}\system\config\mode_staff_text_eu.psb.m");
            PrepareAccess($@"{basePath}\system\config\mode_staff_text_jp.psb.m");
            PrepareAccess($@"{basePath}\system\config\mode_staff_text_us.psb.m");
            PrepareAccess($@"{basePath}\system\font\mdmini_basefont.psb.m");
            PrepareAccess($@"{basePath}\system\script\mode_title_select.nut.m");

            PrepareAccess($@"{basePath}\system\config\struct_systemdata.psb.m");
            PrepareAccess($@"{basePath}\system\config\struct_systemdata_title.psb.m");
            PrepareAccess($@"{basePath}\system\config\struct_systemdata_titles.psb.m");
            PrepareAccess($@"{basePath}\system\script\play_standalone.nut.m");

            LockControls(false, cmdAddGame);
            dataLoadaed = true;
        }

        private void ReloadRegionsDropDown()
        {
            cboSystemRegion.Enabled = false;
            cboSystemRegion.Items.Clear();
            foreach (DirectoryInfo folder in new DirectoryInfo($@"{lunarPath}\console").GetDirectories())
            {
                cboSystemRegion.Items.Add(regionNames[folder.Name]);
            }
            cboSystemRegion.SelectedItem = regionNames[dev_id];
        }

        string GetGameName(string value)
        {
            if (value.Trim().Equals(string.Empty))
            {
                value = "SYSTEM RESERVED";
            }
            return value.Replace("┓", "®");
        }

        private void TxtDirectoryPath_TextChanged(object sender, EventArgs e)
        {
            basePath = txtDirectoryPath.Text;
        }

        private void CboRomSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataLoadaed)
            {
                CmdLoad_Click(sender, e);
                if (treeGames.SelectedNode != null)
                {
                    TreeGames_KeyUp(sender, new KeyEventArgs(Keys.Up));
                }
            }
        }

        private void DownloadStatus(ulong transferred)
        {
            //pbFreeSpace.Invoke((MethodInvoker)delegate
            //{
            //    pbFreeSpace.Value = (int)transferred;
            //});
        }

        private void DownloadFile(string source, string destination)
        {
            using (FileStream downloadFile = File.OpenWrite(destination))
            {
                using (SftpClient sftp = new SftpClient(connInfo))
                {
                    sftp.Connect();
                    sftp.DownloadFile(source, downloadFile, DownloadStatus);
                }
            }
        }

        private void UploadFile(string source, string destination)
        {
            using (FileStream uploadFile = File.OpenRead(source))
            {
                using (SftpClient sftp = new SftpClient(connInfo))
                {
                    sftp.Connect();
                    sftp.UploadFile(uploadFile, destination);
                }
            }
        }

        private void FrmGameManager_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            ShowLoadingBox("INITIALIZING LOCAL DATA");
            UpdateStatus("Initializing...");

            if (!Directory.Exists(lunarPath))
            {
                Directory.CreateDirectory(lunarPath);
            }

            if (downloadFromConsole)
            {
                UpdateStatus("Downloading local data from console...");
                dev_id = GetDevId();
                Directory.CreateDirectory(basePath);
                Directory.CreateDirectory($@"{basePath}\{dev_id}");
                Directory.CreateDirectory($@"{basePath}\system");
                using (ScpClient scp = new ScpClient("169.254.215.100", "root", "5A7213"))
                {
                    scp.Connect();
                    scp.Download($"/usr/game/{dev_id}/", new DirectoryInfo($@"{basePath}\{dev_id}"));
                    scp.Download($"/usr/game/system/", new DirectoryInfo($@"{basePath}\system"));
                    scp.Download($"/usr/game/m2engage", new DirectoryInfo($@"{sysPath}"));
                }
                downloadFromConsole = false;
            }
            else
            {
                //if (!File.Exists($@"{lunarPath}\alldata.bin"))
                //{
                //    //Download alldata files from the console
                //    UpdateStatus("Downloading original data...");
                //    try
                //    {
                //        DownloadFile("/usr/game/m2engage", $@"{sysPath}\m2engage");
                //        DownloadFile("/usr/game/alldata.psb.m", $@"{sysPath}\alldata.psb.m");
                //        DownloadFile("/usr/game/alldata.bin", $@"{sysPath}\alldata.bin");
                //    }
                //    catch (Exception ex)
                //    {
                //        SwingMessageBox.Show($"Unable to download files.\r\n{ex.ToString()}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        return;
                //    }
                //}
            }
            bool dataExtracted = false;
            touchedFiles.Clear();
            if (!Directory.Exists($@"{sysPath}\alldata.psb_extracted"))
            {
                UpdateStatus("Extracting original data...");

                //Unpack alldata.bin
                AllDataPacker.UnpackFiles($@"{sysPath}\alldata.psb.m", basePath, packer);
                BackupData();

                PrepareAccess($@"{basePath}\system\config\mode_staff_text_as.psb.m");
                PrepareAccess($@"{basePath}\system\config\mode_staff_text_eu.psb.m");
                PrepareAccess($@"{basePath}\system\config\mode_staff_text_jp.psb.m");
                PrepareAccess($@"{basePath}\system\config\mode_staff_text_us.psb.m");
                PrepareAccess($@"{basePath}\system\font\mdmini_basefont.psb.m");
                DirectoryCopy($@"{lunarPath}\system\", $@"{basePath}\system\", true, true);

                Alldata.ModeTitleSelectNutPatch(basePath, packer, touchedFiles);
                Alldata.SysDataStructPatch(150, 255, basePath, packer, touchedFiles);
                Alldata.PlayStandalonePatch(basePath, packer, touchedFiles);

                string romPath = $@"{lunarPath}\bootmenu.bin";
                string gameName = "Project Lunar Boot Menu";
                string description = "Release year: 2019\r\n\r\nReturns to the boot menu.";
                AddSpecialRom(romPath, gameName, description, dev_id, SpecialLunarRom.BootMenu);

                romPath = $@"{lunarPath}\retroarch.bin";
                gameName = "RetroArch";
                description = "Release year: 2019\r\n\r\nLaunches RetroArch.";
                AddSpecialRom(romPath, gameName, description, dev_id, SpecialLunarRom.RetroArch);

                foreach (string file in touchedFiles)
                {
                    PrepareForCompression(file);
                }
                packer.CompressDirectory(basePath);
                touchedFiles.Clear();

                dataExtracted = true;
            }

            if (delayedFSW)
            {
                SetupFSW();
                delayedFSW = false;
            }

            PrepareAccess($@"{basePath}\system\script\mode_title_select.nut.m");
            PrepareAccess($@"{basePath}\system\script\utils.nut.m");

            PrepareAccess($@"{basePath}\system\config\mode_staff_text_as.psb.m");
            PrepareAccess($@"{basePath}\system\config\mode_staff_text_eu.psb.m");
            PrepareAccess($@"{basePath}\system\config\mode_staff_text_jp.psb.m");
            PrepareAccess($@"{basePath}\system\config\mode_staff_text_us.psb.m");
            PrepareAccess($@"{basePath}\system\font\mdmini_basefont.psb.m");
            PrepareAccess($@"{basePath}\system\script\mode_title_select.nut.m");

            PrepareAccess($@"{basePath}\system\config\struct_systemdata.psb.m");
            PrepareAccess($@"{basePath}\system\config\struct_systemdata_title.psb.m");
            PrepareAccess($@"{basePath}\system\config\struct_systemdata_titles.psb.m");
            PrepareAccess($@"{basePath}\system\script\play_standalone.nut.m");
            //PrepareAccess($@"{basePath}\system\script\systemdata_window_info.nut.m");
            //PrepareAccess($@"{basePath}\system\config\struct_systemdata.psb.m");
            //PrepareAccess($@"{basePath}\system\config\struct_systemdata_title.psb.m");
            //PrepareAccess($@"{basePath}\system\config\struct_systemdata_titles.psb.m");

            CmdLoad_Click(this, e);
            bool initialSync = false;


            if (dataExtracted)
            {
                ///Initial opening of the gamemanager.
                SwingMessageBox.Show("Welcome to Project Lunar!\nThanks for installing! Before you continue, we would highly " +
                         "recommend that you backup your NAND backups to a safe location. You can do this by " +
                         "clicking:\nTools > Advanced > Export Backup\nfrom the tool menu above.", "Welcome!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (SwingMessageBox.Show("A sync is required to obtain accurate free-space information. \n" +
                                         "Do you want to perform this sync now?", "Initial Sync",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
                {
                    initialSync = true;
                    Task.Run(() => SyncGames());
                }
            }

            switch (sysRegion)
            {
                case "eu":
                    cboRomSet.SelectedIndex = 0;
                    break;
                case "jp":
                case "as":
                    cboRomSet.SelectedIndex = 1;
                    break;
                case "us":
                    cboRomSet.SelectedIndex = 2;
                    break;
            }

            CloseLoadingBox();

            using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
            {
                try
                {
                    ssh.Connect();
                    UpdateStatus("Connected");

                    // Console connected, activate sync
                    cmdSaveChanges.Enabled = true;
                    manageModsToolStripMenuItem.Enabled = true;
                }
                catch
                {
                    if (!initialSync)
                    {
                        SwingMessageBox.Show("Unable to retrieve info from console. Is your Mini plugged in?\n" +
                                             "Remember you just need to connect it normally without holding the RESET button!", "No connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UpdateStatus("Disconnected");
                    }

                    // Console connected, deactivate sync
                    cmdSaveChanges.Enabled = false;
                    manageModsToolStripMenuItem.Enabled = false;
                }
            }
            timer.Enabled = true;
        }

        private string GetDevId()
        {
            try
            {
                using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
                {
                    ssh.Connect();

                    string fileName = ssh.RunCommand("cat /version").Result.Replace("\n", "");
                    string systemRegion = fileName.Split('-')[5].Substring(4).ToLower();
                    return (systemRegion == "jp" ? "030" : (systemRegion == "us" ? "031" : (systemRegion == "eu" ? "032" : "033")));
                }
            }
            catch 
            {
                return null;
            }
            //string systemJson = File.ReadAllText(PrepareAccess($@"{basePath}\system\config\system_prof.psb.m"));
            //JObject systemObject = JObject.Parse(systemJson);
            //string dev_name = systemObject["root"]["dev_name"].ToString();
            //JToken systemEntry = systemObject["root"]["title_list"]
            //                                    .Where(c => c["dev_name"].ToString().Equals(dev_name))
            //                                    .FirstOrDefault();
            //string name_usa = systemEntry["name"]["usa"].ToString();
            //return ((int)systemEntry["dev_id"]).ToString("000");
        }

        private void BackupData()
        {
            UpdateStatus("Backing up...");
            string systemJson = File.ReadAllText(PrepareAccess($@"{basePath}\system\config\system_prof.psb.m"));
            JObject systemObject = JObject.Parse(systemJson);
            string dev_name = systemObject["root"]["dev_name"].ToString();
            JToken systemEntry = systemObject["root"]["title_list"]
                                                .Where(c => c["dev_name"].ToString().Equals(dev_name))
                                                .FirstOrDefault();
            dev_id = ((int)systemEntry["dev_id"]).ToString("000");


            if (!Directory.Exists($@"{lunarPath}\Backup"))
            {
                Directory.CreateDirectory($@"{lunarPath}\Backup");
            }
            if (!Directory.Exists($@"{lunarPath}\Backup\{dev_id}"))
            {
                Directory.CreateDirectory($@"{lunarPath}\Backup\{dev_id}");
                File.Copy($@"{sysPath}\alldata.bin", $@"{lunarPath}\Backup\{dev_id}\alldata.bin", true);
                File.Copy($@"{sysPath}\alldata.psb.m", $@"{lunarPath}\Backup\{dev_id}\alldata.psb.m", true);
                File.Copy($@"{sysPath}\m2engage", $@"{lunarPath}\Backup\{dev_id}\m2engage", true);
            }
        }

        private void UpdateStatus(string text)
        {
            toolStripStatusLabel.Text = $"Status: {text}";
            Application.DoEvents();
        }

        private string PrepareAccess(string path)
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

        private void AddGame(string[] roms = null)
        {

            if (roms == null)
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Browse ROM file(s)";
                openFileDialog.Filter = "ROM Image|*.bin;*.gen;*.md;*.zip";
                openFileDialog.FileName = "Browse ROM file(s)";
                if (openFileDialog.ShowDialog().Equals(DialogResult.Cancel))
                {
                    return;
                }

                roms = openFileDialog.FileNames;
            }

            string romFile;
            bool romFromZip = false;
            foreach (string file in roms)
            {
                romFile = file;
                if (romFile.Contains(".zip"))
                {
                    romFromZip = true;
                    string extractedFilePath = string.Empty;
                    using (var archive = ZipFile.OpenRead(romFile))
                    {
                        var extensions = new string[] { ".zip", ".gen", ".md", ".bin" };
                        //Grab the first entry
                        ZipArchiveEntry romEntry = null;
                        foreach (var entry in archive.Entries)
                        {
                            //Grab the relative pathname for the rom
                            string fileName = entry.FullName;
                            if (extensions.Contains(Path.GetExtension(fileName)))
                            {
                                //intended path once extracted would be
                                extractedFilePath = Path.Combine(lunarPath, fileName);
                                romEntry = entry;
                                break;
                            }
                        }
                        //ZipFile.ExtractToDirectory(romFile, lunarPath);
                        if (romEntry == null)
                        {
                            MessageBox.Show($"Adding game failed. The file {file} does not contain a valid rom.", "Add new game",
                                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        romEntry.ExtractToFile(extractedFilePath, true);
                    }
                    romFile = extractedFilePath;
                }

                frmAddGame addGameForm = new frmAddGame();
                addGameForm.ImageAttributes = imageAttr;
                addGameForm.TexturesObject = texturesObject;
                addGameForm.textureSources = textureSources;
                addGameForm.TitlesObject = titlesObject;
                addGameForm.RomsObject = romsObject;
                addGameForm.ArtRegion = artRegion;
                addGameForm.SysRegion = sysRegion;
                addGameForm.dev_name = dev_name;
                addGameForm.RomPath = romFile;
                addGameForm.basePath = basePath;
                addGameForm.sysPath = sysPath;
                addGameForm.lunarPath = lunarPath;
                addGameForm.OtherTexturesObject = new Dictionary<string, JObject>();
                List<string> otherRegions = new List<string>();
                switch (artRegion)
                {
                    case "eu":
                        {
                            otherRegions.AddRange(new string[] { "jp", "us" });
                            break;
                        }
                    case "jp":
                        {
                            otherRegions.AddRange(new string[] { "eu", "us" });
                            break;
                        }
                    case "us":
                        {
                            otherRegions.AddRange(new string[] { "jp", "eu" });
                            break;
                        }
                }
                foreach (string region in otherRegions)
                {
                    string texturesJsonPath1 = PrepareAccess($@"{basePath}\{dev_id}\motion\{sysRegion}_titleselect_{region}.psb.m");
                    string texturesJson1 = File.ReadAllText(texturesJsonPath1);
                    JObject textureObject1 = JObject.Parse(texturesJson1);
                    addGameForm.OtherTexturesObject.Add(region, textureObject1);
                }

                try
                {
                    if (addGameForm.ShowDialog().Equals(DialogResult.OK))
                    {
                        PrepareAccess($@"{Path.GetDirectoryName(romsJsonPath)}\{Path.GetFileNameWithoutExtension(romsJsonPath)}.m");
                        PrepareAccess($@"{Path.GetDirectoryName(titlesJsonPath)}\{Path.GetFileNameWithoutExtension(titlesJsonPath)}.m");
                        PrepareAccess($@"{Path.GetDirectoryName(texturesJsonPath)}\{Path.GetFileNameWithoutExtension(texturesJsonPath)}.m");
                        File.WriteAllText(romsJsonPath, romsObject.ToString());
                        File.WriteAllText(titlesJsonPath, titlesObject.ToString());
                        File.WriteAllText(texturesJsonPath, texturesObject.ToString());

                        foreach (var otherTextObj in addGameForm.OtherTexturesObject)
                        {
                            string textJsonPath = PrepareAccess($@"{basePath}\{dev_id}\motion\{sysRegion}_titleselect_{otherTextObj.Key}.psb.m");
                            File.WriteAllText(textJsonPath, otherTextObj.Value.ToString());
                        }

                        foreach (var textureSrc in textureSources)
                        {
                            byte[] streamData = new byte[182 * 216 * 4];
                            Bitmap streamTexture = textureSrc.Value.Values.Last();
                            BitmapData bmpData = streamTexture.LockBits(new Rectangle(0, 0, streamTexture.Width, streamTexture.Height), ImageLockMode.WriteOnly, streamTexture.PixelFormat);
                            Marshal.Copy(bmpData.Scan0, streamData, 0, streamData.Length);
                            streamTexture.UnlockBits(bmpData);

                            string textJsonPath = PrepareAccess($@"{basePath}\{dev_id}\motion\{sysRegion}_titleselect_{textureSrc.Key}.psb.m");

                            File.WriteAllBytes($@"{Path.GetDirectoryName(textJsonPath)}\{Path.GetFileNameWithoutExtension(textJsonPath)}.streams\stream_{addGameForm.StreamNumber[textureSrc.Key]}", streamData);
                        }

                        string romDestPath = $@"{basePath}\system\roms\{sysRegion}_en_{addGameForm.RomName}.bin";
                        File.Copy(addGameForm.RomPath, romDestPath, true);
                        File.SetAttributes(romDestPath, FileAttributes.Normal);

                        //Edit the nut script to allow an extra game
                        Alldata.ModeTitleSelectNutAddUpdate(basePath, packer, touchedFiles);

                        dataLoadaed = false;
                    }
                }
                finally
                {
                    if (romFromZip)
                    {
                        File.Delete(romFile);
                    }

                    CmdLoad_Click(this, new EventArgs());
                }
            }
        }

        private void treeGames_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void treeGames_DragDrop(object sender, DragEventArgs e){
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var extensions = new string[] { ".zip", ".gen", ".md", ".bin" };

            string[] validRoms = files.Where(c => extensions.Contains(Path.GetExtension(c))).ToArray();

            if (validRoms.Length == 0)
            {
                SwingMessageBox.Show("No cannot add games. Only .bin, .md, .gen and .zip are supported at this time!", 
                                     "Format not supported", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            AddGame(validRoms);
        }

        private void CmdAddGame_Click(object sender, EventArgs e)
        {
            int gameVersionValue = romsObject["root"]["game_versions"].Children().Children().Max(c => (int)c[0]);

            if (gameVersionValue.Equals(255))
            {
                SwingMessageBox.Show("Cannot add more games. Maximum limit supported by the stock UI has been reached.", "Add new game",
                                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            AddGame();
        }

        private void CmdSaveChanges_Click(object sender, EventArgs e)
        {
            if (!SwingMessageBox.Show("Do you want to sync now?", "Sync changes", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
            {
                return;
            }

            Task.Run(() => SyncGames());
        }

        private void SetLockedState(ControlLockType lockType, bool controlsLocked)
        {
            switch (lockType)
            {
                case ControlLockType.GameFields:
                    txtTname.ReadOnly = controlsLocked;
                    txtDescription.ReadOnly = controlsLocked;
                    txtCopy.ReadOnly = controlsLocked;
                    cboGenre.Enabled = !controlsLocked;
                    cboPlayerNum.Enabled = !controlsLocked;
                    break;
                case ControlLockType.ActionButtons:
                    cmdRemoveGame.Enabled = !controlsLocked;
                    cmdAddGame.Enabled = !controlsLocked;
                    cmdSaveChanges.Enabled = !controlsLocked;
                    break;
                case ControlLockType.AllControls:
                    txtTname.ReadOnly = controlsLocked;
                    txtDescription.ReadOnly = controlsLocked;
                    txtCopy.ReadOnly = controlsLocked;
                    cboGenre.Enabled = !controlsLocked;
                    cboPlayerNum.Enabled = !controlsLocked;
                    cmdRemoveGame.Enabled = !controlsLocked;
                    cmdAddGame.Enabled = !controlsLocked;
                    cmdSaveChanges.Enabled = !controlsLocked;
                    break;
            }
        }

        private void LockControls(bool controlLocked, params Control[] controls)
        {
            foreach (Control control in controls)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke((MethodInvoker)delegate
                    {
                        control.Enabled = !controlLocked;
                    });
                }
                else
                {
                    control.Enabled = !controlLocked;
                }
            }
        }

        private void LockControls(bool controlsLocked, ControlLockType lockType)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    SetLockedState(lockType, controlsLocked);
                });
            }
            else
            {
                SetLockedState(lockType, controlsLocked);
            }
        }

        private void LockControls(bool controlsLocked, bool allControls)
        {
            // Sync can occur during three stages:
            //   - No game selected (no control) partiallyLocked = false
            //   - Stock game selected (Only partial control) partiallyLocked = true
            //   - Custom game selected (full control) partiallyLocked = false

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    //Disabled locking the tree because of the color scheme not supporting the disabled state
                    //treeGames.Enabled = !controlsLocked;
                    cmdRemoveGame.Enabled = !controlsLocked;
                    txtTname.ReadOnly = controlsLocked;
                    txtDescription.ReadOnly = controlsLocked;
                    txtCopy.ReadOnly = controlsLocked;
                    cboGenre.Enabled = !controlsLocked;
                    cboPlayerNum.Enabled = !controlsLocked;
                    if (allControls == true)
                    {
                        cmdAddGame.Enabled = !controlsLocked;
                        cmdSaveChanges.Enabled = !controlsLocked;
                    }
                });
            }
            else
            {
                treeGames.Enabled = !controlsLocked;
                cmdRemoveGame.Enabled = !controlsLocked;
                txtTname.ReadOnly = controlsLocked;
                txtDescription.ReadOnly = controlsLocked;
                txtCopy.ReadOnly = controlsLocked;
                cboGenre.Enabled = !controlsLocked;
                cboPlayerNum.Enabled = !controlsLocked;
                if (allControls == true)
                {
                    cmdAddGame.Enabled = !controlsLocked;
                    cmdSaveChanges.Enabled = !controlsLocked;
                }
            }
        }

        private void SyncGames(bool fullUSBSync = false)
        {
            bool m2EngageRunning = false;

            ShowLoadingBox("SYNC IN PROGRESS...");
            LockControls(true, ControlLockType.AllControls);
            inLockOutEvent = true;

            try
            {
                using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
                {
                    UpdateStatus("Waiting for console connection...");
                    while (!ssh.IsConnected)
                    {
                        try
                        {
                            ssh.Connect();
                        }
                        catch
                        {
                            Thread.Sleep(500);
                        }
                    }

                    string result = ssh.RunCommand("cat /opt/project_lunar/pl_version").Result;
                    if (!result.ToLower().Contains("lunar"))
                    {
                        SwingMessageBox.Show("Cannot sync. Project Lunar is not installed in this device.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CloseLoadingBox();
                        LockControls(false, ControlLockType.AllControls);
                        inLockOutEvent = false;

                        if (editLocked == true)
                        {
                            //Unlock partial controls
                            LockControls(false, ControlLockType.GameFields);
                            LockControls(true, cmdRemoveGame);
                        }

                        return;
                    }

                    result = ssh.RunCommand("ps aux | grep \"[m]2engage\"").Result;
                    m2EngageRunning = result.Contains("m2engage");
                }
            }
            catch
            {
                SwingMessageBox.Show("Cannot sync. MD Mini is not connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseLoadingBox();
                LockControls(false, ControlLockType.AllControls);
                inLockOutEvent = false;

                if (editLocked == true)
                {
                    //Unlock partial controls
                    LockControls(false, ControlLockType.GameFields);
                    LockControls(true, cmdRemoveGame);
                }

                return;
            }

            UpdateFormCursor(Cursors.WaitCursor);
            UpdateStatus("Preparing files");
            lstCreated.Clear();
            fsw.EnableRaisingEvents = true;
            foreach (string file in touchedFiles)
            {
                PrepareForCompression(file);
            }
            touchedFiles.Clear();

            packer.CompressDirectory(basePath);
            fsw.EnableRaisingEvents = false;

            UpdateStatus("Checking free space");
            long updateSize = 0;
            long localSize = 0;
            int maxSpace = 0;
            int freeSpace = 0;
            using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
            {
                ssh.Connect();
                foreach (string file in lstCreated)
                {
                    updateSize += (int)Math.Round((new FileInfo(file).Length) / 1024.0);

                    string linuxFile = $"/usr/game{file.Replace(basePath, "").Replace("\\", "/")}";
                    string result = ssh.RunCommand($"du {linuxFile}").Result;
                    if (!result.Equals(string.Empty))
                    {
                        localSize += int.Parse(result.Split('\t')[0]);
                    }
                }

                //check size for extra roms that were compressed before but not sent yet
                List<string> systemGameList = ssh.RunCommand("ls /usr/game/system/roms").Result.Split('\n').Where(c => !c.Trim().Equals("")).ToList();
                List<string> localGameList = Directory.GetFiles($@"{basePath}\system\roms").Select(c => Path.GetFileName(c)).ToList();

                List<string> extraGames = localGameList.Where(c => !systemGameList.Contains(c)).ToList();

                int extraGameSize = 0;
                foreach (string extraGame in extraGames)
                {
                    string extraRomPath = $@"{basePath}\system\roms\{extraGame}";
                    if (!lstCreated.Contains(extraRomPath))
                    {
                        extraGameSize += (int)(new FileInfo(extraRomPath).Length);
                    }
                }

                extraGameSize = (int)Math.Round(extraGameSize / 1024.0);
                updateSize += extraGameSize;

                maxSpace = Convert.ToInt32(ssh.RunCommand("df -k /rootfs_data/ | awk '{ print $1; }' | tail -n1").Result);
                freeSpace = Convert.ToInt32(ssh.RunCommand("df -k /rootfs_data/ | awk '{ print $3; }' | tail -n1").Result);
            }

            int usedSpace = maxSpace - freeSpace;
            if (usedSpace - localSize + updateSize >= maxSpace)
            {
                SwingMessageBox.Show("Cannot sync. There isn't enough space in the system.\n" +
                                $"Space available:\t{maxSpace} KB\n" +
                                $"Space required:\t{usedSpace - localSize + updateSize} KB\n" +
                                "Remove some games and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                ReloadData();
                UpdateFormCursor(Cursors.Default);
                CloseLoadingBox();
                LockControls(false, ControlLockType.AllControls);

                inLockOutEvent = false;
                if (editLocked == true)
                {
                    //Unlock partial controls
                    LockControls(false, ControlLockType.GameFields);
                    LockControls(true, cmdRemoveGame);
                }
                return;
            }

            //AllDataPacker.Build(basePath, $@"{sysPath}\alldata", packer, null);

            //Delete removed roms
            UpdateStatus("Removing unused ROMs");
            List<string> gamesOnSystem = new List<string>();
            using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
            {
                ssh.Connect();

                List<string> systemGameList = ssh.RunCommand("ls /usr/game/system/roms").Result.Split('\n').Where(c => !c.Trim().Equals("")).ToList();
                List<string> localGameList = Directory.GetFiles($@"{basePath}\system\roms").Select(c => Path.GetFileName(c)).ToList();

                List<string> extraGames = systemGameList.Where(c => !localGameList.Contains(c)).Select(c => c = $"\"{c}\"").ToList();

                if (m2EngageRunning)
                {
                    string result = ssh.RunCommand("stop_m2engage").Result;
                }

                if (extraGames.Count() > 0)
                {
                    ssh.RunCommand($"cd /usr/game/system/roms;rm {string.Join(" ", extraGames.ToArray())}");
                }

                using (ScpClient scp = new ScpClient("169.254.215.100", "root", "5A7213"))
                {
                    scp.Connect();

                    if (!fullUSBSync)
                    {
                        UpdateStatus("Uploading files");
                        foreach (string file in lstCreated)
                        {
                            string destPath = file.Replace(basePath, string.Empty).Replace("\\", "/");
                            //ssh.RunCommand($"rm /usr/game{destPath}"); remove to attempt preventing whiteout weird thing
                            scp.Upload(new FileInfo(file), $"/usr/game{destPath}");
                        }

                        //Delete removed roms
                        gamesOnSystem.Clear();

                        systemGameList = ssh.RunCommand("ls /usr/game/system/roms").Result.Split('\n').Where(c => !c.Trim().Equals("")).ToList();
                        localGameList = Directory.GetFiles($@"{basePath}\system\roms").Select(c => Path.GetFileName(c)).ToList();

                        extraGames = localGameList.Where(c => !systemGameList.Contains(c)).ToList();

                        foreach (string extraGame in extraGames)
                        {
                            scp.Upload(new FileInfo($@"{basePath}\system\roms\{extraGame}"), $"/usr/game/system/roms/{extraGame}");
                        }

                    }
                    else
                    {
                        UpdateStatus("Performing full sync");
                        scp.Upload(new DirectoryInfo($@"{basePath}\system"), "/usr/game/system");
                        scp.Upload(new DirectoryInfo($@"{basePath}\{dev_id}"), $"/usr/game/{dev_id}");
                    }
                }
            }
            if (m2EngageRunning)
            {
                UpdateStatus("Restarting M2Engage");
                using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
                {
                    ssh.Connect();
                    var shell = ssh.CreateShellStream("Lunar", 120, 9999, 120, 9999, 65536);
                    shell.DataReceived += Shell_DataReceived;
                    shell.WriteLine($"start_m2engage");

                    Thread.Sleep(2000);
                }
            }

            ReloadData();
            UpdateStatus("Sync complete");
            //if (this.InvokeRequired)
            //{
            //    this.Invoke((MethodInvoker)delegate
            //    {
            //        SwingMessageBox.Show(this, "Sync complete.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    });
            //}

            CloseLoadingBox();
            LockControls(false, ControlLockType.AllControls);

            inLockOutEvent = false;
            if (editLocked == true)
            {
                //Unlock partial controls
                LockControls(false, ControlLockType.GameFields);
                LockControls(true, cmdRemoveGame);
            }

            UpdateFormCursor(Cursors.Default);

        }

        private void ReloadData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    CmdLoad_Click(this, new EventArgs());
                });
            }
            else
            {
                CmdLoad_Click(this, new EventArgs());
            }
        }

        private void ShowLoadingBox(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    loadingForm.status.Text = message;
                    loadingForm.Show(this);
                });
            }
            else
            {
                loadingForm.status.Text = message;
                loadingForm.Show(this);
            }
        }

        private void CloseLoadingBox()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    loadingForm.Hide();
                });
            }
            else
            {
                loadingForm.Hide();
            }
        }

        private void UpdateFormCursor(Cursor cursor)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Cursor = cursor;
                });
            }
            else
            {
                this.Cursor = cursor;
            }
        }

        private void ChangeUpdateMenuEnabledState(bool enabled)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    updatesToolStripMenuItem.Enabled = enabled;
                });
            }
            else
            {
                updatesToolStripMenuItem.Enabled = enabled;
            }
        }

        private long GetDirectorySize(DirectoryInfo di)
        {
            long dirSize = 0;
            foreach (FileInfo file in di.GetFiles())
            {
                dirSize += file.Length;
            }
            foreach (DirectoryInfo subDi in di.GetDirectories())
            {
                dirSize += GetDirectorySize(subDi);
            }

            return dirSize;
        }

        private void PrepareForCompression(string file)
        {
            if (Path.GetFileName(file).Contains("psb"))
            {
                Utils.SerializePsb(file, 4, PsbFlags.None, null, true, false);
                File.Delete(file);
                string psbPath = $@"{Path.GetDirectoryName(file)}\{Path.GetFileNameWithoutExtension(file)}";
                packer.CompressFile(psbPath);
                if (Directory.Exists($@"{psbPath}.streams"))
                {
                    Directory.Delete($@"{psbPath}.streams", true);
                }
            }
            else
            {
                packer.CompressFile(file);
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool overwrite = false)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, overwrite);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs, overwrite);
                }
            }
        }

        private void CboPlayerNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGenre.SelectedIndex.Equals(-1))
            {
                picNumPlayers.Image = new Bitmap(61, 48);
            }
            else
            {
                LoadSpecialIcon($"p{cboPlayerNum.SelectedIndex}", picNumPlayers);
            }
        }

        private void LoadSpecialIcon(string iconCode, PictureBox pictureBox)
        {
            string iconnNme = texturesObject["object"]["icon"]["motion"]
                                        [iconCode]["layer"][0]
                                        ["frameList"][0]["content"]["icon"].ToString();
            string textureName = texturesObject["object"]["icon"]["motion"]
                                               [iconCode]["layer"][0]
                                               ["frameList"][0]["content"]["src"].ToString();

            JToken icon = texturesObject["source"][textureName]["icon"][iconnNme];

            using (Graphics gfx = Graphics.FromImage(pictureBox.Image))
            {
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.DrawImage(textureSources[artRegion][textureName], new Rectangle(0, 0, 61, 48),
                                                           (int)icon["left"], (int)icon["top"], (int)icon["width"], (int)icon["height"],
                                                           GraphicsUnit.Pixel, imageAttr);

            }

            pictureBox.Refresh();
        }

        private void CboGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGenre.SelectedIndex.Equals(-1))
            {
                picGenre.Image = new Bitmap(61, 48);
            }
            else if (cboGenre.SelectedIndex < 10)
            {
                LoadSpecialIcon($"g{cboGenre.SelectedIndex}", picGenre);
            }
            else
            {
                LoadSpecialIcon($"d0", picGenre);
            }
        }

        private void FrmGameManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            this.Owner.Show();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*SwingMessageBox.Show("Project Lunar Desktop UI v1.0\n" +
                            "© 2019 ModMyClassic.com\n" +
                            "Proper credits will go here, or on a prettier window.");*/
            // Create a new instance of the Form2 class
            frmAbout aboutForm = new frmAbout();

            // Show the settings form
            aboutForm.ShowDialog();
        }

        private void restoreGamesFromBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SwingMessageBox.Show("This will remove all the games you added and reset to the state at the installation backup.\nAre you sure you want to continue?",
                                "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation).Equals(DialogResult.Yes))
            {
                return;
            }

            if (!File.Exists($@"{sysPath}\alldata.bin"))
            {
                if (File.Exists($@"{lunarPath}\Backup\{dev_id}\alldata.bin"))
                {
                    File.Copy($@"{lunarPath}\Backup\{dev_id}\alldata.bin", $@"{sysPath}\alldata.bin", true);
                    File.Copy($@"{lunarPath}\Backup\{dev_id}\alldata.psb.m", $@"{sysPath}\alldata.psb.m", true);
                }
                else
                {
                    SwingMessageBox.Show("Backup is missing. Please copy the backup folder from the computer where Project Lunar was installed from.",
                                         "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            UpdateFormCursor(Cursors.WaitCursor);
            while (Directory.Exists(basePath))
            {
                try
                {
                    Directory.Delete(basePath, true);
                }
                catch
                {
                    if (SwingMessageBox.Show($"Cannot delete {basePath}. Make sure there are no open files and try again.", "Error",
                                    MessageBoxButtons.RetryCancel, MessageBoxIcon.Error).Equals(DialogResult.Cancel))
                    {
                        UpdateFormCursor(Cursors.Default);
                        return;
                    }
                }
            }
            //timer.Stop();
            //this.Close();
            dataLoadaed = false;
            FrmGameManager_Shown(sender, e);
            UpdateFormCursor(Cursors.Default);
        }

        private void cmdRemoveGame_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = treeGames.SelectedNode;

            if (treeNode == null)
            {
                return;
            }

            if (treeNode.Nodes.Count > 0)
            {
                treeNode = treeNode.Nodes[1];
            }
            int gamesCount = titlesObject["items"].Count() / 8;
            int gameNumber = (int)treeNode.Parent.Tag;

            if (gameNumber < 50)
            {
                SwingMessageBox.Show("Cannot delete stock games.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string gameName = treeNode.Parent.Text;
            if (!SwingMessageBox.Show($"Are you sure you want to DELETE {gameName}?",
                                "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation).Equals(DialogResult.Yes))
            {
                return;
            }

            UpdateFormCursor(Cursors.WaitCursor);
            int gameTime = ((int)titlesObject["items"][gameNumber]["image"]);

            //Delete from Texture object(s)
            Alldata.DeleteGameFromTextureObject(gameTime, texturesObject, artRegion, basePath, dev_id, sysRegion, packer, touchedFiles);
            foreach (var otherRegion in LoadOtherRegionsTextureObjects())
            {
                Alldata.DeleteGameFromTextureObject(gameTime, otherRegion.Value, otherRegion.Key, basePath, dev_id, sysRegion, packer, touchedFiles);
            }

            //Delete from Title profile
            string romCode = Alldata.DeleteGameFromTitlesObject(gameNumber, gamesCount, texturesObject, titlesObject, titlesJsonPath, packer, touchedFiles);

            //Delete from Rom profile
            Alldata.DeleteGameFromRomsObject(romCode, romsObject, basePath, romsJsonPath, packer, touchedFiles);

            //Update Script
            Alldata.ModeTitleSelectNutAddUpdate(basePath, packer, touchedFiles, false);

            txtTname.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtCopy.Text = string.Empty;
            cboGenre.SelectedIndex = -1;
            cboPlayerNum.SelectedIndex = -1;
            picBoxArt.Image = new Bitmap(152, 216);
            picSpine.Image = new Bitmap(30, 216);
            cmdRemoveGame.Enabled = false;

            //foreach (string file in touchedFiles)
            //{
            //    PrepareForCompression(file);
            //}
            //touchedFiles.Clear();
            //packer.CompressDirectory(basePath);

            UpdateFormCursor(Cursors.Default);
            dataLoadaed = false;
            CmdLoad_Click(sender, e);
        }

        private DateTime GetReleaseDate(string description)
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

        static string GetMd5Hash(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                return GetMd5Hash(stream);
            }
        }

        static string GetMd5Hash(Stream input)
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

        private void installUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Open update package";
            openFileDialog.Filter = "Project LUNAR Update package|*.tar.gz";
            openFileDialog.FileName = "*.tar.gz";
            if (openFileDialog.ShowDialog().Equals(DialogResult.Cancel))
            {
                return;
            }
            string fileName = openFileDialog.FileName;
            UpdateFormCursor(Cursors.WaitCursor);
            string hashMD5 = GetMd5Hash(File.OpenRead(fileName));

            try
            {
                DeliverUpdate(fileName, hashMD5);
            }
            catch (Exception ex)
            {
                SwingMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateFormCursor(Cursors.Default);
                return;
            }
            UpdateFormCursor(Cursors.Default);
            SwingMessageBox.Show("Update sent and executed. The console will restart.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeliverUpdate(string fileName, string md5)
        {
            using (FileStream fileData = File.OpenRead(fileName))
            {
                DeliverUpdate(fileData, fileName, md5);
                fileData.Close();
            }
            File.Delete(fileName);
        }

        private void DeliverUpdate(Stream sourceData, string filePath, string md5)
        {
            ShowLoadingBox("UPDATE IN PROGRESS...");
            LockControls(true, ControlLockType.AllControls);
            inLockOutEvent = true;

            using (ScpClient scp = new ScpClient("169.254.215.100", "root", "5A7213"))
            {
                scp.Connect();

                SetStatusBarText("Status: Uploading payload");

                string fileName = Path.GetFileName(filePath);
                scp.Upload(sourceData, $"/tmp/{fileName}");

                using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
                {
                    ssh.Connect();
                    SetStatusBarText("Status: Checking payload integrity...");
                    string md5CommandText = $"cd /tmp;[ \"$(md5sum {fileName})\" = " +
                                                      $"\"$(echo $'{md5}  {fileName}')\" ] " +
                                                      "&& echo \"File integrity OK\" || " +
                                                      "echo \"File intergrity FAIL\"";
                    string result = ssh.RunCommand(md5CommandText).Result;

                    if (result.Contains("File integrity OK"))
                    {
                        SetStatusBarText("Status: Decompressing payload");
                        ssh.RunCommand($@"cd /tmp;gunzip -d {fileName}");
                        ssh.RunCommand($@"cd /tmp;tar -xvf {Path.GetFileNameWithoutExtension(filePath)}");

                        SetStatusBarText("Status: Installing update");
                        var shell = ssh.CreateShellStream("Lunar", 120, 9999, 120, 9999, 65536);
                        shell.DataReceived += Shell_DataReceived;
                        shell.WriteLine($"cd /tmp && ./update");

                        while (ssh.IsConnected)
                        {
                            Thread.Sleep(500);
                        }

                        SetStatusBarText("Status: Update installation complete");
                    }
                    else
                    {
                        throw new InvalidDataException("Update has corrupted in transit. Please try again.");
                    }
                }
            }

            CloseLoadingBox();
            LockControls(false, ControlLockType.AllControls);
            inLockOutEvent = false;
            if (editLocked == true)
            {
                //Unlock partial controls
                LockControls(false, ControlLockType.GameFields);
                LockControls(true, cmdRemoveGame);
            }
        }

        private void checkOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateFormCursor(Cursors.WaitCursor);
            try
            {
                CheckOnlineUpdates();
            }
            catch (Exception ex)
            {
                SwingMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateFormCursor(Cursors.Default);
        }

        private void CheckOnlineUpdates(bool showUpToDateMessage = true)
        {
            string installedVersion = string.Empty;
            using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
            {
                ssh.Connect();
                installedVersion = ssh.RunCommand("cat /opt/project_lunar/pl_version").Result.Replace("\n", "");
            }

            string payloadName = string.Empty;
            string updateDir = $@"{lunarPath}\updatecache";
            string gitVersion = string.Empty;
            string payloadMD5 = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMinutes(10);
                httpClient.DefaultRequestHeaders.Add("User-Agent", Properties.Resources.PL_UserAgent);

                payloadName = httpClient.GetStringAsync("https://classicmodscloud.com/project_lunar/.desktop_payload/.latest_build").Result.Replace("\n", "");

                if (!Directory.Exists(updateDir))
                {
                    Directory.CreateDirectory(updateDir);
                }

                int gitStart = payloadName.LastIndexOf("-") + 1;
                int gitEnd = payloadName.IndexOf(".tar.gz");
                gitVersion = payloadName.Substring(gitStart, gitEnd - gitStart);  //.Split('-')[3].Split('.')[0];

                int installedGitStart = installedVersion.LastIndexOf("-") + 1;
                int installedGitEnd = installedVersion.Length;
                string installedGitVersion = installedVersion.Substring(installedGitStart, installedGitEnd - installedGitStart);  //.Split('-')[3].Split('.')[0];

                if (!installedGitVersion.Equals(gitVersion))
                {
                    if (!SwingMessageBox.Show($"An update is available for Project Lunar. ({gitVersion})\nWould you like to download it now?",
                    "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
                    {
                        ChangeUpdateMenuEnabledState(true);
                        UpdateFormCursor(Cursors.Default);
                        return;
                    }

                    ChangeUpdateMenuEnabledState(false);
                    payloadMD5 = httpClient.GetStringAsync($"https://classicmodscloud.com/project_lunar/.desktop_payload/{payloadName.Replace("tar.gz", "md5")}").Result.Split(' ')[0];
                    string DownloadFileName = $@"{updateDir}\{payloadName}";
                    if (!(File.Exists(DownloadFileName) && GetMd5Hash(DownloadFileName).Equals(payloadMD5)))
                    {
                        SetStatusBarText($"Status: Downloading update ({gitVersion})");
                        HttpContent payloadContent = httpClient.GetAsync($"https://classicmodscloud.com/project_lunar/.desktop_payload/{payloadName}").Result.Content;
                        using (FileStream updateData = new FileStream($@"{updateDir}\{payloadName}", FileMode.Create))
                        {
                            Task download = payloadContent.CopyToAsync(updateData);
                            download.Wait();
                        }
                        SetStatusBarText($"Status: Downloading complete ({gitVersion})");
                    }
                }
                else
                {
                    if (showUpToDateMessage)
                    {
                        SwingMessageBox.Show($"Your console is up to date. ({gitVersion})", "Update check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    ChangeUpdateMenuEnabledState(true);
                    return;
                }
            }

            string payloadPath = $@"{updateDir}\{payloadName}";
            FileInfo fileInfo = new FileInfo(payloadPath);
            if (fileInfo.Length.Equals(0))
            {
                SwingMessageBox.Show($@"The download failed. Please try again.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                UpdateFormCursor(Cursors.Default);
                ChangeUpdateMenuEnabledState(true);
                return;
            }
            UpdateFormCursor(Cursors.WaitCursor);
            if (!SwingMessageBox.Show($"Update download complete. ({gitVersion})\nWould you like to install it now?",
                                "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
            {
                SwingMessageBox.Show($@"The update can be found at {payloadPath}, for later installation.",
                                "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateFormCursor(Cursors.Default);
                ChangeUpdateMenuEnabledState(true);
                return;
            }

            try
            {
                DeliverUpdate($@"{updateDir}\{payloadName}", payloadMD5);
            }
            catch (Exception ex)
            {
                SwingMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseLoadingBox();
                UpdateFormCursor(Cursors.Default);
                ChangeUpdateMenuEnabledState(true);
                return;
            }
            UpdateFormCursor(Cursors.Default);
            SwingMessageBox.Show("Update sent and executed. The console will restart",
                            "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ChangeUpdateMenuEnabledState(true);
        }

        private void SetStatusBarText(string statusText)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    toolStripStatusLabel.Text = statusText;
                });
            }
            else
            {
                toolStripStatusLabel.Text = statusText;
            }
        }

        private void Shell_DataReceived(object sender, ShellDataEventArgs e)
        {
            Debug.Write(Encoding.Default.GetString(e.Data));
        }

        private void enterRecoveryModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInstallRestore installRestoreForm = new frmInstallRestore();
            installRestoreForm.RecoveryMode = true;
            installRestoreForm.ShowDialog();
        }

        private void openlocalDataFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo process = new ProcessStartInfo("explorer.exe", lunarPath);
            process.UseShellExecute = true;
            Process.Start(process);
        }

        private void discordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://discord.me/modmyclassic");
            Process.Start(sInfo);
        }

        private void supportChatRoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://discordapp.com/invite/8gygsrw");
            Process.Start(sInfo);
        }

        private void mMCWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://modmyclassic.com");
            Process.Start(sInfo);
        }

        private void fAQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://modmyclassic.com/project-lunar/#FAQ");
            Process.Start(sInfo);
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            //int gameNumber = GetGameNumber();
            //int gamesCount = titlesObject["items"].Count() / 8;
            //UpdateTitlesObject(gamesCount);
            //UpdateTexturesObject();

            //SwingMessageBox.Show("Changes saved", "Update Game", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateTexturesObject(string artRegion, JObject texturesObj)
        {
            string texturesJsonPath = PrepareAccess($@"{basePath}\{dev_id}\motion\{sysRegion}_titleselect_{artRegion}.psb.m");
            File.WriteAllText(texturesJsonPath, texturesObj.ToString());
        }

        private int GetGameNumber()
        {
            TreeNode treeNode = treeGames.SelectedNode;
            if (treeNode.Nodes.Count > 0)
            {
                treeNode = treeNode.Nodes[1];
            }
            int gamesCount = titlesObject["items"].Count() / 8;
            int gameNumber = (int)treeNode.Parent.Tag + (gamesCount * treeNode.Index);
            return gameNumber;
        }

        private void UpdateTreeText(string value)
        {
            TreeNode treeNode = treeGames.SelectedNode;
            if (treeNode.Nodes.Count > 0)
            {
                treeNode.Text = value;
                treeNode = treeNode.Nodes[1];
            }
            switch (treeNode.Index)
            {
                case 0:
                    value = $"(JP) {value}";
                    break;
                case 1:
                    value = $"(EN) {value}";
                    break;
                case 2:
                    value = $"(ES) {value}";
                    break;
                case 3:
                    value = $"(FR) {value}";
                    break;
                case 4:
                    value = $"(IT) {value}";
                    break;
                case 5:
                    value = $"(DE) {value}";
                    break;
                case 6:
                    value = $"(CN) {value}";
                    break;
                case 7:
                    value = $"(KO) {value}";
                    break;
            }
            treeNode.Text = value;
        }

        private void txtDescription_Leave(object sender, EventArgs e)
        {
            if (txtDescription.Text.Equals(descState))
            {
                return;
            }

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

            descState = txtDescription.Text;
            UpdateTitleField("desc", txtDescription.Text.Trim());
            int gamesCount = titlesObject["items"].Count() / 8;
            Alldata.UpdateTitlesObject(gamesCount, titlesObject, texturesObject, titlesJsonPath, packer, touchedFiles);
        }

        private void UpdateTitleField(string field, string value)
        {
            int gameIndex = GetGameNumber();
            JToken game = titlesObject["items"][gameIndex];
            game[field] = value;
        }

        private void txtTname_Leave(object sender, EventArgs e)
        {
            if (txtTname.Text.Equals(tnameState))
            {
                return;
            }
            tnameState = txtTname.Text;
            UpdateTitleField("tname", txtTname.Text.Trim());
            int gamesCount = titlesObject["items"].Count() / 8;
            Alldata.UpdateTitlesObject(gamesCount, titlesObject, texturesObject, titlesJsonPath, packer, touchedFiles);
            UpdateTreeText(txtTname.Text.Trim());
        }

        private void txtCopy_Leave(object sender, EventArgs e)
        {
            if (txtCopy.Text.Equals(copyState))
            {
                return;
            }
            copyState = txtCopy.Text;
            UpdateTitleField("copy", txtCopy.Text.Trim());
            int gamesCount = titlesObject["items"].Count() / 8;
            Alldata.UpdateTitlesObject(gamesCount, titlesObject, texturesObject, titlesJsonPath, packer, touchedFiles);
        }

        private void cboPlayerNum_Leave(object sender, EventArgs e)
        {
            if (cboPlayerNum.SelectedIndex.Equals(playerNumState))
            {
                return;
            }
            playerNumState = cboPlayerNum.SelectedIndex;

            int gameIndex = GetGameNumber();
            int gameTime = ((int)titlesObject["items"][gameIndex]["image"]);

            //Update pnum icon list
            UpdatePlayerNumber(gameTime, texturesObject);
            UpdateTexturesObject(artRegion, texturesObject);
            foreach (var otherRegion in LoadOtherRegionsTextureObjects())
            {
                UpdatePlayerNumber(gameTime, otherRegion.Value);
                UpdateTexturesObject(otherRegion.Key, otherRegion.Value);
            }
        }

        private Dictionary<string, JObject> LoadOtherRegionsTextureObjects()
        {
            Dictionary<string, JObject> otherRegionsObjs = new Dictionary<string, JObject>();
            List<string> otherRegions = new List<string>();
            switch (artRegion)
            {
                case "eu":
                    {
                        otherRegions.AddRange(new string[] { "jp", "us" });
                        break;
                    }
                case "jp":
                    {
                        otherRegions.AddRange(new string[] { "eu", "us" });
                        break;
                    }
                case "us":
                    {
                        otherRegions.AddRange(new string[] { "jp", "eu" });
                        break;
                    }
            }
            foreach (string region in otherRegions)
            {
                string texturesJsonPath1 = PrepareAccess($@"{basePath}\{dev_id}\motion\{sysRegion}_titleselect_{region}.psb.m");
                string texturesJson1 = File.ReadAllText(texturesJsonPath1);
                JObject textureObject1 = JObject.Parse(texturesJson1);
                otherRegionsObjs.Add(region, textureObject1);
            }
            return otherRegionsObjs;
        }

        private void UpdatePlayerNumber(int gameTime, JObject textureObj)
        {
            JToken bigpPnum = textureObj["object"]["pkgData"]["motion"]["big_pnum"];
            JToken bigpPnumFramelist = bigpPnum["layer"][0]["frameList"];
            JToken bigPnumEntry = bigpPnumFramelist.Where(c => c["time"].ToString().Equals(gameTime.ToString())).FirstOrDefault();
            bigPnumEntry["content"]["icon"] = $"p{cboPlayerNum.SelectedIndex}";
        }

        private void cboGenre_Leave(object sender, EventArgs e)
        {
            if (cboGenre.SelectedIndex.Equals(genreState))
            {
                return;
            }
            genreState = cboGenre.SelectedIndex;

            int gameIndex = GetGameNumber();
            int gameTime = ((int)titlesObject["items"][gameIndex]["image"]);

            //Update Genre icon list
            UpdateGenre(gameTime, texturesObject);
            UpdateTexturesObject(artRegion, texturesObject);
            foreach (var otherRegion in LoadOtherRegionsTextureObjects())
            {
                UpdateGenre(gameTime, otherRegion.Value);
                UpdateTexturesObject(otherRegion.Key, otherRegion.Value);
            }
        }

        private void UpdateGenre(int gameTime, JObject textureObj)
        {
            JToken bigGenre = textureObj["object"]["pkgData"]["motion"]["big_genre"];
            JToken bigGenreFramelist = bigGenre["layer"][0]["frameList"];
            JToken bigGenreEntry = bigGenreFramelist.Where(c => c["time"].ToString().Equals(gameTime.ToString())).FirstOrDefault();
            bigGenreEntry["content"]["icon"] = $"g{cboGenre.SelectedIndex}";
        }

        private void disableForScanlinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string utilsScriptPath = PrepareAccess($@"{basePath}\system\script\utils.nut.m");
            string utilsScript = File.ReadAllText(utilsScriptPath);

            if (utilsScript.Contains("::g_emu_task.smoothing  = scale_tbl[num][2] == 1 ? true : false;"))
            {
                utilsScript = utilsScript.Replace("::g_emu_task.smoothing  = scale_tbl[num][2] == 1 ? true : false;",
                                                  "::g_emu_task.smoothing  = false;");
            }
            else if (utilsScript.Contains("::g_emu_task.smoothing  = true;"))
            {
                utilsScript = utilsScript.Replace("::g_emu_task.smoothing  = true;",
                                                  "::g_emu_task.smoothing  = false;");
            }
            else
            {
                //Already disabled... show message?
                return;
            }

            disableForScanlinesToolStripMenuItem.Checked = true;
            restoreDefaultToolStripMenuItem.Checked = false;
            enableAlwaysToolStripMenuItem.Checked = false;

            File.WriteAllText(utilsScriptPath, utilsScript);
            SwingMessageBox.Show("Smoothing has been disabled for scanlines. Please sync to commit the changes to the console",
                            "Disable Smoothing", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void enableAlwaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string utilsScriptPath = PrepareAccess($@"{basePath}\system\script\utils.nut.m");
            string utilsScript = File.ReadAllText(utilsScriptPath);
            if (utilsScript.Contains("::g_emu_task.smoothing  = scale_tbl[num][2] == 1 ? true : false;"))
            {
                utilsScript = utilsScript.Replace("::g_emu_task.smoothing  = scale_tbl[num][2] == 1 ? true : false;",
                                                  "::g_emu_task.smoothing  = true;");
            }
            else if (utilsScript.Contains("::g_emu_task.smoothing  = false;"))
            {
                utilsScript = utilsScript.Replace("::g_emu_task.smoothing  = false;",
                                                  "::g_emu_task.smoothing  = true;");
            }
            else
            {
                //Already enabled... show message?
                return;
            }

            enableAlwaysToolStripMenuItem.Checked = true;
            restoreDefaultToolStripMenuItem.Checked = false;
            disableForScanlinesToolStripMenuItem.Checked = false;

            File.WriteAllText(utilsScriptPath, utilsScript);
            SwingMessageBox.Show("Smoothing has been enabled for all modes. Please sync to commit the changes to the console",
                            "Enable Smoothing", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void restoreDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string utilsScriptPath = PrepareAccess($@"{basePath}\system\script\utils.nut.m");
            string utilsScript = File.ReadAllText(utilsScriptPath);
            if (utilsScript.Contains("::g_emu_task.smoothing  = true;"))
            {
                utilsScript = utilsScript.Replace("::g_emu_task.smoothing  = true;",
                                                  "::g_emu_task.smoothing  = scale_tbl[num][2] == 1 ? true : false;");
            }
            else if (utilsScript.Contains("::g_emu_task.smoothing  = false;"))
            {
                utilsScript = utilsScript.Replace("::g_emu_task.smoothing  = false;",
                                                  "::g_emu_task.smoothing  = scale_tbl[num][2] == 1 ? true : false;");
            }
            else
            {
                //Already default... show message?
                return;
            }

            restoreDefaultToolStripMenuItem.Checked = true;
            enableAlwaysToolStripMenuItem.Checked = false;
            disableForScanlinesToolStripMenuItem.Checked = false;

            File.WriteAllText(utilsScriptPath, utilsScript);
            SwingMessageBox.Show("Smoothing behavior has been restored to default. Please sync to commit the changes to the console",
                            "Smoothing Default", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtTname_Enter(object sender, EventArgs e)
        {
            tnameState = txtTname.Text;
        }

        private void txtDescription_Enter(object sender, EventArgs e)
        {
            descState = txtDescription.Text;
        }

        private void txtCopy_Enter(object sender, EventArgs e)
        {
            copyState = txtCopy.Text;
        }

        private void cboPlayerNum_Enter(object sender, EventArgs e)
        {
            playerNumState = cboPlayerNum.SelectedIndex;
        }

        private void cboGenre_Enter(object sender, EventArgs e)
        {
            genreState = cboGenre.SelectedIndex;
        }

        private void manageModsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageMods modManager = new frmManageMods();
            modManager.ShowDialog();
        }

        private void AddSpecialRom(string romPath, string gameName, string description, string dev_id, SpecialLunarRom romType)
        {
            File.WriteAllBytes(romPath, Properties.Resources.ProjectLunar);

            string systemJson = File.ReadAllText(PrepareAccess($@"{basePath}\system\config\system_prof.psb.m"));
            JObject systemObject = JObject.Parse(systemJson);
            string dev_name = systemObject["root"]["dev_name"].ToString();
            JToken systemEntry = systemObject["root"]["title_list"]
                                                .Where(c => c["dev_name"].ToString().Equals(dev_name))
                                                .FirstOrDefault();
            name_usa = systemEntry["name"]["usa"].ToString();
            dev_id = ((int)systemEntry["dev_id"]).ToString("000");

            string systemRegion = "jp";
            if (name_usa.Contains("USA"))
            {
                systemRegion = "us";
            }
            else if (name_usa.Contains("EUR"))
            {
                systemRegion = "eu";
            }
            else if (name_usa.Contains("ASIA"))
            {
                systemRegion = "as";
            }

            string artRegion = (systemRegion.Equals("as") ? "jp" : systemRegion);

            string titlesJsonPath = PrepareAccess($@"{basePath}\{dev_id}\config\title_mode_top.psb.m");
            string titlesJson = File.ReadAllText(titlesJsonPath);
            JObject titlesObject = JObject.Parse(titlesJson);
            string texturesJsonPath = PrepareAccess($@"{basePath}\{dev_id}\motion\{systemRegion}_titleselect_{artRegion}.psb.m");
            string texturesJson = File.ReadAllText(texturesJsonPath);
            JObject texturesObject = JObject.Parse(texturesJson);
            string romsJsonPath = PrepareAccess($@"{basePath}\{dev_id}\config\title_prof.psb.m");
            string romsJson = File.ReadAllText(romsJsonPath);
            JObject romsObject = JObject.Parse(romsJson);

            Dictionary<string, Dictionary<string, Bitmap>> textureSources = new Dictionary<string, Dictionary<string, Bitmap>>();
            string[] regionList = new string[] { "eu", "jp", "us" };
            foreach (string region in regionList)
            {
                string textJsonPath = PrepareAccess($@"{basePath}\{dev_id}\motion\{systemRegion}_titleselect_{region}.psb.m");
                string textJson = File.ReadAllText(textJsonPath);
                JObject textObject = JObject.Parse(textJson);

                Dictionary<string, Bitmap> textureSrc = new Dictionary<string, Bitmap>();
                foreach (var textureSource in textObject["source"])
                {
                    var textureData = textureSource.Children().FirstOrDefault();
                    Bitmap bmp = new Bitmap((int)textureData["texture"]["width"], (int)textureData["texture"]["height"]);
                    string streamName = textureData["texture"]["pixel"].ToString();
                    LoadRawBitmap(bmp, $@"{basePath}\{dev_id}\motion\{systemRegion}_titleselect_{region}.psb.streams\stream_{streamName.Substring(streamName.LastIndexOf(":") + 1)}");
                    string textureName = textureData.Path.Substring(textureData.Path.LastIndexOf(".") + 1);
                    textureSrc.Add(textureName, bmp);
                }
                textureSources.Add(region, textureSrc);
            }

            frmAddGame addGameForm = new frmAddGame();

            addGameForm.ImageAttributes = imageAttr;
            addGameForm.TexturesObject = texturesObject;
            addGameForm.textureSources = textureSources;
            addGameForm.TitlesObject = titlesObject;
            addGameForm.RomsObject = romsObject;
            addGameForm.ArtRegion = artRegion;
            addGameForm.SysRegion = systemRegion;
            addGameForm.dev_name = dev_name;
            addGameForm.DemoTime = 5;
            addGameForm.RomPath = romPath;
            addGameForm.basePath = basePath;
            addGameForm.sysPath = sysPath;
            addGameForm.lunarPath = lunarPath;
            addGameForm.OtherTexturesObject = new Dictionary<string, JObject>();
            List<string> otherRegions = new List<string>();

            Graphics gxSpine = Graphics.FromImage(addGameForm.picSpine.Image);
            Graphics gxBox = Graphics.FromImage(addGameForm.picBoxArt.Image);
            int posX = 0;
            int posY = 0;
            switch (artRegion)
            {
                case "jp":
                    {
                        otherRegions.AddRange(new string[] { "eu", "us" });
                        posX = 0;
                        break;
                    }
                case "eu":
                    {
                        otherRegions.AddRange(new string[] { "jp", "us" });
                        posX = 182;
                        break;
                    }
                case "us":
                    {
                        otherRegions.AddRange(new string[] { "jp", "eu" });
                        posX = 182 * 2;
                        break;
                    }
            }

            string specialRomCode = string.Empty;
            switch (romType)
            {
                case SpecialLunarRom.BootMenu:
                    specialRomCode = "BOOTMENU_____";
                    posY = 0;
                    break;
                case SpecialLunarRom.RetroArch:
                    specialRomCode = "RETROARCH____";
                    posY = 216;
                    break;
            }

            addGameForm.SpecialRomCode = specialRomCode;

            gxSpine.DrawImage(Properties.Resources.BootMenu_RA, new Rectangle(0, 0, 30, 216),
                                    posX, posY, 30, 216,
                                    GraphicsUnit.Pixel);
            gxBox.DrawImage(Properties.Resources.BootMenu_RA, new Rectangle(0, 0, 152, 216),
                                  posX + 30, posY, 152, 216,
                                  GraphicsUnit.Pixel);

            foreach (string region in otherRegions)
            {
                string texturesJsonPath1 = PrepareAccess($@"{basePath}\{dev_id}\motion\{systemRegion}_titleselect_{region}.psb.m");
                string texturesJson1 = File.ReadAllText(texturesJsonPath1);
                JObject textureObject1 = JObject.Parse(texturesJson1);
                addGameForm.OtherTexturesObject.Add(region, textureObject1);
            }

            addGameForm.txtTname.Text = gameName;
            addGameForm.txtDescription.Text = description;
            addGameForm.txtCopyright.Text = "ModMyClassic";
            addGameForm.cboGenre.SelectedIndex = 9;
            addGameForm.cboPlayerNum.SelectedIndex = 4;

            try
            {
                addGameForm.CmdSave_Click(this, new EventArgs());

                PrepareAccess($@"{Path.GetDirectoryName(romsJsonPath)}\{Path.GetFileNameWithoutExtension(romsJsonPath)}.m");
                PrepareAccess($@"{Path.GetDirectoryName(titlesJsonPath)}\{Path.GetFileNameWithoutExtension(titlesJsonPath)}.m");
                PrepareAccess($@"{Path.GetDirectoryName(texturesJsonPath)}\{Path.GetFileNameWithoutExtension(texturesJsonPath)}.m");
                File.WriteAllText(romsJsonPath, romsObject.ToString());
                File.WriteAllText(titlesJsonPath, titlesObject.ToString());
                File.WriteAllText(texturesJsonPath, texturesObject.ToString());

                foreach (var otherTextObj in addGameForm.OtherTexturesObject)
                {
                    string textJsonPath = PrepareAccess($@"{basePath}\{dev_id}\motion\{systemRegion}_titleselect_{otherTextObj.Key}.psb.m");
                    File.WriteAllText(textJsonPath, otherTextObj.Value.ToString());
                }

                foreach (var textureSrc in textureSources)
                {
                    byte[] streamData = new byte[182 * 216 * 4];
                    Bitmap streamTexture = textureSrc.Value.Values.Last();
                    BitmapData bmpData = streamTexture.LockBits(new Rectangle(0, 0, streamTexture.Width, streamTexture.Height), ImageLockMode.WriteOnly, streamTexture.PixelFormat);
                    Marshal.Copy(bmpData.Scan0, streamData, 0, streamData.Length);
                    streamTexture.UnlockBits(bmpData);

                    string textJsonPath = PrepareAccess($@"{basePath}\{dev_id}\motion\{systemRegion}_titleselect_{textureSrc.Key}.psb.m");

                    File.WriteAllBytes($@"{Path.GetDirectoryName(textJsonPath)}\{Path.GetFileNameWithoutExtension(textJsonPath)}.streams\stream_{addGameForm.StreamNumber[textureSrc.Key]}", streamData);
                }

                string romDestPath = $@"{basePath}\system\roms\{systemRegion}_en_{addGameForm.RomName}.bin";
                File.Copy(addGameForm.RomPath, romDestPath, true);
                File.SetAttributes(romDestPath, FileAttributes.Normal);

                //Edit the nut script to allow an extra game
                Alldata.ModeTitleSelectNutAddUpdate(basePath, packer, touchedFiles);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (File.Exists(romPath))
                {
                    File.Delete(romPath);
                }
            }
        }

        private void failSafeRestoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SwingMessageBox.Show("This is very dangerous and should only be used incase of emergency! Only use " +
                                     "if you have already spoken to a dev and your console's nand needs a full restore. This " +
                                     "restore is generated from your original NAND partition backups. DO NOT USE THIS TO FACTORY " +
                                     "RESET YOUR CONSOLE! PLEASE UNINSTALL INSTEAD! Do you wish to continue?",
                                     "WARNING!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning).Equals(DialogResult.Yes))
            {
                return;
            }
            if (!SwingMessageBox.Show("Are you sure you want to continue? Please note that ModMyClassic will not be responsible for any damages caused " +
                                     "to your console. Please ensure you speak to a developer first!",
                                     "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation).Equals(DialogResult.Yes))
            {
                return;
            }
            frmInstallRestore installRestoreForm = new frmInstallRestore();
            installRestoreForm.RestoreMode = true;
            installRestoreForm.ShowDialog();
        }

        private void exportBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = $"MiniBackup-{dev_id}-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}";
            if (saveFileDialog.ShowDialog().Equals(DialogResult.Cancel))
            {
                return;
            }

            string fileName = saveFileDialog.FileName;

            this.Cursor = Cursors.WaitCursor;
            ZipFile.CreateFromDirectory($@"{lunarPath}\Backup\{dev_id}", fileName);

            SwingMessageBox.Show($"Backup exported to {fileName}.", "Backup Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Cursor = Cursors.Default;
        }

        private void getRetroArchCoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://modmyclassic.com/project-lunar-cores/");
            Process.Start(sInfo);
        }

        private void toolStripMenuChangeBGmusic_Click(object sender, EventArgs e)
        {
            SwingMessageBox.Show($"Pardon our dust... We haven't implemented this feature yet please check back later!", "Nothing to see here!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void submitABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://github.com/Project-Lunar/Project-Lunar-Issue-Tracker/issues");
            Process.Start(sInfo);
        }

        private void cboSystemRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSystemRegion.Enabled)
            {
                string item = cboSystemRegion.SelectedItem.ToString();
                string selectedDevId = (item.Contains("Japan") ? "030" : (item.Contains("USA") ? "031" : (item.Contains("Europe") ? "032" : "033")));

                ReloadRegion(selectedDevId);
            }
        }

        private void getIPSPatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://classicmodscloud.com/project_lunar/ips_patches/ips.zip");
            Process.Start(sInfo);
        }

        private void openLocalIPSFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo process = new ProcessStartInfo("explorer.exe", $@"{lunarPath}\IPS\");
            process.UseShellExecute = true;
            Process.Start(process);
        }
    }
}