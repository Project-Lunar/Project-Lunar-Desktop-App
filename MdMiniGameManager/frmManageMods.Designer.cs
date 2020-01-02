namespace ProjectLunarUI
{
    partial class frmManageMods
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManageMods));
            this.grdMods = new System.Windows.Forms.DataGridView();
            this.cmdAddNew = new DarkUI.Controls.DarkButton();
            this.cmdRemove = new DarkUI.Controls.DarkButton();
            this.cmdCancel = new DarkUI.Controls.DarkButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.noModsPanel = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnMMCwebsite = new DarkUI.Controls.DarkButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdMods)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdMods
            // 
            this.grdMods.AllowUserToAddRows = false;
            this.grdMods.AllowUserToResizeRows = false;
            this.grdMods.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdMods.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.grdMods.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdMods.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdMods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMods.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdMods.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.grdMods.Location = new System.Drawing.Point(12, 12);
            this.grdMods.Name = "grdMods";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdMods.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grdMods.RowHeadersVisible = false;
            this.grdMods.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.grdMods.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Gainsboro;
            this.grdMods.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(87)))), ((int)(((byte)(87)))));
            this.grdMods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdMods.Size = new System.Drawing.Size(373, 395);
            this.grdMods.TabIndex = 0;
            // 
            // cmdAddNew
            // 
            this.cmdAddNew.Location = new System.Drawing.Point(391, 32);
            this.cmdAddNew.Name = "cmdAddNew";
            this.cmdAddNew.Padding = new System.Windows.Forms.Padding(5);
            this.cmdAddNew.Size = new System.Drawing.Size(126, 36);
            this.cmdAddNew.TabIndex = 1;
            this.cmdAddNew.Text = "Install new mod(s)";
            this.cmdAddNew.Click += new System.EventHandler(this.cmdAddNew_Click);
            // 
            // cmdRemove
            // 
            this.cmdRemove.Location = new System.Drawing.Point(391, 87);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Padding = new System.Windows.Forms.Padding(5);
            this.cmdRemove.Size = new System.Drawing.Size(126, 36);
            this.cmdRemove.TabIndex = 1;
            this.cmdRemove.Text = "Remove selected mod(s)";
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(391, 372);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Padding = new System.Windows.Forms.Padding(5);
            this.cmdCancel.Size = new System.Drawing.Size(126, 36);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Close";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "*.mod";
            this.openFileDialog.Filter = "PL Mod|*.mod";
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.Title = "Select mod file(s)...";
            // 
            // noModsPanel
            // 
            this.noModsPanel.AutoSize = true;
            this.noModsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(59)))));
            this.noModsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.noModsPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noModsPanel.ForeColor = System.Drawing.Color.Gainsboro;
            this.noModsPanel.Image = global::ProjectLunarUI.Properties.Resources.icn_download;
            this.noModsPanel.Location = new System.Drawing.Point(12, 12);
            this.noModsPanel.MinimumSize = new System.Drawing.Size(373, 395);
            this.noModsPanel.Name = "noModsPanel";
            this.noModsPanel.Size = new System.Drawing.Size(373, 395);
            this.noModsPanel.TabIndex = 2;
            this.noModsPanel.Text = "\r\n\r\nNo mods installed! Go check \r\nout ModMyClassic.com for\r\nsome mods!\r\n";
            this.noModsPanel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.noModsPanel.Visible = false;
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
            this.statusStrip.Location = new System.Drawing.Point(0, 416);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(0, 5, 0, 3);
            this.statusStrip.Size = new System.Drawing.Size(524, 24);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 71;
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
            // btnMMCwebsite
            // 
            this.btnMMCwebsite.Location = new System.Drawing.Point(391, 196);
            this.btnMMCwebsite.Name = "btnMMCwebsite";
            this.btnMMCwebsite.Padding = new System.Windows.Forms.Padding(5);
            this.btnMMCwebsite.Size = new System.Drawing.Size(126, 103);
            this.btnMMCwebsite.TabIndex = 72;
            this.btnMMCwebsite.Text = "Check out available mods at ModMyClassic.com";
            this.btnMMCwebsite.Click += new System.EventHandler(this.btnMMCwebsite_Click);
            // 
            // frmManageMods
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(524, 440);
            this.Controls.Add(this.btnMMCwebsite);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.noModsPanel);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdRemove);
            this.Controls.Add(this.cmdAddNew);
            this.Controls.Add(this.grdMods);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmManageMods";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Lunar Mod Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmManageMods_FormClosing);
            this.Shown += new System.EventHandler(this.frmManageMods_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.grdMods)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdMods;
        private DarkUI.Controls.DarkButton cmdAddNew;
        private DarkUI.Controls.DarkButton cmdRemove;
        private DarkUI.Controls.DarkButton cmdCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label noModsPanel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private DarkUI.Controls.DarkButton btnMMCwebsite;
    }
}