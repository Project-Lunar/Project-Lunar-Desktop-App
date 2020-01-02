using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Collections;
using DarkUI.Config;
using DarkUI.Controls;
using DarkUI.Docking;
using DarkUI.Forms;
using DarkUI.Renderers;
using Newtonsoft.Json.Linq;

// Possible icon choices
// "chat":"icn_chat"
// "construction":"icn_construction"
// "download":"icn_download"
// "electric":"icn_elec"
// "electron":"icn_electron"
// "fire":"icn_fire"
// "info":"icn_info"
// "announcement":"icn_megaphone"
// "noaccess":"icn_noaccess"
// "nuke":"icn_nuke"
// "options":"icn_option"
// "sdcard":"icn_sdcard"
// "stop":"icn_stop"
// "upload":"icn_upload"
// "warn":"icn_warn"
// "wizard":"icn_wizard"
// "question":"icn_question"
// "checkmark":"icn_ok"

// Possible dialog types
// OK (OK)
// OKCAN (OK, Cancel)
// YESNO (Yes, No)
// YESNOCAN (Yes, No, Cancel)
// RETRY (Retry, Cancel)

namespace ProjectLunarUI
{
    public partial class frmMsgDialog : DarkForm
    {
        private static JObject msgIconsJSON = JObject.Parse(Properties.Resources.msgIconsJson);
        private static string[] possibleTypes = { "OK", "OKCAN", "YESNO", "RETRY", "YESNOCAN"};
        private bool okToRun = true;
        private string iconFilename = null;

        public frmMsgDialog(string dialogIcon, string dialogTitle, string dialogMessage, string dialogType)
        {
            InitializeComponent();
            
            // Input validation & parsing
            try
            {
                iconFilename = (string)msgIconsJSON.SelectToken(dialogIcon);
            }
            catch
            {
                okToRun = false;
            }
            if (string.IsNullOrEmpty(dialogTitle))
            {
                dialogTitle = "Project Lunar";
            }
            if (string.IsNullOrEmpty(dialogMessage))
            {
                okToRun = false;
            }
            if (possibleTypes.Contains(dialogType) == false)
            {
                okToRun = false;
            }

            // Display message or error
            if (okToRun == true)
            {
                switch (dialogType)
                {
                    case "OK":
                        btnOK.Visible = true;
                        this.AcceptButton = this.btnOK;
                        break;
                    case "OKCAN":
                        btnOK2.Visible = true;
                        btnCancel.Visible = true;
                        this.AcceptButton = this.btnOK;
                        this.CancelButton = this.btnCancel;
                        break;
                    case "YESNO":
                        btnYes.Visible = true;
                        btnNo.Visible = true;
                        this.AcceptButton = this.btnYes;
                        this.CancelButton = this.btnNo;
                        break;
                    case "YESNOCAN":
                        btnYes.Visible = true;
                        btnNo.Visible = true;
                        btnCancel.Visible = true;
                        btnNo.Left = btnOK2.Left;
                        btnYes.Left = btnNo.Left - (btnCancel.Left - btnNo.Left);
                        btnYes.TabIndex = 0;
                        btnNo.TabIndex = 1;
                        btnCancel.TabIndex = 2;
                        this.AcceptButton = this.btnYes;
                        this.CancelButton = this.btnCancel;
                        break;
                    case "RETRY":
                        btnRetry.Visible = true;
                        btnCancel.Visible = true;
                        this.AcceptButton = this.btnRetry;
                        this.CancelButton = this.btnCancel;
                        break;
                }
                this.Text = dialogTitle;
                icon.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject(iconFilename);
                messageTxt.Text = dialogMessage;
            } 
            else
            {
                this.Text = "CODING ERROR";
                icon.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("icn_stop");
                messageTxt.Text = "Bad use of the messaging system! Check your code";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnOK2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }
    }
}
