using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class ServiceConfiguration
    {
        private iniFileManager m_iniFileMgr;

        private int m_instanceNo;
        private string m_strIniFilePath;

        private List<SettingData> m_allSettingData;

        public GlobalConfiguration m_globalNode;
        public InputsConfiguration m_inputsNode;
        public OutputsConfiguration m_outputsNode;

        public ServiceConfiguration(int instanceNo)
        {
            m_instanceNo = instanceNo;
            m_strIniFilePath = ConfigManager.Instance.GetIniFilePath(instanceNo);
            m_iniFileMgr = new iniFileManager(m_strIniFilePath);

            m_allSettingData = new List<SettingData>();

            LoadAllSettingData();
        }

        public iniFileManager GetIniFileManager()
        {
            return m_iniFileMgr;
        }

        public int GetInstanceNo()
        {
            return m_instanceNo;
        }

        public string GetServiceName()
        {
            return string.Format("{0}{1}", ConfigManager.Instance.GetPrefixServiceName(), m_instanceNo.ToString("00"));
        }

        public string GetIniFilePath()
        {
            return m_strIniFilePath;
        }

        public string[] LoadDefaultKey()
        {
            string[] strArray = new string[3];

            strArray[0] = GlobalConfiguration.SECTION_GLOBAL;
            strArray[1] = InputsConfiguration.SECTION_INPUTS;
            strArray[2] = OutputsConfiguration.SECTION_OUTPUTS;

            return strArray;
        }

        public string[] LoadDefaultValueOfKey()
        {
            string[] strArray = new string[3];

            strArray[0] = "";
            strArray[1] = "";
            strArray[2] = "";

            return strArray;
        }

        public List<FieldShow> LoadDefaultFieldShow()
        {
            List<FieldShow> listFieldShow = new List<FieldShow>();

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

            return listDefaultRemovable;
        }

        public List<bool> LoadDefaultSpecialField()
        {
            List<bool> listDefaultSpecialField = new List<bool>();

            listDefaultSpecialField.Add(false);
            listDefaultSpecialField.Add(false);
            listDefaultSpecialField.Add(false);

            return listDefaultSpecialField;
        }

        public void LoadAllSettingData()
        {
            m_globalNode = new GlobalConfiguration(m_instanceNo, m_iniFileMgr);
            m_inputsNode = new InputsConfiguration(m_instanceNo, m_iniFileMgr);
            m_outputsNode = new OutputsConfiguration(m_instanceNo, m_iniFileMgr);

            m_allSettingData.Clear();

            List<SettingData> allSettingData = new List<SettingData>();

            string[] strArrayDefKey = LoadDefaultKey();
            string[] strArrayDefDisp = LoadDefaultKey();
            string[] strArrayDefValofKey = LoadDefaultValueOfKey();
            List<FieldShow> listDefFieldShow = LoadDefaultFieldShow();
            List<bool> listDefRemovable = LoadDefaultRemovable();
            List<bool> listDefSpecialField = LoadDefaultSpecialField();

            string[] strArrayReadIni = LoadDefaultKey();

            ConfigManager.Instance.AddSettingData(false
                                                   , true
                                                   , strArrayReadIni
                                                   , strArrayDefKey
                                                   , strArrayDefDisp
                                                   , strArrayDefValofKey
                                                   , ""
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

        public List<SettingData> GetAllSettingData()
        {
            LoadAllSettingData();

            return m_allSettingData;
        }

        public bool IsServiceRunning()
        {
            return ConfigManager.Instance.IsServiceRunning(GetServiceName());
        }

        public bool IsServiceRunningWithMasterSlave()
        {
            return ConfigManager.Instance.IsServiceRunningWithMasterSlave(GetServiceName());
        }
    }
}
