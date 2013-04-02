using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FikrPos.Modules.Printing;
namespace FikrPos.Forms.Printing
{
    public partial class TestPrinter : Form
    {
        public TestPrinter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReceiptPrinting printer = new ReceiptPrinting();
            printer.testPrint();
        }
    }
}
