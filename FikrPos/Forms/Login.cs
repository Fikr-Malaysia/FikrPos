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
            FikrPosDataContext db = new FikrPosDataContext();
            AppFeatures.userLogin = (from u in db.AppUsers
                            where u.username == cboUsername.Text
                            && u.password == Cryptho.Encrypt(txtPassword.Text)
                            select u).SingleOrDefault();
            
            if (AppFeatures.userLogin == null)
            {
                MessageBox.Show("Empty");                
            }
            else
            {
                if (AppFeatures.userLogin.role.Equals(Roles.Admin))
                {
                    if (Program.adminWindow != null)
                    {
                        Program.adminWindow.Dispose();
                    }
                    
                    Program.adminWindow = new AdminWindow();
                    Program.adminWindow.ShowDialog();
                }
                else if (AppFeatures.userLogin.role.Equals(Roles.Cashier))
                {
                    if (Program.posGui != null)
                    {
                        Program.posGui.Dispose();
                    }
                    Program.posGui = new PosGui();
                    Program.posGui.Show();
                }
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
