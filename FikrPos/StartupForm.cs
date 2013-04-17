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
            Width = 0;
            Height = 0;
            WindowState = FormWindowState.Minimized;
            Hide();

            RegistrySettings.getInstance().loadValues();
            if (RegistrySettings.getInstance().newInstall == null)
            {
                MessageBox.Show("As this is your first time running this application, please configure your database connection");
                Settings settings = new Settings();
                DialogResult dr = settings.ShowDialog();

            }

            Program.StartupPath = Application.StartupPath;
            DataManager.getInstance().initData();

            if (Program.appInfo==null || (Program.appInfo!=null && Program.appInfo.IsInit == 1))
            {
                AdministratorPassword admPwd = new AdministratorPassword();
                admPwd.ShowDialog();

                FikrPosDataContext db = Program.getDb();
                db.ExecuteCommand("Delete from AppUser");
                AppUser root = new AppUser();
                root.Username = admPwd.AdminUsername;
                root.Password = Cryptho.Encrypt(admPwd.AdminPassword);
                root.Role = Roles.Admin;
                db.AppUsers.InsertOnSubmit(root);

                //let's clear AppInfo
                db.ExecuteCommand("Delete from AppInfo");
                AppInfo appInfo = new AppInfo();                
                appInfo.IsInit = 0;
                db.AppInfos.InsertOnSubmit(appInfo);

                db.SubmitChanges();
                MessageBox.Show("Now you can login with admin username and password");
            }

            if (Program.userLogin == null)
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

        private void StartupForm_Shown(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
