using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class RecordCacheConfiguration
    {
        private int m_instanceNo;
        private iniFileManager m_iniFileMgr;
        private List<SettingData> m_allSettingData = new List<SettingData>();

        public const string SECTION_RECORD_CACHE = "RecordCache";

        public const string KEY_SIZE = "Size";
        public const string KEY_RE_REQUEST_ENABLE = "ReRequestEnable";
        public const string KEY_RE_REQUEST_TIMEOUT = "ReRequestTimeout";
        public const string KEY_FORCE_SNAPSHOT_TICKET_REQ = "ForceSnapshotTicketReq";

        public RecordCacheConfiguration(int instanceNo, iniFileManager iniFileMgr)
        {
            m_instanceNo = instanceNo;
            m_iniFileMgr = iniFileMgr;

            LoadAllSettingData();
        }

        public string[] LoadDefaultKey()
        {
            string[] strArray = new string[4];


            strArray[0] = KEY_SIZE;
            strArray[1] = KEY_RE_REQUEST_ENABLE;
            strArray[2] = KEY_RE_REQUEST_TIMEOUT;
            strArray[3] = KEY_FORCE_SNAPSHOT_TICKET_REQ;

            return strArray;
        }

        public string[] LoadDefaultValueOfKey()
        {
            string[] strArray = new string[4];

            strArray[0] = "500";
            strArray[1] = "TRUE";
            strArray[2] = "5000";
            strArray[3] = "FALSE";

            return strArray;
        }

        public List<FieldShow> LoadDefaultFieldShow()
        {
            List<FieldShow> listFieldShow = new List<FieldShow>();

            listFieldShow.Add(FieldShow.Both);
            listFieldShow.Add(FieldShow.Both);
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

            return listDefaultRemovable;
        }

        public List<bool> LoadDefaultSpecialField()
        {
            List<bool> listDefaultSpecialField = new List<bool>();

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

            string section = SECTION_RECORD_CACHE;
            string[] strArrayReadIni = m_iniFileMgr.GetAllKeysInSection(SECTION_RECORD_CACHE);

            ConfigManager.Instance.AddSettingData(true
                                                   , true
                                                   , strArrayReadIni
                                                   , strArrayDefKey
                                                   , strArrayDefDisp
                                                   , strArrayDefValofKey
                                                   , section
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
                    case KEY_RE_REQUEST_ENABLE:
                        {
                            if (settingData.m_status == FieldStaus.Red)
                            {
                                m_iniFileMgr.IniWriteValue(SECTION_RECORD_CACHE, settingData.m_key, strArrayDefValofKey[1]);
                            }
                        }
                        break;

                    case KEY_RE_REQUEST_TIMEOUT:
                        {
                            if (settingData.m_status == FieldStaus.Red)
                            {
                                m_iniFileMgr.IniWriteValue(SECTION_RECORD_CACHE, settingData.m_key, strArrayDefValofKey[2]);
                            }
                        }
                        break;

                    case KEY_FORCE_SNAPSHOT_TICKET_REQ:
                        {
                            if (settingData.m_status == FieldStaus.Red)
                            {
                                m_iniFileMgr.IniWriteValue(SECTION_RECORD_CACHE, settingData.m_key, strArrayDefValofKey[3]);
                            }
                        }
                        break;
                }
            }

            m_allSettingData = ConfigManager.Instance.MakeSettingDataByView(allSettingData);
        }

        public string GetServiceName()
        {
            return string.Format("{0}{1}", ConfigManager.Instance.GetPrefixServiceName(), m_instanceNo.ToString("00"));
        }

        public List<SettingData> GetAllSettingData()
        {
            LoadAllSettingData();

            return m_allSettingData;
        }

        public int GetInstanceNo()
        {
            return m_instanceNo;
        }
    }
}

