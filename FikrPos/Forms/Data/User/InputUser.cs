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
            FikrPosDataContext db = new FikrPosDataContext();
            if (FormMode == FormModeEnum.Insert)
            {   
                appUser = new AppUser();
                appUser.username = txtUsername.Text;
                appUser.password = Cryptho.Encrypt(txtPassword.Text);
                appUser.role = cboRole.Text;
                db.AppUsers.InsertOnSubmit(appUser);
                db.SubmitChanges();
            }
            else if (FormMode == FormModeEnum.Update)
            {
                var appUser = (from u in db.AppUsers
                               where u.username.Equals(oldKey)
                               select u).SingleOrDefault();
                appUser.username = txtUsername.Text;
                appUser.password = Cryptho.Encrypt(txtPassword.Text);
                appUser.role = cboRole.Text;
                db.SubmitChanges();
            }
        }

        internal void prepareForm(string key)
        {
            oldKey = key;
            FikrPosDataContext db = new FikrPosDataContext();
            var appUser = (from u in db.AppUsers
                           where u.username.Equals(oldKey)
                           select u).SingleOrDefault();
            if (appUser != null)
            {
                txtUsername.Text = appUser.username;
                txtPassword.Text = Cryptho.Decrypt(appUser.password);
                cboRole.Text = appUser.role;
            }
        }
    }
}
