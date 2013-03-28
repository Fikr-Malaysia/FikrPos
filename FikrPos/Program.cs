using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FikrPos.Forms;
using FikrPos.Forms.Pos;
using FikrPos.Models;
using System.Data;
using System.Diagnostics;

namespace FikrPos
{
    public static class Program
    {
        static string EventLogName = "FikrPos";
        public static StartupForm startupForm;
        public static MainWindow mainWindow;
        public static PosGui posGui;
        public static bool graceClose = false;
        public static FikrPosDataContext db = null;
        public static bool ForceClose = false;
        public static string StartupPath;
        public static AppUser userLogin;

        [STAThread]
        static void Main(string[] args)
        {
            string username = null;
            string password = null;
            bool forceInit = false;

            //example argument : admin admin noForceInit forceClose
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
                }
            }
            
            if(username!=null && password!=null)
                Program.userLogin = Program.Login(username, password);

            if (forceInit)
            {
                FikrPosDataContext db = Program.getDb();
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
                if(Program.userLogin.Role.Equals(Roles.Admin))
                {
                    Program.mainWindow.Close();
                }
                else if (Program.userLogin.Role.Equals(Roles.Cashier))
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

        public static FikrPosDataContext getDb()
        {
            if (db == null)
            {
                string connectionString = null;

                if (RegistrySettings.getInstance().serverLogin)
                {
                    connectionString = "Server=" + RegistrySettings.getInstance().SqlHost + ";Database=" + RegistrySettings.getInstance().SqlDatabase + ";Uid=" + RegistrySettings.getInstance().SqlUsername + ";Pwd=" + RegistrySettings.getInstance().SqlPassword;
                }
                else
                {
                    connectionString = "Server=" + RegistrySettings.getInstance().SqlHost + ";Database=" + RegistrySettings.getInstance().SqlDatabase + ";Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
                }
                db = new FikrPosDataContext(connectionString);
            }
            return db;
        }

        public static void closeConnection()
        {
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
            if(!EventLog.SourceExists(AppStates.EventLogName))
            {
                EventLog.CreateEventSource(AppStates.EventLogName,AppStates.EventLogName);
            }
            if (RegistrySettings.getInstance().loggingLevel.Equals("None")) return;
            if ((logLevel == LogLevel.Debug || logLevel == LogLevel.Normal) && RegistrySettings.getInstance().loggingLevel.Equals("Debug"))
            {
                EventLog.WriteEntry(AppStates.EventLogName, message, eventLogEntryType);
            }
            else if (logLevel == LogLevel.Normal && RegistrySettings.getInstance().loggingLevel.Equals("Normal"))
            {
                EventLog.WriteEntry(AppStates.EventLogName, message, eventLogEntryType);
            }
        }

        
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