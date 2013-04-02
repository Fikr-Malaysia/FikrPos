using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FikrPos.Forms
{
    public partial class Settings : Form
    {
        public bool connectionSucces = false;
        public Settings()
        {
            InitializeComponent();
            RegistrySettings.getInstance().loadValues();
            ReadValues();
        }
        private void ReadValues()
        {
            txtHost.Text = RegistrySettings.getInstance().SqlHost;
            cboDatabaseType.Text = RegistrySettings.getInstance().dbType;
            optServerLogin.Checked = RegistrySettings.getInstance().serverLogin;
            optWindowsLogin.Checked = RegistrySettings.getInstance().windowsLogin;
            txtDatabase.Text = RegistrySettings.getInstance().SqlDatabase;
            txtUsername.Text = RegistrySettings.getInstance().SqlUsername;
            txtPassword.Text = RegistrySettings.getInstance().SqlPassword;
            cboLogLevel.Text = RegistrySettings.getInstance().loggingLevel;
        }

        private void SaveValues()
        {

            RegistrySettings.getInstance().dbType = cboDatabaseType.Text;
            RegistrySettings.getInstance().windowsLogin = optWindowsLogin.Checked;
            RegistrySettings.getInstance().serverLogin = optServerLogin.Checked;
            RegistrySettings.getInstance().SqlHost = txtHost.Text;
            RegistrySettings.getInstance().SqlDatabase = txtDatabase.Text;
            RegistrySettings.getInstance().SqlUsername = txtUsername.Text;
            RegistrySettings.getInstance().SqlPassword = txtPassword.Text;
            RegistrySettings.getInstance().loggingLevel = cboLogLevel.Text;
        }
        

        private void btnOK_Click(object sender, EventArgs e)
        {
            btnTestDatabaseConnection_Click(sender, e);
            if (!connectionSucces && MessageBox.Show("Your connection is not verified yet. If you cancel, application will exit. Are you sure?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Environment.Exit(-1);
            }

            //Mark that it's success 
            RegistrySettings.getInstance().installationSuccess();
            SaveValues();
            RegistrySettings.getInstance().writeValues();
            RegistrySettings.getInstance().loadValues();
            Close();            
        }

        private void btnTestDatabaseConnection_Click(object sender, EventArgs e)
        {
            connectionSucces = false;
            SaveValues();
            RegistrySettings.getInstance().writeValues();
            FikrPosDataContext db = Program.getDb(false);
            db.Connection.Close();
            
            try
            {

                db.Connection.Open();
                connectionSucces = true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Database Connection");
            }
            

            if (connectionSucces)
            {
                MessageBox.Show("Connection success");
                db.Connection.Close();
            }
            
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceClose)
            {

                e.Cancel = !connectionSucces;
            }
        }

        bool forceClose = false;
        private void btnCancel_Click(object sender, EventArgs e)
        {
           
            forceClose = true;
            Close();
        }
    }
}
