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

namespace ProjectLunarUI
{
    public partial class frmTutorial : DarkForm
    {
        private int pageNumber = 1;
        private int numberOfPages = 5;
        private string[] pageText = new string[5];

        public frmTutorial(string mode)
        {
            InitializeComponent();
            pageText[0] = "STEP 1:\r\n" +
                "Remove all cables from console. \r\nMake sure you have a micro USB cable " +
                "with data lines (The bundled SEGA cable does NOT work!)\r\n";
            pageText[1] = "STEP 2:\r\n" +
                "Ensure Power switch is in the on position\r\n";
            pageText[2] = "STEP 3:\r\n" +
                "Hold down the RESET button\r\n";
            pageText[3] = "STEP 4:\r\n" +
                "(Whilst holding RESET) Plug in the PC-connected USB to your console\r\n" +
                "(Must be a data enabled micro-USB cable. The bundled SEGA cable will NOT work!)\r\n";
            pageText[4] = "STEP 5:\r\n" +
                "Keep holding RESET, the LED will flash off and back on. Once the LED has " +
                "flashed back on you can let go of RESET after a second or so.\r\n" +
                "The install process will now begin and progress will be shown in the install window. It will take approximately 10 minutes\r\n";
    }

    private void btnRight_Click(object sender, EventArgs e)
        {
            if (pageNumber < numberOfPages)
            {
                pageNumber++;
                imageContainer.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("ProjectLunar_tut" + pageNumber);
                textContainer.Text = pageText[(pageNumber - 1)];
                if (pageNumber > 1 && btnLeft.Enabled == false)
                {
                    btnLeft.Enabled = true;
                }
                if (pageNumber == numberOfPages && btnRight.Enabled == true)
                {
                    btnRight.Enabled = false;
                }
            }

        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            pageNumber--;
            imageContainer.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("ProjectLunar_tut" + pageNumber);
            textContainer.Text = pageText[(pageNumber - 1)];
            if (pageNumber == 1 && btnLeft.Enabled == true)
            {
                btnLeft.Enabled = false;
            }
            if (pageNumber < numberOfPages && btnRight.Enabled == false)
            {
                btnRight.Enabled = true;
            }
        }
    }
}
