using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FikrPos.Forms.Pos
{
    public partial class PosGui : Form
    {
        public PosGui()
        {
            InitializeComponent();
            lblCompanyName.Text = AppStates.appInfo.Company_Name;
            lblAddress.Text = AppStates.appInfo.Company_Address;
        }

        private void PosGui_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.Exit();
        }
    }
}
