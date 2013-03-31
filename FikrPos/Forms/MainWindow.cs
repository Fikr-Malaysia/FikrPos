using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FikrPos.Forms.Data;
using FikrPos.Forms.Data.ProductFolder;
using FikrPos.Forms.Stock_Control;

namespace FikrPos.Forms
{
    public partial class MainWindow : Form
    {        
        private int childFormNumber = 0;

        public MainWindow()
        {
            InitializeComponent();
            Program.graceClose = false;
            Text = Application.ProductName + " version " + Application.ProductVersion;
        }

        public void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Exit();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Logout();
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataUser dataUser = new DataUser();
            dataUser.WindowState = FormWindowState.Maximized;
            dataUser.MdiParent = this;
            dataUser.Show();
        }

        private void AdminWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.graceClose)
            {
                e.Cancel = Program.Exit();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }

        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataProduct dataProduct = new DataProduct();
            dataProduct.WindowState = FormWindowState.Maximized;
            dataProduct.MdiParent = this;
            dataProduct.Show();
        }

        private void AdminWindow_Load(object sender, EventArgs e)
        {
            if (AppStates.appInfo==null || AppStates.appInfo.Company_Name == null)
            {
                MessageBox.Show("Please initialize your company data first");
                
                CompanyProfile companyProfile = new CompanyProfile();
                companyProfile.ShowDialog();
            }
        }

        private void companyProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(new CompanyProfile());
        }

        private void ShowForm(Form form)
        {
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            inventoryListToolStripMenuItem_Click(sender, e);
        }

        internal void startAutoupdateTimer()
        {
            timerAutoUpdate.Enabled = true;
        }

        private void timerAutoUpdate_Tick(object sender, EventArgs e)
        {
            timerAutoUpdate.Enabled = false;
            if (AppStates.appInfo != null)//assume that company name must be initialized!
            {
                AutoupdateEngine.automaticUpdate = true;
                checkLatestSoftwareUpdate();
            }
        }

        private void checkLatestSoftwareUpdate()
        {
            AutoupdateEngine.getInstance().CheckForUpdates(new NAppUpdate.Framework.Sources.SimpleWebSource("http://fikrpos.swdevbali.com/fikrpos.xml"));
            //AutoupdateEngine.CheckForUpdates(new NAppUpdate.Framework.Sources.SimpleWebSource("http://localhost/fikrpos/fikrpos.xml"));
        }

        private void checkLatestUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoupdateEngine.automaticUpdate = false;
            checkLatestSoftwareUpdate();
        }

        private void inventoryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InventoryControl form = new InventoryControl();
            form.WindowState = FormWindowState.Maximized;
            form.MdiParent = this;
            form.Show();
        }

    
    }
}
