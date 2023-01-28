using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class InputDtf : Input  // derived class (child)
    {
        private List<SettingData> m_allSettingData;

        public const string KEY_PORT_NUMBER = "PortNumber";
        public const string KEY_IP_ADDRESS = "IPAddress";
        public const string KEY_RECONNECT = "Reconnect";
        public const string KEY_RECONNECT_COUNT = "ReconnectCount";
        public const string KEY_RECONNECT_TIMEOUT = "ReconnectTimeout";
        public const string KEY_HEARTBEAT = "Heartbeat";
        public const string KEY_HEARTBEAT_RECORD = "HeartbeatRecord";
        public const string KEY_HEARTBEAT_INTERVAL = "HeartbeatInterval";

        public InputDtf(int instanceNo, iniFileManager iniFileMgr, string inputKey, InputsConfiguration.INPUT_TYPE inputType) : base(instanceNo, iniFileMgr, inputKey, inputType)
        {
            m_allSettingData = new List<SettingData>();
            m_allSettingData = GetAllSettingData();
        }

        public string[] LoadDefaultKey()
        {
            string[] strArray = new string[11];

            strArray[0] = RecordMaps.SECTION_RECORDMAPS;
            strArray[1] = KEY_MARKETFEED_LOGGING;
            strArray[2] = KEY_IP_ADDRESS;
            strArray[3] = KEY_PORT_NUMBER;
            strArray[4] = KEY_RECONNECT;
            strArray[5] = KEY_RECONNECT_COUNT;
            strArray[6] = KEY_RECONNECT_TIMEOUT;
            strArray[7] = KEY_HEARTBEAT;
            strArray[8] = KEY_HEARTBEAT_RECORD;
            strArray[9] = KEY_HEART_BEAT_INTERVAL;
            strArray[10] = KEY_TCID;

            return strArray;
        }

        public string[] LoadDefaultValueOfKey()
        {
            string[] strArray = new string[11];

            strArray[0] = "";
            strArray[1] = "MED";
            strArray[2] = "#N/A";
            strArray[3] = "#N/A";
            strArray[4] = "TRUE";
            strArray[5] = "0";
            strArray[6] = "30";
            strArray[7] = "TRUE";
            strArray[8] = "#S";
            strArray[9] = "30";
            strArray[10] = "#N/A";

            return strArray;
        }

        public List<FieldShow> LoadDefaultFieldShow()
        {
            List<FieldShow> listFieldShow = new List<FieldShow>();

            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.Both);
            listFieldShow.Add(FieldShow.Both);
            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.OnlyAdvance);
            listFieldShow.Add(FieldShow.Both);

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
                    case RecordMaps.SECTION_RECORDMAPS:
                        {
                            if (settingData.m_status == FieldStaus.Red)
                            {
                                for (int index = 0; index < strArrayDefKey.Length; index++)
                                {
                                    if (strArrayDefKey[index] == RecordMaps.SECTION_RECORDMAPS)
                                    {
                                        settingData.m_value = strArrayDefValofKey[index];
                                        settingData.m_status = FieldStaus.Inactive_RecordMaps;
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
    }
}
