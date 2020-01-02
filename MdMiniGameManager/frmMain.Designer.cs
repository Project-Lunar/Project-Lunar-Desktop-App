namespace ProjectLunarUI
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmdInstall = new DarkUI.Controls.DarkButton();
            this.cmdGameManager = new DarkUI.Controls.DarkButton();
            this.mmcHyperLink = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmcHyperLink)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::ProjectLunarUI.Properties.Resources.PL_fullColour_Large2;
            this.pictureBox1.Location = new System.Drawing.Point(120, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(360, 166);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // cmdInstall
            // 
            this.cmdInstall.Location = new System.Drawing.Point(235, 217);
            this.cmdInstall.Name = "cmdInstall";
            this.cmdInstall.Padding = new System.Windows.Forms.Padding(5);
            this.cmdInstall.Size = new System.Drawing.Size(150, 42);
            this.cmdInstall.TabIndex = 1;
            this.cmdInstall.Text = "Install/Uninstall";
            this.cmdInstall.Click += new System.EventHandler(this.CmdInstall_Click);
            this.cmdInstall.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdInstall_MouseUp);
            // 
            // cmdGameManager
            // 
            this.cmdGameManager.Location = new System.Drawing.Point(235, 276);
            this.cmdGameManager.Name = "cmdGameManager";
            this.cmdGameManager.Padding = new System.Windows.Forms.Padding(5);
            this.cmdGameManager.Size = new System.Drawing.Size(150, 42);
            this.cmdGameManager.TabIndex = 2;
            this.cmdGameManager.Text = "Open Game Manager";
            this.cmdGameManager.Click += new System.EventHandler(this.CmdGameManager_Click);
            // 
            // mmcHyperLink
            // 
            this.mmcHyperLink.BackColor = System.Drawing.Color.Transparent;
            this.mmcHyperLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mmcHyperLink.Location = new System.Drawing.Point(463, 294);
            this.mmcHyperLink.Name = "mmcHyperLink";
            this.mmcHyperLink.Size = new System.Drawing.Size(110, 16);
            this.mmcHyperLink.TabIndex = 3;
            this.mmcHyperLink.TabStop = false;
            this.mmcHyperLink.Click += new System.EventHandler(this.mmcHyperLink_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ProjectLunarUI.Properties.Resources.AutoBoot_Screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(604, 343);
            this.Controls.Add(this.mmcHyperLink);
            this.Controls.Add(this.cmdGameManager);
            this.Controls.Add(this.cmdInstall);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project Lunar";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmcHyperLink)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private DarkUI.Controls.DarkButton cmdInstall;
        private DarkUI.Controls.DarkButton cmdGameManager;
        private System.Windows.Forms.PictureBox mmcHyperLink;
    }
}