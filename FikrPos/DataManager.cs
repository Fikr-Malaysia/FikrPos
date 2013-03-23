using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;

namespace FikrPos
{
    public class DataManager
    {
        private static DataManager instance;        
        private DataManager()
        {
        }

        public static DataManager getInstance()
        {
            if (instance == null)
            {
                instance = new DataManager();
            }
            return instance;
        }

        public void initData()
        {
            FikrPosDataContext db = new FikrPosDataContext();
            AppStates.appInfo = db.AppInfos.Single();
        }
    }
}
