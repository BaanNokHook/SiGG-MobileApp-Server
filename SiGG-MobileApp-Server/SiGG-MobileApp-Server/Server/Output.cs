using SiGG_MobileApp_Server.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class Output   // base class (parent)   
    {
        public int m_instanceNo;
        public iniFileManager m_iniFileManager;

        public OutputsConfigure.OUTPUT_TYPE m_outputType;
        public string m_key;
        public string m_section;

        public string m_marketfeedLog;
        public bool m_isSendRTL;
        public string m_sendInputDead;
        public string m_accessSllowList;
        public int m_portNumber;
        public string m_sourceName;

        public Dictionary<string, OutputUserLogon> m_dictYserLogon;
        private List<string> m_allUserLogonkey = new List<string>();

        public const string KEY_PORT_NUMBER = "PortNumber";
        public const string KEY_SOURCE_NAME = "SourceName";
        public const string KEY_ACCESS_ALLOWED_LIST = "AccessAllowedList";
        public const string KEY_CLIENTS_ACCESS_ALLOWED_LIST = "AccessAllowedList";
        public const string KEY_MARKETFEED_LOGGING = "MarketfeedLogging";
        public const string KEY_SEND_RTL = "SendRTL";
        public const string KEY_SEND_INPUT_DEAD = "SendInputDead";
        public const string KEY_ENABLE_USER = "EnableUser";
        public const string KEY_USER_ACCOUNT = "UserAccount";
        public const string KEY_USER_ACCESS_ALLOWED_LIST = "UserAccessAllowedList";
        public const string KEY_USER_ACCESS_ALLOWED_LIST_STATUS = "UserAccessAllowedListStatus";

        public const string SPKEY_USERLOGONS_NAME = "User Logons";
        public const string SPKEY_USERLOGONS = "Outputs\\{0}\\UserLogons";

        public Output(int instanceNo, iniFileManager iniFileMgr, OutputsConfiguration.OUTPUT_TYPE outputType, string key, string section)
        {
            m_instanceNo = instanceNo;
            m_iniFileMgr = iniFileMgr;
            m_outputType = outputType;
            m_key = key;
            m_section = section;

            m_dictUserLogon = new Dictionary<string, OutputUserLogon>();
            m_allUserLogonKey = new List<string>();

            Initialise();
        }

        public virtual void Initialise()
        {
            // Access Allow List	e.g. SIMA_TOF,SIMA_DTF
            m_accessAllowList = m_iniFileMgr.IniReadValue(m_section, KEY_CLIENTS_ACCESS_ALLOWED_LIST);

            if (m_accessAllowList.Length == 0)
            {
                m_accessAllowList = "ALL";
            }

            // Examine common output keys
            m_marketfeedLog = m_iniFileMgr.IniReadValue(m_section, KEY_MARKETFEED_LOGGING);

            // Get RTL
            string rtl = m_iniFileMgr.IniReadValue(m_section, KEY_SEND_RTL);
            m_isSendRTL = rtl.Equals("TRUE");

            // Send notification of input dying...
            m_sendInputDead = m_iniFileMgr.IniReadValue(m_section, KEY_SEND_INPUT_DEAD);

            m_portNumber = 0;
            m_sourceName = "";

            switch (m_outputType)
            {
                case OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_SOCKET:
                case OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_DTAPP:
                    {
                        //string port = m_iniFileMgr.IniReadValue(m_section, KEY_PORT_NUMBER);
                        //int.TryParse(port, out m_portNumber);

                        if (m_outputType == OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_DTAPP)
                        {
                            CreateUserLogonMap(m_key);
                        }
                    }
                    break;
            }
        }

        private void CreateUserLogonMap(string key)
        {
            m_dictUserLogon.Clear();
            m_allUserLogonKey.Clear();

            //[Outputs\<output_name>\UserLogons]
            string section = string.Format(SPKEY_USERLOGONS, key);
            string[] strArray = m_iniFileMgr.GetAllKeysInSection(section);

            for (int index = 0; index < strArray.Length; index++)
            {
                string strName = "";
                string strSection = "";

                m_iniFileMgr.SplitKeyAndValue(strArray[index], ref strName, ref strSection);

                strName = strName.Trim();

                if (!m_dictUserLogon.ContainsKey(strName))
                {
                    string enableUser = m_iniFileMgr.IniReadValue(strSection, KEY_ENABLE_USER);

                    if (enableUser.Length == 0)
                    {
                        enableUser = "FALSE";
                        m_iniFileMgr.IniWriteValue(strSection, KEY_ENABLE_USER, enableUser);
                    }

                    bool isEnableUser = false;
                    if (enableUser.Equals("TRUE"))
                    {
                        isEnableUser = true;
                    }
                    else
                    {
                        isEnableUser = false;
                    }

                    string userAccount = m_iniFileMgr.IniReadValue(strSection, KEY_USER_ACCOUNT).Trim();
                    string userAccessAllowedListStatus = m_iniFileMgr.IniReadValue(strSection, KEY_USER_ACCESS_ALLOWED_LIST_STATUS);
                    string userAccessAllowedList = m_iniFileMgr.IniReadValue(strSection, KEY_USER_ACCESS_ALLOWED_LIST);

                    OutputUserLogon userLogon = new OutputUserLogon(strSection, m_iniFileMgr, userAccount, isEnableUser, userAccessAllowedListStatus, userAccessAllowedList);
                    m_dictUserLogon.Add(strName, userLogon);
                    m_allUserLogonKey.Add(strName);
                }
            }
        }

        public List<string> GetAllUserLogonKey()
        {
            return m_allUserLogonKey;
        }

        public Dictionary<string, OutputUserLogon> GetMapUserLogon()
        {
            return m_dictUserLogon;
        }

        public int GetTotalUserLogon()
        {
            return m_dictUserLogon.Count;
        }

        public OutputUserLogon GetUserInfo(string userLogon)
        {
            OutputUserLogon outputUserLogon = null;

            if (m_outputType == OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_DTAPP)
            {
                if (m_dictUserLogon.ContainsKey(userLogon.ToUpper()))
                {
                    outputUserLogon = m_dictUserLogon[userLogon.ToUpper()];
                }
            }

            return outputUserLogon;
        }

        public iniFileManager GetIniFileManager()
        {
            return m_iniFileMgr;
        }
    }
}
