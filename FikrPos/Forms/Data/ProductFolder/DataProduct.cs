using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FikrPos.Forms.Data.User;

namespace FikrPos.Forms.Data.ProductFolder
{
    public partial class DataProduct : Form
    {
        int oldRow;
        int oldCol;

        public DataProduct()
        {
            InitializeComponent();
            readData();
        }

        private void readData()
        {
            
            FikrPosDataContext db = Program.getDb();
            var product = from p in db.Products 
                           select p;

            if (dataGridView1.SelectedCells.Count > 0)
            {
                oldRow = dataGridView1.SelectedCells[0].RowIndex;
                oldCol = dataGridView1.SelectedCells[0].ColumnIndex;
            }
            else
            {
                oldRow = 0;
                oldCol = 1;
            }

            dataGridView1.DataSource = product;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            if (oldRow >= dataGridView1.Rows.Count)
            {
                oldRow = dataGridView1.Rows.Count - 1;
            }

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[oldRow].Cells[oldCol].Selected = true;
            }
            dataGridView1.Focus();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            InputProduct inputProduct = new InputProduct();
            inputProduct.FormMode = FormModeEnum.Insert;
            DialogResult dr = inputProduct.ShowDialog();
            
            if (dr == DialogResult.OK)
            {
                readData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string username = getKey();
            InputProduct inputProduct = new InputProduct();
            inputProduct.FormMode = FormModeEnum.Update;
            inputProduct.prepareForm(username);
            DialogResult dr = inputProduct.ShowDialog();
            if (dr == DialogResult.OK)
            {
                readData();
            }
        }

        private string getKey()
        {
            return dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string oldKey = getKey();
            if (MessageBox.Show("Are you sure yo want to delete this product?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FikrPosDataContext db = Program.getDb();
                var product = (from p in db.Products
                               where p.Code.Equals(oldKey)
                               select p).SingleOrDefault();
                db.Products.DeleteOnSubmit(product);
                db.SubmitChanges();
                readData();
            }
        }
    }
}
