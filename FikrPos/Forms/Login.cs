using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FikrPos.Models;
using FikrPos.Forms.Pos;

namespace FikrPos.Forms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            Program.graceClose = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            AppFeatures.userLogin = Program.Login(cboUsername.Text, txtPassword.Text);
            
            if (AppFeatures.userLogin == null)
            {
                MessageBox.Show("Empty");                
            }
            else
            {
                Program.UserEnter();
                Program.graceClose = true;
                Close();
                Program.graceClose = false;
            }

        }

        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Program.Exit();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.graceClose)
            {
                e.Cancel = Program.Exit();
            }            
        }
    }
}
