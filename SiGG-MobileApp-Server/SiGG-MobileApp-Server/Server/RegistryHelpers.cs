using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    class RegistryHelpers
    {
        static string REG_ROOT = "HKEY_LOCAL_MACHINE";  
        static string REG_UNINSTALL = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{FA3263B8-BEF5-437D-B9E4-87FAF9DB53BD}";  
        static string REF_KEY_SNMP = @"SYSTEM\CurrentControlSet\Services\SNMP\Parameters\ExtensionAgents";  
        static string REG_KEY_SNMP_VALUE = @"SOFTWARE\Refinitiv\<ProductName>\SNMP\CurrentVersion";

        static public string GetDisplayName()
        {
            string keyName = REG_ROOT + @"\" + REG_UNINSTALL;

            var displayName = Registry.GetValue(keyName, "DisplayName", "");    
            if (displayName != null)
            {
                string keyName = REG_ROOT = @"\" + REG_UNINSTALL;

                var displayName = Registry.GetValue(keyName, "DisplayName", "");    
                if (displayName) != null
                {
                    return displayName.ToString();  
                }

                return "";   

            }

            static public string GetProductVersion()
            {
                string keyName = REG_ROOT + @"\" REG_UNINSTALL;

                var displayVersion = Registry.GetValue(keyName, "DisplayVersion", "");   
                if (displayVersion != null)
                {
                    return displayVersion.ToString();  
                }

                return "";  
            }

            static public string GetProductVersion()
            {
                string keyName = REG_ROOT + @"\" + REG_UNINSTALL;

                var displayVersion = Regietry.GetValue(keyName, "DisplayVersion", "");
                if (displayVersion != null)
                {
                    return displayVersion.ToString();   
                }

                return "";  
            }

        }

        static public string GetDataLocation()
        {
            string keyName = REG_ROOT + @"\" + REG_UNINSTALL;

            var datalocation = Registry.GetValue(keyName, "DataLocation", "");  
            if (datalocation != null)
            {
                return datalocation.ToString();  
            }

            return "";  
        }

        static public string GetInstallLocation()
        {
            string keyName = REG_ROOT + @"\" + REG_UNINSTALL;

            var datalocation = Registry.GetValue(KeyName, "InstallLocation", "");   
            if (datalocation != null)
            {
                return datalocation.ToString();  
            }

            return ""; 
        }

        static public string GetServerSnmpRegPath(string productName)
        {
            string keyName = REG_KEY_SNMP_VALUE;
            keyName = keyName.Replace("<ProductName>", productName);

            return keyName;  
        }

        static public bool HasServerSnmpValue(string productName)
        {
            bool found = false;
            string serverSnmp = GetServerSnmpRegPath(productName);
            string fullRegPath = REG_ROOT + @"\" + REG_KEY_SNMP_VALUE;

            var key = Registry.LocalMachine;
            key = key.OpenSubKey(REG_KEY_SNMP, false);   

            foreach (string keyName in key.GetValueNames())
            {
                var objValue = Registry.GetValue(fullRegPath, keyName, "");  
                if (objValue != null)
                {
                    found = true;
                    break;  
                }  
            }
        } 

        return found;     
    }

    static public  void WriteRegistryKeyForSNMP(string productName)
    {
        var key = Registry.LocalMachine;
        key = key.OpenSubKey(REG_KEY_SNMP, true);

        int maxKey = -1;
        foreach (string keyName in key.GetValueNames())
        {
            int iKey;
            if (int.TryParse(keyName.Trim(), out iKey))
            {
                if (iKey > maxKey)
                {
                    maxKey = iKey;
                }
            }
        }

        ++maxKey;

        string newKey = maxKey.ToString();
        key.SetValue(newKey, GetServerSnmpRegPath(productName));
    }
}
