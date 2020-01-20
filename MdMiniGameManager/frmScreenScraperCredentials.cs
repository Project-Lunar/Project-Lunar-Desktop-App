using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectLunarUI
{
    public partial class frmScreenScraperCredentials : DarkForm
    {
        public frmScreenScraperCredentials()
        {
            InitializeComponent();
        }

        private void frmScreenScraperCredentials_Load(object sender, EventArgs e)
        {
            Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            try
            {
                txtSSID.Text = appConfig.AppSettings.Settings["ssid"].Value;
                string base64pwd = appConfig.AppSettings.Settings["sspassword"].Value;
                txtSSPassword.Text = Encoding.UTF8.GetString(Convert.FromBase64String(base64pwd));
            }
            catch { }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string base64pwd = Convert.ToBase64String(Encoding.UTF8.GetBytes(txtSSPassword.Text));
            Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            try
            {
                appConfig.AppSettings.Settings["ssid"].Value = txtSSID.Text;
                appConfig.AppSettings.Settings["sspassword"].Value = base64pwd;
            }
            catch
            {
                appConfig.AppSettings.Settings.Add("ssid", txtSSID.Text);
                appConfig.AppSettings.Settings.Add("sspassword", base64pwd);
            }
            appConfig.Save();
        }
    }
}
