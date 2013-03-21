using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data.SqlServerCe;
using System.Data;

namespace FikrPos.Library
{
    public class Features
    {
        private SqlCeConnection dataConnection = null;
        private static Features instance = null;
        public static string StartupPath;

        private Features()
        {
            RegistrySettings.loadValues();
            resetConnection();
        }
        public static Features getInstance()
        {
            if (instance == null)
            {
                instance = new Features();
            }
            return instance;
        }

        public SqlCeConnection getDataConnection()
        {
            if (dataConnection.State == ConnectionState.Closed)
            {
                try
                {
                    dataConnection.Open();
                }
                catch (SqlCeInvalidDatabaseFormatException ex)
                {
                    UpgradeDatabasewithCaseSensitive();
                }
                catch (Exception ex)
                {
                    LogActivity(LogLevel.Normal, "Database connection error : " + ex.Message, EventLogEntryType.Error);
                    return null;
                }                
            }
            return dataConnection;
        }

        public void resetConnection()
        {
            if (dataConnection != null)
            {
                if (dataConnection.State == ConnectionState.Open) dataConnection.Close();
                dataConnection.Dispose();
            }
            dataConnection = new SqlCeConnection("Data Source=" + StartupPath +"\\Database\\FikrPosClient.sdf");
            
        }

        public void LogActivity(LogLevel logLevel, string message, EventLogEntryType eventLogEntryType)
        {
            if(!EventLog.SourceExists(Program.EventLogName))
            {
                EventLog.CreateEventSource(Program.EventLogName,Program.EventLogName);
            }
            if (RegistrySettings.loggingLevel.Equals("None")) return;
            if ((logLevel == LogLevel.Debug || logLevel == LogLevel.Normal) && RegistrySettings.loggingLevel.Equals("Debug"))
            {
                EventLog.WriteEntry(Program.EventLogName, message, eventLogEntryType);
            }
            else if (logLevel == LogLevel.Normal && RegistrySettings.loggingLevel.Equals("Normal"))
            {
                EventLog.WriteEntry(Program.EventLogName, message, eventLogEntryType);
            }
        }

        public static void UpgradeDatabasewithCaseSensitive()
        {
            // <Snippet2>
            // Default case-insentive connection string.
            // Note that Northwind.sdf is an old 3.1 version database.

            string connStringCI = "Data Source=" + StartupPath +"\\Database\\FikrPosClient.sdf" + "; LCID= 1033";

            // Set "Case Sensitive" to true to change the collation from CI to CS.
            string connStringCS = "Data Source=" + StartupPath +"\\Database\\FikrPosClient.sdf" + "; LCID= 1033; Case Sensitive=true";

            SqlCeEngine engine = new SqlCeEngine(connStringCI);

            // The collation of the database will be case sensitive because of 
            // the new connection string used by the Upgrade method.                
            engine.Upgrade(connStringCS);

            SqlCeConnection conn = null;
            conn = new SqlCeConnection(connStringCI);
            conn.Open();

            //Retrieve the connection string information - notice the 'Case Sensitive' value.
            List<KeyValuePair<string, string>> dbinfo = conn.GetDatabaseInfo();

            Console.WriteLine("\nGetDatabaseInfo() results:");

            foreach (KeyValuePair<string, string> kvp in dbinfo)
            {
                Console.WriteLine(kvp);
            }
            // </Snippet2>

        }

    }

    public enum LogLevel
    {
        Normal,
        Debug
    }
}
