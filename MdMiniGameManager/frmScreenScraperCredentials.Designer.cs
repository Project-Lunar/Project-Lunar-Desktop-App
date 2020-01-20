namespace ProjectLunarUI
{
    partial class frmScreenScraperCredentials
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.darkLabel2 = new DarkUI.Controls.DarkLabel();
            this.txtSSID = new DarkUI.Controls.DarkTextBox();
            this.txtSSPassword = new DarkUI.Controls.DarkTextBox();
            this.cmdSave = new DarkUI.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(21, 20);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(46, 13);
            this.darkLabel1.TabIndex = 1;
            this.darkLabel1.Text = "User ID:";
            // 
            // darkLabel2
            // 
            this.darkLabel2.AutoSize = true;
            this.darkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel2.Location = new System.Drawing.Point(21, 46);
            this.darkLabel2.Name = "darkLabel2";
            this.darkLabel2.Size = new System.Drawing.Size(56, 13);
            this.darkLabel2.TabIndex = 2;
            this.darkLabel2.Text = "Password:";
            // 
            // txtSSID
            // 
            this.txtSSID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtSSID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSSID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtSSID.Location = new System.Drawing.Point(83, 18);
            this.txtSSID.Name = "txtSSID";
            this.txtSSID.Size = new System.Drawing.Size(173, 20);
            this.txtSSID.TabIndex = 3;
            // 
            // txtSSPassword
            // 
            this.txtSSPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtSSPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSSPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtSSPassword.Location = new System.Drawing.Point(83, 44);
            this.txtSSPassword.Name = "txtSSPassword";
            this.txtSSPassword.PasswordChar = '•';
            this.txtSSPassword.Size = new System.Drawing.Size(173, 20);
            this.txtSSPassword.TabIndex = 4;
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(104, 79);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Padding = new System.Windows.Forms.Padding(5);
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 5;
            this.cmdSave.Text = "Save";
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // frmScreenScraperCredentials
            // 
            this.AcceptButton = this.cmdSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 114);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.txtSSPassword);
            this.Controls.Add(this.txtSSID);
            this.Controls.Add(this.darkLabel2);
            this.Controls.Add(this.darkLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScreenScraperCredentials";
            this.Text = "ScreenScraper Credentials";
            this.Load += new System.EventHandler(this.frmScreenScraperCredentials_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkLabel darkLabel2;
        private DarkUI.Controls.DarkTextBox txtSSID;
        private DarkUI.Controls.DarkTextBox txtSSPassword;
        private DarkUI.Controls.DarkButton cmdSave;
    }
}