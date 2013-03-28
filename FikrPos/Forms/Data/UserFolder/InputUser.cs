using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FikrPos.Forms.Data.User
{
    public partial class InputUser : Form
    {
        public FormModeEnum FormMode;
        public AppUser appUser;

        private string oldKey;
        public InputUser()
        {
            InitializeComponent();
        }

        private void InputUser_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            FikrPosDataContext db = Program.getDb();
            if (FormMode == FormModeEnum.Insert)
            {   
                appUser = new AppUser();
                appUser.Username = txtUsername.Text;
                appUser.Password = Cryptho.Encrypt(txtPassword.Text);
                appUser.Role = cboRole.Text;
                db.AppUsers.InsertOnSubmit(appUser);
                db.SubmitChanges();
            }
            else if (FormMode == FormModeEnum.Update)
            {
                var appUser = (from u in db.AppUsers
                               where u.Username.Equals(oldKey)
                               select u).SingleOrDefault();
                appUser.Username = txtUsername.Text;
                appUser.Password = Cryptho.Encrypt(txtPassword.Text);
                appUser.Role = cboRole.Text;
                db.SubmitChanges();
            }
        }

        internal void prepareForm(string key)
        {
            oldKey = key;
            FikrPosDataContext db = Program.getDb();
            var appUser = (from u in db.AppUsers
                           where u.Username.Equals(oldKey)
                           select u).SingleOrDefault();
            if (appUser != null)
            {
                txtUsername.Text = appUser.Username;
                txtPassword.Text = Cryptho.Decrypt(appUser.Password);
                cboRole.Text = appUser.Role;
            }
        }
    }
}
