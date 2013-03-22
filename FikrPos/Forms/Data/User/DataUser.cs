﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FikrPos.Forms.Data.User;

namespace FikrPos.Forms.Data
{
    public partial class DataUser : Form
    {
        public DataUser()
        {
            InitializeComponent();
            readData();
        }

        private void readData()
        {
            FikrPosDataContext db = new FikrPosDataContext();
            var appUsers = from u in db.AppUsers
                           select new { Username = u.username, Role = u.role };

            dataGridView1.DataSource = appUsers;
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            InputUser inputUser = new InputUser();
            inputUser.FormMode = FormModeEnum.Insert;
            DialogResult dr = inputUser.ShowDialog();
            if (dr == DialogResult.OK)
            {
                readData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string username = getKey();            
            InputUser inputUser = new InputUser();
            inputUser.FormMode = FormModeEnum.Update;
            inputUser.prepareForm(username);
            DialogResult dr = inputUser.ShowDialog();
            if (dr == DialogResult.OK)
            {
                readData();
            }
        }

        private string getKey()
        {
            return dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string username = getKey();
            if (MessageBox.Show("Are you sure yo want to delete this user", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FikrPosDataContext db = new FikrPosDataContext();
                var appUser = (from u in db.AppUsers
                               where u.username.Equals(username)
                               select u).SingleOrDefault();
                db.AppUsers.DeleteOnSubmit(appUser);
                db.SubmitChanges();
                readData();
            }
        }
    }
}
