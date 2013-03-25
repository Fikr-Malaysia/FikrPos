using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FikrPos;

namespace FikrPosTest
{
    /// <summary>
    /// Summary description for LinqTest
    /// </summary>
    [TestClass]
    public class LinqTest
    {
        FikrPosDataContext db;
        AppUser appUser;

        public LinqTest()
        {
            db = FikrPos.Program.getDb();
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void PrepareDataTest()
        {
            appUser = db.AppUsers.Where(u => u.Username == "eko").SingleOrDefault();
            insertProducts();
            insertSale();
            
        }

        private void insertSale()
        {
            db.ExecuteCommand("Delete from Sale");

            Sale sale;
            sale = new Sale();
            sale.UserId = appUser.ID;
            sale.Date = new DateTime();
            
            Product p0 = db.Products.Where(p=>p.Code=="000").SingleOrDefault();
            Product p1 = db.Products.Where(p => p.Code == "001").SingleOrDefault();
            Product p2 = db.Products.Where(p => p.Code == "002").SingleOrDefault();
            
            SaleDetail saleDetail;
            saleDetail = new SaleDetail();
            saleDetail.Product = p0;
            saleDetail.Qty = 5;
            sale.SaleDetails.Add(saleDetail);

            saleDetail = new SaleDetail();
            saleDetail.Product = p1;
            saleDetail.Qty = 2;
            sale.SaleDetails.Add(saleDetail);

            db.Sales.InsertOnSubmit(sale);
            db.SubmitChanges();

            

            

        }

        private void insertProducts()
        {
            db.ExecuteCommand("Delete from Product");

            Product p;
            p = new Product();
            p.Code = "000";
            p.Name = "Product 000";
            p.Price = 100;
            p.Stock = 10;
            p.Minimum_Stock = 1;
            p.Tax = 0;
            p.Discount = 0;
            p.Unit = "box";
            db.Products.InsertOnSubmit(p);


            p = new Product();
            p.Code = "001";
            p.Name = "Product 001";
            p.Price = 1000;
            p.Stock = 100;
            p.Minimum_Stock = 10;
            p.Tax = 1;
            p.Discount = 1;
            p.Unit = "pcs";
            db.Products.InsertOnSubmit(p);

            p = new Product();
            p.Code = "002";
            p.Name = "Product 002";
            p.Price = 2000;
            p.Stock = 200;
            p.Minimum_Stock = 20;
            p.Tax = 2;
            p.Discount = 2;
            p.Unit = "dus";
            db.Products.InsertOnSubmit(p);

            db.SubmitChanges();
        }
    }
}
