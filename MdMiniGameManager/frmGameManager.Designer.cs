namespace ProjectLunarUI
{
    partial class frmGameManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGameManager));
            this.cmdLoad = new DarkUI.Controls.DarkButton();
            this.txtDescription = new DarkUI.Controls.DarkTextBox();
            this.lblTitleName = new DarkUI.Controls.DarkLabel();
            this.picBoxArt = new System.Windows.Forms.PictureBox();
            this.treeGames = new System.Windows.Forms.TreeView();
            this.label2 = new DarkUI.Controls.DarkLabel();
            this.label4 = new DarkUI.Controls.DarkLabel();
            this.label6 = new DarkUI.Controls.DarkLabel();
            this.label8 = new DarkUI.Controls.DarkLabel();
            this.label10 = new DarkUI.Controls.DarkLabel();
            this.label12 = new DarkUI.Controls.DarkLabel();
            this.label14 = new DarkUI.Controls.DarkLabel();
            this.label16 = new DarkUI.Controls.DarkLabel();
            this.label18 = new DarkUI.Controls.DarkLabel();
            this.label20 = new DarkUI.Controls.DarkLabel();
            this.label22 = new DarkUI.Controls.DarkLabel();
            this.label24 = new DarkUI.Controls.DarkLabel();
            this.txtTname = new DarkUI.Controls.DarkTextBox();
            this.txtAction = new DarkUI.Controls.DarkTextBox();
            this.txtCopy = new DarkUI.Controls.DarkTextBox();
            this.txtCsize = new DarkUI.Controls.DarkTextBox();
            this.txtDemoTime = new DarkUI.Controls.DarkTextBox();
            this.txtDevName = new DarkUI.Controls.DarkTextBox();
            this.txtName = new DarkUI.Controls.DarkTextBox();
            this.txtRegionTag = new DarkUI.Controls.DarkTextBox();
            this.txtSorDate = new DarkUI.Controls.DarkTextBox();
            this.txtSorDemo = new DarkUI.Controls.DarkTextBox();
            this.txtSorGenr = new DarkUI.Controls.DarkTextBox();
            this.txtSorName = new DarkUI.Controls.DarkTextBox();
            this.txtSorPnum = new DarkUI.Controls.DarkTextBox();
            this.txtDirectoryPath = new DarkUI.Controls.DarkTextBox();
            this.label1 = new DarkUI.Controls.DarkLabel();
            this.cmdBrowse = new DarkUI.Controls.DarkButton();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.lblImage = new DarkUI.Controls.DarkLabel();
            this.picSpine = new System.Windows.Forms.PictureBox();
            this.txtRom = new DarkUI.Controls.DarkTextBox();
            this.label3 = new DarkUI.Controls.DarkLabel();
            this.cboRomSet = new DarkUI.Controls.DarkComboBox();
            this.lblSystem = new DarkUI.Controls.DarkLabel();
            this.cmdAddGame = new DarkUI.Controls.DarkButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cmdSaveChanges = new DarkUI.Controls.DarkButton();
            this.groupBox1 = new DarkUI.Controls.DarkGroupBox();
            this.picGenre = new System.Windows.Forms.PictureBox();
            this.picNumPlayers = new System.Windows.Forms.PictureBox();
            this.label5 = new DarkUI.Controls.DarkLabel();
            this.cboGenre = new DarkUI.Controls.DarkComboBox();
            this.label7 = new DarkUI.Controls.DarkLabel();
            this.cboPlayerNum = new DarkUI.Controls.DarkComboBox();
            this.label9 = new DarkUI.Controls.DarkLabel();
            this.groupBox2 = new DarkUI.Controls.DarkGroupBox();
            this.groupBox3 = new DarkUI.Controls.DarkGroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new DarkUI.Controls.DarkGroupBox();
            this.groupBox5 = new DarkUI.Controls.DarkGroupBox();
            this.pbFreeSpace = new System.Windows.Forms.ProgressBar();
            this.lblFreeSpace = new DarkUI.Controls.DarkLabel();
            this.label15 = new DarkUI.Controls.DarkLabel();
            this.lblVolume = new DarkUI.Controls.DarkLabel();
            this.lblMediaType = new DarkUI.Controls.DarkLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new DarkUI.Controls.DarkMenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageModsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getRetroArchCoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getIPSPatchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.updatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkOnlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openlocalDataFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLocalIPSFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.restoreGamesFromBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enterRecoveryModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.failSafeRestoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixMisplacedSavestatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuChangeBGmusic = new System.Windows.Forms.ToolStripMenuItem();
            this.smoothingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableForScanlinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableAlwaysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pixelPerfectModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pixelPOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PixelPOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.correct87ModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.on87ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.off87ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceSpecificRegionBGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceENToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bgForceUSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bgForceJPNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bgRestoreDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.supportChatRoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.submitABugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fAQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mMCWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.discordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdRemoveGame = new DarkUI.Controls.DarkButton();
            this.menuStripLine = new System.Windows.Forms.PictureBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.cboSystemRegion = new DarkUI.Controls.DarkComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxArt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSpine)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGenre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNumPlayers)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menuStripLine)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdLoad
            // 
            this.cmdLoad.Location = new System.Drawing.Point(280, 727);
            this.cmdLoad.Margin = new System.Windows.Forms.Padding(2);
            this.cmdLoad.Name = "cmdLoad";
            this.cmdLoad.Padding = new System.Windows.Forms.Padding(5);
            this.cmdLoad.Size = new System.Drawing.Size(97, 30);
            this.cmdLoad.TabIndex = 1;
            this.cmdLoad.Text = "Load Data";
            this.cmdLoad.Click += new System.EventHandler(this.CmdLoad_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtDescription.Location = new System.Drawing.Point(79, 52);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(2);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(256, 133);
            this.txtDescription.TabIndex = 2;
            this.txtDescription.Enter += new System.EventHandler(this.txtDescription_Enter);
            this.txtDescription.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // lblTitleName
            // 
            this.lblTitleName.AutoSize = true;
            this.lblTitleName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lblTitleName.Location = new System.Drawing.Point(6, 31);
            this.lblTitleName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitleName.Name = "lblTitleName";
            this.lblTitleName.Size = new System.Drawing.Size(59, 13);
            this.lblTitleName.TabIndex = 3;
            this.lblTitleName.Text = "Title name:";
            // 
            // picBoxArt
            // 
            this.picBoxArt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.picBoxArt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBoxArt.Location = new System.Drawing.Point(42, 28);
            this.picBoxArt.Margin = new System.Windows.Forms.Padding(2);
            this.picBoxArt.Name = "picBoxArt";
            this.picBoxArt.Size = new System.Drawing.Size(152, 216);
            this.picBoxArt.TabIndex = 4;
            this.picBoxArt.TabStop = false;
            // 
            // treeGames
            // 
            this.treeGames.AllowDrop = true;
            this.treeGames.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.treeGames.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeGames.ForeColor = System.Drawing.Color.Gainsboro;
            this.treeGames.Location = new System.Drawing.Point(6, 58);
            this.treeGames.Margin = new System.Windows.Forms.Padding(2);
            this.treeGames.Name = "treeGames";
            this.treeGames.Size = new System.Drawing.Size(325, 484);
            this.treeGames.TabIndex = 6;
            this.treeGames.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeGames_NodeMouseClick);
            this.treeGames.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeGames_DragDrop);
            this.treeGames.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeGames_DragEnter);
            this.treeGames.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TreeGames_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label2.Location = new System.Drawing.Point(5, 34);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "action:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label4.Location = new System.Drawing.Point(5, 58);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "copy:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label6.Location = new System.Drawing.Point(5, 81);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "csize:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label8.Location = new System.Drawing.Point(5, 155);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "name:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label10.Location = new System.Drawing.Point(5, 131);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "dev_name:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label12.Location = new System.Drawing.Point(5, 106);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "demo_time:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label14.Location = new System.Drawing.Point(5, 225);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "sor_demo:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label16.Location = new System.Drawing.Point(6, 202);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(51, 13);
            this.label16.TabIndex = 22;
            this.label16.Text = "sor_date:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label18.Location = new System.Drawing.Point(5, 178);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(58, 13);
            this.label18.TabIndex = 20;
            this.label18.Text = "regionTag:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label20.Location = new System.Drawing.Point(5, 297);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(56, 13);
            this.label20.TabIndex = 30;
            this.label20.Text = "sor_pnum:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label22.Location = new System.Drawing.Point(5, 274);
            this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(56, 13);
            this.label22.TabIndex = 28;
            this.label22.Text = "sor_name:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label24.Location = new System.Drawing.Point(5, 249);
            this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(51, 13);
            this.label24.TabIndex = 26;
            this.label24.Text = "sor_genr:";
            // 
            // txtTname
            // 
            this.txtTname.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.txtTname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTname.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtTname.Location = new System.Drawing.Point(79, 28);
            this.txtTname.Margin = new System.Windows.Forms.Padding(2);
            this.txtTname.Name = "txtTname";
            this.txtTname.Size = new System.Drawing.Size(256, 20);
            this.txtTname.TabIndex = 31;
            this.txtTname.Enter += new System.EventHandler(this.txtTname_Enter);
            this.txtTname.Leave += new System.EventHandler(this.txtTname_Leave);
            // 
            // txtAction
            // 
            this.txtAction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtAction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtAction.Location = new System.Drawing.Point(74, 29);
            this.txtAction.Margin = new System.Windows.Forms.Padding(2);
            this.txtAction.Name = "txtAction";
            this.txtAction.Size = new System.Drawing.Size(225, 20);
            this.txtAction.TabIndex = 32;
            // 
            // txtCopy
            // 
            this.txtCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.txtCopy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCopy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtCopy.Location = new System.Drawing.Point(79, 188);
            this.txtCopy.Margin = new System.Windows.Forms.Padding(2);
            this.txtCopy.Name = "txtCopy";
            this.txtCopy.Size = new System.Drawing.Size(256, 20);
            this.txtCopy.TabIndex = 33;
            this.txtCopy.Enter += new System.EventHandler(this.txtCopy_Enter);
            this.txtCopy.Leave += new System.EventHandler(this.txtCopy_Leave);
            // 
            // txtCsize
            // 
            this.txtCsize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtCsize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCsize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtCsize.Location = new System.Drawing.Point(74, 77);
            this.txtCsize.Margin = new System.Windows.Forms.Padding(2);
            this.txtCsize.Name = "txtCsize";
            this.txtCsize.Size = new System.Drawing.Size(225, 20);
            this.txtCsize.TabIndex = 34;
            // 
            // txtDemoTime
            // 
            this.txtDemoTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtDemoTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDemoTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtDemoTime.Location = new System.Drawing.Point(74, 101);
            this.txtDemoTime.Margin = new System.Windows.Forms.Padding(2);
            this.txtDemoTime.Name = "txtDemoTime";
            this.txtDemoTime.Size = new System.Drawing.Size(225, 20);
            this.txtDemoTime.TabIndex = 35;
            // 
            // txtDevName
            // 
            this.txtDevName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtDevName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDevName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtDevName.Location = new System.Drawing.Point(74, 126);
            this.txtDevName.Margin = new System.Windows.Forms.Padding(2);
            this.txtDevName.Name = "txtDevName";
            this.txtDevName.Size = new System.Drawing.Size(225, 20);
            this.txtDevName.TabIndex = 36;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtName.Location = new System.Drawing.Point(74, 150);
            this.txtName.Margin = new System.Windows.Forms.Padding(2);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(225, 20);
            this.txtName.TabIndex = 37;
            // 
            // txtRegionTag
            // 
            this.txtRegionTag.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtRegionTag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRegionTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtRegionTag.Location = new System.Drawing.Point(74, 174);
            this.txtRegionTag.Margin = new System.Windows.Forms.Padding(2);
            this.txtRegionTag.Name = "txtRegionTag";
            this.txtRegionTag.Size = new System.Drawing.Size(225, 20);
            this.txtRegionTag.TabIndex = 38;
            // 
            // txtSorDate
            // 
            this.txtSorDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtSorDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSorDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtSorDate.Location = new System.Drawing.Point(74, 198);
            this.txtSorDate.Margin = new System.Windows.Forms.Padding(2);
            this.txtSorDate.Name = "txtSorDate";
            this.txtSorDate.Size = new System.Drawing.Size(225, 20);
            this.txtSorDate.TabIndex = 39;
            // 
            // txtSorDemo
            // 
            this.txtSorDemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtSorDemo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSorDemo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtSorDemo.Location = new System.Drawing.Point(74, 222);
            this.txtSorDemo.Margin = new System.Windows.Forms.Padding(2);
            this.txtSorDemo.Name = "txtSorDemo";
            this.txtSorDemo.Size = new System.Drawing.Size(225, 20);
            this.txtSorDemo.TabIndex = 40;
            // 
            // txtSorGenr
            // 
            this.txtSorGenr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtSorGenr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSorGenr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtSorGenr.Location = new System.Drawing.Point(74, 246);
            this.txtSorGenr.Margin = new System.Windows.Forms.Padding(2);
            this.txtSorGenr.Name = "txtSorGenr";
            this.txtSorGenr.Size = new System.Drawing.Size(225, 20);
            this.txtSorGenr.TabIndex = 41;
            // 
            // txtSorName
            // 
            this.txtSorName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtSorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSorName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtSorName.Location = new System.Drawing.Point(74, 270);
            this.txtSorName.Margin = new System.Windows.Forms.Padding(2);
            this.txtSorName.Name = "txtSorName";
            this.txtSorName.Size = new System.Drawing.Size(225, 20);
            this.txtSorName.TabIndex = 42;
            // 
            // txtSorPnum
            // 
            this.txtSorPnum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtSorPnum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSorPnum.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtSorPnum.Location = new System.Drawing.Point(74, 294);
            this.txtSorPnum.Margin = new System.Windows.Forms.Padding(2);
            this.txtSorPnum.Name = "txtSorPnum";
            this.txtSorPnum.Size = new System.Drawing.Size(225, 20);
            this.txtSorPnum.TabIndex = 43;
            // 
            // txtDirectoryPath
            // 
            this.txtDirectoryPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtDirectoryPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDirectoryPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtDirectoryPath.Location = new System.Drawing.Point(11, 700);
            this.txtDirectoryPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtDirectoryPath.Name = "txtDirectoryPath";
            this.txtDirectoryPath.Size = new System.Drawing.Size(456, 20);
            this.txtDirectoryPath.TabIndex = 44;
            this.txtDirectoryPath.Text = "C:\\Temp\\MD-Mini\\alldata.psb_extracted";
            this.txtDirectoryPath.TextChanged += new System.EventHandler(this.TxtDirectoryPath_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label1.Location = new System.Drawing.Point(11, 686);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 13);
            this.label1.TabIndex = 45;
            this.label1.Text = "Path to fully unpacked alldata.bin folder";
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Location = new System.Drawing.Point(473, 698);
            this.cmdBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Padding = new System.Windows.Forms.Padding(5);
            this.cmdBrowse.Size = new System.Drawing.Size(28, 21);
            this.cmdBrowse.TabIndex = 46;
            this.cmdBrowse.Text = "...";
            this.cmdBrowse.Click += new System.EventHandler(this.CmdBrowse_Click);
            // 
            // lblImage
            // 
            this.lblImage.AutoSize = true;
            this.lblImage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lblImage.Location = new System.Drawing.Point(5, 316);
            this.lblImage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(115, 13);
            this.lblImage.TabIndex = 47;
            this.lblImage.Text = "Debug: game.Image = ";
            // 
            // picSpine
            // 
            this.picSpine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.picSpine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSpine.Location = new System.Drawing.Point(10, 28);
            this.picSpine.Margin = new System.Windows.Forms.Padding(2);
            this.picSpine.Name = "picSpine";
            this.picSpine.Size = new System.Drawing.Size(30, 216);
            this.picSpine.TabIndex = 48;
            this.picSpine.TabStop = false;
            // 
            // txtRom
            // 
            this.txtRom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtRom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtRom.Location = new System.Drawing.Point(74, 354);
            this.txtRom.Margin = new System.Windows.Forms.Padding(2);
            this.txtRom.Name = "txtRom";
            this.txtRom.Size = new System.Drawing.Size(186, 20);
            this.txtRom.TabIndex = 49;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label3.Location = new System.Drawing.Point(71, 336);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 50;
            this.label3.Text = "ROM File:";
            // 
            // cboRomSet
            // 
            this.cboRomSet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.cboRomSet.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cboRomSet.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cboRomSet.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cboRomSet.ButtonIcon = ((System.Drawing.Bitmap)(resources.GetObject("cboRomSet.ButtonIcon")));
            this.cboRomSet.DrawDropdownHoverOutline = false;
            this.cboRomSet.DrawFocusRectangle = false;
            this.cboRomSet.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboRomSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRomSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboRomSet.ForeColor = System.Drawing.Color.Gainsboro;
            this.cboRomSet.FormattingEnabled = true;
            this.cboRomSet.Items.AddRange(new object[] {
            "Europe Art set",
            "Japan Art set",
            "US Art set"});
            this.cboRomSet.Location = new System.Drawing.Point(6, 31);
            this.cboRomSet.Margin = new System.Windows.Forms.Padding(2);
            this.cboRomSet.Name = "cboRomSet";
            this.cboRomSet.Size = new System.Drawing.Size(159, 21);
            this.cboRomSet.TabIndex = 51;
            this.cboRomSet.Text = "Europe Art set";
            this.cboRomSet.TextPadding = new System.Windows.Forms.Padding(2);
            this.cboRomSet.SelectedIndexChanged += new System.EventHandler(this.CboRomSet_SelectedIndexChanged);
            // 
            // lblSystem
            // 
            this.lblSystem.AutoSize = true;
            this.lblSystem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lblSystem.Location = new System.Drawing.Point(6, 25);
            this.lblSystem.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(79, 13);
            this.lblSystem.TabIndex = 52;
            this.lblSystem.Text = "Console Name:";
            // 
            // cmdAddGame
            // 
            this.cmdAddGame.Location = new System.Drawing.Point(240, 554);
            this.cmdAddGame.Margin = new System.Windows.Forms.Padding(2);
            this.cmdAddGame.Name = "cmdAddGame";
            this.cmdAddGame.Padding = new System.Windows.Forms.Padding(5);
            this.cmdAddGame.Size = new System.Drawing.Size(89, 32);
            this.cmdAddGame.TabIndex = 53;
            this.cmdAddGame.Text = "Add new game";
            this.cmdAddGame.Click += new System.EventHandler(this.CmdAddGame_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "Browse ROM file";
            this.openFileDialog.Filter = "ROM Image|*.bin;*.gen;*.md";
            // 
            // cmdSaveChanges
            // 
            this.cmdSaveChanges.Location = new System.Drawing.Point(459, 128);
            this.cmdSaveChanges.Margin = new System.Windows.Forms.Padding(2);
            this.cmdSaveChanges.Name = "cmdSaveChanges";
            this.cmdSaveChanges.Padding = new System.Windows.Forms.Padding(5);
            this.cmdSaveChanges.Size = new System.Drawing.Size(86, 32);
            this.cmdSaveChanges.TabIndex = 53;
            this.cmdSaveChanges.Text = "Sync";
            this.cmdSaveChanges.Click += new System.EventHandler(this.CmdSaveChanges_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.groupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtRom);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.txtAction);
            this.groupBox1.Controls.Add(this.lblImage);
            this.groupBox1.Controls.Add(this.txtSorPnum);
            this.groupBox1.Controls.Add(this.txtCsize);
            this.groupBox1.Controls.Add(this.txtSorName);
            this.groupBox1.Controls.Add(this.txtDemoTime);
            this.groupBox1.Controls.Add(this.txtSorGenr);
            this.groupBox1.Controls.Add(this.txtDevName);
            this.groupBox1.Controls.Add(this.txtSorDemo);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.txtSorDate);
            this.groupBox1.Controls.Add(this.txtRegionTag);
            this.groupBox1.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Location = new System.Drawing.Point(994, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 389);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // picGenre
            // 
            this.picGenre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGenre.Location = new System.Drawing.Point(272, 240);
            this.picGenre.Margin = new System.Windows.Forms.Padding(2);
            this.picGenre.Name = "picGenre";
            this.picGenre.Size = new System.Drawing.Size(63, 50);
            this.picGenre.TabIndex = 63;
            this.picGenre.TabStop = false;
            // 
            // picNumPlayers
            // 
            this.picNumPlayers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picNumPlayers.Location = new System.Drawing.Point(79, 240);
            this.picNumPlayers.Margin = new System.Windows.Forms.Padding(2);
            this.picNumPlayers.Name = "picNumPlayers";
            this.picNumPlayers.Size = new System.Drawing.Size(63, 50);
            this.picNumPlayers.TabIndex = 64;
            this.picNumPlayers.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label5.Location = new System.Drawing.Point(6, 191);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 62;
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
            this.cboGenre.Location = new System.Drawing.Point(195, 214);
            this.cboGenre.Margin = new System.Windows.Forms.Padding(2);
            this.cboGenre.Name = "cboGenre";
            this.cboGenre.Size = new System.Drawing.Size(140, 21);
            this.cboGenre.TabIndex = 59;
            this.cboGenre.Text = "Action/Shooting";
            this.cboGenre.TextPadding = new System.Windows.Forms.Padding(2);
            this.cboGenre.SelectedIndexChanged += new System.EventHandler(this.CboGenre_SelectedIndexChanged);
            this.cboGenre.Enter += new System.EventHandler(this.cboGenre_Enter);
            this.cboGenre.Leave += new System.EventHandler(this.cboGenre_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label7.Location = new System.Drawing.Point(152, 217);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 61;
            this.label7.Text = "Genre:";
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
            this.cboPlayerNum.Location = new System.Drawing.Point(79, 214);
            this.cboPlayerNum.Margin = new System.Windows.Forms.Padding(2);
            this.cboPlayerNum.Name = "cboPlayerNum";
            this.cboPlayerNum.Size = new System.Drawing.Size(63, 21);
            this.cboPlayerNum.TabIndex = 58;
            this.cboPlayerNum.Text = "1";
            this.cboPlayerNum.TextPadding = new System.Windows.Forms.Padding(2);
            this.cboPlayerNum.SelectedIndexChanged += new System.EventHandler(this.CboPlayerNum_SelectedIndexChanged);
            this.cboPlayerNum.Enter += new System.EventHandler(this.cboPlayerNum_Enter);
            this.cboPlayerNum.Leave += new System.EventHandler(this.cboPlayerNum_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label9.Location = new System.Drawing.Point(6, 217);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 13);
            this.label9.TabIndex = 60;
            this.label9.Text = "Num Players:";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.groupBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.groupBox2.Controls.Add(this.picNumPlayers);
            this.groupBox2.Controls.Add(this.lblTitleName);
            this.groupBox2.Controls.Add(this.picGenre);
            this.groupBox2.Controls.Add(this.txtDescription);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtTname);
            this.groupBox2.Controls.Add(this.cboGenre);
            this.groupBox2.Controls.Add(this.txtCopy);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.cboPlayerNum);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBox2.Location = new System.Drawing.Point(337, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(344, 308);
            this.groupBox2.TabIndex = 65;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Game Information";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.groupBox3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.groupBox3.Controls.Add(this.picSpine);
            this.groupBox3.Controls.Add(this.picBoxArt);
            this.groupBox3.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBox3.Location = new System.Drawing.Point(687, 58);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(205, 308);
            this.groupBox3.TabIndex = 66;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Artwork";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(46, 799);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(341, 20);
            this.tabControl1.TabIndex = 67;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(333, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(333, 0);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(333, 0);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.groupBox4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.groupBox4.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBox4.Location = new System.Drawing.Point(46, 813);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(339, 100);
            this.groupBox4.TabIndex = 68;
            this.groupBox4.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.groupBox5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.groupBox5.Controls.Add(this.pbFreeSpace);
            this.groupBox5.Controls.Add(this.lblFreeSpace);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.lblVolume);
            this.groupBox5.Controls.Add(this.lblMediaType);
            this.groupBox5.Controls.Add(this.lblSystem);
            this.groupBox5.Controls.Add(this.cmdSaveChanges);
            this.groupBox5.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBox5.Location = new System.Drawing.Point(337, 372);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(555, 170);
            this.groupBox5.TabIndex = 69;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "System Information";
            // 
            // pbFreeSpace
            // 
            this.pbFreeSpace.Location = new System.Drawing.Point(9, 87);
            this.pbFreeSpace.Name = "pbFreeSpace";
            this.pbFreeSpace.Size = new System.Drawing.Size(536, 23);
            this.pbFreeSpace.TabIndex = 57;
            // 
            // lblFreeSpace
            // 
            this.lblFreeSpace.AutoSize = true;
            this.lblFreeSpace.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lblFreeSpace.Location = new System.Drawing.Point(6, 68);
            this.lblFreeSpace.Name = "lblFreeSpace";
            this.lblFreeSpace.Size = new System.Drawing.Size(175, 13);
            this.lblFreeSpace.TabIndex = 56;
            this.lblFreeSpace.Text = "Estimated free space: Please wait...";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label15.Location = new System.Drawing.Point(229, 25);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(87, 13);
            this.label15.TabIndex = 55;
            this.label15.Text = "Mod Version: 1.0";
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lblVolume.Location = new System.Drawing.Point(229, 46);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(77, 13);
            this.lblVolume.TabIndex = 55;
            this.lblVolume.Text = "Volume: DATA";
            // 
            // lblMediaType
            // 
            this.lblMediaType.AutoSize = true;
            this.lblMediaType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lblMediaType.Location = new System.Drawing.Point(6, 46);
            this.lblMediaType.Name = "lblMediaType";
            this.lblMediaType.Size = new System.Drawing.Size(100, 13);
            this.lblMediaType.TabIndex = 54;
            this.lblMediaType.Text = "Media Type: NAND";
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = false;
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.statusStrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip.Location = new System.Drawing.Point(0, 593);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 3);
            this.statusStrip.Size = new System.Drawing.Size(898, 24);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 70;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel.Margin = new System.Windows.Forms.Padding(0, -1, 2, 2);
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(103, 15);
            this.toolStripStatusLabel.Text = "Status: Connected";
            this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.menuStrip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(898, 24);
            this.menuStrip1.TabIndex = 71;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageModsToolStripMenuItem,
            this.getRetroArchCoresToolStripMenuItem,
            this.getIPSPatchesToolStripMenuItem,
            this.toolStripSeparator3,
            this.updatesToolStripMenuItem,
            this.openlocalDataFolderToolStripMenuItem,
            this.openLocalIPSFolderToolStripMenuItem,
            this.toolStripMenuItem1,
            this.restoreGamesFromBackupToolStripMenuItem,
            this.enterRecoveryModeToolStripMenuItem,
            this.advancedToolStripMenuItem});
            this.toolsToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // manageModsToolStripMenuItem
            // 
            this.manageModsToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.manageModsToolStripMenuItem.Name = "manageModsToolStripMenuItem";
            this.manageModsToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.manageModsToolStripMenuItem.Text = "&Manage mods";
            this.manageModsToolStripMenuItem.Click += new System.EventHandler(this.manageModsToolStripMenuItem_Click);
            // 
            // getRetroArchCoresToolStripMenuItem
            // 
            this.getRetroArchCoresToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.getRetroArchCoresToolStripMenuItem.Name = "getRetroArchCoresToolStripMenuItem";
            this.getRetroArchCoresToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.getRetroArchCoresToolStripMenuItem.Text = "Get RetroArch &cores";
            this.getRetroArchCoresToolStripMenuItem.Click += new System.EventHandler(this.getRetroArchCoresToolStripMenuItem_Click);
            // 
            // getIPSPatchesToolStripMenuItem
            // 
            this.getIPSPatchesToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.getIPSPatchesToolStripMenuItem.Name = "getIPSPatchesToolStripMenuItem";
            this.getIPSPatchesToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.getIPSPatchesToolStripMenuItem.Text = "Get IPS &Patches";
            this.getIPSPatchesToolStripMenuItem.Click += new System.EventHandler(this.getIPSPatchesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(224, 6);
            // 
            // updatesToolStripMenuItem
            // 
            this.updatesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkOnlineToolStripMenuItem,
            this.installUpdateToolStripMenuItem});
            this.updatesToolStripMenuItem.Enabled = false;
            this.updatesToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.updatesToolStripMenuItem.Name = "updatesToolStripMenuItem";
            this.updatesToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.updatesToolStripMenuItem.Text = "&Updates";
            // 
            // checkOnlineToolStripMenuItem
            // 
            this.checkOnlineToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.checkOnlineToolStripMenuItem.Name = "checkOnlineToolStripMenuItem";
            this.checkOnlineToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.checkOnlineToolStripMenuItem.Text = "Check &online";
            this.checkOnlineToolStripMenuItem.Click += new System.EventHandler(this.checkOnlineToolStripMenuItem_Click);
            // 
            // installUpdateToolStripMenuItem
            // 
            this.installUpdateToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.installUpdateToolStripMenuItem.Name = "installUpdateToolStripMenuItem";
            this.installUpdateToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.installUpdateToolStripMenuItem.Text = "From payload &package";
            this.installUpdateToolStripMenuItem.Click += new System.EventHandler(this.installUpdateToolStripMenuItem_Click);
            // 
            // openlocalDataFolderToolStripMenuItem
            // 
            this.openlocalDataFolderToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.openlocalDataFolderToolStripMenuItem.Name = "openlocalDataFolderToolStripMenuItem";
            this.openlocalDataFolderToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.openlocalDataFolderToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.openlocalDataFolderToolStripMenuItem.Text = "Open &local data folder";
            this.openlocalDataFolderToolStripMenuItem.Click += new System.EventHandler(this.openlocalDataFolderToolStripMenuItem_Click);
            // 
            // openLocalIPSFolderToolStripMenuItem
            // 
            this.openLocalIPSFolderToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.openLocalIPSFolderToolStripMenuItem.Name = "openLocalIPSFolderToolStripMenuItem";
            this.openLocalIPSFolderToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.openLocalIPSFolderToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.openLocalIPSFolderToolStripMenuItem.Text = "Open local &IPS folder";
            this.openLocalIPSFolderToolStripMenuItem.Click += new System.EventHandler(this.openLocalIPSFolderToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(224, 6);
            // 
            // restoreGamesFromBackupToolStripMenuItem
            // 
            this.restoreGamesFromBackupToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.restoreGamesFromBackupToolStripMenuItem.Name = "restoreGamesFromBackupToolStripMenuItem";
            this.restoreGamesFromBackupToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.restoreGamesFromBackupToolStripMenuItem.Text = "Reset local data from &backup";
            this.restoreGamesFromBackupToolStripMenuItem.Click += new System.EventHandler(this.restoreGamesFromBackupToolStripMenuItem_Click);
            // 
            // enterRecoveryModeToolStripMenuItem
            // 
            this.enterRecoveryModeToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.enterRecoveryModeToolStripMenuItem.Name = "enterRecoveryModeToolStripMenuItem";
            this.enterRecoveryModeToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.enterRecoveryModeToolStripMenuItem.Text = "Enter &recovery mode";
            this.enterRecoveryModeToolStripMenuItem.Click += new System.EventHandler(this.enterRecoveryModeToolStripMenuItem_Click);
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.failSafeRestoreToolStripMenuItem,
            this.exportBackupToolStripMenuItem,
            this.fixMisplacedSavestatesToolStripMenuItem});
            this.advancedToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
            this.advancedToolStripMenuItem.Text = "&Advanced";
            // 
            // failSafeRestoreToolStripMenuItem
            // 
            this.failSafeRestoreToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.failSafeRestoreToolStripMenuItem.Name = "failSafeRestoreToolStripMenuItem";
            this.failSafeRestoreToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.failSafeRestoreToolStripMenuItem.Text = "&Fail-safe restore";
            this.failSafeRestoreToolStripMenuItem.Click += new System.EventHandler(this.failSafeRestoreToolStripMenuItem_Click);
            // 
            // exportBackupToolStripMenuItem
            // 
            this.exportBackupToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.exportBackupToolStripMenuItem.Name = "exportBackupToolStripMenuItem";
            this.exportBackupToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.exportBackupToolStripMenuItem.Text = "&Export backup";
            this.exportBackupToolStripMenuItem.Click += new System.EventHandler(this.exportBackupToolStripMenuItem_Click);
            // 
            // fixMisplacedSavestatesToolStripMenuItem
            // 
            this.fixMisplacedSavestatesToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.fixMisplacedSavestatesToolStripMenuItem.Name = "fixMisplacedSavestatesToolStripMenuItem";
            this.fixMisplacedSavestatesToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.fixMisplacedSavestatesToolStripMenuItem.Text = "Fix misplaced save-states";
            this.fixMisplacedSavestatesToolStripMenuItem.Click += new System.EventHandler(this.fixMisplacedSavestatesToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuChangeBGmusic,
            this.smoothingToolStripMenuItem,
            this.pixelPerfectModeToolStripMenuItem,
            this.correct87ModeToolStripMenuItem,
            this.forceSpecificRegionBGToolStripMenuItem});
            this.optionsToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // toolStripMenuChangeBGmusic
            // 
            this.toolStripMenuChangeBGmusic.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuChangeBGmusic.Name = "toolStripMenuChangeBGmusic";
            this.toolStripMenuChangeBGmusic.Size = new System.Drawing.Size(218, 22);
            this.toolStripMenuChangeBGmusic.Text = "&Change background music";
            this.toolStripMenuChangeBGmusic.Click += new System.EventHandler(this.toolStripMenuChangeBGmusic_Click);
            // 
            // smoothingToolStripMenuItem
            // 
            this.smoothingToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.smoothingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableForScanlinesToolStripMenuItem,
            this.enableAlwaysToolStripMenuItem,
            this.restoreDefaultToolStripMenuItem});
            this.smoothingToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.smoothingToolStripMenuItem.Name = "smoothingToolStripMenuItem";
            this.smoothingToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.smoothingToolStripMenuItem.Text = "&Smoothing";
            // 
            // disableForScanlinesToolStripMenuItem
            // 
            this.disableForScanlinesToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.disableForScanlinesToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.disableForScanlinesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.disableForScanlinesToolStripMenuItem.Name = "disableForScanlinesToolStripMenuItem";
            this.disableForScanlinesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.disableForScanlinesToolStripMenuItem.Text = "&Disable for Scanlines";
            this.disableForScanlinesToolStripMenuItem.Click += new System.EventHandler(this.disableForScanlinesToolStripMenuItem_Click);
            // 
            // enableAlwaysToolStripMenuItem
            // 
            this.enableAlwaysToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.enableAlwaysToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.enableAlwaysToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.enableAlwaysToolStripMenuItem.Name = "enableAlwaysToolStripMenuItem";
            this.enableAlwaysToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.enableAlwaysToolStripMenuItem.Text = "&Enable Always";
            this.enableAlwaysToolStripMenuItem.Click += new System.EventHandler(this.enableAlwaysToolStripMenuItem_Click);
            // 
            // restoreDefaultToolStripMenuItem
            // 
            this.restoreDefaultToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.restoreDefaultToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.restoreDefaultToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.restoreDefaultToolStripMenuItem.Name = "restoreDefaultToolStripMenuItem";
            this.restoreDefaultToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.restoreDefaultToolStripMenuItem.Text = "&Restore Default";
            this.restoreDefaultToolStripMenuItem.Click += new System.EventHandler(this.restoreDefaultToolStripMenuItem_Click);
            // 
            // pixelPerfectModeToolStripMenuItem
            // 
            this.pixelPerfectModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pixelPOnToolStripMenuItem,
            this.PixelPOffToolStripMenuItem});
            this.pixelPerfectModeToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.pixelPerfectModeToolStripMenuItem.Name = "pixelPerfectModeToolStripMenuItem";
            this.pixelPerfectModeToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.pixelPerfectModeToolStripMenuItem.Text = "&Enable pixel perfect mode";
            // 
            // pixelPOnToolStripMenuItem
            // 
            this.pixelPOnToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.pixelPOnToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pixelPOnToolStripMenuItem.Name = "pixelPOnToolStripMenuItem";
            this.pixelPOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.pixelPOnToolStripMenuItem.Text = "&Enable Always";
            this.pixelPOnToolStripMenuItem.Click += new System.EventHandler(this.onToolStripMenuItem_Click);
            // 
            // PixelPOffToolStripMenuItem
            // 
            this.PixelPOffToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.PixelPOffToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.PixelPOffToolStripMenuItem.Name = "PixelPOffToolStripMenuItem";
            this.PixelPOffToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.PixelPOffToolStripMenuItem.Text = "&Restore Default";
            this.PixelPOffToolStripMenuItem.Click += new System.EventHandler(this.PixelPOffToolStripMenuItem_Click);
            // 
            // correct87ModeToolStripMenuItem
            // 
            this.correct87ModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.on87ToolStripMenuItem,
            this.off87ToolStripMenuItem});
            this.correct87ModeToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.correct87ModeToolStripMenuItem.Name = "correct87ModeToolStripMenuItem";
            this.correct87ModeToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.correct87ModeToolStripMenuItem.Text = "&Force 8:7 mode for 256x224";
            this.correct87ModeToolStripMenuItem.Visible = false;
            // 
            // on87ToolStripMenuItem
            // 
            this.on87ToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.on87ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.on87ToolStripMenuItem.Name = "on87ToolStripMenuItem";
            this.on87ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.on87ToolStripMenuItem.Text = "&Enable Always";
            this.on87ToolStripMenuItem.Click += new System.EventHandler(this.on87ToolStripMenuItem_Click);
            // 
            // off87ToolStripMenuItem
            // 
            this.off87ToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.off87ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.off87ToolStripMenuItem.Name = "off87ToolStripMenuItem";
            this.off87ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.off87ToolStripMenuItem.Text = "&Restore Default";
            this.off87ToolStripMenuItem.Click += new System.EventHandler(this.off87ToolStripMenuItem_Click);
            // 
            // forceSpecificRegionBGToolStripMenuItem
            // 
            this.forceSpecificRegionBGToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.forceENToolStripMenuItem,
            this.bgForceUSToolStripMenuItem,
            this.bgForceJPNToolStripMenuItem,
            this.bgRestoreDefaultToolStripMenuItem});
            this.forceSpecificRegionBGToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.forceSpecificRegionBGToolStripMenuItem.Name = "forceSpecificRegionBGToolStripMenuItem";
            this.forceSpecificRegionBGToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.forceSpecificRegionBGToolStripMenuItem.Text = "Force specific region BG";
            // 
            // forceENToolStripMenuItem
            // 
            this.forceENToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.forceENToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.forceENToolStripMenuItem.Name = "forceENToolStripMenuItem";
            this.forceENToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.forceENToolStripMenuItem.Text = "Force &EN background";
            this.forceENToolStripMenuItem.Click += new System.EventHandler(this.forceENToolStripMenuItem_Click);
            // 
            // bgForceUSToolStripMenuItem
            // 
            this.bgForceUSToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.bgForceUSToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.bgForceUSToolStripMenuItem.Name = "bgForceUSToolStripMenuItem";
            this.bgForceUSToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.bgForceUSToolStripMenuItem.Text = "Force &US background";
            this.bgForceUSToolStripMenuItem.Click += new System.EventHandler(this.bgForceUSToolStripMenuItem_Click);
            // 
            // bgForceJPNToolStripMenuItem
            // 
            this.bgForceJPNToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.bgForceJPNToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.bgForceJPNToolStripMenuItem.Name = "bgForceJPNToolStripMenuItem";
            this.bgForceJPNToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.bgForceJPNToolStripMenuItem.Text = "Force &JPN background";
            this.bgForceJPNToolStripMenuItem.Click += new System.EventHandler(this.bgForceJPNToolStripMenuItem_Click);
            // 
            // bgRestoreDefaultToolStripMenuItem
            // 
            this.bgRestoreDefaultToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.bgRestoreDefaultToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.bgRestoreDefaultToolStripMenuItem.Name = "bgRestoreDefaultToolStripMenuItem";
            this.bgRestoreDefaultToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.bgRestoreDefaultToolStripMenuItem.Text = "&Restore Default";
            this.bgRestoreDefaultToolStripMenuItem.Click += new System.EventHandler(this.bgRestoreDefaultToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.toolStripSeparator2,
            this.supportChatRoomToolStripMenuItem,
            this.submitABugToolStripMenuItem,
            this.fAQToolStripMenuItem,
            this.toolStripSeparator1,
            this.mMCWebsiteToolStripMenuItem,
            this.discordToolStripMenuItem});
            this.helpToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(176, 6);
            // 
            // supportChatRoomToolStripMenuItem
            // 
            this.supportChatRoomToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.supportChatRoomToolStripMenuItem.Name = "supportChatRoomToolStripMenuItem";
            this.supportChatRoomToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.supportChatRoomToolStripMenuItem.Text = "Support Chat Room";
            this.supportChatRoomToolStripMenuItem.Click += new System.EventHandler(this.supportChatRoomToolStripMenuItem_Click);
            // 
            // submitABugToolStripMenuItem
            // 
            this.submitABugToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.submitABugToolStripMenuItem.Name = "submitABugToolStripMenuItem";
            this.submitABugToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.submitABugToolStripMenuItem.Text = "Register a bug";
            this.submitABugToolStripMenuItem.Click += new System.EventHandler(this.submitABugToolStripMenuItem_Click);
            // 
            // fAQToolStripMenuItem
            // 
            this.fAQToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.fAQToolStripMenuItem.Name = "fAQToolStripMenuItem";
            this.fAQToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.fAQToolStripMenuItem.Text = "FAQ";
            this.fAQToolStripMenuItem.Click += new System.EventHandler(this.fAQToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(176, 6);
            // 
            // mMCWebsiteToolStripMenuItem
            // 
            this.mMCWebsiteToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mMCWebsiteToolStripMenuItem.Name = "mMCWebsiteToolStripMenuItem";
            this.mMCWebsiteToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.mMCWebsiteToolStripMenuItem.Text = "MMC Website";
            this.mMCWebsiteToolStripMenuItem.Click += new System.EventHandler(this.mMCWebsiteToolStripMenuItem_Click);
            // 
            // discordToolStripMenuItem
            // 
            this.discordToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.discordToolStripMenuItem.Name = "discordToolStripMenuItem";
            this.discordToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.discordToolStripMenuItem.Text = "Discord";
            this.discordToolStripMenuItem.Click += new System.EventHandler(this.discordToolStripMenuItem_Click);
            // 
            // cmdRemoveGame
            // 
            this.cmdRemoveGame.Enabled = false;
            this.cmdRemoveGame.Location = new System.Drawing.Point(6, 554);
            this.cmdRemoveGame.Margin = new System.Windows.Forms.Padding(2);
            this.cmdRemoveGame.Name = "cmdRemoveGame";
            this.cmdRemoveGame.Padding = new System.Windows.Forms.Padding(5);
            this.cmdRemoveGame.Size = new System.Drawing.Size(89, 32);
            this.cmdRemoveGame.TabIndex = 53;
            this.cmdRemoveGame.Text = "Remove game";
            this.cmdRemoveGame.Click += new System.EventHandler(this.cmdRemoveGame_Click);
            // 
            // menuStripLine
            // 
            this.menuStripLine.BackColor = System.Drawing.Color.Gainsboro;
            this.menuStripLine.Location = new System.Drawing.Point(0, 0);
            this.menuStripLine.Name = "menuStripLine";
            this.menuStripLine.Size = new System.Drawing.Size(898, 25);
            this.menuStripLine.TabIndex = 72;
            this.menuStripLine.TabStop = false;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "zip";
            this.saveFileDialog.Filter = "*.zip|Backup Export";
            // 
            // cboSystemRegion
            // 
            this.cboSystemRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.cboSystemRegion.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.cboSystemRegion.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.cboSystemRegion.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.cboSystemRegion.ButtonIcon = ((System.Drawing.Bitmap)(resources.GetObject("cboSystemRegion.ButtonIcon")));
            this.cboSystemRegion.DrawDropdownHoverOutline = false;
            this.cboSystemRegion.DrawFocusRectangle = false;
            this.cboSystemRegion.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboSystemRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSystemRegion.Enabled = false;
            this.cboSystemRegion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboSystemRegion.ForeColor = System.Drawing.Color.Gainsboro;
            this.cboSystemRegion.FormattingEnabled = true;
            this.cboSystemRegion.Location = new System.Drawing.Point(172, 31);
            this.cboSystemRegion.Margin = new System.Windows.Forms.Padding(2);
            this.cboSystemRegion.Name = "cboSystemRegion";
            this.cboSystemRegion.Size = new System.Drawing.Size(159, 21);
            this.cboSystemRegion.TabIndex = 73;
            this.cboSystemRegion.Text = "System Region";
            this.cboSystemRegion.TextPadding = new System.Windows.Forms.Padding(2);
            this.cboSystemRegion.SelectedIndexChanged += new System.EventHandler(this.cboSystemRegion_SelectedIndexChanged);
            // 
            // frmGameManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 617);
            this.Controls.Add(this.cboSystemRegion);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdRemoveGame);
            this.Controls.Add(this.cmdAddGame);
            this.Controls.Add(this.cboRomSet);
            this.Controls.Add(this.cmdBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDirectoryPath);
            this.Controls.Add(this.treeGames);
            this.Controls.Add(this.cmdLoad);
            this.Controls.Add(this.menuStripLine);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "frmGameManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project Lunar Game Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmGameManager_FormClosing);
            this.Load += new System.EventHandler(this.FrmGameManager_Load);
            this.Shown += new System.EventHandler(this.FrmGameManager_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxArt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSpine)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGenre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNumPlayers)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menuStripLine)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DarkUI.Controls.DarkButton cmdLoad;
        private DarkUI.Controls.DarkTextBox txtDescription;
        private DarkUI.Controls.DarkLabel lblTitleName;
        private System.Windows.Forms.PictureBox picBoxArt;
        private System.Windows.Forms.TreeView treeGames;
        private DarkUI.Controls.DarkLabel label2;
        private DarkUI.Controls.DarkLabel label4;
        private DarkUI.Controls.DarkLabel label6;
        private DarkUI.Controls.DarkLabel label8;
        private DarkUI.Controls.DarkLabel label10;
        private DarkUI.Controls.DarkLabel label12;
        private DarkUI.Controls.DarkLabel label14;
        private DarkUI.Controls.DarkLabel label16;
        private DarkUI.Controls.DarkLabel label18;
        private DarkUI.Controls.DarkLabel label20;
        private DarkUI.Controls.DarkLabel label22;
        private DarkUI.Controls.DarkLabel label24;
        private DarkUI.Controls.DarkTextBox txtTname;
        private DarkUI.Controls.DarkTextBox txtAction;
        private DarkUI.Controls.DarkTextBox txtCopy;
        private DarkUI.Controls.DarkTextBox txtCsize;
        private DarkUI.Controls.DarkTextBox txtDemoTime;
        private DarkUI.Controls.DarkTextBox txtDevName;
        private DarkUI.Controls.DarkTextBox txtName;
        private DarkUI.Controls.DarkTextBox txtRegionTag;
        private DarkUI.Controls.DarkTextBox txtSorDate;
        private DarkUI.Controls.DarkTextBox txtSorDemo;
        private DarkUI.Controls.DarkTextBox txtSorGenr;
        private DarkUI.Controls.DarkTextBox txtSorName;
        private DarkUI.Controls.DarkTextBox txtSorPnum;
        private DarkUI.Controls.DarkTextBox txtDirectoryPath;
        private DarkUI.Controls.DarkLabel label1;
        private DarkUI.Controls.DarkButton cmdBrowse;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private DarkUI.Controls.DarkLabel lblImage;
        private System.Windows.Forms.PictureBox picSpine;
        private DarkUI.Controls.DarkTextBox txtRom;
        private DarkUI.Controls.DarkLabel label3;
        private DarkUI.Controls.DarkComboBox cboRomSet;
        private DarkUI.Controls.DarkLabel lblSystem;
        private DarkUI.Controls.DarkButton cmdAddGame;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private DarkUI.Controls.DarkButton cmdSaveChanges;
        private DarkUI.Controls.DarkGroupBox groupBox1;
        private System.Windows.Forms.PictureBox picGenre;
        private System.Windows.Forms.PictureBox picNumPlayers;
        private DarkUI.Controls.DarkLabel label5;
        private DarkUI.Controls.DarkComboBox cboGenre;
        private DarkUI.Controls.DarkLabel label7;
        private DarkUI.Controls.DarkComboBox cboPlayerNum;
        private DarkUI.Controls.DarkLabel label9;
        private DarkUI.Controls.DarkGroupBox groupBox2;
        private DarkUI.Controls.DarkGroupBox groupBox3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private DarkUI.Controls.DarkGroupBox groupBox4;
        private DarkUI.Controls.DarkGroupBox groupBox5;
        private DarkUI.Controls.DarkLabel label15;
        private DarkUI.Controls.DarkLabel lblVolume;
        private DarkUI.Controls.DarkLabel lblMediaType;
        private System.Windows.Forms.ProgressBar pbFreeSpace;
        private DarkUI.Controls.DarkLabel lblFreeSpace;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private DarkUI.Controls.DarkMenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageModsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreGamesFromBackupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private DarkUI.Controls.DarkButton cmdRemoveGame;
        private System.Windows.Forms.ToolStripMenuItem updatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkOnlineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enterRecoveryModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openlocalDataFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem supportChatRoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fAQToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mMCWebsiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem discordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smoothingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableForScanlinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableAlwaysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreDefaultToolStripMenuItem;
        private System.Windows.Forms.PictureBox menuStripLine;
        private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem failSafeRestoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportBackupToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem getRetroArchCoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuChangeBGmusic;
        private System.Windows.Forms.ToolStripMenuItem submitABugToolStripMenuItem;
        private DarkUI.Controls.DarkComboBox cboSystemRegion;
        private System.Windows.Forms.ToolStripMenuItem getIPSPatchesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLocalIPSFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixMisplacedSavestatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pixelPerfectModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pixelPOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PixelPOffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem correct87ModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem on87ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem off87ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceSpecificRegionBGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceENToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bgForceUSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bgForceJPNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bgRestoreDefaultToolStripMenuItem;
    }
}

