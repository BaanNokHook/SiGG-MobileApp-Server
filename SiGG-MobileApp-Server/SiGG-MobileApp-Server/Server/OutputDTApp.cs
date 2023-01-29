using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    class OutputDTApp : Output  // derived class (child)
    {
        private List<SettingData> m_allSettingData;

        private bool m_isSecureHashIn;
        private bool m_isSecureHashOut;
        private string m_secureHashKey;
        private int m_sendQueueSize;
        private int m_socketSendBufferSize;
        private int m_sleepWouldBlockTime;

        private bool m_isTCPKeepAliveEnabled;
        private int m_tcpKeepAliveTime;

        private bool m_isReloadUserInfoEnabled;
        private int m_reloadUserInfoInterval;

        public const string KEY_HASH_KEY = "HashKey";
        public const string KEY_REQUIRE_HASH_DATA_IN = "RequireHashDataIn";
        public const string KEY_TCP_KEEPALIVE_TIME = "TCPKeepAliveTime";
        public const string KEY_TCP_KEEPALIVE_ENABLED = "TCPKeepAliveEnabled";
        public const string KEY_CLIENTS_SEND_QUEUE_SIZE = "SendQueueSize";
        public const string KEY_SOCKET_SEND_BUFFER = "SocketSendBufferSize";
        public const string KEY_SLEEP_WOULDBLOCK_TIME = "SleepWouldBlockTime";
        public const string KEY_REQUIRE_HASH_DATA_OUT = "RequireHashDataOut";
        public const string KEY_ENABLE_RELOAD_USER_INFO = "EnableReloadUserInfo";
        public const string KEY_RELOAD_USER_INFO_INTERVAL = "ReloadUserInfoInterval"; //Minute

        public const int MAX_SLEEP_WOULDBLOCK_TIME = 10000;

        public const int DEF_TCP_KEEPALIVE_TIME = 60000;
        public const int DEF_CLIENTS_SEND_QUEUE_SIZE = 0;
        public const int DEF_SOCKET_SEND_BUFFER = 0;	// Default windows socket send buffer is 8 KB
        public const int DEF_SLEEP_WOULDBLOCK_TIME = 100;
        public const string DEF_ENABLE_RELOAD_USER_INFO = "FALSE";
        public const int MIN_RELOAD_USER_INFO_INTERVAL = 5;     //Minute
        public const int MAX_RELOAD_USER_INFO_INTERVAL = 60;    //Minute and Default

        public OutputDTApp(int instanceNo, iniFileManager iniFileMgr, OutputsConfiguration.OUTPUT_TYPE outputType, string key, string section) : base(instanceNo, iniFileMgr, outputType, key, section)
        {
            m_allSettingData = new List<SettingData>();
        }

        public void LoadConfiguration()
        {
            string mmcType = m_iniFileMgr.IniReadValue(m_section, ConfigManager.Instance.KEY_MMC_TYPE);
            if (mmcType.Length == 0)
            {
                m_iniFileMgr.IniWriteValue(m_section, ConfigManager.Instance.KEY_MMC_TYPE, OutputsConfiguration.OUTPUT_TYPE_DTAPP);
            }

            //	Get Secure Hash
            m_isSecureHashIn = false;
            string secureHashIn = m_iniFileMgr.IniReadValue(m_section, KEY_REQUIRE_HASH_DATA_IN);
            if (secureHashIn.Equals("TRUE"))
            {
                m_isSecureHashIn = true;
            }
            else
            {
                m_isSecureHashIn = false;
            }

            m_isSecureHashOut = false;
            string secureHashOut = m_iniFileMgr.IniReadValue(m_section, KEY_REQUIRE_HASH_DATA_OUT);
            if (secureHashOut.Equals("TRUE"))
            {
                m_isSecureHashOut = true;
            }
            else
            {
                m_isSecureHashOut = false;
            }

            if (m_isSecureHashIn || m_isSecureHashOut)
            {
                //	Get Secure Hash Key
                string secureHashKey = m_iniFileMgr.IniReadValue(m_section, KEY_HASH_KEY);
                if (secureHashKey.Length == 0)
                {
                    m_isSecureHashIn = false;
                    m_isSecureHashOut = false;
                }
                else
                {
                    m_secureHashKey = secureHashKey;
                }
            }

            //	Get port number form ini file
            m_portNumber = 0;
            int.TryParse(m_iniFileMgr.IniReadValue(m_section, KEY_PORT_NUMBER), out m_portNumber);

            m_sendQueueSize = DEF_CLIENTS_SEND_QUEUE_SIZE; // Disable send queue
            int.TryParse(m_iniFileMgr.IniReadValue(m_section, KEY_CLIENTS_SEND_QUEUE_SIZE), out m_sendQueueSize);
            if (m_sendQueueSize < 0)
            {
                m_sendQueueSize = DEF_CLIENTS_SEND_QUEUE_SIZE;
            }

            m_socketSendBufferSize = DEF_SOCKET_SEND_BUFFER;
            int.TryParse(m_iniFileMgr.IniReadValue(m_section, KEY_SOCKET_SEND_BUFFER), out m_socketSendBufferSize);
            if (m_socketSendBufferSize < 0)
            {
                m_socketSendBufferSize = DEF_SOCKET_SEND_BUFFER;
            }

            m_sleepWouldBlockTime = DEF_SLEEP_WOULDBLOCK_TIME;
            int.TryParse(m_iniFileMgr.IniReadValue(m_section, KEY_SLEEP_WOULDBLOCK_TIME), out m_sleepWouldBlockTime);
            if (m_sleepWouldBlockTime > MAX_SLEEP_WOULDBLOCK_TIME)
            {
                m_sleepWouldBlockTime = MAX_SLEEP_WOULDBLOCK_TIME;
            }
            else if (m_sleepWouldBlockTime < 0)
            {
                m_sleepWouldBlockTime = DEF_SLEEP_WOULDBLOCK_TIME;
            }

            string tcpKeepAliveEnabled = m_iniFileMgr.IniReadValue(m_section, KEY_TCP_KEEPALIVE_ENABLED);
            if (tcpKeepAliveEnabled.Equals("TRUE"))
            {
                m_isTCPKeepAliveEnabled = true;
            }
            else
            {
                m_isTCPKeepAliveEnabled = false;
            }

            m_tcpKeepAliveTime = DEF_TCP_KEEPALIVE_TIME;
            int.TryParse(m_iniFileMgr.IniReadValue(m_section, KEY_TCP_KEEPALIVE_TIME), out m_tcpKeepAliveTime);

            string enableReloadUserInfo = m_iniFileMgr.IniReadValue(m_section, KEY_ENABLE_RELOAD_USER_INFO);

            m_reloadUserInfoInterval = 0;
            int.TryParse(m_iniFileMgr.IniReadValue(m_section, KEY_RELOAD_USER_INFO_INTERVAL), out m_reloadUserInfoInterval);

            if (enableReloadUserInfo.Equals("TRUE"))
            {
                m_isReloadUserInfoEnabled = true;

                if ((m_reloadUserInfoInterval < MIN_RELOAD_USER_INFO_INTERVAL) || (m_reloadUserInfoInterval > MAX_RELOAD_USER_INFO_INTERVAL))  // Minute must check 5 to 60
                {
                    m_reloadUserInfoInterval = MAX_RELOAD_USER_INFO_INTERVAL;
                    m_iniFileMgr.IniWriteValue(m_section, KEY_RELOAD_USER_INFO_INTERVAL, m_reloadUserInfoInterval.ToString());
                }
            }
            else
            {
                m_isReloadUserInfoEnabled = false;

                if (enableReloadUserInfo.Trim().Length == 0)
                {
                    m_iniFileMgr.IniWriteValue(m_section, KEY_ENABLE_RELOAD_USER_INFO, DEF_ENABLE_RELOAD_USER_INFO);
                }

                string reloadUserInfoInterval = m_iniFileMgr.IniReadValue(m_section, KEY_RELOAD_USER_INFO_INTERVAL);
                if (reloadUserInfoInterval.Trim().Length == 0)
                {
                    m_reloadUserInfoInterval = MAX_RELOAD_USER_INFO_INTERVAL;
                    m_iniFileMgr.IniWriteValue(m_section, KEY_RELOAD_USER_INFO_INTERVAL, m_reloadUserInfoInterval.ToString());
                }
            }
        }

        public string[] LoadDefaultKey()
        {
            string[] strArray = new string[7];

            strArray[0] = SPKEY_USERLOGONS_NAME;    //User Logons Node in Treeview
            strArray[1] = KEY_MARKETFEED_LOGGING;
            strArray[2] = KEY_SEND_RTL;
            strArray[3] = KEY_PORT_NUMBER;
            strArray[4] = KEY_SEND_INPUT_DEAD;
            strArray[5] = KEY_ENABLE_RELOAD_USER_INFO;
            strArray[6] = KEY_RELOAD_USER_INFO_INTERVAL;

            return strArray;
        }

        public string[] LoadDefaultValueOfKey()
        {
            string[] strArray = new string[7];

            strArray[0] = ""; //User Logons Node in Treeview
            strArray[1] = "MED";
            strArray[2] = "TRUE";
            strArray[3] = "#N/A";
            strArray[4] = "TRUE";
            strArray[5] = DEF_ENABLE_RELOAD_USER_INFO;
            strArray[6] = "60"; //MAX_RELOAD_USER_INFO_INTERVAL

            return strArray;
        }

        public List<FieldShow> LoadDefaultFieldShow()
        {
            List<FieldShow> listFieldShow = new List<FieldShow>();

            listFieldShow.Add(FieldShow.Both);
            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.Both);
            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.OnlyAdvance);

            return listFieldShow;
        }

        public List<bool> LoadDefaultRemovable()
        {
            List<bool> listDefaultRemovable = new List<bool>();

            listDefaultRemovable.Add(false);
            listDefaultRemovable.Add(false);
            listDefaultRemovable.Add(false);
            listDefaultRemovable.Add(false);
            listDefaultRemovable.Add(false);
            listDefaultRemovable.Add(false);
            listDefaultRemovable.Add(false);

            return listDefaultRemovable;
        }

        public List<bool> LoadDefaultSpecialField()
        {
            List<bool> listDefaultSpecialField = new List<bool>();

            listDefaultSpecialField.Add(false);
            listDefaultSpecialField.Add(false);
            listDefaultSpecialField.Add(false);
            listDefaultSpecialField.Add(false);
            listDefaultSpecialField.Add(false);
            listDefaultSpecialField.Add(false);
            listDefaultSpecialField.Add(false);

            return listDefaultSpecialField;
        }

        public void LoadAllSettingData()
        {
            m_allSettingData.Clear();
            List<SettingData> allSettingData = new List<SettingData>();

            string[] strArrayDefKey = LoadDefaultKey();
            string[] strArrayDefDisp = LoadDefaultKey();
            string[] strArrayDefValofKey = LoadDefaultValueOfKey();
            List<FieldShow> listDefFieldShow = LoadDefaultFieldShow();
            List<bool> listDefRemovable = LoadDefaultRemovable();
            List<bool> listDefSpecialField = LoadDefaultSpecialField();

            string[] strArrayReadIni_tmp = m_iniFileMgr.GetAllKeysInSection(m_section);

            #region ignor Access Allowed List Key
            List<string> listReadIni = new List<string>();

            for (int i = 0; i < strArrayReadIni_tmp.Length; i++)
            {
                if (strArrayReadIni_tmp[i].IndexOf(Output.KEY_ACCESS_ALLOWED_LIST) == 0)
                {
                    continue;
                }

                listReadIni.Add(strArrayReadIni_tmp[i]);
            }

            string[] strArrayReadIni = new string[listReadIni.Count];

            for (int i = 0; i < listReadIni.Count; i++)
            {
                strArrayReadIni[i] = listReadIni[i];
            }
            #endregion

            ConfigManager.Instance.AddSettingData(true
                                                   , true
                                                   , strArrayReadIni
                                                   , strArrayDefKey
                                                   , strArrayDefDisp
                                                   , strArrayDefValofKey
                                                   , m_section
                                                   , listDefFieldShow
                                                   , listDefRemovable
                                                   , listDefSpecialField
                                                   , m_iniFileMgr
                                                   , GetServiceName()
                                                   , ref allSettingData);

            for (int i = 0; i < allSettingData.Count; i++)
            {
                SettingData settingData = allSettingData[i];
                ConfigManager.Instance.SetFieldStausToMissingRequired(settingData.m_key, settingData.m_value, ref settingData.m_status);

                switch (settingData.m_key)
                {
                    case SPKEY_USERLOGONS_NAME:
                        {
                            if (settingData.m_status == FieldStaus.Red)
                            {
                                for (int index = 0; index < strArrayDefKey.Length; index++)
                                {
                                    if (strArrayDefKey[index] == SPKEY_USERLOGONS_NAME)
                                    {
                                        settingData.m_value = strArrayDefValofKey[index];
                                        settingData.m_status = FieldStaus.Inactive_User_Logon;
                                        break;
                                    }
                                }
                            }
                        }
                        break;

                    case KEY_ACCESS_ALLOWED_LIST:
                        {
                            if (settingData.m_status == FieldStaus.Red)
                            {
                                for (int index = 0; index < strArrayDefKey.Length; index++)
                                {
                                    if (strArrayDefKey[index] == KEY_ACCESS_ALLOWED_LIST)
                                    {
                                        m_iniFileMgr.IniWriteValue(m_section, settingData.m_key, strArrayDefValofKey[index]);
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                }
            }

            m_allSettingData = ConfigManager.Instance.MakeSettingDataByView(allSettingData);
        }

        public List<SettingData> GetAllSettingData()
        {
            LoadAllSettingData();

            return m_allSettingData;
        }

        public List<SettingData> GetAllSettingDataWithoutMode()
        {
            LoadAllSettingData();

            return m_allSettingData;
        }

        public int GetInstanceNo()
        {
            return m_instanceNo;
        }

        public string GetServiceName()
        {
            return string.Format("{0}{1}", ConfigManager.Instance.GetPrefixServiceName(), m_instanceNo.ToString("00"));
        }

        public int GetPortNumber()
        {
            return m_portNumber;
        }

        public bool GetSecureHashIn()
        {
            return m_isSecureHashIn;
        }

        public bool GetSecureHashOut()
        {
            return m_isSecureHashOut;
        }

        public string GetSecureHashKey()
        {
            return m_secureHashKey;
        }

        public int GetSendQueueSize()
        {
            return m_sendQueueSize;
        }

        public int GetSocketSendBufferSize()
        {
            return m_socketSendBufferSize;
        }

        public int GetSleepWouldBlockTime()
        {
            return m_sleepWouldBlockTime;
        }

        public bool GetTCPKeepAliveEnabled()
        {
            return m_isTCPKeepAliveEnabled;
        }

        public int GetTCPKeepAliveTime()
        {
            return m_tcpKeepAliveTime;
        }

        public bool GetEnableReloadUserInfo()
        {
            return m_isReloadUserInfoEnabled;
        }

        public int GetReloadUserInfoInterval()
        {
            return m_reloadUserInfoInterval;
        }
    }
}
