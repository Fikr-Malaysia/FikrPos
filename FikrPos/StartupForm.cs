using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FikrPos.Client.Forms;

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
            if (AppStates.IsInit)
            {
                AdministratorPassword admPwd = new AdministratorPassword();
                admPwd.ShowDialog();

                FikrPosDataContext db = new FikrPosDataContext();
                db.ExecuteCommand("Delete from AppUser");
                AppUser root = new AppUser();
                root.username = "root";
                root.password = Cryptho.Encrypt(admPwd.AdminPassword);
                root.isadmin = 1;
                db.AppUsers.InsertOnSubmit(root);

                AppInfo appInfo = db.AppInfos.SingleOrDefault();
                appInfo.IsInit = 0;

                db.SubmitChanges();
            }
        }

        private void StartupForm_Load(object sender, EventArgs e)
        {   
        }
    }
}
