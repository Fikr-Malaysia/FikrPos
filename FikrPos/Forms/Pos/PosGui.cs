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
        PosStateEnum posState;
        public PosGui()
        {
            InitializeComponent();
            Program.graceClose = false;
            lblCompanyName.Text = AppStates.appInfo.Company_Name;
            lblAddress.Text = AppStates.appInfo.Company_Address;
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            prepareNewPosTransaction();
        }

        private void prepareNewPosTransaction()
        {
            posState = PosStateEnum.EnteringTransaction;
            hashSaleDetail = new Hashtable();
            sale = new Sale();
            sale.UserId = AppFeatures.userLogin.ID;
            sale.Date = new DateTime();

            txtDiscount.Text = "";
            txtTax.Text = "";
            txtSubTotal.Text = "";
            txtTotal.Text = "";
            txtQuantity.Text = "";

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
                    saleDetail.Price = product.Price;
                    saleDetail.Discount = product.Discount;
                    saleDetail.Tax = Convert.ToDouble(product.Tax);
                    double flatPrice = saleDetail.Price * saleDetail.Qty;
                    double discount = flatPrice * (saleDetail.Discount / 100);
                    double tax = flatPrice * (saleDetail.Tax / 100);
                    saleDetail.Extended_Price = flatPrice - discount + tax;
                    foreach (DataGridViewRow r in dataGridView1.Rows)
                    {
                        if (r.Cells[0].Value.Equals(productCode))
                        {
                            r.Cells[2].Value = Convert.ToInt32(r.Cells[2].Value) + 1;
                            r.Cells[6].Value = saleDetail.Extended_Price;
                            break;
                        }
                    }
                }else
                {
                    saleDetail = new SaleDetail();
                    saleDetail.ProductID = product.ID;
                    saleDetail.Price = product.Price;
                    saleDetail.Qty = 1;
                    saleDetail.Discount = product.Discount;
                    saleDetail.Tax = Convert.ToDouble(product.Tax);
                    double flatPrice = saleDetail.Price * saleDetail.Qty;
                    double discount = flatPrice * (saleDetail.Discount / 100);
                    double tax = flatPrice * (saleDetail.Tax/100);
                    saleDetail.Extended_Price = flatPrice - discount + tax;
                    
                    sale.SaleDetails.Add(saleDetail);
                    hashSaleDetail.Add(productCode, saleDetail);

                    dataGridView1.Rows.Add(new Object[] { product.Code, product.Name, saleDetail.Qty, saleDetail.Price,saleDetail.Discount,saleDetail.Tax,saleDetail.Extended_Price });
                }

                calculateFooter();
                txtScanCode.Text = "";
            }
            else
            {
                MessageBox.Show("Product unknown");
            }
        }

        private void calculateFooter()
        {
            //calculate footer
            double footerSubTotal = 0;
            int footerQty = 0;
            double footerDiscount = 0;
            double footerTax = 0;
            double footerTotal = 0;

            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                double rowExtendedPrice = Convert.ToDouble(r.Cells[6].Value);
                double rowPrice = Convert.ToDouble(r.Cells[3].Value);
                int rowQty = Convert.ToInt32(r.Cells[2].Value);

                footerSubTotal += (rowQty * rowPrice);
                footerQty += rowQty;

                double tax = Convert.ToDouble(r.Cells[5].Value);
                double rowTax = 0;
                if (tax != 0)
                {
                    rowTax = ((rowQty * rowPrice) * (tax / 100));
                    footerTax += rowTax;
                }

                double discount = Convert.ToDouble(r.Cells[4].Value);
                double rowDiscount = 0;
                if (discount != 0)
                {
                    rowDiscount = ((rowQty * rowPrice) * (discount / 100));
                    footerDiscount += rowDiscount;
                }
            }
            footerTotal = footerSubTotal + footerTax - footerDiscount; ;

            txtQuantity.Text = footerQty + "";
            txtDiscount.Text = footerDiscount + "";
            txtTax.Text = footerTax + "";
            txtSubTotal.Text = footerSubTotal + "";
            txtTotal.Text = footerTotal + "";

            sale.Total_Price = footerSubTotal;
            sale.Total_Quantity = footerQty;
            sale.Total_Discount = footerDiscount;
            sale.Total_Extended_Price = footerTotal;
            sale.Total_Tax = footerTax;
        }

        private void PosGui_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    txtScanCode.Focus();
                    break;
                case Keys.Up:
                    if (dataGridView1.Focused && dataGridView1.SelectedCells != null)//&& dataGridView1.SelectedCells[0].RowIndex == 0
                    {
                        txtScanCode.Focus();
                        e.SuppressKeyPress = false;
                    }
                    break;
                case Keys.Down:
                    if (txtScanCode.Focused)
                    {
                        dataGridView1.Focus();
                        dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[2].Selected = true;
                    }
                    break;
                case Keys.F12:
                    if (posState == PosStateEnum.EnteringTransaction)
                    {
                        posState = PosStateEnum.ReceivingPayment;
                        ReceivePayment receivePayment = new ReceivePayment();
                        receivePayment.prepareForm(Convert.ToDouble(txtTotal.Text));
                        DialogResult dr = receivePayment.ShowDialog();
                        if (dr == DialogResult.OK)
                        {
                            FikrPosDataContext db = Program.getDb();
                            sale.Payment = receivePayment.payment;
                            sale.Change = receivePayment.change;
                            db.Sales.InsertOnSubmit(sale);
                            db.SubmitChanges();
                            prepareNewPosTransaction();
                        }
                    }
                    break;
                
            }
        }

        private void PosGui_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.graceClose)
            {
                e.Cancel = Program.Exit();
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            calculateFooter();
        }
    }

    enum PosStateEnum
    {
        EnteringTransaction,
        ReceivingPayment
    }
}
