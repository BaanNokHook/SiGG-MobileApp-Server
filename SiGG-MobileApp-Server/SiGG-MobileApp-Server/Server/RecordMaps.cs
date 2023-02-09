using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class RecordMaps
    {
        public int m_instanceNo;
        public iniFileManager m_iniFileMgr;
        public string m_inputKey;
        public List<string> m_allRecordMapNode = null;
        public List<string> m_allRecordMapSection = null;
        private List<SettingData> m_allSettingData = null;
        public List<RecordMapSettingData> m_allRecordMapSettingData = null;

        public const string SECTION_RECORDMAPS = "RecordMaps";

        public const string KEY_PREFIX = "Prefix";
        public const string KEY_INPUT = "Input";
        public const string KEY_FLAGS = "Flags";

        public const string FLAG_HAS_UPDATES = "HasUpdates";
        public const string FLAG_NEVER_CHANGES = "NeverChanges";
        public const string FLAG_MUST_STORE = "MustStore";
        public const string FLAG_MUST_NOT_STORE = "MustNotStore";
        public const string FLAG_CANNOT_CANCEL_UPDATES = "CannotCancelUpdates";

        public RecordMaps(int instanceNo, iniFileManager iniFileMgr, string inputKey)
        {
            m_instanceNo = instanceNo;
            m_iniFileMgr = iniFileMgr;
            m_inputKey = inputKey;
            m_allRecordMapNode = new List<string>();
            m_allRecordMapSection = new List<string>();
            m_allRecordMapSettingData = new List<RecordMapSettingData>();

            m_allSettingData = new List<SettingData>();

            LoadAllAllRecordMaps();
        }

        public void LoadAllAllRecordMaps()
        {
            m_allRecordMapNode.Clear();
            m_allRecordMapSection.Clear();
            m_allRecordMapSettingData.Clear();
            m_allSettingData.Clear();

            #region Find RecordMap Section
            string[] allRecordMapSection = m_iniFileMgr.GetAllSectionNames();

            for (int i = 0; i < allRecordMapSection.Length; i++)
            {
                if (allRecordMapSection[i].IndexOf(SECTION_RECORDMAPS + "\\") == 0)
                {
                    string input = m_iniFileMgr.IniReadValue(allRecordMapSection[i], KEY_INPUT);

                    if (input == m_inputKey)
                    {
                        m_allRecordMapSection.Add(allRecordMapSection[i]);

                        int index = allRecordMapSection[i].IndexOf("\\");
                        string nodeName = allRecordMapSection[i].Substring((index + 1), (allRecordMapSection[i].Length - index - 1));
                        string prefix = m_iniFileMgr.IniReadValue(allRecordMapSection[i], KEY_PREFIX);
                        string flags = m_iniFileMgr.IniReadValue(allRecordMapSection[i], KEY_FLAGS);

                        RecordMapSettingData recordMapSettigData = new RecordMapSettingData(nodeName, prefix, input, flags, allRecordMapSection[i]);
                        m_allRecordMapSettingData.Add(recordMapSettigData);

                        m_allRecordMapNode.Add(nodeName);

                        SettingData settingData = new SettingData(nodeName, "", nodeName, "", "", FieldStaus.Inactive_RecordMap, FieldShow.OnlyAdvance, true, false);
                        m_allSettingData.Add(settingData);
                    }
                }
            }
            #endregion
        }

        public List<string> GetAllRecordMapNode()
        {
            LoadAllAllRecordMaps();

            return m_allRecordMapNode;
        }

        public List<string> GetAllRecordMapSection()
        {
            LoadAllAllRecordMaps();

            return m_allRecordMapSection;
        }

        public List<SettingData> GetAllSettingData()
        {
            LoadAllAllRecordMaps();

            return m_allSettingData;
        }

        public List<SettingData> GetAllRecordMapSettingData(string recordMapName)
        {
            RecordMapSettingData recordMapSettingData = null;
            List<SettingData> allRecordMapSettingData = new List<SettingData>();

            LoadAllAllRecordMaps();

            for (int i = 0; i < m_allRecordMapSettingData.Count; i++)
            {
                if (m_allRecordMapSettingData[i].m_nodeName == recordMapName)
                {
                    recordMapSettingData = m_allRecordMapSettingData[i];
                    break;
                }
            }

            if (recordMapSettingData != null)
            {
                string valuePrefix = recordMapSettingData.m_prefix;

                ConfigManager.Instance.AddSettingData(true
                                                      , false
                                                      , RecordMaps.KEY_PREFIX
                                                      , valuePrefix
                                                      , RecordMaps.KEY_PREFIX
                                                      , valuePrefix
                                                      , recordMapSettingData.m_section
                                                      , FieldStaus.Inactive_Prefix
                                                      , FieldShow.OnlyAdvance
                                                      , false
                                                      , false
                                                      , GetServiceName()
                                                      , ref allRecordMapSettingData);

                string valueFlags = recordMapSettingData.m_flags;

                ConfigManager.Instance.AddSettingData(true
                                                      , false
                                                      , RecordMaps.KEY_FLAGS
                                                      , valueFlags
                                                      , RecordMaps.KEY_FLAGS
                                                      , valueFlags
                                                      , recordMapSettingData.m_section
                                                      , FieldStaus.Inactive_Flags
                                                      , FieldShow.OnlyAdvance
                                                      , false
                                                      , false
                                                      , GetServiceName()
                                                      , ref allRecordMapSettingData);

            }

            return allRecordMapSettingData;
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
