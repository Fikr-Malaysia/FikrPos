using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FikrPos
{
    public class RegistrySettings
    {
        private static RegistrySettings instance = null;
        private static ModifyRegistry reg = new ModifyRegistry();

        public string dbType;
        public bool windowsLogin;
        public bool serverLogin;
        public string SqlHost;
        public string SqlDatabase;
        public string SqlUsername;
        public string SqlPassword;
        public string loggingLevel;
        public string newInstall;

        private RegistrySettings()
        {
            reg.SubKey = "SOFTWARE\\Fikr Malaysia\\Pos";
            loadValues();
        }

        public static RegistrySettings getInstance()
        {
            if (instance == null)
            {
                instance = new RegistrySettings();
            }
            return instance;

        }

        public void loadValues()
        {
            windowsLogin = ((int)reg.Read("windowsLogin", 1) == 1);            
            serverLogin = ((int)reg.Read("serverLogin", 0) == 1);
            dbType = (string)reg.Read("dbType", null);
            SqlHost = (string)reg.Read("SqlHost", null);
            SqlDatabase = (string)reg.Read("SqlDatabase", null);
            SqlUsername = (string)reg.Read("SqlUsername", null);
            SqlPassword = Cryptho.Decrypt((string)reg.Read("SqlPassword", Cryptho.Encrypt("")));
            loggingLevel = (string)reg.Read("loggingLevel", "Normal");
            //special value for new installation
            newInstall = (string)reg.Read("newInstall", null);
        }

        public void installationSuccess()
        {
            reg.Write("newInstall", "FALSE");
        }

        public void writeValues()
        {
            reg.Write("windowsLogin", Convert.ToInt32(windowsLogin));
            reg.Write("serverLogin", Convert.ToInt32(serverLogin));
            reg.Write("dbType", dbType);
            reg.Write("SqlHost", SqlHost);
            reg.Write("SqlDatabase", SqlDatabase);
            reg.Write("SqlUsername", SqlUsername);
            reg.Write("SqlPassword", Cryptho.Encrypt(SqlPassword));
            reg.Write("loggingLevel", loggingLevel);
        }
    }
}
