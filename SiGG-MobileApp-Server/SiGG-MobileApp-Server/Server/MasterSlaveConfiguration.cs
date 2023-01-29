using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class MasterSlaveConfiguration
    {
        private iniFileManager m_iniFileMgr;
        private int m_instanceNo;

        private bool m_isEnableMasterSlave;
        private List<string> m_listSlaveInstance = new List<string>();
        private List<SettingData> m_allSettingData = new List<SettingData>();

        public const string SECTION_MASTER_SLAVE = "Master-Slave";

        public const string KEY_ENABLE_MASTER_SLAVE = "EnableMasterSlave";
        public const string KEY_SLAVE_INSTANCE_1 = "SlaveInstance1";
        public const string KEY_SLAVE_INSTANCE_2 = "SlaveInstance2";
        public const string KEY_SLAVE_INSTANCE_3 = "SlaveInstance3";
        public const string KEY_SLAVE_INSTANCE_4 = "SlaveInstance4";

        public const string MASTERSLAVE_DEFAULT_SLAVEINSTANCE = "None";

        public MasterSlaveConfiguration(int instanceNo, iniFileManager iniFileMgr)
        {
            m_instanceNo = instanceNo;
            m_iniFileMgr = iniFileMgr;

            LoadAllSettingData();
        }

        public bool IsEnableMasterSlave()
        {
            return m_isEnableMasterSlave;
        }

        public List<string> GetAllSlaveInstance()
        {
            return m_listSlaveInstance;
        }

        public string[] LoadDefaultKey()
        {
            string[] strArray = new string[5];

            strArray[0] = KEY_ENABLE_MASTER_SLAVE;
            strArray[1] = KEY_SLAVE_INSTANCE_1;
            strArray[2] = KEY_SLAVE_INSTANCE_2;
            strArray[3] = KEY_SLAVE_INSTANCE_3;
            strArray[4] = KEY_SLAVE_INSTANCE_4;

            return strArray;
        }

        public string[] LoadDefaultValueOfKey()
        {
            string[] strArray = new string[5];

            strArray[0] = "FALSE";
            strArray[1] = "0";
            strArray[2] = "0";
            strArray[3] = "0";
            strArray[4] = "0";

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

        private void LoadAllSettingData()
        {
            m_allSettingData.Clear();
            m_listSlaveInstance.Clear();

            #region Read Master-Slave configuration from instance no 1 only
            if (m_instanceNo == ConfigManager.Instance.INSTATNCE_NUMBER_MIN)
            {
                List<SettingData> allSettingData = new List<SettingData>();

                string[] strArrayDefKey = LoadDefaultKey();
                string[] strArrayDefDisp = LoadDefaultKey();
                string[] strArrayDefValofKey = LoadDefaultValueOfKey();
                List<FieldShow> listDefFieldShow = LoadDefaultFieldShow();
                List<bool> listDefRemovable = LoadDefaultRemovable();
                List<bool> listDefSpecialField = LoadDefaultSpecialField();

                string section = SECTION_MASTER_SLAVE;
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

                    switch (settingData.m_key)
                    {
                        case KEY_ENABLE_MASTER_SLAVE:
                            {
                                string enableMasterSlave = m_iniFileMgr.IniReadValue(SECTION_MASTER_SLAVE, KEY_ENABLE_MASTER_SLAVE);

                                m_isEnableMasterSlave = false;

                                if (enableMasterSlave == "TRUE")
                                {
                                    m_isEnableMasterSlave = true;
                                }

                                if (settingData.m_status == FieldStaus.Red)
                                {
                                    m_iniFileMgr.IniWriteValue(SECTION_MASTER_SLAVE, settingData.m_key, strArrayDefValofKey[0]);
                                }
                            }
                            break;

                        case KEY_SLAVE_INSTANCE_1:
                            {
                                if (settingData.m_status == FieldStaus.Red)
                                {
                                    m_iniFileMgr.IniWriteValue(SECTION_MASTER_SLAVE, settingData.m_key, strArrayDefValofKey[1]);
                                    m_listSlaveInstance.Add(strArrayDefValofKey[1]);
                                }
                                else
                                {
                                    m_listSlaveInstance.Add(settingData.m_value);
                                }
                            }
                            break;

                        case KEY_SLAVE_INSTANCE_2:
                            {
                                if (settingData.m_status == FieldStaus.Red)
                                {
                                    m_iniFileMgr.IniWriteValue(SECTION_MASTER_SLAVE, settingData.m_key, strArrayDefValofKey[2]);
                                    m_listSlaveInstance.Add(strArrayDefValofKey[2]);
                                }
                                else
                                {
                                    m_listSlaveInstance.Add(settingData.m_value);
                                }
                            }
                            break;

                        case KEY_SLAVE_INSTANCE_3:
                            {
                                if (settingData.m_status == FieldStaus.Red)
                                {
                                    m_iniFileMgr.IniWriteValue(SECTION_MASTER_SLAVE, settingData.m_key, strArrayDefValofKey[3]);
                                    m_listSlaveInstance.Add(strArrayDefValofKey[3]);
                                }
                                else
                                {
                                    m_listSlaveInstance.Add(settingData.m_value);
                                }
                            }
                            break;

                        case KEY_SLAVE_INSTANCE_4:
                            {
                                if (settingData.m_status == FieldStaus.Red)
                                {
                                    m_iniFileMgr.IniWriteValue(SECTION_MASTER_SLAVE, settingData.m_key, strArrayDefValofKey[4]);
                                    m_listSlaveInstance.Add(strArrayDefValofKey[4]);
                                }
                                else
                                {
                                    m_listSlaveInstance.Add(settingData.m_value);
                                }
                            }
                            break;

                        default:
                            {
                                if (settingData.m_status == FieldStaus.Yellow)
                                {
                                    m_listSlaveInstance.Add(settingData.m_value);
                                }
                            }
                            break;
                    }
                }

                m_allSettingData = ConfigManager.Instance.MakeSettingDataByView(allSettingData);
            }
            #endregion
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
    }
}
