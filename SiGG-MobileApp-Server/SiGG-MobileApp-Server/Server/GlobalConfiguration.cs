using SiGG_MobileApp_Server.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class GlobalConfiguration
    {
        private int m_instanceNo;
        private intiFileManager m_iniFileMgr;
        private bool m_isShowMasterSlaveNode;
        private List<SettingData> m_allSettingData;

        private MasterSlaveConfiguration m_masterSlaveNode;
        private ErrorLogConfiguration m_errorLogNode;
        private RecordCacheConfiguration m_recordCacheNode;

        public const string SELECTION_GLOBAL = "Global";

        public const string KEY_SERVICE_INSTANCE = "ServiceInstance";  

        public GlobalConfiguration(int instanceNo, IniFileManager iniFileMgr)
        {
            m_instanceNo = instanceNo;
            m_iniFileMgr = iniFileMgr;
            m_isShowMasterSlaveNode = (m_instanceNo == ConfigManager.Instance.INSTATNCE_NUMBER_MIN) && (ProductKeyManager.Instance.GetTotalCurrentInstances() > 1);
            m_allSettingData = new List<SettingData>();

            LoadAllSettingData();
        }

        public string[] LoadDefaultKey()
        {
            string[] strArray = null;

            if (m_isShowMasterSlaveNode)
            {
                strArray = new string[3];

                strArray[0] = MasterSlaveConfiguration.SECTION_MASTER_SLAVE;
                strArray[1] = ErrorLogConfiguration.SECTION_ERRORLOG;
                strArray[2] = RecordCacheConfiguration.SECTION_RECORD_CACHE;
            }
            else
            {
                strArray = new string[2];

                strArray[0] = ErrorLogConfiguration.SECTION_ERRORLOG;
                strArray[1] = RecordCacheConfiguration.SECTION_RECORD_CACHE;
            }

            return strArray;
        }

        public string[] LoadDefaultValueOfKey()
        {
            string[] strArray = null;

            if (m_isShowMasterSlaveNode)
            {
                strArray = new string[4];

                strArray[0] = "";
                strArray[1] = "";
                strArray[2] = "";
                strArray[3] = "";
            }
            else
            {
                strArray = new string[3];

                strArray[0] = "";
                strArray[1] = "";
                strArray[2] = "";
            }

            return strArray;
        }

        public List<FieldShow> LoadDefaultFieldShow()
        {
            List<FieldShow> listFieldShow = new List<FieldShow>();

            if (m_isShowMasterSlaveNode)
            {
                listFieldShow.Add(FieldShow.Both);
                listFieldShow.Add(FieldShow.Both);
                listFieldShow.Add(FieldShow.Both);
                listFieldShow.Add(FieldShow.Both);
            }
            else
            {
                listFieldShow.Add(FieldShow.Both);
                listFieldShow.Add(FieldShow.Both);
                listFieldShow.Add(FieldShow.Both);
            }

            return listFieldShow;
        }

        public List<bool> LoadDefaultRemovable()
        {
            List<bool> listDefaultRemovable = new List<bool>();

            if (m_isShowMasterSlaveNode)
            {
                listDefaultRemovable.Add(false);
                listDefaultRemovable.Add(false);
                listDefaultRemovable.Add(false);
                listDefaultRemovable.Add(false);
            }
            else
            {
                listDefaultRemovable.Add(false);
                listDefaultRemovable.Add(false);
                listDefaultRemovable.Add(false);
            }

            return listDefaultRemovable;
        }

        public List<bool> LoadDefaultSpecialField()
        {
            List<bool> listDefaultSpecialField = new List<bool>();

            if (m_isShowMasterSlaveNode)
            {
                listDefaultSpecialField.Add(false);
                listDefaultSpecialField.Add(false);
                listDefaultSpecialField.Add(false);
                listDefaultSpecialField.Add(false);
            }
            else
            {
                listDefaultSpecialField.Add(false);
                listDefaultSpecialField.Add(false);
                listDefaultSpecialField.Add(false);
            }

            return listDefaultSpecialField;
        }

        public void LoadAllSettingData()
        {
            m_masterSlaveNode = new MasterSlaveConfiguration(m_instanceNo, m_iniFileMgr);
            m_errorLogNode = new ErrorLogConfiguration(m_instanceNo, m_iniFileMgr);
            m_recordCacheNode = new RecordCacheConfiguration(m_instanceNo, m_iniFileMgr);

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

        public MasterSlaveConfiguration GetMasterSlaveNode()
        {
            return m_masterSlaveNode;
        }

        public ErrorLogConfiguration GetErrorLogNode()
        {
            return m_errorLogNode;
        }

        public RecordCacheConfiguration GetRecordCacheNode()
        {
            return m_recordCacheNode;
        }
    }
}