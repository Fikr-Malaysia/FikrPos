using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FikrPos.Client.Forms
{
    public partial class AdministratorPassword : Form
    {
        bool graceClose = false;
        bool valid = false;
        public string AdminPassword;
        public AdministratorPassword()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            txtNewPassword_Validating(null, null);
            txtConfirmPassword_Validating(null, null);
            if (valid)
            {
                valid = txtNewPassword.Text.Equals(txtConfirmPassword.Text);
            }


            if (valid)
            {
                DialogResult = DialogResult.OK;
                graceClose = true;
                AdminPassword = txtConfirmPassword.Text;
                Close();
            }
        }

        private void btnOK_Validating(object sender, CancelEventArgs e)
        {
            if (!txtNewPassword.Text.Equals(txtConfirmPassword.Text))
            {
                errorProvider1.SetError(btnOK, "Password not match");
                valid = false;
                return;
            }
            valid = true;
            errorProvider1.Clear();
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtNewPassword.Text.Equals(""))
            {
                errorProvider1.SetError(txtNewPassword, "Please supply new password");
                valid = false;
                return;
            }
            valid = true;
            errorProvider1.Clear();

        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
           
            if (txtConfirmPassword.Text.Equals(""))
            {
                errorProvider1.SetError(txtConfirmPassword, "Please supply new password");
                valid = false;
                return;
            }
            valid = true;
            errorProvider1.Clear();
        }

        private void AdministratorPassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (graceClose)
            {
                return;
            }
            graceClose = false;
            e.Cancel = true;
        }
    }
}
