using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Collections;

namespace FikrPos.Library
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
            string sql;

            sql = "Select * from App Info";
            SqlCeCommand cmd = new SqlCeCommand(sql, Features.getInstance().getDataConnection());
            SqlCeDataReader rdr = cmd.ExecuteReader();
            List<List<Object>> rows = new List<List<Object>>();
            if (rdr.Read())
            {
                while (rdr.NextResult())
                {
                    List<Object> cols = new List<Object>();
                    IEnumerator enu = rdr.GetEnumerator();
                    while (enu.MoveNext())
                    {
                        cols.Add(enu.Current);
                    }
                    rows.Add(cols);
                }
            }
            rdr.Close();
            cmd.Dispose();
        }
    }
}
