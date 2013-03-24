using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FikrPos.Client.Forms;
using FikrPos.Forms;
using FikrPos.Models;

namespace FikrPos
{
    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
            Visible = false;

            AppFeatures.StartupPath = Application.StartupPath;
            DataManager.getInstance().initData();

            if (AppStates.appInfo.IsInit == 1)
            {
                AdministratorPassword admPwd = new AdministratorPassword();
                admPwd.ShowDialog();

                FikrPosDataContext db = new FikrPosDataContext();
                db.ExecuteCommand("Delete from AppUser");
                AppUser root = new AppUser();
                root.Username = "admin";
                root.Password = Cryptho.Encrypt(admPwd.AdminPassword);
                root.Role = Roles.Admin;
                db.AppUsers.InsertOnSubmit(root);

                AppInfo appInfo = db.AppInfos.SingleOrDefault();
                appInfo.IsInit = 0;

                db.SubmitChanges();
            }

            if (AppFeatures.userLogin == null)
            {
                Login login = new Login();
                login.ShowDialog();
            }
            else
            {
                Program.UserEnter();
            }
            
            
        }

        private void StartupForm_Load(object sender, EventArgs e)
        {   
        }
    }
}
