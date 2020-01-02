using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FelLib;
using DarkUI.Collections;
using DarkUI.Config;
using DarkUI.Controls;
using DarkUI.Docking;
using DarkUI.Forms;
using DarkUI.Renderers;

namespace ProjectLunarUI
{
    public partial class frmMain : DarkForm
    {
        private bool shiftPressed;
        private bool controlPressed;

        public frmMain()
        {
            InitializeComponent();
            this.Text = this.Text + " - Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void CmdInstall_Click(object sender, EventArgs e)
        {
            frmInstallRestore installRestoreForm = new frmInstallRestore();

            if (shiftPressed)
            {
                shiftPressed = false;
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
                installRestoreForm.RestoreMode = true;
            }
            else if (controlPressed)
            {
                installRestoreForm.RecoveryMode = true;
            }

            if (installRestoreForm.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void CmdGameManager_Click(object sender, EventArgs e)
        {
            frmGameManager frmGameManager = new frmGameManager();
            frmGameManager.Show(this);
            if (frmGameManager.DialogResult.Equals(DialogResult.Cancel))
            {
                return;
            }
            this.Hide();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void mmcHyperLink_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://modmyclassic.com");
            Process.Start(sInfo);
        }

        private void cmdInstall_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.Equals(Keys.Shift | Keys.ShiftKey))
            {
                shiftPressed = true;
            }
            if (e.KeyData.Equals(Keys.Control | Keys.ControlKey))
            {
                controlPressed = true;
            }
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData.Equals(Keys.ShiftKey))
            {
                shiftPressed = false;
            }
            if (e.KeyData.Equals(Keys.ControlKey))
            {
                controlPressed = false;
            }
            if (e.KeyData.Equals(Keys.F4))
            {
                ProcessStartInfo process = new ProcessStartInfo("explorer.exe", $@"{Application.StartupPath}\lunar_data\");
                process.UseShellExecute = true;
                Process.Start(process);
            }
        }
    }
}
