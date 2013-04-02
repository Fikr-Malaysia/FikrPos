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
    public partial class ReceivePayment : Form
    {
        double totalTransaction = 0;
        public double payment = -1;
        public double change = 0;
        string oldTxtPayment;

        public ReceivePayment()
        {
            InitializeComponent();
            btnOK.Enabled=false;
        }

     


        private void btnOK_Click(object sender, EventArgs e)
        {
            change = Convert.ToDouble(txtChange.Text);
            payment = Convert.ToDouble(txtPayment.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
        }

        internal void prepareForm(double totalTransaction)
        {
            this.totalTransaction = totalTransaction;
            txtTotalTransaction.Text = totalTransaction + "";
            btnOK.Enabled = false;
            oldTxtPayment = "0";
            txtPayment.Text = oldTxtPayment;
        }

        private void txtPayment_TextChanged(object sender, EventArgs e)
        {

            double currentPayment = 0;
            try
            {
                currentPayment = Convert.ToDouble(txtPayment.Text);
            }
            catch (Exception ex)
            {
                txtPayment.Text = oldTxtPayment;
                currentPayment = Convert.ToDouble(txtPayment.Text);
            }
            change = currentPayment - totalTransaction;
            txtChange.Text = change + "";

            updateOkButton();
            oldTxtPayment = txtPayment.Text;

        }

        private void updateOkButton()
        {
            if (change < 0)
            {
                btnOK.Enabled = false;
            }
            else
            {
                btnOK.Enabled = true;
            }
        }
    }
}
