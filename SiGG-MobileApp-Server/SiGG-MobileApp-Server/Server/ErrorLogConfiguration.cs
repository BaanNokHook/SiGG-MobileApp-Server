using SiGG_MobileApp_Server.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class ErrorLogConfiguration
    {
        private int m_instanceNo;
        private iniFileManager m_iniFileMgr;
        private List<SettingData> m_allSettingData = new List<SettingData>();

        public const string SECTION_ERRORLOG = "ErrorLog";

        public const string KEY_SYSTEM = "System";
        public const string KEY_RECORDSTORE = "Recordstore";
        public const string KEY_INPUTS = "Inputs";
        public const string KEY_OUTPUTS = "Outputs";
        public const string KEY_NAME_RESOLUTION = "NameResolution";
        public const string KEY_INITIALISATION = "Initialisation";
        public const string KEY_MARKET_FEED_LOGGING = "MarketfeedLogging";

        public ErrorLogConfiguration(int instanceNo, iniFileManager iniFileMgr)
        {
            m_instanceNo = instanceNo;
            m_iniFileMgr = iniFileMgr;

            LoadAllSettingData();
        }

        public string[] LoadDefaultKey()
        {
            string[] strArray = new string[7];

            strArray[0] = KEY_SYSTEM;
            strArray[1] = KEY_RECORDSTORE;
            strArray[2] = KEY_INPUTS;
            strArray[3] = KEY_OUTPUTS;
            strArray[4] = KEY_NAME_RESOLUTION;
            strArray[5] = KEY_INITIALISATION;
            strArray[6] = KEY_MARKET_FEED_LOGGING;

            return strArray;
        }

        public string[] LoadDefaultValueOfKey()
        {
            string[] strArray = new string[7];

            strArray[0] = "";
            strArray[1] = "";
            strArray[2] = "";
            strArray[3] = "";
            strArray[4] = "";
            strArray[5] = "";
            strArray[6] = "MED";

            return strArray;
        }

        public List<FieldShow> LoadDefaultFieldShow()
        {
            List<FieldShow> listFieldShow = new List<FieldShow>();

            listFieldShow.Add(FieldShow.Both);
            listFieldShow.Add(FieldShow.Both);
            listFieldShow.Add(FieldShow.Both);
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

            string section = SECTION_ERRORLOG;
            string[] strArrayReadIni = m_iniFileMgr.GetAllKeysInSection(section);

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
