using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FikrPos.Forms.Stock_Control
{
    public partial class OpenStock : Form
    {
        Product product;
        FikrPosDataContext db;
        public OpenStock()
        {
            InitializeComponent();
            db = Program.getDb();
        }

        internal void prepareForm(string productCode)
        {
            this.productCode = productCode;
            product = db.Products.Where(p => p.Code == productCode).SingleOrDefault();
            Text = "Open Stock for Product " + product.Code + " : " + product.Name;
            readData();
        }

        private void readData()
        {
            var stockHistory = db.Inventories.Join(db.InventoryDetails, p => p.ID, i => i.InventoryID, (p, i) => new { p.ProductID, i.Date, i.Message, i.Change, i.Current_Stock }).Where(p => p.ProductID == product.ID).OrderByDescending(i=>i.Date);
            dataGridView1.DataSource = stockHistory;
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        public string productCode { get; set; }

        private void btnStockAdjustment_Click(object sender, EventArgs e)
        {
            StockAdjustment stockAdjustment = new StockAdjustment();
            stockAdjustment.prepareForm(product);
            if (stockAdjustment.ShowDialog() == DialogResult.OK)
            {
                db.InsertInventoryChange(stockAdjustment.inventory.ID, product.ID, stockAdjustment.stockChange, stockAdjustment.currentStock, stockAdjustment.Date, stockAdjustment.Message);
                db.SubmitChanges();
                readData();
            }
        }
    }
}
