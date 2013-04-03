using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FikrPos.Business_Logic
{    
    class FikrPosBusinessLogic
    {
        private static FikrPosBusinessLogic instance = null;
        private FikrPosBusinessLogic()
        {
        }

        public static FikrPosBusinessLogic getInstance()
        {
            if (instance == null)
            {
                instance = new FikrPosBusinessLogic();
            }
            return instance;
        }

        internal void deleteSale(Sale sale)
        {
            FikrPosDataContext db = Program.getDb();
            try
            {
                sale = db.Sales.Where(p => p.ID == sale.ID).SingleOrDefault();
                db.Transaction = db.Connection.BeginTransaction();
                foreach (SaleDetail sd in sale.SaleDetails)
                {
                    Inventory inventory = db.Inventories.Where(p => p.ProductID == sd.ProductID).SingleOrDefault();
                    insertInventoryChange(inventory.ID, sd.ProductID, sd.Qty, inventory.Current_Quantity + sd.Qty, DateTime.Now, "Sale cancellation by " + Program.userLogin.Username);
                }
                //should not make change in Report Sale : it must be generated
                db.Sales.DeleteOnSubmit(sale);
                db.SubmitChanges();
                db.Transaction.Commit();
            }
            catch (Exception ex)
            {
                db.Transaction.Rollback();
                throw ex;
            }

        }

        internal void insertInventoryChange(int inventoryId, int productId, int change, int currentQuantity, DateTime dateTime, string message)
        {
            FikrPosDataContext db = Program.getDb();
            try
            {
                db.Transaction = db.Connection.BeginTransaction();
                InventoryDetail inventoryDetail = new InventoryDetail();
                inventoryDetail.InventoryID = inventoryId;
                inventoryDetail.Date = dateTime;
                inventoryDetail.Change = change;
                inventoryDetail.Message = message;
                inventoryDetail.Current_Stock = currentQuantity;
                db.InventoryDetails.InsertOnSubmit(inventoryDetail);

                Inventory inventory = db.Inventories.Where(p => p.ID == inventoryId).SingleOrDefault();
                inventory.Current_Quantity = currentQuantity;
                inventory.Date = dateTime;
                inventory.Message = message;
                db.SubmitChanges();
                db.Transaction.Commit();
            }
            catch (Exception e)
            {
                db.Transaction.Rollback();
                throw e;
            }
        }
    }
}
