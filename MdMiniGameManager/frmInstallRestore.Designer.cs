namespace ProjectLunarUI
{
    partial class frmInstallRestore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInstallRestore));
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            this.LblStatus = new DarkUI.Controls.DarkLabel();
            this.txtLog = new DarkUI.Controls.DarkTextBox();
            this.cmdFinish = new DarkUI.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // pbStatus
            // 
            this.pbStatus.Location = new System.Drawing.Point(12, 45);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(512, 23);
            this.pbStatus.TabIndex = 0;
            // 
            // LblStatus
            // 
            this.LblStatus.AutoSize = true;
            this.LblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.LblStatus.Location = new System.Drawing.Point(12, 29);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(106, 13);
            this.LblStatus.TabIndex = 1;
            this.LblStatus.Text = "Checking mod status";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtLog.Location = new System.Drawing.Point(12, 74);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(512, 325);
            this.txtLog.TabIndex = 2;
            // 
            // cmdFinish
            // 
            this.cmdFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdFinish.Location = new System.Drawing.Point(230, 405);
            this.cmdFinish.Name = "cmdFinish";
            this.cmdFinish.Padding = new System.Windows.Forms.Padding(5);
            this.cmdFinish.Size = new System.Drawing.Size(75, 23);
            this.cmdFinish.TabIndex = 3;
            this.cmdFinish.Text = "Cancel";
            this.cmdFinish.Click += new System.EventHandler(this.CmdFinish_Click);
            // 
            // frmInstallRestore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 435);
            this.Controls.Add(this.cmdFinish);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.LblStatus);
            this.Controls.Add(this.pbStatus);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInstallRestore";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Install or Remove Project Lunar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmInstallRestore_FormClosing);
            this.Load += new System.EventHandler(this.FrmInstallRestore_Load);
            this.Shown += new System.EventHandler(this.FrmInstallRestore_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbStatus;
        private DarkUI.Controls.DarkLabel LblStatus;
        private DarkUI.Controls.DarkTextBox txtLog;
        private DarkUI.Controls.DarkButton cmdFinish;
    }
}