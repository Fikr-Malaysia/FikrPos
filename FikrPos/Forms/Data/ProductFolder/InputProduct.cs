using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FikrPos.Forms.Data.ProductFolder
{
    public partial class InputProduct : Form
    {
        public FormModeEnum FormMode;
        public Product product;

        private string oldKey;
        public InputProduct()
        {
            InitializeComponent();
        }

        private void InputUser_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            FikrPosDataContext db = new FikrPosDataContext();
            if (FormMode == FormModeEnum.Insert)
            {   
                product = new Product();
                product.Code = txtCode.Text;
                product.Name = txtName.Text;
                product.Price = Convert.ToDouble(txtPrice.Text);
                product.Unit = cboUnit.Text;
                product.Discount = Convert.ToDouble(txtDiscount.Text);
                product.Tax = Convert.ToDouble(txtTax.Text);
                product.Stock = Convert.ToInt32(txtStock.Text);
                product.Minimum_Stock = Convert.ToInt32(txtMinimumStock.Text);
                db.Products.InsertOnSubmit(product);
                db.SubmitChanges();
            }
            else if (FormMode == FormModeEnum.Update)
            {
                var product = (from p in db.Products
                               where p.Code.Equals(oldKey)
                               select p).SingleOrDefault();
                product.Code = txtCode.Text;
                product.Name = txtName.Text;
                product.Price = Convert.ToDouble(txtPrice.Text);
                product.Unit = cboUnit.Text;
                product.Discount = Convert.ToDouble(txtDiscount.Text);
                product.Tax = Convert.ToDouble(txtTax.Text);
                product.Stock = Convert.ToInt32(txtStock.Text);
                product.Minimum_Stock = Convert.ToInt32(txtMinimumStock.Text);
                db.SubmitChanges();
            }
        }

        internal void prepareForm(string key)
        {
            oldKey = key;
            FikrPosDataContext db = new FikrPosDataContext();
            var product = (from p in db.Products
                           where p.Code.Equals(oldKey)
                           select p).SingleOrDefault();
            if (product != null)
            {
                txtCode.Text = product.Code;
                txtName.Text = product.Name;
                txtPrice.Text = product.Price.ToString();
                cboUnit.Text = product.Unit;
                txtDiscount.Text = product.Discount.ToString();
                txtTax.Text = product.Tax.ToString();
                txtStock.Text = product.Stock.ToString();
                txtMinimumStock.Text = product.Minimum_Stock.ToString();
                
            }
        }
    }
}
