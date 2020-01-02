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
    public partial class frmLoading : DarkForm
    {
        public frmLoading(String statusString)
        {
            InitializeComponent();
            this.BringToFront();
            this.Activate();
            this.status.Text = statusString;
        }
    }
}
