using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FikrPos;
using FikrPos.Models;
using System.Data.SqlClient;

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
        public void TestNormalSale()
        {
            initialData();
            insertNormalSale();            
        }

        [TestMethod]
        public void TestNegativeSale()
        {
            initialData();
            insertNegativeSale();
        }

        private void insertNegativeSale()
        {
            try
            {
                db.Transaction = db.Connection.BeginTransaction();
                db.ExecuteCommand("Delete from Sale");
                Sale sale;
                sale = new Sale();
                sale.UserId = appUser.ID;
                sale.Date = new DateTime();
                Product p0 = db.Products.Where(p => p.Code == "000").SingleOrDefault();

                SaleDetail sd = new SaleDetail();
                sd.ProductID = p0.ID;
                sd.Qty = 10 + 2;
                sd.Tax = Convert.ToDouble(p0.Tax);
                sd.Discount= p0.Discount;
                sd.Price= p0.Price;
                sale.SaleDetails.Add(sd);
                db.Sales.InsertOnSubmit(sale);
                db.SubmitChanges();
                db.Transaction.Commit();
            }
            catch (SqlException ex)
            {
                db.Transaction.Rollback();
            }
        }

        private void initialData()
        {
            insertAppUsers();
            appUser = db.AppUsers.Where(u => u.Username == "eko").SingleOrDefault();
            insertProducts();
        }

        private void insertAppUsers()
        {
            db.ExecuteCommand("Delete from AppUser");
            appUser = new AppUser();
            appUser.Username = "eko";   
            appUser.Password = Cryptho.Encrypt("muhammad");
            appUser.Role = Roles.Cashier;
            db.AppUsers.InsertOnSubmit(appUser);
            appUser = new AppUser();
            appUser.Username = "admin";
            appUser.Password = Cryptho.Encrypt("admin");
            appUser.Role = Roles.Admin;
            db.AppUsers.InsertOnSubmit(appUser);
            db.SubmitChanges();
        }

        private void insertNormalSale()
        {
            try
            {
                db.Transaction = db.Connection.BeginTransaction();
                db.ExecuteCommand("Delete from Sale");
                Sale sale;
                sale = new Sale();
                sale.UserId = appUser.ID;
                sale.Date = new DateTime();
                Product p0 = db.Products.Where(p => p.Code == "000").SingleOrDefault();

                SaleDetail sd = new SaleDetail();
                sd.ProductID = p0.ID;
                sd.Qty = 1;
                sd.Tax = Convert.ToDouble(p0.Tax);
                sd.Discount = p0.Discount;
                sd.Price = p0.Price;
                sale.SaleDetails.Add(sd);
                db.Sales.InsertOnSubmit(sale);
                db.SubmitChanges();
                db.Transaction.Commit();
            }
            catch (SqlException ex)
            {
                db.Transaction.Rollback();
            }
        }

        private void insertProducts()
        {
            db.ExecuteCommand("Delete from Product");

            Product p;
            p = new Product();
            p.Code = "000";
            p.Name = "Product 000";
            p.Price = 100;
            int Stock = 10;
            int Minimum_Stock = 1;
            p.Tax = 0;
            p.Discount = 0;
            p.Unit = "box";
            db.InsertProduct(p.Code, p.Name, p.Price, p.Unit, p.Discount, p.Tax, Stock, Minimum_Stock);

            
            p = new Product();
            p.Code = "001";
            p.Name = "Product 001";
            p.Price = 1000;
            Stock = 100;
            Minimum_Stock = 10;
            p.Tax = 1;
            p.Discount = 1;
            p.Unit = "pcs";
            db.InsertProduct(p.Code, p.Name, p.Price, p.Unit, p.Discount, p.Tax, Stock, Minimum_Stock);
            

            p = new Product();
            p.Code = "002";
            p.Name = "Product 002";
            p.Price = 2000;
            Stock = 200;
            Minimum_Stock = 20;
            p.Tax = 2;
            p.Discount = 2;
            p.Unit = "dus";
            db.InsertProduct(p.Code, p.Name, p.Price, p.Unit, p.Discount, p.Tax, Stock, Minimum_Stock);
            db.SubmitChanges();            
        }
    }
}
