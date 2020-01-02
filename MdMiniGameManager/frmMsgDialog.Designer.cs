namespace ProjectLunarUI
{
    partial class frmMsgDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMsgDialog));
            this.btnOK = new DarkUI.Controls.DarkButton();
            this.icon = new System.Windows.Forms.PictureBox();
            this.messageTxt = new DarkUI.Controls.DarkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCancel = new DarkUI.Controls.DarkButton();
            this.btnYes = new DarkUI.Controls.DarkButton();
            this.btnNo = new DarkUI.Controls.DarkButton();
            this.btnRetry = new DarkUI.Controls.DarkButton();
            this.btnOK2 = new DarkUI.Controls.DarkButton();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(369, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Windows.Forms.Padding(5);
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // icon
            // 
            this.icon.BackgroundImage = global::ProjectLunarUI.Properties.Resources.icn_warn;
            this.icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.icon.Location = new System.Drawing.Point(25, 24);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(81, 81);
            this.icon.TabIndex = 11;
            this.icon.TabStop = false;
            // 
            // messageTxt
            // 
            this.messageTxt.AutoUpdateHeight = true;
            this.messageTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.messageTxt.Location = new System.Drawing.Point(134, 12);
            this.messageTxt.MaximumSize = new System.Drawing.Size(310, 108);
            this.messageTxt.MinimumSize = new System.Drawing.Size(310, 108);
            this.messageTxt.Name = "messageTxt";
            this.messageTxt.Size = new System.Drawing.Size(310, 108);
            this.messageTxt.TabIndex = 10;
            this.messageTxt.Text = "Message goes here!";
            this.messageTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(-2, 130);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(465, 50);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(369, 140);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(5);
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(288, 140);
            this.btnYes.Name = "btnYes";
            this.btnYes.Padding = new System.Windows.Forms.Padding(5);
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 4;
            this.btnYes.Text = "Yes";
            this.btnYes.Visible = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(369, 140);
            this.btnNo.Name = "btnNo";
            this.btnNo.Padding = new System.Windows.Forms.Padding(5);
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 2;
            this.btnNo.Text = "No";
            this.btnNo.Visible = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnRetry
            // 
            this.btnRetry.Location = new System.Drawing.Point(288, 140);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Padding = new System.Windows.Forms.Padding(5);
            this.btnRetry.Size = new System.Drawing.Size(75, 23);
            this.btnRetry.TabIndex = 5;
            this.btnRetry.Text = "Retry";
            this.btnRetry.Visible = false;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // btnOK2
            // 
            this.btnOK2.Location = new System.Drawing.Point(288, 140);
            this.btnOK2.Name = "btnOK2";
            this.btnOK2.Padding = new System.Windows.Forms.Padding(5);
            this.btnOK2.Size = new System.Drawing.Size(75, 23);
            this.btnOK2.TabIndex = 6;
            this.btnOK2.Text = "OK";
            this.btnOK2.Visible = false;
            this.btnOK2.Click += new System.EventHandler(this.btnOK2_Click);
            // 
            // frmMsgDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 173);
            this.Controls.Add(this.btnOK2);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.messageTxt);
            this.Controls.Add(this.icon);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pictureBox1);
            this.FlatBorder = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMsgDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alert!";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DarkUI.Controls.DarkButton btnOK;
        private System.Windows.Forms.PictureBox icon;
        private DarkUI.Controls.DarkLabel messageTxt;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DarkUI.Controls.DarkButton btnCancel;
        private DarkUI.Controls.DarkButton btnYes;
        private DarkUI.Controls.DarkButton btnNo;
        private DarkUI.Controls.DarkButton btnRetry;
        private DarkUI.Controls.DarkButton btnOK2;
    }
}