﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FikrPos.Models;
using FikrPos.Forms.Pos;

namespace FikrPos.Forms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            Program.graceClose = false;
            lblCompanyName.Text = Program.appInfo.Company_Name;
            FikrPosDataContext db = Program.getDb();
            var users = from user in db.AppUsers
                        orderby user.Username
                        select user;
            cboUsername.Items.Clear();
            foreach (AppUser user in users)
            {
                cboUsername.Items.Add(user.Username);
            }
            db.Dispose();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            Program.userLogin = Program.Login(cboUsername.Text, txtPassword.Text);
            
            if (Program.userLogin == null)
            {
                MessageBox.Show("Please check your username and password again");
            }
            else
            {
                
                Program.graceClose = true;
                Hide();
                Close();
                Program.graceClose = false;
                Program.UserEnter();
            }

        }

        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Program.Exit();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.graceClose)
            {
                e.Cancel = Program.Exit();
            }
            else
            {
                e.Cancel = false;
            }
            
        }
    }
}
