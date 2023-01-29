using SiGG_MobileApp_Server.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class OutputUserLogon
    {
        private string m_section;
        private iniFileManager m_iniFileMgr;
        private string m_userAccount;
        private bool m_isEnableUser;
        private string m_userAccessAllowedListStatus;
        private string m_userAccessAllowedList;  

        public OutputUserLogon(string section, iniFileManager iniFileMgr, string userAccount, bool isEnableUser, string userAccessAllowedListStatus, string userAccessAllowedList)
        {
            m_section = section;
            m_iniFileMgr = iniFileMgr;
            m_userAccount = userAccount;
            m_isEnableUser = isEnableUser;
            m_userAccessAllowedListStatus = userAccessAllowedListStatus;  
            m_userAccessAllowedList = userAccessAllowedList
        }

        public string GetUserAccount()
        {
            return m_userAccount;
        }
        public bool IsEnableUser()
        {
            return m_isEnableUser;
        }

        public string GetUserAccessAllowedListStatus() { return m_userAccessAllowedListStatus; }

        public string GetUserAccessAllowedList()
        {
            return m_iniFileMgr.IniReadValue(m_section, Output.KEY_USER_ACCESS_ALLOWED_LIST);
        }
    }

}
