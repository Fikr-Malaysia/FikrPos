using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace FikrPos.Forms.Pos
{
    public partial class PosGui : Form
    {        
        Hashtable hashSaleDetail;
        Sale sale;
        public PosGui()
        {
            InitializeComponent();
            lblCompanyName.Text = AppStates.appInfo.Company_Name;
            lblAddress.Text = AppStates.appInfo.Company_Address;
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            prepareNewPosTransaction();
        }

        private void prepareNewPosTransaction()
        {
            hashSaleDetail = new Hashtable();
            sale = new Sale();
            sale.UserId = AppFeatures.userLogin.ID;
            sale.Date = new DateTime();
            dataGridView1.Rows.Clear();
        }

        private void txtScanCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Return:
                    string productCode = txtScanCode.Text;
                    enterProduct(productCode);
                    break;
                case (char) Keys.Escape:
                    txtScanCode.Text = "";
                    break;
            }
        }

        
        private void enterProduct(string productCode)
        {
            FikrPosDataContext db = Program.getDb();
            var product = db.Products.Where(p => p.Code == productCode).SingleOrDefault();
            
            if (product != null)
            {
                SaleDetail saleDetail = null;
                if(hashSaleDetail.ContainsKey(productCode))
                {
                    saleDetail = (SaleDetail) hashSaleDetail[productCode];
                    ++saleDetail.Qty;
                    foreach (DataGridViewRow r in dataGridView1.Rows)
                    {
                        if (r.Cells[0].Value.Equals(productCode))
                        {
                            r.Cells[2].Value = Convert.ToInt32(r.Cells[2].Value) + 1;
                        }
                    }
                }else
                {
                    saleDetail = new SaleDetail();
                    saleDetail.ProductID = product.ID;
                    saleDetail.Qty = 1;
                    sale.SaleDetails.Add(saleDetail);
                    hashSaleDetail.Add(productCode, saleDetail);
                    dataGridView1.Rows.Add(new Object[] { product.Code, product.Name, saleDetail.Qty });
                }                

                
                txtScanCode.Text = "";
            }
            else
            {
                MessageBox.Show("Product unknown");
            }
        }

        private void PosGui_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F12:
                    FikrPosDataContext db = Program.getDb();
                    db.Sales.InsertOnSubmit(sale);
                    db.SubmitChanges();
                    prepareNewPosTransaction();
                    break;
            }
        }

        
    }
}
