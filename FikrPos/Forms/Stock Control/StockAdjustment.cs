using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Linq;

namespace FikrPos.Forms.Stock_Control
{
    public partial class StockAdjustment : Form
    {
        
        public Inventory inventory = null;
        public Product product { get; set; }
        bool fromStockChange = false;
        bool fromCurrentStock = false;
        public int stockChange { get; set; }
        public int currentStock { get; set; }
        public DateTime? Date { get; set; }
        public string Message { get; set; }

        public StockAdjustment()
        {
            InitializeComponent();
        }

        
        internal void prepareForm(Product product)
        {
            FikrPosDataContext db = Program.getDb();            
            txtMessage.Text = "Manual stock adjustment";
            this.product = product;            
            inventory = db.Inventories.Where(i => i.ProductID == product.ID).Single();
            numCurrentStock.Value = inventory.Current_Quantity;
            numStockChange.Minimum = -inventory.Current_Quantity;
            numStockChange.Value = 0;
            numStockChange.Focus();
        }

        

        private void numStockChange_ValueChanged(object sender, EventArgs e)
        {
            fromStockChange = true;
            numCurrentStock.Value = inventory.Current_Quantity - -numStockChange.Value;
            fromStockChange = false;
        }

        private void numCurrentStock_ValueChanged(object sender, EventArgs e)
        {
            if (!fromStockChange)
            {
                fromCurrentStock = true;
                numStockChange.Value = numCurrentStock.Value - inventory.Current_Quantity;
                fromCurrentStock = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            numStockChange_ValueChanged(sender, e);
            numCurrentStock_ValueChanged(sender, e);

            this.stockChange = Convert.ToInt32(numStockChange.Value);
            this.currentStock = Convert.ToInt32(numCurrentStock.Value);
            this.Date = dateTimePicker1.Value;
            this.Message = txtMessage.Text;

        }


    }
}
