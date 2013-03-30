using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FikrPos.Forms.Pos
{
    public partial class FindProduct : Form
    {
        public string ProductCode;
        public FindProduct()
        {
            InitializeComponent();
            readData();
        }

        private void readData()
        {
            FikrPosDataContext db = Program.getDb();

            var product = db.Products.Join(db.Inventories, p => p.ID, i=>i.ProductID, (p, i) => new {p.Code, p.Name, p.Price, p.Discount, p.Tax, i.Current_Quantity});
            /*var product = from p in db.Products
                          where p.Name.Contains(txtName.Text)
                          select new { p.Code, p.Name, p.Price, p.Discount, p.Tax, p.Stock };*/


            dataGridView1.DataSource = product;
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            
            
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            readData();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ProductCode = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
        }
    }
}
