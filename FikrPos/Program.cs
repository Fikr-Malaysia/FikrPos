using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FikrPos.Forms;
using FikrPos.Forms.Pos;
using FikrPos.Models;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data.Linq;
using FikrPos.Forms.Printing;

namespace FikrPos
{
    public static class Program
    {
        public static AppInfo appInfo { get; set; }
        public static AuditRoll activeRoll { get; set; }
        static string EventLogName = "FikrPos";
        public static StartupForm startupForm;
        public static MainWindow mainWindow;
        public static PosGui posGui;
        public static bool graceClose = false;        
        public static bool ForceClose = false;
        public static string StartupPath;
        public static AppUser userLogin;

        [STAThread]
        static void Main(string[] args)
        {
            string username = null;
            string password = null;
            bool forceInit = false;
            bool testPrinter = false;

            //example argument : admin admin noForceInit forceClose testPrinter
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
                    case 3:
                        ForceClose = args[i].Equals("forceClose");
                        break;
                    case 4:
                        testPrinter = args[i].Equals("testPrinter");
                        break;
                    case 5:
                        isExactPayment = args[i].Equals("exactPayment");
                        break;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            if(username!=null && password!=null)
                Program.userLogin = Program.Login(username, password);

            if (forceInit)
            {
                FikrPosDataContext db = Program.getDb();
                Program.appInfo = db.AppInfos.SingleOrDefault();
                Program.appInfo.IsInit = 1;
                Program.appInfo.Company_Name = null;
                Program.appInfo.Company_Address = null;
                db.SubmitChanges();
            }

            if (testPrinter)
            {
                Application.Run(new TestPrinter());
            }
            else
            {
                startupForm = new StartupForm();
                startupForm.Visible = false;
                Application.Run(startupForm);
            }
            
        }

        public static void BeginDataInitializationProcess()
        {
            
        }

        internal static bool Logout()
        {
            if (MessageBox.Show("Are you sure you want to logout?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Program.graceClose = true;
                if(Program.userLogin.Role.Equals(Roles.Admin))
                {
                    Program.mainWindow.Hide();
                    Program.mainWindow.Close();
                }
                else if (Program.userLogin.Role.Equals(Roles.Cashier))
                {
                    Program.posGui.Hide();
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
            if (!ForceClose)
            {
                if (MessageBox.Show("Are you sure you want to exit?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Program.graceClose = true;
                    Application.Exit();
                    return false;
                }
            }
            else
            {
                Program.graceClose = true;
                Application.Exit();
                return false;
            }
            return true;
        }



        internal static AppUser Login(string username, string password)
        {
            FikrPosDataContext db = Program.getDb();
           
            return (from u in db.AppUsers
                            where u.Username == username
                            && u.Password == Cryptho.Encrypt(password)
                            select u).SingleOrDefault();

        }

        internal static void UserEnter()
        {
            if (Program.userLogin.Role.Equals(Roles.Admin))
            {
                if (Program.mainWindow != null)
                {
                    Program.mainWindow.Dispose();
                }

                Program.mainWindow = new MainWindow();
                Program.mainWindow.startAutoupdateTimer();
                Program.mainWindow.ShowDialog();
            }
            else if (Program.userLogin.Role.Equals(Roles.Cashier))
            {
                if (Program.posGui != null)
                {
                    Program.posGui.Dispose();
                }
                Program.posGui = new PosGui();
                Program.posGui.Show();
            }
        }

        public static FikrPosDataContext getDb(bool openSettingPage=true)
        {
            
            string connectionString = null;
            //RegistrySettings.getInstance().loadValues();
            if (RegistrySettings.getInstance().serverLogin)
            {
                connectionString = "Server=" + RegistrySettings.getInstance().SqlHost + ";Database=" + RegistrySettings.getInstance().SqlDatabase + ";Uid=" + RegistrySettings.getInstance().SqlUsername + ";Pwd=" + RegistrySettings.getInstance().SqlPassword;
            }
            else
            {
                connectionString = "Server=" + RegistrySettings.getInstance().SqlHost + ";Database=" + RegistrySettings.getInstance().SqlDatabase + ";Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
            }
            FikrPosDataContext db = new FikrPosDataContext(connectionString);            

            try
            {

                if (db.Connection.State == ConnectionState.Closed)
                {
                    db.Connection.Open();
                }
            }
            catch (SqlException ex)
            {
                //MessageBox.Show("Database not functional. Please correct it first");
                if (openSettingPage)
                {
                    Settings settings = new Settings();
                    DialogResult dr = settings.ShowDialog();
                    if (!settings.connectionSucces && dr == DialogResult.Cancel)
                    {
                        MessageBox.Show("Application must use a valid database. Application will now exit");
                        Environment.Exit(-1);
                    }
                    else
                    {
                        db = getDb();
                    }
                }
                
            }
            db.Refresh(RefreshMode.OverwriteCurrentValues);
            return db;
        }

        public static void closeConnection()
        {
            FikrPosDataContext db = getDb();
            if (db != null)
            {
                if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                db.Connection.Dispose();
                db.Dispose();
                db = null;
            }
        }

        public static void LogActivity(LogLevel logLevel, string message, EventLogEntryType eventLogEntryType)
        {
            //TODO : if I need it
        }

        public static bool isExactPayment { get; set; }
    }
}

public enum FormModeEnum
{
    Insert,
    Update
}

public enum LogLevel
    {
        Normal,
        Debug
    }