using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FikrPos.Library;

namespace FikrPos.Client.Forms
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            RegistrySettings.loadValues();
            ReadValues();
        }
        private void ReadValues()
        {
            txtHost.Text = RegistrySettings.SqlHost;
            cboDatabaseType.Text = RegistrySettings.dbType;
            optServerLogin.Checked = RegistrySettings.serverLogin;
            optWindowsLogin.Checked = RegistrySettings.windowsLogin;
            txtDatabase.Text = RegistrySettings.SqlDatabase;
            txtUsername.Text = RegistrySettings.SqlUsername;
            txtPassword.Text = RegistrySettings.SqlPassword;
            cboLogLevel.Text = RegistrySettings.loggingLevel;
        }

        private void SaveValues()
        {

            RegistrySettings.dbType = cboDatabaseType.Text;
            RegistrySettings.windowsLogin = optWindowsLogin.Checked;
            RegistrySettings.serverLogin = optServerLogin.Checked;
            RegistrySettings.SqlHost = txtHost.Text;
            RegistrySettings.SqlDatabase = txtDatabase.Text;
            RegistrySettings.SqlUsername = txtUsername.Text;
            RegistrySettings.SqlPassword = txtPassword.Text;
            RegistrySettings.loggingLevel = cboLogLevel.Text;
        }
        

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveValues();
            RegistrySettings.writeValues();
            RegistrySettings.loadValues();
            AppFeatures.getInstance().resetConnection();
            Close();            
        }
    }
}
