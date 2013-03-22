using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace FikrPos
{
    public class AppFeatures
    {
        private SqlConnection dataConnection = null;
        private static AppFeatures instance = null;
        public static string StartupPath;
        public static AppUser userLogin;

        private AppFeatures()
        {
            RegistrySettings.loadValues();
            resetConnection();
        }
        public static AppFeatures getInstance()
        {
            if (instance == null)
            {
                instance = new AppFeatures();
            }
            return instance;
        }

        public SqlConnection getDataConnection()
        {
            if (dataConnection.State == ConnectionState.Closed)
            {
                try
                {
                    dataConnection.Open();
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
            if (RegistrySettings.windowsLogin)
            {
                dataConnection = new SqlConnection("Server=" + RegistrySettings.SqlHost + ";Database=" + RegistrySettings.SqlDatabase + ";;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False");
            }
            else
            {
                dataConnection = new SqlConnection("Server=" + RegistrySettings.SqlHost + ";Database=" + RegistrySettings.SqlDatabase + ";Uid=" + RegistrySettings.SqlUsername + ";Pwd=" + RegistrySettings.SqlPassword);
            }
        }

        public void LogActivity(LogLevel logLevel, string message, EventLogEntryType eventLogEntryType)
        {
            if(!EventLog.SourceExists(AppStates.EventLogName))
            {
                EventLog.CreateEventSource(AppStates.EventLogName,AppStates.EventLogName);
            }
            if (RegistrySettings.loggingLevel.Equals("None")) return;
            if ((logLevel == LogLevel.Debug || logLevel == LogLevel.Normal) && RegistrySettings.loggingLevel.Equals("Debug"))
            {
                EventLog.WriteEntry(AppStates.EventLogName, message, eventLogEntryType);
            }
            else if (logLevel == LogLevel.Normal && RegistrySettings.loggingLevel.Equals("Normal"))
            {
                EventLog.WriteEntry(AppStates.EventLogName, message, eventLogEntryType);
            }
        }

        
    }

    public enum LogLevel
    {
        Normal,
        Debug
    }
}
