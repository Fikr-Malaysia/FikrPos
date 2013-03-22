﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FikrPos.Forms;

namespace FikrPos
{
    static class Program
    {
        static string EventLogName = "FikrPos";
        public static StartupForm startupForm;
        public static AdminWindow adminWindow;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            startupForm = new StartupForm();
            Application.Run(startupForm);
        }

        public static void BeginDataInitializationProcess()
        {
            
        }

        internal static bool logout()
        {
            if (MessageBox.Show("Are you sure you want to logout?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Program.adminWindow.graceClose = true;
                Program.adminWindow.Close();
                Login login = new Login();
                login.ShowDialog();
                return false;
            }
            return true;
        }

        internal static void Exit()
        {
            if (MessageBox.Show("Are you sure you want to exit?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
