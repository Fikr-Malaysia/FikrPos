using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FikrPos.Forms.Data.User;
using FikrPos.Business_Logic;

namespace FikrPos.Forms.Data.SaleFolder
{
    public partial class DataSale : Form
    {
        int oldRow;
        int oldCol;

        public DataSale()
        {
            InitializeComponent();
            readData();
        }

        private void readData()
        {
            
            FikrPosDataContext db = Program.getDb();
            var product = from p in db.Sales
                          select new {p.ID, p.Date, p.AppUser.Username, p.Total_Quantity, p.Total_Price, p.Total_Tax, p.Total_Discount, p.Total_Extended_Price };

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
            //InputProduct inputProduct = new InputProduct();
            //inputProduct.FormMode = FormModeEnum.Insert;
            //DialogResult dr = inputProduct.ShowDialog();
            
            //if (dr == DialogResult.OK)
            //{
            //    readData();
            //}
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //string username = getKey();
            //InputProduct inputProduct = new InputProduct();
            //inputProduct.FormMode = FormModeEnum.Update;
            //inputProduct.prepareForm(username);
            //DialogResult dr = inputProduct.ShowDialog();
            //if (dr == DialogResult.OK)
            //{
            //    readData();
            //}
        }

        private int getKey()
        {
            return Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int key = getKey();
            if (MessageBox.Show("Are you sure yo want to delete this sale?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FikrPosDataContext db = Program.getDb();
                var sale = (from p in db.Sales
                               where p.ID==key
                               select p).SingleOrDefault();
                FikrPosBusinessLogic logic = FikrPosBusinessLogic.getInstance();
                logic.deleteSale(sale);                
                readData();
            }
        }
    }
}
