using DarkUI.Forms;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectLunarUI
{
    public partial class frmManageMods : DarkForm
    {
        private int modCount = 0;
        private int selectedNumberOfMods = 0;
        frmLoading loadingForm = new frmLoading("");
        Task checkModTask;
        Task installerTask;
        CancellationTokenSource chkModCts = new CancellationTokenSource();
        CancellationTokenSource installCts = new CancellationTokenSource();

        public frmManageMods()
        {
            InitializeComponent();
        }

        private void frmManageMods_Shown(object sender, EventArgs e)
        {
            toolStripStatusLabel.Alignment = ToolStripItemAlignment.Right;
            UpdateStatus("Checking for installed mods");
            //Replace with spinner
            LockButtons(true);
            this.Cursor = Cursors.WaitCursor;
            checkModTask = Task.Run(() => GetInstalledMods(), chkModCts.Token);
            this.Cursor = Cursors.Default;
            UpdateStatus("Ready");
        }

        private void LockButtons(bool locked)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    cmdAddNew.Enabled = !locked;
                    cmdRemove.Enabled = !locked;
                });
            }
            else
            {
                cmdAddNew.Enabled = !locked;
                cmdRemove.Enabled = !locked;
            }
        }

        private void GetInstalledMods()
        {
            ShowLoadingBox("CHECKING MODS STATUS");
            using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
            {
                while (!ssh.IsConnected)
                {
                    try
                    {
                        ssh.Connect();
                    }
                    catch
                    {
                        if (chkModCts.IsCancellationRequested)
                        {
                            chkModCts.Dispose();
                            return;
                        }
                        Thread.Sleep(500);
                    }
                }

                string result = ssh.RunCommand("mod-list").Result;

                StringReader reader = new StringReader(result);
                reader.ReadLine();
                reader.ReadLine();

                string line = string.Empty;
                List<LunarMod> modList = new List<LunarMod>();
                while (true)
                {
                    line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        break;
                    }

                    while (line.IndexOf("  ") >= 0)
                    {
                        line = line.Replace("  ", " ");
                    }

                    string[] values = line.Split(' ');
                    modList.Add(new LunarMod() { Name = values[values.Length - 2], Version = values[values.Length - 1] });
                    modCount++;

                    if (noModsPanel.Visible == true)
                    {
                        if (noModsPanel.InvokeRequired)
                        {
                            noModsPanel.Invoke((MethodInvoker)delegate
                            {
                                noModsPanel.Visible = false;
                            });
                        }
                        else
                        {
                            noModsPanel.Visible = false;
                        }
                    }
                }

                if (grdMods.InvokeRequired)
                {
                    grdMods.Invoke((MethodInvoker)delegate
                    {
                        grdMods.DataSource = modList;
                    });
                }
                else
                {
                    grdMods.DataSource = modList;
                }
            }
            
            if (grdMods.Rows.Count.Equals(0))
            {
                if (noModsPanel.InvokeRequired)
                {
                    noModsPanel.Invoke((MethodInvoker)delegate
                    {
                        noModsPanel.Visible = true;
                    });
                }
                else
                {
                    noModsPanel.Visible = true;
                }
            }
            CloseLoadingBox();
            UpdateStatus("Ready");
            LockButtons(false);
        }

        private void cmdAddNew_Click(object sender, EventArgs e)
        {
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select mod file(s)...";
            openFileDialog.Filter = "Mod Packages|*.mod";
            openFileDialog.FileName = "*.mod";

            if (openFileDialog.ShowDialog().Equals(DialogResult.Cancel))
            {
                return;
            }

            LockButtons(true);
            installerTask = Task.Run(() => AddMods(openFileDialog.FileNames), installCts.Token);
        }

        private void AddMods(string[] fileNames)
        {
            ShowLoadingBox("INSTALLING MOD(S)");
            foreach (string filePath in fileNames)
            {
                try
                {
                    using (ScpClient scp = new ScpClient("169.254.215.100", "root", "5A7213"))
                    {
                        string md5 = GetMd5Hash(filePath);
                        scp.Connect();
                        string fileName = Path.GetFileName(filePath);
                        string parsedfileName = fileName.Replace("_SEGAMD", "").Replace(".mod", "").ToUpper();
                        UpdateStatus("Uploading " + fileName);
                        scp.Upload(File.OpenRead(filePath), $"/tmp/{fileName}");

                        using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
                        {
                            ssh.Connect();
                            string md5CommandText = $"cd /tmp;[ \"$(md5sum {fileName})\" = " +
                                                              $"\"$(echo $'{md5}  {fileName}')\" ] " +
                                                              "&& echo \"File integrity OK\" || " +
                                                              "echo \"File intergrity FAIL\"";
                            string result = ssh.RunCommand(md5CommandText).Result;

                            if (result.Contains("File integrity OK"))
                            {
                                UpdateStatus("Installing " + fileName);
                                result = ssh.RunCommand($"cd /tmp ; mod-install {fileName}").Result;
                                if (result.Contains("[PROJECT LUNAR](ERROR)"))
                                {
                                    result = result.Replace("[PROJECT LUNAR](ERROR)", "");
                                    throw new InvalidDataException(parsedfileName + "'\n\rFailed to install!\n\rReason: " + result);
                                }
                            }
                            else
                            {
                                throw new InvalidDataException(parsedfileName + "'\n\rFailed to install!\n\rReason: Mod has corrupted in transit. Please try again.");
                            }
                        }
                    }
                }
                catch (InvalidDataException ex)
                {
                    string failureResult = ex.ToString();
                    Console.WriteLine(failureResult);
                    failureResult = failureResult.Replace("System.IO.InvalidDataException: ", "'");
                    string[] splitFailureResult = failureResult.Split(new string[] { "at ProjectLunarUI" }, StringSplitOptions.None);
                    SwingMessageBox.Show(splitFailureResult[0], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
            {
                ssh.Connect();
                ssh.RunCommand("killall sdl_display &> \"/dev/null\"");
                ssh.RunCommand("sdl_display /opt/project_lunar/etc/project_lunar/IMG/PL_UpdateDone.png &");
                UpdateStatus("Restarting console");
                Thread.Sleep(5000);
                var shell = ssh.CreateShellStream("Lunar", 120, 9999, 120, 9999, 65536);
                shell.DataReceived += Shell_DataReceived;
                shell.WriteLine($"cd /");
                Thread.Sleep(500);
                shell.WriteLine($"kill_ui_programs");
                Thread.Sleep(500);
                shell.WriteLine($"restart");
                while (ssh.IsConnected)
                {
                    if (installCts.IsCancellationRequested)
                    {
                        installCts.Dispose();
                        return;
                    }
                    Thread.Sleep(500);
                }
                Debug.WriteLine("Finished wait.");
            }
            UpdateStatus("Waiting for console");
            ShowLoadingBox("RESTARTING CONSOLE");
            checkModTask = Task.Run(() => GetInstalledMods(), chkModCts.Token);
            //SwingMessageBox.Show("Mods installed successfuly.", "Mod Install", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Shell_DataReceived(object sender, ShellDataEventArgs e)
        {
            Debug.Write(Encoding.Default.GetString(e.Data));
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

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            selectedNumberOfMods = 0;
            List<string> modList = new List<string>();
            foreach (DataGridViewRow gridRow in grdMods.SelectedRows)
            {
                modList.Add(gridRow.Cells[0].Value.ToString());
                selectedNumberOfMods++;
            }

            if (selectedNumberOfMods == 0)
            {
                SwingMessageBox.Show("You haven't selected any mods to uninstall!", "Mod Uninistall", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (SwingMessageBox.Show($"Do you really want to remove {string.Join(", ", modList.ToArray())}?", "Remove Mod",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
            {
                return;
            }

            LockButtons(true);

            installerTask = Task.Run(() => RemoveSelectedMods(), installCts.Token);
            // Below message Will be replaced by spinner
        }

        private void RemoveSelectedMods()
        {
            ShowLoadingBox("REMOVING MOD(S)");

            foreach (DataGridViewRow gridRow in grdMods.SelectedRows)
            {
                string modName = gridRow.Cells[0].Value.ToString();
                try
                {
                    using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
                    {
                        ssh.Connect();
                        UpdateStatus("Uninstalling " + modName);
                        string result = ssh.RunCommand($"mod-remove {modName}").Result;
                    }
                }
                catch
                {

                }
            }

            using (SshClient ssh = new SshClient("169.254.215.100", "root", "5A7213"))
            {
                ssh.Connect();
                ssh.RunCommand("killall sdl_display &> \"/dev/null\"");
                ssh.RunCommand("sdl_display /opt/project_lunar/etc/project_lunar/IMG/PL_UpdateDone.png &");
                UpdateStatus("Restarting console");
                Thread.Sleep(5000);
                var shell = ssh.CreateShellStream("Lunar", 120, 9999, 120, 9999, 65536);
                shell.DataReceived += Shell_DataReceived;
                shell.WriteLine($"cd /");
                Thread.Sleep(500);
                shell.WriteLine($"kill_ui_programs");
                Thread.Sleep(500);
                while (ssh.IsConnected)
                {
                    if (installCts.IsCancellationRequested)
                    {
                        installCts.Dispose();
                        return;
                    }
                    Thread.Sleep(500);
                }
                Debug.WriteLine("Finished wait.");
            }
            UpdateStatus("Waiting for console");
            ShowLoadingBox("RESTARTING CONSOLE");
            //SwingMessageBox.Show("Mods removed successfuly.", "Mod Uninistall", MessageBoxButtons.OK, MessageBoxIcon.Information);
            checkModTask = Task.Run(() => GetInstalledMods(), chkModCts.Token);
        }

        private void UpdateStatus(string text)
        {
            toolStripStatusLabel.Text = $"Status: {text}";
            Application.DoEvents();
        }

        private void btnMMCwebsite_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://modmyclassic.com/project-lunar-mods");
            Process.Start(sInfo);
        }

        private void ShowLoadingBox(string message)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    loadingForm.status.Text = message;
                    if (!loadingForm.Visible)
                    {
                        loadingForm.Show(this);
                    }
                });
            }
            else
            {
                loadingForm.status.Text = message;
                if (!loadingForm.Visible)
                {
                    loadingForm.Show(this);
                }
            }
        }

        private void CloseLoadingBox()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    loadingForm.Hide();
                });
            }
            else
            {
                loadingForm.Hide();
            }
        }

        private void frmManageMods_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkModTask != null)
            {
                if (checkModTask.Status.Equals(TaskStatus.Running))
                {
                    chkModCts.Cancel();
                }
            }
            if (installerTask != null)
            {
                if (installerTask.Status.Equals(TaskStatus.Running))
                {
                    installCts.Cancel();
                }
            }
        }
    }

    class LunarMod
    {
        public string Name
        {
            get; set;
        }

        public string Version
        {
            get; set;
        }
    }
}
