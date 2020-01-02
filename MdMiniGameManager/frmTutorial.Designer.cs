namespace ProjectLunarUI
{
    partial class frmTutorial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTutorial));
            this.imageContainer = new System.Windows.Forms.PictureBox();
            this.btnLeft = new DarkUI.Controls.DarkButton();
            this.btnRight = new DarkUI.Controls.DarkButton();
            this.textContainer = new DarkUI.Controls.DarkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.imageContainer)).BeginInit();
            this.SuspendLayout();
            // 
            // imageContainer
            // 
            this.imageContainer.BackgroundImage = global::ProjectLunarUI.Properties.Resources.ProjectLunar_tut1;
            this.imageContainer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imageContainer.Location = new System.Drawing.Point(63, 12);
            this.imageContainer.Name = "imageContainer";
            this.imageContainer.Size = new System.Drawing.Size(610, 360);
            this.imageContainer.TabIndex = 0;
            this.imageContainer.TabStop = false;
            // 
            // btnLeft
            // 
            this.btnLeft.Enabled = false;
            this.btnLeft.Location = new System.Drawing.Point(12, 12);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Padding = new System.Windows.Forms.Padding(5);
            this.btnLeft.Size = new System.Drawing.Size(45, 455);
            this.btnLeft.TabIndex = 2;
            this.btnLeft.Text = "<<";
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(679, 12);
            this.btnRight.Name = "btnRight";
            this.btnRight.Padding = new System.Windows.Forms.Padding(5);
            this.btnRight.Size = new System.Drawing.Size(45, 455);
            this.btnRight.TabIndex = 1;
            this.btnRight.Text = ">>";
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // textContainer
            // 
            this.textContainer.AutoSize = true;
            this.textContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textContainer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textContainer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.textContainer.Location = new System.Drawing.Point(64, 379);
            this.textContainer.MaximumSize = new System.Drawing.Size(610, 88);
            this.textContainer.MinimumSize = new System.Drawing.Size(610, 88);
            this.textContainer.Name = "textContainer";
            this.textContainer.Padding = new System.Windows.Forms.Padding(3);
            this.textContainer.Size = new System.Drawing.Size(610, 88);
            this.textContainer.TabIndex = 3;
            this.textContainer.Text = "STEP 1:\r\nRemove all cables from console. \r\nMake sure you have a micro USB cable w" +
    "ith data lines (The bundled SEGA cable does NOT work!)\r\n";
            // 
            // frmTutorial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 479);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.imageContainer);
            this.Controls.Add(this.textContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmTutorial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Lunar Tutorial";
            ((System.ComponentModel.ISupportInitialize)(this.imageContainer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imageContainer;
        private DarkUI.Controls.DarkButton btnLeft;
        private DarkUI.Controls.DarkButton btnRight;
        private DarkUI.Controls.DarkLabel textContainer;
    }
}