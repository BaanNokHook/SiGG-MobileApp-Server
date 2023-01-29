using SiGG_MobileApp_Server.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SiGG_MobileApp_Server.Server
{
    public class OutputSocket : Output // derived class (child)
    {
        private List<SettitngData> m_allSettingData;

        private bool m_isSecureHashIn;
        private bool m_isSecureHashOut;
        private string m_secureHashKey;
        private int m_sendQueueSize;
        private int m_socketSendBufferSize;
        private int m_sleepWouldBlockTime;

        private bool m_isTCPKeepAliveEnabled;
        private int m_tcpKeepAliveTime;

        public const string KEY_REQUIRE_HASH_DATA_IN = "RequireHashDataIn";
        public const string KEY_REQUIRE_HASH_DATA_OUT = "RequireHashDataOut";
        public const string KEY_HASH_KEY = "HashKey";
        public const string KEY_CLIENTS_SEND_QUEUE_SIZE = "SendQueueSize";
        public const string KEY_SOCKET_SEND_BUFFER = "SocketSendBufferSize";
        public const string KEY_SLEEP_WOULDBLOCK_TIME = "SleepWouldBlockTime";
        public const string KEY_TCP_KEEPALIVE_ENABLED = "TCPKeepAliveEnabled";
        public const string KEY_TCP_KEEPALIVE_TIME = "TCPKeepAliveTime";

        public const int DEF_CLIENTS_SEND_QUEUE_SIZE = 0;
        public const int DEF_SOCKET_SEND_BUFFER = 0;  // Default windows socket send buffer is 8 KB
        public const int DEF_SLEEP_WOULDBLOCK_TIME = 100;
        public const int DEF_TCP_KEEPALIVE_TIME = 60000;

        public const int MAX_SLEEP_WOULDBLOCK_TIME = 10000;

        public OutputSocket(int instanceNo, iniFileManager iniFileMgr, OutputsConfiguration.OUTPUT_TYPE outputType, string key, string section) : base(instanceNo, iniFileMgr, outputType, key, section)
        {
            m_allSettingData = new List<SettingData>();
        }

        public void LoadConfiguration()
        {
            string mmcType = m_iniFileMgr.IniReadValue(m_section, ConfigManager.Instance.KEY_MMC_TYPE);
            if (mmcType.Length == 0)
            {
                m_iniFileMgr.IniWriteValue(m_section, ConfigManager.Instance.KEY_MMC_TYPE, OutputsConfiguration.OUTPUT_TYPE_SOCKET);
            }

            m_isSecureHashIn = false;
            string secureHashIn = m_iniFileMgr.IniReadValue(m_section, KEY_REQUIRE_HASH_DATA_IN);
            if (secureHashIn.Equals("TRUE"))
            {
                m_isSecureHashIn = true;
            }

            m_isSecureHashOut = false;
            string secureHashOut = m_iniFileMgr.IniReadValue(m_section, KEY_REQUIRE_HASH_DATA_OUT);
            if (secureHashOut.Equals("TRUE"))
            {
                m_isSecureHashOut = true;
            }

            if (m_isSecureHashIn || m_isSecureHashOut)
            {
                //Get Secure Hash Key
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

            m_portNumber = 0;
            int.TryParse(m_iniFileMgr.IniReadValue(m_section, KEY_PORT_NUMBER), out m_portNumber);

            m_sendQueueSize = 0;
            int.TryParse(m_iniFileMgr.IniReadValue(m_section, KEY_CLIENTS_SEND_QUEUE_SIZE), out m_sendQueueSize);
            if (m_sendQueueSize < 0)
            {
                m_sendQueueSize = DEF_CLIENTS_SEND_QUEUE_SIZE;
            }

            m_socketSendBufferSize = 0;
            int.TryParse(m_iniFileMgr.IniReadValue(m_section, KEY_SOCKET_SEND_BUFFER), out m_socketSendBufferSize);
            if (m_socketSendBufferSize < 0)
            {
                m_socketSendBufferSize = DEF_SOCKET_SEND_BUFFER;
            }

            m_sleepWouldBlockTime = 0;
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

            m_tcpKeepAliveTime = 0;
            int.TryParse(m_iniFileMgr.IniReadValue(m_section, KEY_TCP_KEEPALIVE_TIME), out m_tcpKeepAliveTime);
            if (m_tcpKeepAliveTime == 0)
            {
                m_tcpKeepAliveTime = DEF_TCP_KEEPALIVE_TIME;
            }

            LoadAllSettingData();
        }

        public string[] LoadDefaultKey()
        {
            string[] strArray = new string[5];

            strArray[0] = KEY_MARKETFEED_LOGGING;
            strArray[1] = KEY_SEND_RTL;
            strArray[2] = KEY_PORT_NUMBER;
            strArray[3] = KEY_SEND_INPUT_DEAD;
            strArray[4] = KEY_ACCESS_ALLOWED_LIST;

            return strArray;
        }

        public string[] LoadDefaultValueOfKey()
        {
            string[] strArray = new string[5];

            strArray[0] = "MED";
            strArray[1] = "TRUE";
            strArray[2] = "#N/A";
            strArray[3] = "TRUE";
            strArray[4] = "ALL";

            return strArray;
        }

        public List<FieldShow> LoadDefaultFieldShow()
        {
            List<FieldShow> listFieldShow = new List<FieldShow>();

            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.Both);
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

            string[] strArrayReadIni = m_iniFileMgr.GetAllKeysInSection(m_section);

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

    }
}