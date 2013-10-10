using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JsonRequest;

namespace FikrPos.Test
{
    public class DailyCashFlow
    {
        public string branch_name;
        public string branch_token;
        public string company_token;
       
        public class CashFlowData
        {
            public string day; //todo : DATE
            public int cash_start_of_day;
            public int cash_end_of_day;
        }

        public CashFlowData data;
    }
    public partial class TestJson : Form
    {
        string baseurl = "http://10.0.2.2:8888/api";

        public TestJson()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string daily_cash_flow = @"{ 
                 'branch_name' : 'Kopjar',
                 'branch_token' : 'empty token',
                 'company_token': 'qwerty-82827272',
                 'data' :
                   {
                       'day' : '2013-12-01',
                       'cash_start_of_day': '100000',
                       'cash_end_of_day' : '1500000'
                   } 
                }";

            var test = new DailyCashFlow
            {
                branch_name = "Kopjar",
                branch_token = "empty token",
                company_token = "not used",
                data = new DailyCashFlow.CashFlowData
                {
                    day = "2013-12-01",
                    cash_start_of_day = 100000,
                    cash_end_of_day = 1500000
                }

            };
            var request = new Request();
            request.Content = "application/x-www-form-urlencoded";
            var response = request.Execute(baseurl + "/cashflow",test, "POST");
            txtResponse.Text = response + "";
            
        }
    }
}
