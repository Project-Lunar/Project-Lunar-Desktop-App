namespace ProjectLunarUI
{
    partial class frmAddGame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddGame));
            this.picSpine = new System.Windows.Forms.PictureBox();
            this.picBoxArt = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new DarkUI.Controls.DarkGroupBox();
            this.cmdModMyClassicArt = new DarkUI.Controls.DarkButton();
            this.label9 = new DarkUI.Controls.DarkLabel();
            this.label8 = new DarkUI.Controls.DarkLabel();
            this.label7 = new DarkUI.Controls.DarkLabel();
            this.rdoUsModern = new DarkUI.Controls.DarkRadioButton();
            this.rdoJapan = new DarkUI.Controls.DarkRadioButton();
            this.rdoUsClassic = new DarkUI.Controls.DarkRadioButton();
            this.rdoEuModern = new DarkUI.Controls.DarkRadioButton();
            this.rdoEuClassic = new DarkUI.Controls.DarkRadioButton();
            this.cmdLoadSpine = new DarkUI.Controls.DarkButton();
            this.cmdLoadBox = new DarkUI.Controls.DarkButton();
            this.grpDetails = new DarkUI.Controls.DarkGroupBox();
            this.chkApplyIPS = new DarkUI.Controls.DarkCheckBox();
            this.chk6ButtonHack = new DarkUI.Controls.DarkCheckBox();
            this.label10 = new DarkUI.Controls.DarkLabel();
            this.cboScraper = new DarkUI.Controls.DarkComboBox();
            this.label6 = new DarkUI.Controls.DarkLabel();
            this.lstScrapeItems = new System.Windows.Forms.ListBox();
            this.picGenre = new System.Windows.Forms.PictureBox();
            this.picNumPlayers = new System.Windows.Forms.PictureBox();
            this.txtCopyright = new DarkUI.Controls.DarkTextBox();
            this.label5 = new DarkUI.Controls.DarkLabel();
            this.cboGenre = new DarkUI.Controls.DarkComboBox();
            this.label4 = new DarkUI.Controls.DarkLabel();
            this.cboPlayerNum = new DarkUI.Controls.DarkComboBox();
            this.label3 = new DarkUI.Controls.DarkLabel();
            this.cmdScrape = new DarkUI.Controls.DarkButton();
            this.txtTname = new DarkUI.Controls.DarkTextBox();
            this.txtDescription = new DarkUI.Controls.DarkTextBox();
            this.label2 = new DarkUI.Controls.DarkLabel();
            this.label1 = new DarkUI.Controls.DarkLabel();
            this.cmdSave = new DarkUI.Controls.DarkButton();
            this.cmdCancel = new DarkUI.Controls.DarkButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new DarkUI.Controls.DarkGroupBox();
            this.rdoU = new DarkUI.Controls.DarkRadioButton();
            this.rdoE = new DarkUI.Controls.DarkRadioButton();
            this.rdoJ = new DarkUI.Controls.DarkRadioButton();
            this.rdoDetected = new DarkUI.Controls.DarkRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.picSpine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxArt)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGenre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNumPlayers)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // picSpine
            // 
            this.picSpine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.picSpine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSpine.Location = new System.Drawing.Point(11, 18);
            this.picSpine.Margin = new System.Windows.Forms.Padding(2);
            this.picSpine.Name = "picSpine";
            this.picSpine.Size = new System.Drawing.Size(32, 218);
            this.picSpine.TabIndex = 50;
            this.picSpine.TabStop = false;
            // 
            // picBoxArt
            // 
            this.picBoxArt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.picBoxArt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBoxArt.Location = new System.Drawing.Point(44, 18);
            this.picBoxArt.Margin = new System.Windows.Forms.Padding(2);
            this.picBoxArt.Name = "picBoxArt";
            this.picBoxArt.Size = new System.Drawing.Size(154, 218);
            this.picBoxArt.TabIndex = 49;
            this.picBoxArt.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.groupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.groupBox1.Controls.Add(this.cmdModMyClassicArt);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.rdoUsModern);
            this.groupBox1.Controls.Add(this.rdoJapan);
            this.groupBox1.Controls.Add(this.rdoUsClassic);
            this.groupBox1.Controls.Add(this.rdoEuModern);
            this.groupBox1.Controls.Add(this.rdoEuClassic);
            this.groupBox1.Controls.Add(this.cmdLoadSpine);
            this.groupBox1.Controls.Add(this.cmdLoadBox);
            this.groupBox1.Controls.Add(this.picBoxArt);
            this.groupBox1.Controls.Add(this.picSpine);
            this.groupBox1.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Location = new System.Drawing.Point(555, 6);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(206, 396);
            this.groupBox1.TabIndex = 51;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Box Art";
            // 
            // cmdModMyClassicArt
            // 
            this.cmdModMyClassicArt.Location = new System.Drawing.Point(11, 335);
            this.cmdModMyClassicArt.Margin = new System.Windows.Forms.Padding(2);
            this.cmdModMyClassicArt.Name = "cmdModMyClassicArt";
            this.cmdModMyClassicArt.Padding = new System.Windows.Forms.Padding(5);
            this.cmdModMyClassicArt.Size = new System.Drawing.Size(187, 26);
            this.cmdModMyClassicArt.TabIndex = 53;
            this.cmdModMyClassicArt.Text = "Use ModMyClassic Artwork";
            this.cmdModMyClassicArt.Click += new System.EventHandler(this.CmdModMyClassicArt_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label9.Location = new System.Drawing.Point(152, 244);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(22, 13);
            this.label9.TabIndex = 52;
            this.label9.Text = "US";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label8.Location = new System.Drawing.Point(91, 244);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 13);
            this.label8.TabIndex = 52;
            this.label8.Text = "JP";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label7.Location = new System.Drawing.Point(27, 244);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 13);
            this.label7.TabIndex = 52;
            this.label7.Text = "EU";
            // 
            // rdoUsModern
            // 
            this.rdoUsModern.AutoSize = true;
            this.rdoUsModern.Location = new System.Drawing.Point(137, 278);
            this.rdoUsModern.Name = "rdoUsModern";
            this.rdoUsModern.Size = new System.Drawing.Size(61, 17);
            this.rdoUsModern.TabIndex = 11;
            this.rdoUsModern.Text = "Modern";
            this.rdoUsModern.CheckedChanged += new System.EventHandler(this.rdoEuClassic_CheckedChanged);
            // 
            // rdoJapan
            // 
            this.rdoJapan.AutoSize = true;
            this.rdoJapan.Location = new System.Drawing.Point(73, 260);
            this.rdoJapan.Name = "rdoJapan";
            this.rdoJapan.Size = new System.Drawing.Size(62, 17);
            this.rdoJapan.TabIndex = 9;
            this.rdoJapan.Text = "Generic";
            this.rdoJapan.CheckedChanged += new System.EventHandler(this.rdoEuClassic_CheckedChanged);
            // 
            // rdoUsClassic
            // 
            this.rdoUsClassic.AutoSize = true;
            this.rdoUsClassic.Location = new System.Drawing.Point(137, 260);
            this.rdoUsClassic.Name = "rdoUsClassic";
            this.rdoUsClassic.Size = new System.Drawing.Size(58, 17);
            this.rdoUsClassic.TabIndex = 10;
            this.rdoUsClassic.Text = "Classic";
            this.rdoUsClassic.CheckedChanged += new System.EventHandler(this.rdoEuClassic_CheckedChanged);
            // 
            // rdoEuModern
            // 
            this.rdoEuModern.AutoSize = true;
            this.rdoEuModern.Location = new System.Drawing.Point(11, 278);
            this.rdoEuModern.Name = "rdoEuModern";
            this.rdoEuModern.Size = new System.Drawing.Size(61, 17);
            this.rdoEuModern.TabIndex = 8;
            this.rdoEuModern.Text = "Modern";
            this.rdoEuModern.CheckedChanged += new System.EventHandler(this.rdoEuClassic_CheckedChanged);
            // 
            // rdoEuClassic
            // 
            this.rdoEuClassic.AutoSize = true;
            this.rdoEuClassic.Checked = true;
            this.rdoEuClassic.Location = new System.Drawing.Point(11, 260);
            this.rdoEuClassic.Name = "rdoEuClassic";
            this.rdoEuClassic.Size = new System.Drawing.Size(58, 17);
            this.rdoEuClassic.TabIndex = 7;
            this.rdoEuClassic.TabStop = true;
            this.rdoEuClassic.Text = "Classic";
            this.rdoEuClassic.CheckedChanged += new System.EventHandler(this.rdoEuClassic_CheckedChanged);
            // 
            // cmdLoadSpine
            // 
            this.cmdLoadSpine.Location = new System.Drawing.Point(11, 300);
            this.cmdLoadSpine.Margin = new System.Windows.Forms.Padding(2);
            this.cmdLoadSpine.Name = "cmdLoadSpine";
            this.cmdLoadSpine.Padding = new System.Windows.Forms.Padding(5);
            this.cmdLoadSpine.Size = new System.Drawing.Size(89, 26);
            this.cmdLoadSpine.TabIndex = 12;
            this.cmdLoadSpine.Text = "Load Spine Art";
            this.cmdLoadSpine.Click += new System.EventHandler(this.CmdLoadSpine_Click);
            // 
            // cmdLoadBox
            // 
            this.cmdLoadBox.Location = new System.Drawing.Point(109, 300);
            this.cmdLoadBox.Margin = new System.Windows.Forms.Padding(2);
            this.cmdLoadBox.Name = "cmdLoadBox";
            this.cmdLoadBox.Padding = new System.Windows.Forms.Padding(5);
            this.cmdLoadBox.Size = new System.Drawing.Size(89, 26);
            this.cmdLoadBox.TabIndex = 13;
            this.cmdLoadBox.Text = "Load Box Art";
            this.cmdLoadBox.Click += new System.EventHandler(this.CmdLoadBox_Click);
            // 
            // grpDetails
            // 
            this.grpDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.grpDetails.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.grpDetails.Controls.Add(this.chkApplyIPS);
            this.grpDetails.Controls.Add(this.chk6ButtonHack);
            this.grpDetails.Controls.Add(this.label10);
            this.grpDetails.Controls.Add(this.cboScraper);
            this.grpDetails.Controls.Add(this.label6);
            this.grpDetails.Controls.Add(this.lstScrapeItems);
            this.grpDetails.Controls.Add(this.picGenre);
            this.grpDetails.Controls.Add(this.picNumPlayers);
            this.grpDetails.Controls.Add(this.txtCopyright);
            this.grpDetails.Controls.Add(this.label5);
            this.grpDetails.Controls.Add(this.cboGenre);
            this.grpDetails.Controls.Add(this.label4);
            this.grpDetails.Controls.Add(this.cboPlayerNum);
            this.grpDetails.Controls.Add(this.label3);
            this.grpDetails.Controls.Add(this.cmdScrape);
            this.grpDetails.Controls.Add(this.txtTname);
            this.grpDetails.Controls.Add(this.txtDescription);
            this.grpDetails.Controls.Add(this.label2);
            this.grpDetails.Controls.Add(this.label1);
            this.grpDetails.ForeColor = System.Drawing.Color.Gainsboro;
            this.grpDetails.Location = new System.Drawing.Point(6, 6);
            this.grpDetails.Margin = new System.Windows.Forms.Padding(2);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Padding = new System.Windows.Forms.Padding(2);
            this.grpDetails.Size = new System.Drawing.Size(545, 335);
            this.grpDetails.TabIndex = 52;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Details";
            // 
            // chkApplyIPS
            // 
            this.chkApplyIPS.AutoSize = true;
            this.chkApplyIPS.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkApplyIPS.Enabled = false;
            this.chkApplyIPS.Location = new System.Drawing.Point(240, 306);
            this.chkApplyIPS.Name = "chkApplyIPS";
            this.chkApplyIPS.Size = new System.Drawing.Size(103, 17);
            this.chkApplyIPS.TabIndex = 45;
            this.chkApplyIPS.Text = "Apply IPS Patch";
            // 
            // chk6ButtonHack
            // 
            this.chk6ButtonHack.AutoSize = true;
            this.chk6ButtonHack.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chk6ButtonHack.Location = new System.Drawing.Point(84, 306);
            this.chk6ButtonHack.Name = "chk6ButtonHack";
            this.chk6ButtonHack.Size = new System.Drawing.Size(100, 17);
            this.chk6ButtonHack.TabIndex = 45;
            this.chk6ButtonHack.Text = "6 Buttons Hack";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label10.Location = new System.Drawing.Point(345, 251);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 13);
            this.label10.TabIndex = 44;
            this.label10.Text = "Scraper Source:";
            // 
            // cboScraper
            // 
            this.cboScraper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.cboScraper.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cboScraper.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cboScraper.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cboScraper.ButtonIcon = ((System.Drawing.Bitmap)(resources.GetObject("cboScraper.ButtonIcon")));
            this.cboScraper.DrawDropdownHoverOutline = false;
            this.cboScraper.DrawFocusRectangle = false;
            this.cboScraper.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboScraper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScraper.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboScraper.ForeColor = System.Drawing.Color.Gainsboro;
            this.cboScraper.FormattingEnabled = true;
            this.cboScraper.Location = new System.Drawing.Point(345, 270);
            this.cboScraper.Name = "cboScraper";
            this.cboScraper.Size = new System.Drawing.Size(192, 21);
            this.cboScraper.TabIndex = 43;
            this.cboScraper.Text = null;
            this.cboScraper.TextPadding = new System.Windows.Forms.Padding(2);
            this.cboScraper.SelectedIndexChanged += new System.EventHandler(this.cboScraper_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label6.Location = new System.Drawing.Point(345, 29);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "Scraper matches:";
            // 
            // lstScrapeItems
            // 
            this.lstScrapeItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.lstScrapeItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstScrapeItems.ForeColor = System.Drawing.Color.Gainsboro;
            this.lstScrapeItems.FormattingEnabled = true;
            this.lstScrapeItems.HorizontalScrollbar = true;
            this.lstScrapeItems.Location = new System.Drawing.Point(346, 51);
            this.lstScrapeItems.Margin = new System.Windows.Forms.Padding(2);
            this.lstScrapeItems.Name = "lstScrapeItems";
            this.lstScrapeItems.Size = new System.Drawing.Size(191, 184);
            this.lstScrapeItems.TabIndex = 6;
            this.lstScrapeItems.SelectedIndexChanged += new System.EventHandler(this.LstScrapeItems_SelectedIndexChanged);
            // 
            // picGenre
            // 
            this.picGenre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGenre.Location = new System.Drawing.Point(277, 241);
            this.picGenre.Margin = new System.Windows.Forms.Padding(2);
            this.picGenre.Name = "picGenre";
            this.picGenre.Size = new System.Drawing.Size(63, 50);
            this.picGenre.TabIndex = 40;
            this.picGenre.TabStop = false;
            // 
            // picNumPlayers
            // 
            this.picNumPlayers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picNumPlayers.Location = new System.Drawing.Point(84, 241);
            this.picNumPlayers.Margin = new System.Windows.Forms.Padding(2);
            this.picNumPlayers.Name = "picNumPlayers";
            this.picNumPlayers.Size = new System.Drawing.Size(63, 50);
            this.picNumPlayers.TabIndex = 40;
            this.picNumPlayers.TabStop = false;
            // 
            // txtCopyright
            // 
            this.txtCopyright.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.txtCopyright.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCopyright.ForeColor = System.Drawing.Color.Gainsboro;
            this.txtCopyright.Location = new System.Drawing.Point(84, 190);
            this.txtCopyright.Margin = new System.Windows.Forms.Padding(2);
            this.txtCopyright.Name = "txtCopyright";
            this.txtCopyright.Size = new System.Drawing.Size(256, 20);
            this.txtCopyright.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label5.Location = new System.Drawing.Point(11, 192);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Copyright:";
            // 
            // cboGenre
            // 
            this.cboGenre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.cboGenre.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cboGenre.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cboGenre.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cboGenre.ButtonIcon = ((System.Drawing.Bitmap)(resources.GetObject("cboGenre.ButtonIcon")));
            this.cboGenre.DrawDropdownHoverOutline = false;
            this.cboGenre.DrawFocusRectangle = false;
            this.cboGenre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGenre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboGenre.ForeColor = System.Drawing.Color.Gainsboro;
            this.cboGenre.FormattingEnabled = true;
            this.cboGenre.Items.AddRange(new object[] {
            "Action/Shooting",
            "Action",
            "Adventure",
            "Fighting",
            "Puzzle",
            "Racing/Sport",
            "RPG",
            "Strategy",
            "Shoot\'em Up",
            "Home/Table"});
            this.cboGenre.Location = new System.Drawing.Point(200, 215);
            this.cboGenre.Margin = new System.Windows.Forms.Padding(2);
            this.cboGenre.Name = "cboGenre";
            this.cboGenre.Size = new System.Drawing.Size(140, 21);
            this.cboGenre.TabIndex = 4;
            this.cboGenre.Text = "Action/Shooting";
            this.cboGenre.TextPadding = new System.Windows.Forms.Padding(2);
            this.cboGenre.SelectedIndexChanged += new System.EventHandler(this.CboGenre_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label4.Location = new System.Drawing.Point(157, 218);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Genre:";
            // 
            // cboPlayerNum
            // 
            this.cboPlayerNum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.cboPlayerNum.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cboPlayerNum.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cboPlayerNum.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cboPlayerNum.ButtonIcon = ((System.Drawing.Bitmap)(resources.GetObject("cboPlayerNum.ButtonIcon")));
            this.cboPlayerNum.DrawDropdownHoverOutline = false;
            this.cboPlayerNum.DrawFocusRectangle = false;
            this.cboPlayerNum.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboPlayerNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlayerNum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboPlayerNum.ForeColor = System.Drawing.Color.Gainsboro;
            this.cboPlayerNum.FormattingEnabled = true;
            this.cboPlayerNum.Items.AddRange(new object[] {
            "1",
            "1-2",
            "1-3",
            "1-4",
            "1-5",
            "4"});
            this.cboPlayerNum.Location = new System.Drawing.Point(84, 215);
            this.cboPlayerNum.Margin = new System.Windows.Forms.Padding(2);
            this.cboPlayerNum.Name = "cboPlayerNum";
            this.cboPlayerNum.Size = new System.Drawing.Size(63, 21);
            this.cboPlayerNum.TabIndex = 3;
            this.cboPlayerNum.Text = "1";
            this.cboPlayerNum.TextPadding = new System.Windows.Forms.Padding(2);
            this.cboPlayerNum.SelectedIndexChanged += new System.EventHandler(this.CboPlayerNum_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label3.Location = new System.Drawing.Point(11, 218);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 34;
            this.label3.Text = "Num Players:";
            // 
            // cmdScrape
            // 
            this.cmdScrape.Location = new System.Drawing.Point(371, 300);
            this.cmdScrape.Margin = new System.Windows.Forms.Padding(2);
            this.cmdScrape.Name = "cmdScrape";
            this.cmdScrape.Padding = new System.Windows.Forms.Padding(5);
            this.cmdScrape.Size = new System.Drawing.Size(141, 26);
            this.cmdScrape.TabIndex = 5;
            this.cmdScrape.Text = "Get Game Information";
            this.cmdScrape.Click += new System.EventHandler(this.CmdScrape_Click);
            // 
            // txtTname
            // 
            this.txtTname.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.txtTname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTname.ForeColor = System.Drawing.Color.Gainsboro;
            this.txtTname.Location = new System.Drawing.Point(84, 27);
            this.txtTname.Margin = new System.Windows.Forms.Padding(2);
            this.txtTname.Name = "txtTname";
            this.txtTname.Size = new System.Drawing.Size(256, 20);
            this.txtTname.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtDescription.ForeColor = System.Drawing.Color.Gainsboro;
            this.txtDescription.Location = new System.Drawing.Point(84, 51);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(2);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(256, 133);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label2.Location = new System.Drawing.Point(11, 51);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Description:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label1.Location = new System.Drawing.Point(11, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title Name:";
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(259, 415);
            this.cmdSave.Margin = new System.Windows.Forms.Padding(2);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Padding = new System.Windows.Forms.Padding(5);
            this.cmdSave.Size = new System.Drawing.Size(94, 25);
            this.cmdSave.TabIndex = 14;
            this.cmdSave.Text = "Add Game";
            this.cmdSave.Click += new System.EventHandler(this.CmdSave_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(416, 415);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(2);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Padding = new System.Windows.Forms.Padding(5);
            this.cmdCancel.Size = new System.Drawing.Size(106, 25);
            this.cmdCancel.TabIndex = 15;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.Click += new System.EventHandler(this.CmdCancel_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Image files|*.bmp;*.png;*.jpg";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.groupBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.groupBox2.Controls.Add(this.rdoU);
            this.groupBox2.Controls.Add(this.rdoE);
            this.groupBox2.Controls.Add(this.rdoJ);
            this.groupBox2.Controls.Add(this.rdoDetected);
            this.groupBox2.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBox2.Location = new System.Drawing.Point(6, 345);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(545, 57);
            this.groupBox2.TabIndex = 43;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ROM Region";
            // 
            // rdoU
            // 
            this.rdoU.AutoSize = true;
            this.rdoU.Location = new System.Drawing.Point(410, 28);
            this.rdoU.Margin = new System.Windows.Forms.Padding(2);
            this.rdoU.Name = "rdoU";
            this.rdoU.Size = new System.Drawing.Size(115, 17);
            this.rdoU.TabIndex = 0;
            this.rdoU.Text = "Force USA (NTSC)";
            // 
            // rdoE
            // 
            this.rdoE.AutoSize = true;
            this.rdoE.Location = new System.Drawing.Point(277, 28);
            this.rdoE.Margin = new System.Windows.Forms.Padding(2);
            this.rdoE.Name = "rdoE";
            this.rdoE.Size = new System.Drawing.Size(118, 17);
            this.rdoE.TabIndex = 0;
            this.rdoE.Text = "Force Europe (PAL)";
            // 
            // rdoJ
            // 
            this.rdoJ.AutoSize = true;
            this.rdoJ.Location = new System.Drawing.Point(142, 28);
            this.rdoJ.Margin = new System.Windows.Forms.Padding(2);
            this.rdoJ.Name = "rdoJ";
            this.rdoJ.Size = new System.Drawing.Size(122, 17);
            this.rdoJ.TabIndex = 0;
            this.rdoJ.Text = "Force Japan (NTSC)";
            // 
            // rdoDetected
            // 
            this.rdoDetected.AutoSize = true;
            this.rdoDetected.Checked = true;
            this.rdoDetected.Location = new System.Drawing.Point(8, 28);
            this.rdoDetected.Margin = new System.Windows.Forms.Padding(2);
            this.rdoDetected.Name = "rdoDetected";
            this.rdoDetected.Size = new System.Drawing.Size(75, 17);
            this.rdoDetected.TabIndex = 0;
            this.rdoDetected.TabStop = true;
            this.rdoDetected.Text = "Detected: ";
            // 
            // frmAddGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(769, 451);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.grpDetails);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add new game";
            this.Load += new System.EventHandler(this.FrmAddGame_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picSpine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxArt)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpDetails.ResumeLayout(false);
            this.grpDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGenre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNumPlayers)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private DarkUI.Controls.DarkGroupBox groupBox1;
        private DarkUI.Controls.DarkButton cmdLoadSpine;
        private DarkUI.Controls.DarkButton cmdLoadBox;
        private DarkUI.Controls.DarkGroupBox grpDetails;
        private DarkUI.Controls.DarkLabel label2;
        private DarkUI.Controls.DarkLabel label1;
        private DarkUI.Controls.DarkButton cmdScrape;
        private DarkUI.Controls.DarkButton cmdSave;
        private DarkUI.Controls.DarkButton cmdCancel;
        private DarkUI.Controls.DarkLabel label4;
        private DarkUI.Controls.DarkLabel label3;
        private DarkUI.Controls.DarkLabel label5;
        private System.Windows.Forms.PictureBox picGenre;
        private System.Windows.Forms.PictureBox picNumPlayers;
        private DarkUI.Controls.DarkLabel label6;
        private System.Windows.Forms.ListBox lstScrapeItems;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private DarkUI.Controls.DarkRadioButton rdoUsModern;
        private DarkUI.Controls.DarkRadioButton rdoUsClassic;
        private DarkUI.Controls.DarkRadioButton rdoEuModern;
        private DarkUI.Controls.DarkRadioButton rdoEuClassic;
        private DarkUI.Controls.DarkLabel label9;
        private DarkUI.Controls.DarkLabel label8;
        private DarkUI.Controls.DarkLabel label7;
        private DarkUI.Controls.DarkRadioButton rdoJapan;
        private DarkUI.Controls.DarkButton cmdModMyClassicArt;
        private DarkUI.Controls.DarkGroupBox groupBox2;
        private DarkUI.Controls.DarkRadioButton rdoU;
        private DarkUI.Controls.DarkRadioButton rdoE;
        private DarkUI.Controls.DarkRadioButton rdoJ;
        private DarkUI.Controls.DarkRadioButton rdoDetected;
        private DarkUI.Controls.DarkLabel label10;
        private DarkUI.Controls.DarkComboBox cboScraper;
        public System.Windows.Forms.PictureBox picSpine;
        public System.Windows.Forms.PictureBox picBoxArt;
        public DarkUI.Controls.DarkTextBox txtDescription;
        public DarkUI.Controls.DarkTextBox txtTname;
        public DarkUI.Controls.DarkComboBox cboGenre;
        public DarkUI.Controls.DarkComboBox cboPlayerNum;
        public DarkUI.Controls.DarkTextBox txtCopyright;
        private DarkUI.Controls.DarkCheckBox chk6ButtonHack;
        public DarkUI.Controls.DarkCheckBox chkApplyIPS;
    }
}