using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FikrPos.Modules.Dashboard
{
    public partial class SessionProperty : Form
    {
        public SessionProperty()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            startOfCash = Convert.ToDouble(numStartOfCash.Value);
            endOfCash = Convert.ToDouble(numEndOfCash.Value);
            note = txtNnotes.Text;
        }

        public double startOfCash { get; set; }

        public double endOfCash { get; set; }

        public string note { get; set; }

        internal void prepareForm()
        {
            numEndOfCash.Value = Convert.ToDecimal(endOfCash);
            numStartOfCash.Value = Convert.ToDecimal(startOfCash);
            txtNnotes.Text = note;
        }
    }
}
