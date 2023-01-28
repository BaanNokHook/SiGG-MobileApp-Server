using SiGG_MobileApp_Server.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SiGG_MobileApp_Server.Server
{
    public class InputTof : Input   // derived class (child)   
    {
        private List<SettingData> m_allSettingData;

        public const string KEY_PORT_NUMBER = "PortNumber";
        public const string KEY_IP_ADDRESS = "IPADDRESS";
        public const string KEY_RECONNECT = "Reconnect";
        public const string KEY_RECONNECT_COUNT = "ReconnectCount";
        public const string KEY_RECONNECT_TIMENOUT = "ReconnectTimeout";  
        public const string KEY_HEARTBEAT = "Heartbeat";
        public const string KEY_HEARTBEAT_RECORD = "HeartbeatRecord";
        public const string KEY_HEARTBEAT_INTERVAL = "HeartbeatInterval";

        public InputTof(int instanceNo, iniFileManager iniFileMgr, string inputKey, InputsConfiguration.INPUT_TYPE inputType) : base(instanceNo, iniFileMgr, inputKey, inputType)
        {
            m_fieldOverrides = new FieldOverrides(instanceNo, iniFileMgr, inputKey);

            m_allSettingData = new List<SettingData>();
            m_allSettingData = GetAllSettingData();
        }

        public string[] LoadDefaultKey()
        {
            string[] strArray = new string[13];

            strArray[0] = RecordMaps.SECTION_RECORDMAPS;
            strArray[1] = FieldOverrides.SECTION_FIELD_OVERRIDES;
            strArray[2] = KEY_MARKETFEED_LOGGING;
            strArray[3] = KEY_IP_ADDRESS;
            strArray[4] = KEY_PORT_NUMBER;
            strArray[5] = KEY_RECONNECT;
            strArray[6] = KEY_RECONNECT_COUNT;
            strArray[7] = KEY_RECONNECT_TIMEOUT;
            strArray[8] = KEY_HEARTBEAT;
            strArray[9] = KEY_HEARTBEAT_RECORD;
            strArray[10] = KEY_HEART_BEAT_INTERVAL;
            strArray[11] = KEY_TCID;
            strArray[12] = KEY_ALIAS;

            return strArray;
        }

        public string[] LoadDefaultValueOfKey()
        {
            string[] strArray = new string[13];

            strArray[0] = "";
            strArray[1] = "";
            strArray[2] = "MED";
            strArray[3] = "#N/A";
            strArray[4] = "#N/A";
            strArray[5] = "TRUE";
            strArray[6] = "0";
            strArray[7] = "30";
            strArray[8] = "TRUE";
            strArray[9] = "#S";
            strArray[10] = "30";
            strArray[11] = "#N/A";
            strArray[12] = "#N/A";

            return strArray;
        }

        public List<FieldShow> LoadDefaultFieldShow()
        {
            List<FieldShow> listFieldShow = new List<FieldShow>();

            listFieldShow.Add(FieldShow.OnlyAdvance);
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
                    case FieldOverrides.SECTION_FIELD_OVERRIDES:
                        {
                            if (settingData.m_status == FieldStaus.Red)
                            {
                                for (int index = 0; index < strArrayDefKey.Length; index++)
                                {
                                    switch (settingData.m_key)
                                    {
                                        case RecordMaps.SECTION_RECORDMAPS:
                                            {
                                                if (strArrayDefKey[index] == settingData.m_key)
                                                {
                                                    settingData.m_value = strArrayDefValofKey[index];
                                                    settingData.m_status = FieldStaus.Inactive_RecordMaps;
                                                }
                                            }
                                            break;

                                        case FieldOverrides.SECTION_FIELD_OVERRIDES:
                                            {
                                                if (strArrayDefKey[index] == settingData.m_key)
                                                {
                                                    settingData.m_value = strArrayDefValofKey[index];
                                                    settingData.m_status = FieldStaus.Inactive_FieldOverrides;
                                                }
                                            }
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
