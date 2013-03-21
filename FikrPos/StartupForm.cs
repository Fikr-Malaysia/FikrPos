using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FikrPos.Library;

namespace FikrPos
{
    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
            AppFeatures.StartupPath = Application.StartupPath;
            DataManager.getInstance().initData();

            if (AppStates.IsInit)
            {
                Program.BeginDataInitializationProcess();
            }
            Visible = false;
        }

        private void StartupForm_Load(object sender, EventArgs e)
        {   
        }
    }
}
