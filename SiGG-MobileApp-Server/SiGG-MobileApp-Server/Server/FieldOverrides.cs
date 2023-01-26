using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class FieldOverrides
    {
        public const string SECTION_FIELD_OVERRIDES = "FieldOverrides";
        public const string SECTION_INPUT_FIELD_OVERRIDES = "Inputs\\{0}\\" + SECTION_FIELD_OVERRIDES;

        public int m_instanceNo;
        public iniFileManager m_iniFileMgr;
        string m_inputKey;
        string m_section;

        private List<SettingData> m_allSettingData;

        public FieldOverrides(int instanceNo, iniFileManager iniFileMgr, string inputKey)
        {
            m_instanceNo = instanceNo;
            m_iniFileMgr = iniFileMgr;
            m_inputKey = inputKey;
            m_section = string.Format(SECTION_INPUT_FIELD_OVERRIDES, inputKey);

            m_allSettingData = new List<SettingData>();

            LoadAllSettingData();
        }

        public void LoadAllSettingData()
        {
            m_allSettingData.Clear();

            string[] strArray = m_iniFileMgr.GetAllKeysInSection(m_section);

            for (int i = 0; i < strArray.Length; i++)
            {
                string key = "";
                string value = "";

                m_iniFileMgr.SplitKeyAndValue(strArray[i], ref key, ref value);

                FieldStaus status = FieldStaus.Yellow;

                if (value.Trim().Length == 0)
                {
                    status = FieldStaus.Pink;
                }

                ConfigManager.Instance.AddSettingData(true
                                                       , false
                                                       , key
                                                       , value
                                                       , key
                                                       , value.Trim()
                                                       , m_section
                                                       , status
                                                       , FieldShow.OnlyAdvance
                                                       , true
                                                       , false
                                                       , GetServiceName()
                                                       , ref m_allSettingData);
            }
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

        public string GetServiceName()
        {
            return string.Format("{0}{1}", ConfigManager.Instance.GetPrefixServiceName(), m_instanceNo.ToString("00"));
        }
    }
}
