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
    public partial class InventoryControl : Form
    {
        FikrPosDataContext db;
        public InventoryControl()
        {
            InitializeComponent();
            db = Program.getDb();
            ReadData();
        }

        private void ReadData()
        {

            var product = db.Products.Join(db.Inventories, p => p.ID, i => i.ProductID, (p, i) => new { p.Code, p.Name, i.Current_Quantity, i.Minimum_Stock });            

            dataGridView1.DataSource = product;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }
    }
}
