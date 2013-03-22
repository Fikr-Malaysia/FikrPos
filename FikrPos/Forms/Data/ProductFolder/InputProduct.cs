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
                product.code = txtCode.Text;
                product.name = txtName.Text;
                product.price = Convert.ToDouble(txtPrice.Text);
                product.unit = cboUnit.Text;
                product.discount = Convert.ToDouble(txtDiscount.Text);
                product.stock = Convert.ToInt32(txtStock.Text);
                product.minimum_stock = Convert.ToInt32(txtMinimumStock.Text);
                db.Products.InsertOnSubmit(product);
                db.SubmitChanges();
            }
            else if (FormMode == FormModeEnum.Update)
            {
                var product = (from p in db.Products
                               where p.code.Equals(oldKey)
                               select p).SingleOrDefault();
                product.code = txtCode.Text;
                product.name = txtName.Text;
                product.price = Convert.ToDouble(txtPrice.Text);
                product.unit = cboUnit.Text;
                product.discount = Convert.ToDouble(txtDiscount.Text);
                product.stock = Convert.ToInt32(txtStock.Text);
                product.minimum_stock = Convert.ToInt32(txtMinimumStock.Text);
                db.SubmitChanges();
            }
        }

        internal void prepareForm(string key)
        {
            oldKey = key;
            FikrPosDataContext db = new FikrPosDataContext();
            var product = (from p in db.Products
                           where p.code.Equals(oldKey)
                           select p).SingleOrDefault();
            if (product != null)
            {
                txtCode.Text = product.code;
                txtName.Text = product.name;
                txtPrice.Text = product.price.ToString();
                cboUnit.Text = product.unit;
                txtDiscount.Text = product.discount.ToString();
                txtStock.Text = product.stock.ToString();
                txtMinimumStock.Text = product.minimum_stock.ToString();
            }
        }
    }
}
