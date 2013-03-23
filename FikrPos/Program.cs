using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FikrPos.Forms;
using FikrPos.Forms.Pos;
using FikrPos.Models;

namespace FikrPos
{
    static class Program
    {
        static string EventLogName = "FikrPos";
        public static StartupForm startupForm;
        public static AdminWindow adminWindow;
        public static PosGui posGui;
        public static bool graceClose = false;
        public static FikrPosDataContext db = null;

        [STAThread]
        static void Main(string[] args)
        {
            string username = null;
            string password = null;
            bool forceInit = false;

            for (int i = 0; i < args.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        username = args[i];
                        break;
                    case 1:
                        password = args[i];
                        break;
                    case 2:
                        forceInit = args[i].Equals("forceInit");
                        break;
                }
            }
            
            if(username!=null && password!=null)
                AppFeatures.userLogin = Program.Login(username, password);

            if (forceInit)
            {
                FikrPosDataContext db = new FikrPosDataContext();
                AppStates.appInfo = db.AppInfos.SingleOrDefault();
                AppStates.appInfo.IsInit = 1;
                AppStates.appInfo.Company_Name = null;
                AppStates.appInfo.Company_Address = null;
                db.SubmitChanges();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            startupForm = new StartupForm();
            Application.Run(startupForm);
        }

        public static void BeginDataInitializationProcess()
        {
            
        }

        internal static bool Logout()
        {
            if (MessageBox.Show("Are you sure you want to logout?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Program.graceClose = true;
                if(AppFeatures.userLogin.role.Equals(Roles.Admin))
                {
                    Program.adminWindow.Close();
                }
                else if (AppFeatures.userLogin.role.Equals(Roles.Cashier))
                {
                    Program.posGui.Close();
                }
                Login login = new Login();
                login.ShowDialog();
                return false;
            }
            return true;
        }

        internal static bool Exit()
        {
            if (MessageBox.Show("Are you sure you want to exit?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Program.graceClose = true;
                Application.Exit();
                return false;
            }
            return true;
        }



        internal static AppUser Login(string username, string password)
        {
            FikrPosDataContext db = new FikrPosDataContext();
            return (from u in db.AppUsers
                            where u.username == username
                            && u.password == Cryptho.Encrypt(password)
                            select u).SingleOrDefault();

        }

        internal static void UserEnter()
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
        }

        internal static FikrPosDataContext getDb()
        {
            if (db == null)
            {
                db = new FikrPosDataContext();
            }
            return db;
        }
    }
}

public enum FormModeEnum
{
    Insert,
    Update
}