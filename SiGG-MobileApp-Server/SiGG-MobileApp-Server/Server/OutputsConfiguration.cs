using SiGG_MobileApp_Server.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class OutputsConfiguration
    {
        private int m_instanceNo;
        private iniFileManager m_iniFileMgr;
        private List<SettingData> m_allSettingData;
        private List<Output> m_allOutput;  

        public enum OUTPUT_TYPE
        {
            OUTPUT_TYPE_NOTSET = 0;  
            OUTPUT_TYPE_SOCKET,  
            OUTPUT_TYPE_DTAPP
        }

        public const string OUTPUT_TYPE_SOCKET = "Socket";
        public const string OUTPUT_TYPE_DTAPP = "Socket_DTApp";

        public const string SECTION_OUTPUTS = "Outputs";  

        public OutputsConfiguration(int instanceNo, iniFileManager iniFileMgr)
        {
            m_instanceNo = instanceNo;
            m_iniFileMgr = iniFileMgr;
            m_allSettingData = new List<SettingData>();  
            m_allOutput = new List<Output>();


            LoadAllSettingData();
        }

        public void LoadAllSettingData()
        {
            m_allSettingData.Clear();
            m_allOutput.Clear();

            string[] strArray = m_iniFileMgr.GetAllKeysInSection(SECTION_OUTPUTS);

            for (int i = 0; i < strArray.Length; i++)
            {
                string key = "";
                string s_outputType = "";
                m_iniFileMgr.SplitKeyAndValue(strArray[i], ref key, ref s_outputType);

                string section = string.Format("{0}\\{1}", SECTION_OUTPUTS, key);

                OUTPUT_TYPE outputType = OUTPUT_TYPE.OUTPUT_TYPE_NOTSET;
                FieldStaus status = FieldStaus.NotSet;
                Output output = null;

                switch (s_outputType)
                {
                    case OUTPUT_TYPE_SOCKET:
                        {
                            outputType = OUTPUT_TYPE.OUTPUT_TYPE_SOCKET;
                            status = FieldStaus.Inactive_Output_Socket;

                            output = new OutputSocket(m_instanceNo, m_iniFileMgr, outputType, key, section);
                            OutputSocket outputSock = (OutputSocket)output;
                            outputSock.LoadConfiguration();
                        }
                        break;

                    case OUTPUT_TYPE_DTAPP:
                        {
                            outputType = OUTPUT_TYPE.OUTPUT_TYPE_DTAPP;
                            status = FieldStaus.Inactive_Output_DTAPP;

                            output = new OutputDTApp(m_instanceNo, m_iniFileMgr, outputType, key, section);
                            OutputDTApp outputDTApp = (OutputDTApp)output;
                            outputDTApp.LoadConfiguration();
                        }
                        break;
                }

                if ((outputType != OUTPUT_TYPE.OUTPUT_TYPE_NOTSET) && (status != FieldStaus.NotSet))
                {
                    AddSettingData(key, "", key, "", section, status);

                    m_allOutput.Add(output);
                }
            }
        }

        private void AddSettingData(string key, string value, string display, string defaultValue, string section, FieldStaus status)
        {
            SettingData settingData = settingData = new SettingData(key, value, display, defaultValue, section, status, FieldShow.Both, false, false);
            m_allSettingData.Add(settingData);
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

        public List<Output> GetAllOutput()
        {
            return m_allOutput;
        }
    }
}
