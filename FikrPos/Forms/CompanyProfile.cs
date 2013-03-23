using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FikrPos.Forms
{
    public partial class CompanyProfile : Form
    {
        public CompanyProfile()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            FikrPosDataContext db = Program.getDb();
            AppStates.appInfo = db.AppInfos.SingleOrDefault();
            AppStates.appInfo.Company_Name = txtName.Text;
            AppStates.appInfo.Company_Address = txtAddress.Text;
            db.SubmitChanges();
            Close();
        }

        private void CompanyProfile_Load(object sender, EventArgs e)
        {
            if (AppStates.appInfo != null)
            {
                txtName.Text = AppStates.appInfo.Company_Name;
                txtAddress.Text = AppStates.appInfo.Company_Address;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bool success = false;
            if (AppStates.appInfo != null)
            {
                success = AppStates.appInfo.Company_Name != null && AppStates.appInfo.Company_Address != null;
            }

            if (success)
            {
                Close();
            }
            else
            {
                MessageBox.Show("You must give your company a basic profile");
                return;
            }
        }
    }
}
