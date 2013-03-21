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
            Features.StartupPath = Application.StartupPath;
        }

        private void StartupForm_Load(object sender, EventArgs e)
        {
            if (Features.getInstance().getDataConnection() != null)
            {
                MessageBox.Show("Success!");
            }
        }
    }
}
