using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Printing;
using System.Drawing.Printing;
using System.Drawing;

namespace FikrPos.Modules.Printing
{
    class ReceiptPrinting
    {
        public void testPrint()
        {
            string s = "Hello"; 
            PrintDocument p = new PrintDocument();
            p.DocumentName = "Test";
            p.PrintPage += delegate(object sender, PrintPageEventArgs e)
            {             
                e.Graphics.DrawString(s, new Font("Times New Roman", 12), new SolidBrush(Color.Black), new RectangleF(0, 0, 100, 100));
            };
            try
            {                    
                p.Print();
                cutPaper();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }

        private static void cutPaper()
        {
            string GS = Convert.ToString((char)29);
            string ESC = Convert.ToString((char)27);

            string COMMAND = "";
            COMMAND = ESC + "@";
            COMMAND += GS + "V" + (char)1;
            // must test if its work in print cutting 
            RawPrinterHelper.SendStringToPrinter("Bullzip PDF Printer", COMMAND);
        }

        internal void printPosSale(Sale sale)
        {
            PrintDocument p = new PrintDocument();
            p.DocumentName = "POS";
            p.PrintPage += delegate(object sender, PrintPageEventArgs e)
            {   
                printRow(e, 0, AppStates.appInfo.Company_Name);
                printRow(e, 1, AppStates.appInfo.Company_Address);

                int i = 2;
                foreach (SaleDetail sd in sale.SaleDetails)
                {
                    ++i;
                    printRow(e, i, sd.Product.Code + " " + sd.Product.Name + " " + sd.Qty + " " + sd.Price);
                }

                printRow(e, ++i, sale.Total_Price + "");
            };
            try
            {
                p.Print();
                //cutPaper();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }

        private static void printRow(PrintPageEventArgs e, int row, string s, int width = 500, int yspace=30)
        {
            e.Graphics.DrawString(s, new Font("Times New Roman", 12), new SolidBrush(Color.Black), new RectangleF(0, row * yspace, width, 20));
        }
    }
}
