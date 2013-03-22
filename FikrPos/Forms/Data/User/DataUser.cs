using System;
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
            FikrPosDataContext db = new FikrPosDataContext();
            var appUsers = from u in db.AppUsers
                           select new { Username = u.username, IsAdmin = u.isadmin };

            dataGridView1.DataSource = appUsers;
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            InputUser inputUser = new InputUser();
            inputUser.ShowDialog();
        }
    }
}
