using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FikrPos.Forms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
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
                if (AppFeatures.userLogin.isadmin==1)
                {
                    if (Program.adminWindow != null)
                    {
                        Program.adminWindow.Dispose();
                    }
                    Program.adminWindow = new AdminWindow();
                    Program.adminWindow.Show();
                }
                else
                {
                }
                Close();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Program.Exit();
        }
    }
}
