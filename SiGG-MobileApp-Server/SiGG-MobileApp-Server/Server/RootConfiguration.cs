using SiGG_MobileApp_Server.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class RootConfiguration
    {
        private ComputerConfiguration m_computerNode;
        private List<SettingData> m_allSettingData = new List<SettingData>();  

        public RootConfiguration()
        {
            LoadAllSettingData();  
        }  

        public void LoadAllSettingData()
        {
            m_computerNode = new ComputerConfiguration();

            m_allSettingData.Clear();
            SettingData settingData = new SettingData(ConfigManager.Instance.GetComputerName(), "", ConfigManager.Instance.GetComputerName(), "", "", FieldStatus.NotSet, FieldShow.Both, false, false);
            m_allSettingData.Add(settingData);  
        }

        public List<SettingData> GetAllSettingData()
        {
            //LoadAllSettingData();

            return m_allSettingData;
        }

        public ComputerConfiguration GetComputerNode()
        {
            return m_computerNode;
        }
    }
}