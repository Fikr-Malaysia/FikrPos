using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FikrPos
{
    static class Program
    {
        static string EventLogName = "FikrPos";
        public static StartupForm startupForm;
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
            MessageBox.Show("Init data");
        }
    }
}
