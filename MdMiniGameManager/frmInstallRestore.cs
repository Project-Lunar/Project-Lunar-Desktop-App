using FelLib;
using MArchiveBatchTool;
using MArchiveBatchTool.MArchive;
using MArchiveBatchTool.Psb;
using Newtonsoft.Json.Linq;
using ProjectLunarUI.M2engage;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
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
using System.Configuration;

namespace ProjectLunarUI
{
    public partial class frmInstallRestore : DarkForm
    {
        ConnectionInfo connInfo = null;
        MArchivePacker packer;
        List<string> touchedFiles = new List<string>();

        string basePath;
        string sysPath;
        string lunarPath;
        bool workInProgress = false;
        private bool recoveryMode;
        private bool restoreMode;
        private string[] nandPartitions = { "nanda", "nandb", "nandc", "nandd", "nande", "nandf", "nandg" };

        private string SystemIpAddress = ConfigurationManager.AppSettings["SystemIpAddress"].ToString();
        private string SystemRootUsername = ConfigurationManager.AppSettings["SystemRootUsername"].ToString();
        private string SystemRootPassword = ConfigurationManager.AppSettings["SystemRootPassword"].ToString();

        public bool RecoveryMode
        {
            get => recoveryMode;
            set
            {
                recoveryMode = value;
                if (recoveryMode)
                {
                    this.Text = "Project LUNAR Recovery Mode";
                }
            }
        }

        public bool RestoreMode
        {
            get => restoreMode;
            set
            {
                restoreMode = value;
                if (restoreMode)
                {
                    this.Text = "Project LUNAR Fail Safe Restore Mode";
                }
            }
        }

        public frmInstallRestore()
        {
            InitializeComponent();
        }

        private void FrmInstallRestore_Load(object sender, EventArgs e)
        {
            pbStatus.Maximum = 33;
            lunarPath = Path.Combine(Application.StartupPath, @"lunar_data");
            //basePath = $@"{lunarPath}\alldata.psb_extracted";

            KeyboardInteractiveAuthenticationMethod keyAuth = new KeyboardInteractiveAuthenticationMethod(SystemRootUsername);
            keyAuth.AuthenticationPrompt += KeyAuth_AuthenticationPrompt;
            connInfo = new ConnectionInfo(SystemIpAddress, SystemRootUsername, keyAuth);
        }

        private void KeyAuth_AuthenticationPrompt(object sender, Renci.SshNet.Common.AuthenticationPromptEventArgs e)
        {
            foreach (var prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = SystemRootPassword;
                }
            }
        }

        private void FrmInstallRestore_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (!CheckClassicDriver())
            {
                LogProgress($"Installation did not complete successfully.");
                SwingMessageBox.Show("There was a problem installing the device driver. Installation cannot proceed.", 
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                workInProgress = false;
                ChangeCancelToFinish();
                return;
            }

            bool consoleDetected = false;
            try
            {
                using (SshClient ssh = new SshClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
                {
                    ssh.Connect();
                }
                consoleDetected = true;
            }
            catch
            {
                consoleDetected = false;
            }

            if (recoveryMode)
            {
                if (SwingMessageBox.Show("This will put your console in Recovery Mode. It can be used to troubleshoot issues if your system isn't working properly. Do you want to enter Recovery Mode now?",
                                         "Recovery", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    this.DialogResult = DialogResult.No;
                    this.Close();
                    return;
                }

                if (SwingMessageBox.Show("Would you like to open our interactive how-to help guide to show you how to connect your console?",
                                         "Project Lunar Assistant",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    frmTutorial tutorialForm = new frmTutorial("RECOVER");
                    tutorialForm.Show();
                }
                pbStatus.Maximum = 13;

                Task.Run(() =>
                {
                    bool isNewMutex = false;
                    Mutex taskMutex = new Mutex(true, "Project-Lunar-Installer", out isNewMutex);
                    if (!isNewMutex)
                    {
                        taskMutex.Dispose();
                        return;
                    }

                    try
                    {
                        workInProgress = true;
                        FelMemBoot();
                        using (SshClient ssh = new SshClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
                        {
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
                        }

                        LogProgress("\r\nRecovery mode initiated. You can now SSH into the system.\r\n\r\n" +
                                        "Connection details:\r\n" +
                                        $"IP Address: {SystemIpAddress}\r\n" +
                                        $"Username: {SystemRootUsername}");

                        workInProgress = false;
                        ChangeCancelToFinish();
                    }
                    catch (Exception ex)
                    {
                        LogProgress($"{ex.Message}\r\nInstallation did not complete successfully.");
                        workInProgress = false;
                        ChangeCancelToFinish();
                    }
                });
            }
            else if (restoreMode)
            {
                if (SwingMessageBox.Show("Would you like to open our interactive how-to help guide to show you how to connect your console?",
                                         "Project Lunar Assistant",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    frmTutorial tutorialForm = new frmTutorial("RECOVER");
                    tutorialForm.Show();
                }
                pbStatus.Maximum = 464;

                //Install
                Task.Run(() =>
                {
                    bool isNewMutex = false;
                    Mutex taskMutex = new Mutex(true, "Project-Lunar-Installer", out isNewMutex);
                    if (!isNewMutex)
                    {
                        taskMutex.Dispose();
                        return;
                    }

                    try
                    {
                        workInProgress = true;
                        FelMemBoot();
                        using (SshClient ssh = new SshClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
                        {
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
                        }
                        VerifyConsoleElegibility(true);
                        RestoreNand();
                        workInProgress = false;
                        ChangeCancelToFinish();
                        System.Media.SystemSounds.Asterisk.Play();
                        SwingMessageBox.Show("Restore complete!", "Restore", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    catch (Exception ex)
                    {
                        LogProgress($"{ex.Message}\r\nRestore did not complete successfully.");
                        workInProgress = false;
                        ChangeCancelToFinish();
                    }
                });
            }
            else if (!consoleDetected)
            {
                if (SwingMessageBox.Show("No console detected. Do you wish to start the Project Lunar install process?",
                                    "Installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    this.DialogResult = DialogResult.No;
                    this.Close();
                    return;
                }

                if (SwingMessageBox.Show("Would you like to open our interactive how-to help guide to show you how to install and use Project Lunar?",
                                         "Project Lunar Assistant",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    frmTutorial tutorialForm = new frmTutorial("NORMAL");
                    tutorialForm.Show();
                }
                pbStatus.Maximum = 34;

                //Install
                Task.Run(() =>
                {
                    bool isNewMutex = false;
                    Mutex taskMutex = new Mutex(true, "Project-Lunar-Installer", out isNewMutex);
                    if (!isNewMutex)
                    {
                        taskMutex.Dispose();
                        return;
                    }

                    try
                    {
                        workInProgress = true;
                        string payloadMD5 = DownloadPayload();
                        FelMemBoot();
                        VerifyConsoleElegibility(false);
                        string dev_id = DumpNand();
                        CleanLocalFiles();
                        DownloadDataFiles(dev_id);
                        ExtractAlldata();
                        BackupData(dev_id);
                        UploadPayload(payloadMD5);
                        PerformInstallation(dev_id);

                        LogProgress("\r\nInstall complete.");
                        workInProgress = false;
                        ChangeCancelToFinish();
                        System.Media.SystemSounds.Asterisk.Play();
                        SwingMessageBox.Show("Installation complete!", "Install", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    catch (AggregateException ex)
                    {
                        string allMessages = GetAllAggregateExceptionMessages(ex);
                        LogProgress($"{allMessages}\r\nInstallation did not complete successfully.");
                        workInProgress = false;
                        ChangeCancelToFinish();
                        SwingMessageBox.Show("Could not install Project Lunar. See log for details.", "Install", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        LogProgress($"{ex.Message}\r\nInstallation did not complete successfully.");
                        workInProgress = false;
                        ChangeCancelToFinish();
                        SwingMessageBox.Show("Could not install Project Lunar. See log for details.", "Install", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });
            }
            else
            {
                if (SwingMessageBox.Show("This will remove Project Lunar from your system. Do you wish to uninstall it now?",
                                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    this.DialogResult = DialogResult.No;
                    this.Close();
                    return;
                }
                pbStatus.Maximum = 10;
                UpdateLabel("Uninstalling Project Lunar");

                Task.Run(()=> 
                {
                    bool isNewMutex = false;
                    Mutex taskMutex = new Mutex(true, "Project-Lunar-Installer", out isNewMutex);
                    if (!isNewMutex)
                    {
                        taskMutex.Dispose();
                        return;
                    }
                    PerformUninstall(); 
                });
            }
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

        public bool CheckClassicDriver()
        {
            try
            {
                ProcessStartInfo pnpUtilStartInfo = new ProcessStartInfo()
                {
                    FileName = "pnputil.exe",
                    Arguments = "-e",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process pnpUtilProcess = Process.Start(pnpUtilStartInfo);
                string result = pnpUtilProcess.StandardOutput.ReadToEnd();

                if (result.Contains("USB\\VID_1F3A&PID_EFE8"))
                {
                    return true;
                }

                SwingMessageBox.Show("Device Driver was not found. It will be installed now.", "Installation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ProcessStartInfo driverStartInfo = new ProcessStartInfo()
                {
                    FileName = $@"{lunarPath}\Driver\classic_driver.exe",
                    WorkingDirectory = $@"{lunarPath}\Driver\",
                    Verb = "runas"
                };
                Process driverProcess = Process.Start(driverStartInfo);
                driverProcess.WaitForExit();
                return driverProcess.ExitCode == 0;
            }
            catch (Win32Exception ex) 
            {
                //Cannot determine if driver is installed. May need to do manual installation
                Debug.WriteLine(ex.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }


        private void ChangeCancelToFinish()
        {
            if (cmdFinish.InvokeRequired)
            {
                cmdFinish.Invoke((MethodInvoker)delegate
                {
                    cmdFinish.Text = "Finish";
                });
            }
            else
            {
                cmdFinish.Text = "Finish";
            }
        }

        private void CleanLocalFiles()
        {
            //Cleanup to prevent conflicting installation
            if (Directory.Exists(basePath))
            {
                while (Directory.Exists(basePath))
                {
                    try
                    {
                        Directory.Delete(basePath, true);
                        Thread.Sleep(1000);
                    }
                    catch
                    {
                        if (SwingMessageBox.Show($"Cannot delete {basePath}. Make sure there are no open files and try again.", "Error",
                                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Error).Equals(DialogResult.Cancel))
                        {
                            throw;
                        }
                    }
                }
            }
            if (File.Exists($@"{sysPath}\alldata.bin"))
            {
                File.Delete($@"{sysPath}\alldata.bin");
            }
            if (File.Exists($@"{sysPath}\alldata.psb"))
            {
                File.Delete($@"{sysPath}\alldata.psb");
            }
            if (File.Exists($@"{sysPath}\alldata.psb.m"))
            {
                File.Delete($@"{sysPath}\alldata.psb.m");
            }
            if (File.Exists($@"{sysPath}\m2engage"))
            {
                File.Delete($@"{sysPath}\m2engage");
            }
        }

        private void PerformUninstall()
        {
            bool okToRun = true;
            workInProgress = true;
            LogProgress("Starting uninstall, attempting to connect via SSH");
            string systemRegion = string.Empty;
            using (SshClient ssh = new SshClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
            {
                ssh.Connect();
                string fileName = ssh.RunCommand("cat /version").Result.Replace("\n", "");
                systemRegion = fileName.Split('-')[5].Substring(4).ToLower();
                string dev_id = (systemRegion == "jp" ? "030" : (systemRegion == "us" ? "031" : (systemRegion == "eu" ? "032" : "033")));

                if (File.Exists($@"{lunarPath}\Backup\{dev_id}\alldata.bin") == false)
                {
                    okToRun = false;
                }
                if (File.Exists($@"{lunarPath}\Backup\{dev_id}\alldata.psb.m") == false)
                {
                    okToRun = false;
                }
                if (File.Exists($@"{lunarPath}\Backup\{dev_id}\m2engage") == false)
                {
                    okToRun = false;
                }
                if (okToRun == false)
                {
                    LogProgress("Cannot uninstall as backup files are missing! Aborting");
                    workInProgress = false;
                    ChangeCancelToFinish();
                    throw new InvalidDataException("Cannot uninstall as backup files are missing! Aborting uninstall.");
                }

                LogProgress($"Revert to stock {fileName}");
                LogProgress("Uninstall Phase 1");

                var shell = ssh.CreateShellStream("Lunar", 120, 9999, 120, 9999, 65536);
                shell.DataReceived += Shell_DataReceived;
                shell.WriteLine("/opt/uninstall 1 &");

                using (SftpClient sftp = new SftpClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
                {
                    sftp.Connect();
                    while (!sftp.Exists("/tmp/stage1.flag"))
                    {
                        Thread.Sleep(500);
                    }
                }

                //Thread.Sleep(10000); //Wait 10 seconds before starting to remove stuff


                shell.WriteLine("mount -o remount,rw /");

                Thread.Sleep(500);
                LogProgress("\r\nRemoving symlinks");
                for (int i = 30; i <= 33; i++)
                {
                    if (i.ToString("000").Equals(dev_id))
                    {
                        continue;
                    }
                    //result = ssh.RunCommand($"cd {workingDir};rm {i.ToString("000")}").Result;
                    shell.WriteLine("cd /usr/game");
                    Thread.Sleep(500);
                    shell.WriteLine($"rm {i.ToString("000")}");
                    Thread.Sleep(500);

                }
                LogProgress("\r\nResetting system settings");
                shell.WriteLine("rm -f /rootfs_data/data_008_0000.bin");
                Thread.Sleep(100);
                shell.WriteLine("rm -f /rootfs_data/meta_008_0000.bin");
                Thread.Sleep(100);
                shell.WriteLine("rm -f /media/mega_drive_saves/data_008_0000.bin");
                Thread.Sleep(100);
                shell.WriteLine("rm -f /media/mega_drive_saves/meta_008_0000.bin");
                Thread.Sleep(100);

                LogProgress("\r\nRemoving Project Lunar data");
                //result = ssh.RunCommand($"cd {workingDir};rm -rf {dev_id}/").Result;
                //result = ssh.RunCommand($"cd {workingDir};rm -rf system/").Result;
                shell.WriteLine($"rm -rf {dev_id}/");
                Thread.Sleep(1000);
                shell.WriteLine("rm -rf system/");
                Thread.Sleep(1000);

                LogProgress("\r\nUploading original data from backup");

                ssh.RunCommand("rm -f /usr/game/alldata.bin");
                ssh.RunCommand("rm -f /usr/game/alldata.psb.m");
                ssh.RunCommand("rm -f /usr/game/m2engage");

                using (ScpClient scp = new ScpClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
                {
                    scp.Connect();
                    scp.Upload(new FileInfo($@"{lunarPath}\Backup\{dev_id}\alldata.bin"), "/usr/game/alldata.bin");
                    scp.Upload(new FileInfo($@"{lunarPath}\Backup\{dev_id}\alldata.psb.m"), "/usr/game/alldata.psb.m");
                    scp.Upload(new FileInfo($@"{lunarPath}\Backup\{dev_id}\m2engage"), "/usr/game/m2engage");
                }

                //result = ssh.RunCommand($"cd {workingDir};chmod 644 alldata.bin alldata.psb.bin").Result;
                LogProgress("\r\nRestoring file permissions");
                shell.WriteLine("chmod 755 alldata.bin alldata.psb.m m2engage");
                shell.WriteLine("chmod +x m2engage");
                Thread.Sleep(500);

                LogProgress("\r\nUninstall phase 2");
                shell.WriteLine("/opt/uninstall 2");

                while (ssh.IsConnected)
                {
                    Thread.Sleep(500);
                }
            }

            LogProgress("\r\nUninstall complete");
            if (!Directory.Exists($@"{lunarPath}\logs"))
            {
                Directory.CreateDirectory($@"{lunarPath}\logs");
            }
            File.WriteAllText($@"{lunarPath}\logs\uninstall-{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.txt", txtLog.Text);
            workInProgress = false;
            ChangeCancelToFinish();
            System.Media.SystemSounds.Asterisk.Play();
            SwingMessageBox.Show("Uninstall complete!", "Uninstall", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UploadPayload(string payloadMD5)
        {
            string md5 = payloadMD5.Split(' ')[0];
            string fileName = payloadMD5.Split(' ')[2].Replace("\n", "");
            using (SshClient ssh = new SshClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
            {
                ssh.Connect();

                //Upload TEH payload
                LogProgress("Uploading install scripts");
                using (ScpClient scp = new ScpClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
                {
                    scp.Connect();
                    scp.Upload(new FileInfo($@"{lunarPath}\updatecache\{fileName}"), $"/tmp/{fileName}");
                    try
                    {
                        File.Delete($@"{lunarPath}\updatecache\{fileName}");
                    }
                    catch(Exception ex) 
                    {
                        Debug.Print(ex.ToString());
                    }
                }

                string md5CommandText = $"cd /tmp;[ \"$(md5sum {fileName})\" = " +
                                                    $"\"$(echo $'{md5}  {fileName}')\" ] " +
                                                    "&& echo \"File integrity OK\" || " +
                                                    "echo \"File intergrity FAIL\"";
                string result = ssh.RunCommand(md5CommandText).Result;

                if (!result.Contains("File integrity OK"))
                {
                    throw new InvalidDataException("ERROR: MD5 check failed. Payload is corrupt. Aborting installation.");
                }

                LogProgress("Decompressing into temporary storage");
                string workingDir = "/tmp";
                result = ssh.RunCommand($"cd {workingDir};gunzip -d Project_Lunar*").Result;
                result = ssh.RunCommand($"cd {workingDir};tar -xvf Project_Lunar*").Result;
            }
        }

        private void PerformInstallation(string dev_id)
        {
            using (SshClient ssh = new SshClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
            {
                ssh.Connect();

                try
                {
                    //Remove original data
                    LogProgress("Removing compressed files");
                    string result = string.Empty;
                    string workingDir = "/tmp/mount/nandd/usr/game/";
                    result = ssh.RunCommand($"cd {workingDir};rm alldata.bin").Result;
                    result = ssh.RunCommand($"cd {workingDir};rm alldata.psb.m").Result;

                    //Upload extracted data directories
                    LogProgress("Uploading decompressed data");
                    using (ScpClient scp = new ScpClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
                    {
                        scp.Connect();
                        scp.Upload(new DirectoryInfo($@"{basePath}\system"), "/tmp/mount/nandd/usr/game/system");
                        scp.Upload(new DirectoryInfo($@"{basePath}\{dev_id}"), $"/tmp/mount/nandd/usr/game/{dev_id}");
                    }

                    //Create Symbolic Links
                    LogProgress("Creating symlinks");
                    for (int i = 30; i <= 33; i++)
                    {
                        if (i.ToString("000").Equals(dev_id))
                        {
                            continue;
                        }
                        result = ssh.RunCommand($"cd {workingDir};ln -s {dev_id}/ {i.ToString("000")}").Result;
                    }

                    LogProgress("Running install scripts");
                    //run this on a shell because otherwise the script fails
                    var shell = ssh.CreateShellStream("Lunar", 120, 9999, 120, 9999, 65536);
                    shell.DataReceived += Shell_DataReceived;
                    shell.WriteLine("cd /tmp");
                    shell.WriteLine($"./initrd-install");

                    DateTime now = DateTime.Now;
                    while (ssh.IsConnected)
                    {
                        Thread.Sleep(500);
                    }

                    LogProgress("Performing cleanup");
                    if (Directory.Exists(basePath))
                    {
                        Directory.Delete(basePath, true);
                    }
                }
                catch (Exception ex)
                {
                    SwingMessageBox.Show(ex.ToString());
                }
                if (!Directory.Exists($@"{lunarPath}\logs"))
                {
                    Directory.CreateDirectory($@"{lunarPath}\logs");
                }
                File.WriteAllText($@"{lunarPath}\logs\install-{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.txt", txtLog.Text);
            }
        }
        private void LoadRawBitmap(Bitmap texture, string imageFile)
        {
            byte[] imageData = File.ReadAllBytes(imageFile);
            BitmapData bmpData = texture.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), ImageLockMode.WriteOnly, texture.PixelFormat);
            Marshal.Copy(imageData, 0, bmpData.Scan0, imageData.Length);
            texture.UnlockBits(bmpData);
        }

        private void ExtractAlldata()
        {
            UpdateLabel("Installing Project Lunar");
            if (!Directory.Exists($@"{sysPath}\alldata.psb_extracted"))
            {
                LogProgress("Extracting original data...");
                //Unpack alldata.bin

                string psbKey = m2engage.FindPsbKey($@"{sysPath}\m2engage");
                packer = new MArchivePacker(new ZStandardCodec(), psbKey, 0x40);

                AllDataPacker.UnpackFiles($@"{sysPath}\alldata.psb.m", basePath, packer);
                //string dev_id = BackupData(systemRegion);

                LogProgress("Extraction complete");
            }
        }

        private void DownloadDataFiles(string dev_id)
        {
            if (!Directory.Exists(lunarPath))
            {
                Directory.CreateDirectory(lunarPath);
            }

            if (!Directory.Exists($@"{sysPath}"))
            {
                Directory.CreateDirectory($@"{sysPath}");
            }

            if (!File.Exists($@"{lunarPath}\alldata.bin"))
            {
                //Download alldata files from the console
                LogProgress("Processing original data, please wait");
                try
                {
                    DownloadFileSCP("/tmp/mount/nandd/usr/game/m2engage", $@"{sysPath}\m2engage");
                    DownloadFileSCP("/tmp/mount/nandd/usr/game/alldata.psb.m", $@"{sysPath}\alldata.psb.m");
                    DownloadFileSCP("/tmp/mount/nandd/usr/game/alldata.bin", $@"{sysPath}\alldata.bin");
                }
                catch (Exception ex)
                {
                    SwingMessageBox.Show($"Unable to download files.\r\n{ex.ToString()}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    throw;
                }
            }
        }

        private string RestoreNand()
        {
            string systemRegion = string.Empty;
            using (SshClient ssh = new SshClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
            {
                ssh.Connect();

                string fileName = ssh.RunCommand("cat /tmp/mount/nandd/version").Result.Replace("\n", "");
                systemRegion = fileName.Split('-')[5].Substring(4).ToLower();
                string dev_id = (systemRegion == "jp" ? "030" : (systemRegion == "us" ? "031" : (systemRegion == "eu" ? "032" : "033")));
                string path = $@"{lunarPath}\Backup\{dev_id}";
                string targetPath = $@"{lunarPath}\restore_dir";

                LogProgress($"Device connected: {fileName}");
                LogProgress("Checking for backup files");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (string part in nandPartitions)
                {
                    if (!File.Exists(Path.Combine(path, $@"{fileName}-{part}.gz")))
                    {
                        SwingMessageBox.Show("The file: '" + Path.Combine(path, $@"{fileName}-{part}.gz") + "' is missing! cannot restore!" ,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return "FAIL";
                    }
                }

                LogProgress("Creating directory for restore files");

                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                ssh.RunCommand("cd /");

                LogProgress("Unmounting NAND partitions");

                foreach (string part in nandPartitions)
                {
                    ssh.RunCommand("umount -f /tmp/mount/" + part);
                }

                LogProgress("Starting restore, this will take about 10 min, Please be patient!");

                foreach (string part in nandPartitions)
                {
                    RestoreNAND(ssh, path, fileName, part);
                }

                Thread.Sleep(2000);

                LogProgress("Syncing changes");

                ssh.RunCommand("sync");

                LogProgress("Rebooting console in 2 seconds");
                var shell = ssh.CreateShellStream("Lunar", 120, 9999, 120, 9999, 65536);
                shell.WriteLine("reboot");

                Thread.Sleep(2000);

                Directory.Delete(targetPath, true);
            }

            return systemRegion;
        }

        private string DumpNand()
        {
            string dev_id = string.Empty;
            using (SshClient ssh = new SshClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
            {
                ssh.Connect();

                string fileName = ssh.RunCommand("cat /tmp/mount/nandd/version").Result.Replace("\n", "");
                string systemRegion = fileName.Split('-')[5].Substring(4).ToLower();

                ssh.RunCommand("killall sdl_display &> \"/dev/null\"");
                ssh.RunCommand("sdl_display /opt/project_lunar/PL_NandBack.png &");

                LogProgress($"Device connected: {fileName}");
                LogProgress($"Starting backup, this will take about 10 min if not done before, Please be patient!");

                dev_id = (systemRegion == "jp" ? "030" : (systemRegion == "us" ? "031" : (systemRegion == "eu" ? "032" : "033")));
                sysPath = $@"{lunarPath}\console\{dev_id}";
                basePath = $@"{sysPath}\alldata.psb_extracted";

                string path = $@"{lunarPath}\Backup\{dev_id}";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (var partition in nandPartitions)
                {
                    DumpNAND(ssh, path, fileName, partition);
                }

                //Splash screen update
                ssh.RunCommand("killall sdl_display &> \"/dev/null\"");
                ssh.RunCommand("sdl_display /opt/project_lunar/PL_Installing.png &");
            }

            return dev_id;
        }

        private void VerifyConsoleElegibility(bool restoreMode)
        {
            using (SshClient ssh = new SshClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
            {
                //Wait for SSH connection to establish
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

                string fileName = ssh.RunCommand("cat /tmp/mount/nandd/version").Result.Replace("\n", "");
                if (!fileName.ToLower().Contains("moon"))
                {
                    throw new InvalidOperationException("ERROR: This device is not a Mega Drive or Genesis Mini.\r\nABORTING INSTALLATION");
                }

                string[] rootfsContents = ssh.RunCommand("ls /rootfs_data").Result.ToLower().Split('\n');
                if (rootfsContents.Contains("hakchi"))
                {
                    throw new InvalidOperationException("ERROR: Hakchi mod detected. Project Lunar can only be installed on unmodified consoles.\r\nABORTING INSTALLATION");
                }
                rootfsContents = ssh.RunCommand("ls /tmp/mount/nande").Result.ToLower().Split('\n');
                if (rootfsContents.Contains("hakchi"))
                {
                    throw new InvalidOperationException("ERROR: Hakchi mod detected. Project Lunar can only be installed on unmodified consoles.\r\nABORTING INSTALLATION");
                }

                if (restoreMode == false)
                {
                    string result = ssh.RunCommand("cat /tmp/mount/nandg/pl_version").Result;
                    if (result.Contains("lunar"))
                    {
                        throw new InvalidOperationException("ERROR: Project Lunar is already installed.\r\nABORTING INSTALLATION");
                    }
                }
            }
        }
        private void FelMemBoot()
        {
            if (recoveryMode)
            {
                UpdateLabel("Starting recovery mode");
            }
            else if (restoreMode)
            {
                UpdateLabel("Starting restore mode");
            }
            else
            {
                UpdateLabel("Installing Project Lunar");
            }
            LogProgress("Waiting for FEL mode... (unplug system, power switch on, hold reset, plug system)");
            LogProgress("**Remember the stock SEGA USB cable will NOT work!**");
            while (!Fel.DeviceExists(0x1F3A, 0xEFE8))
            {
                Thread.Sleep(500);
            }
            LogProgress("FEL mode detected. Device: ", false);
            Fel fel = new Fel();
            fel.Fes1Bin = Properties.Resources.fes1;
            fel.Open(0x1F3A, 0xEFE8);
            AWFELVerifyDeviceResponse device = fel.VerifyDevice();
            string deviceName = Encoding.Default.GetString(device.Data, 0, device.DataLength);
            LogProgress(deviceName);
            LogProgress("Transferring FES");
            fel.WriteMemory(0x2000, Properties.Resources.fes1);
            LogProgress("Executing FES");
            fel.Exec(0x2000);
            LogProgress("Transferring uImage");
            fel.WriteMemory(0x43800000, Properties.Resources.uImage);
            LogProgress("Transferring uInitrd");
            fel.WriteMemory(0x48000000, Properties.Resources.uInitrd);
            LogProgress("Transferring custom uBoot");
            fel.WriteMemory(0x47000000, Properties.Resources.u_boot_sun8iw5p1_memboot);
            LogProgress("Transferring env.bin");
            fel.WriteMemory(0x4700D5BC, Properties.Resources.env_initrd);

            LogProgress("Executing custom uBoot");
            fel.Exec(0x47000000);

            LogProgress("Waiting for reboot...");
            //Thread.Sleep(30000); //I clocked 17 seconds for the boot to occur
        }

        private void Shell_DataReceived(object sender, ShellDataEventArgs e)
        {

            string txtToParse = Encoding.Default.GetString(e.Data);

            var removeCharacters = new string[]
            {
                "[00;32m",
                "[00;33m",
                "[00;34m",
                "[00;35m",
                "[00;36m",
                "[01;32m",
                "[01;34m",
                "[0m",
                "[00m",
                "", //Hidden character
                "â–„",
                "â–ˆ",
                "â–ˆ", //With Hidden character
                "â€™",
                "[H[J"
            };

            foreach (var removal in removeCharacters)
            {
                txtToParse = txtToParse.Replace(removal, "");
            }

            // The below is just to prettify the uninstall page,
            // not clean or required but no reason not to...
            txtToParse = txtToParse.Replace("  RAM Usage", "   RAM Usage");
            txtToParse = txtToParse.Replace("Y888P 88", "Y888P  88");
            txtToParse = txtToParse.Replace(" DATA:", "                                      DATA:");

            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke((MethodInvoker)delegate
                {
                    txtLog.AppendText(txtToParse);
                });
            }
            else
            {
                txtLog.AppendText(txtToParse);
            }

            Debug.Write(txtToParse);
        }

        private void RestoreNAND(SshClient ssh, string path, string fileName, string nandName)
        {
            string fileSrc = Path.Combine(path, $@"{fileName}-{nandName}.gz");
            string fileTarget = $@"{lunarPath}\restore_dir\{nandName}.gz";
            string fileFlashTarget = $@"{lunarPath}\restore_dir\{nandName}";
            string processingDir = $@"{lunarPath}\restore_dir";
            DirectoryInfo directorySelected = new DirectoryInfo(processingDir);

            if (File.Exists(fileSrc))
            {
                // Copy and Decompress file
                File.Copy(fileSrc, fileTarget);
                foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.gz"))
                {
                    gzipDecompress(fileToDecompress);
                    File.Delete(fileTarget);
                }
            }

            LogProgress($"Restoring {nandName}");

            byte[] nandData = File.ReadAllBytes(fileFlashTarget);
            using (ScpClient scp = new ScpClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
            {
                scp.Connect();
                int chunkSize = 1024 * 1024; //1 MB
                for (int i = 0; i < nandData.Length / chunkSize; i++)
                {
                    byte[] dataChunk = new byte[chunkSize];
                    int offset = i * chunkSize;
                    Array.Copy(nandData, offset, dataChunk, 0, chunkSize);

                    string chunkName = $"{nandName}-{i}";
                    scp.Upload(new MemoryStream(dataChunk), $"/tmp/{chunkName}");
                    SshCommand cmd = ssh.CreateCommand($"cat /tmp/{chunkName} | dd of=/dev/{nandName} seek={offset / 512} conv=notrunc");
                    cmd.Execute();
                    LogProgress($"{cmd.Error} - ({i + 1} of {nandData.Length / chunkSize})");
                    ssh.RunCommand($"rm /tmp/{chunkName}");
                }
            }
            File.Delete(fileFlashTarget);
        }

        private void DumpNAND(SshClient ssh, string path, string fileName, string nandName)
        {
            if (File.Exists(Path.Combine(path, $@"{fileName}-{nandName}.gz")))
            {
                LogProgress($"{nandName} already exists. Skipping...");
                return;
            }

            LogProgress($"Backing up {nandName}");
            SshCommand command = ssh.CreateCommand($"cd /dev;gzip -c {nandName}");

            var result = command.BeginExecute();
            while (!result.IsCompleted)
            {
                Thread.Sleep(500);
            }

            byte[] data = new byte[command.OutputStream.Length];
            command.OutputStream.Read(data, 0, data.Length);
            File.WriteAllBytes(Path.Combine(path, $@"{fileName}-{nandName}.gz.part"), data);
            File.Move(Path.Combine(path, $@"{fileName}-{nandName}.gz.part"), Path.Combine(path, $@"{fileName}-{nandName}.gz"));
        }

        private void UpdateLabel(string text)
        {
            if (LblStatus.InvokeRequired)
            {
                LblStatus.Invoke((MethodInvoker)delegate
                {
                    LblStatus.Text = text;
                });
            }
            else
            {
                LblStatus.Text = text;
            }
        }

        private void LogProgress(string message, bool addLine = true)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke((MethodInvoker)delegate
                {
                    txtLog.AppendText($"{message}{(addLine ? Environment.NewLine : string.Empty)}");
                });
            }
            else
            {
                txtLog.AppendText($"{message}{(addLine ? Environment.NewLine : string.Empty)}");
            }

            if (pbStatus.InvokeRequired)
            {
                pbStatus.Invoke((MethodInvoker)delegate
                {
                    if (pbStatus.Maximum == pbStatus.Value)
                    {
                        //Prevent exception over max progress bar value...
                        pbStatus.Maximum++;
                    }
                    pbStatus.Value++;
                });
            }
            else
            {
                if (pbStatus.Maximum == pbStatus.Value)
                {
                    pbStatus.Maximum++;
                }
                pbStatus.Value++;
            }
        }

        private void DownloadFile(string source, string destination)
        {
            using (FileStream downloadFile = File.OpenWrite(destination))
            {
                using (SftpClient sftp = new SftpClient(connInfo))
                {
                    sftp.Connect();
                    sftp.DownloadFile(source, downloadFile);
                }
            }
        }

        private void DownloadFileSCP(string source, string destination)
        {
            using (FileStream downloadFile = File.OpenWrite(destination))
            {
                using (ScpClient scp = new ScpClient(SystemIpAddress, SystemRootUsername, SystemRootPassword))
                {
                    scp.Downloading += Scp_Downloading;
                    scp.Connect();
                    scp.Download(source, downloadFile);
                }
            }
        }

        private void Scp_Downloading(object sender, ScpDownloadEventArgs e)
        {
            UpdateLabel($"Downloading..."); //TODO This doesn't work ({e.Downloaded / e.Size * 100}%)");
        }

        private void BackupData(string dev_id)
        {
            LogProgress("Backing up orignal data");
            if (!Directory.Exists($@"{lunarPath}\Backup\{dev_id}"))
            {
                Directory.CreateDirectory($@"{lunarPath}\Backup\{dev_id}");
            }
            File.Copy($@"{sysPath}\alldata.bin", $@"{lunarPath}\Backup\{dev_id}\alldata.bin", true);
            File.Copy($@"{sysPath}\alldata.psb.m", $@"{lunarPath}\Backup\{dev_id}\alldata.psb.m", true);
            File.Copy($@"{sysPath}\m2engage", $@"{lunarPath}\Backup\{dev_id}\m2engage", true);
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

        private void CmdFinish_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FrmInstallRestore_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (workInProgress)
            {
                e.Cancel = SwingMessageBox.Show("ATTENTION! Cancelling duing the install/uninstall procedure may render your system unusable. Are you sure you want to cancel?",
                                           "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Error).Equals(DialogResult.No);
            }
        }

        private string DownloadPayload()
        {
            string payloadName = string.Empty;
            string updateDir = $@"{lunarPath}\updatecache";
            string payloadMD5 = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMinutes(10);
                httpClient.DefaultRequestHeaders.Add("User-Agent", Properties.Resources.PL_UserAgent);
                payloadName = httpClient.GetStringAsync("https://classicmodscloud.com/project_lunar/.desktop_payload/.latest_build").Result.Replace("\n", "");
                LogProgress($@"Downloading payload from the Internet: {payloadName}");

                if (!Directory.Exists(updateDir))
                {
                    Directory.CreateDirectory(updateDir);
                }

                payloadMD5 = httpClient.GetStringAsync($"https://classicmodscloud.com/project_lunar/.desktop_payload/{payloadName.Replace("tar.gz", "md5")}").Result.Split(' ')[0];
                string DownloadFileName = $@"{updateDir}\{payloadName}";
                if (!(File.Exists(DownloadFileName) && GetMd5Hash(DownloadFileName).Equals(payloadMD5)))
                {
                    using (FileStream updateData = new FileStream($@"{updateDir}\{payloadName}", FileMode.Create))
                    {
                        payloadMD5 = httpClient.GetStringAsync($"https://classicmodscloud.com/project_lunar/.desktop_payload/{payloadName.Replace("tar.gz", "md5")}").Result;
                        HttpContent payloadContent = httpClient.GetAsync($"https://classicmodscloud.com/project_lunar/.desktop_payload/{payloadName}").Result.Content;
                        Task download = payloadContent.CopyToAsync(updateData);
                        download.Wait();
                    }
                }
                else
                {
                    payloadMD5 = httpClient.GetStringAsync($"https://classicmodscloud.com/project_lunar/.desktop_payload/{payloadName.Replace("tar.gz", "md5")}").Result;
                }
            }
            return payloadMD5;
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

        public static void gzipCompress(FileInfo fileToCompress)
        {
            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);
                            Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                                fileToCompress.Name, fileToCompress.Length.ToString(), compressedFileStream.Length.ToString());
                        }
                    }
                }
            }
        }

        public static void gzipDecompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
        }
    }
}
