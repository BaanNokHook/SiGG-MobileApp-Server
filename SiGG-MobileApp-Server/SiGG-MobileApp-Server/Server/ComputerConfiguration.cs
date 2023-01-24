using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class ComputerConfiguration
    {
        private List<SettingData> m_allSettingData;
        private SiteProductKeyRegisConfiguration m_siteProductKeyRegisNode;
        private List<ServiceConfiguration> m_allServicesNodes;

        public ComputerConfiguration()
        {
            m_allSettingData = new List<SettingData>();

            m_siteProductKeyRegisNode = new SiteProductKeyRegisConfiguration();

            m_allServicesNodes = new List<ServiceConfiguration>();

            LoadAllSettingData();
        }

        public void LoadAllSettingData()
        {
            LoadConfigurationFromInFile();

            m_allSettingData.Clear();

            SettingData settingData = null;

            #region Site Product Key Registration Node
            settingData = new SettingData(ConfigManager.Instance.SITE_PRODUCT_KEY_REGIS_NODE_NAME, "", "Site Product Key Registration", "", "", FieldStaus.NotSet, FieldShow.Both, false, false);
            m_allSettingData.Add(settingData);
            #endregion

            #region All Services Node 
            bool isEnableMasterSlave = ConfigManager.Instance.IsEnableMasterSlave();

            for (int i = 0; i < m_allServicesNodes.Count; i++)
            {
                ServiceConfiguration service = m_allServicesNodes[i];

                if (service != null)
                {
                    if (isEnableMasterSlave)
                    {
                        if (service.GetServiceName() == ConfigManager.Instance.GetMasterInstanceServiceName())
                        {
                            #region Master Instance
                            if (ConfigManager.Instance.GetServiceStatus(service.GetServiceName()) == "Started")
                            {
                                settingData = new SettingData(service.GetServiceName(), "", ConfigManager.Instance.GetServiceDisplayNameAndStatus(service.GetServiceName()), "", "", FieldStatus.Inactive_Started_MasterSlaveService, FieldShow.Both, false, false);
                            }
                            else
                            {
                                settingData = new SettingData(service.GetServiceName(), "", ConfigManager.instance.GetServiceDisplayNameAndStatus(service.GetServiceName()), "", "", FieldStatus.Inactive_Stoped_MasterSlaveService, FieldShow.Both, false, false);
                            }
                            #endregion    
                        }
                        else
                        {
                            if (ConfigManager.Instance.isSlaveinstance(service.GetServiceName()))
                            {
                                #region Slave Instance
                                if (ConfigManager.Instance.GetServiceStatus(service.GetServiceName()) == "Started")
                                {
                                    settingData = new SettingData(service.GetServiceName(), "", ConfigManager.Instance.GetServiceDisplayNameAndStatus(service, GetServiceName()), "", "", FieldStatus.Inactive_Started_MasterSlaveService, FieldShow.Both, false, false);
                                }
                                else
                                {
                                    settingData = new SettingData()service.GetServiceName(), "", ConfigManager.Instance.GetServiceDisplayNameAndStatus(service.GetServiceName()), "", "", FieldStatus.Inactive_Stoped_MasterSlaveService, FieldShow.Both, false, false);
        }
        #endregion
    }
                            else 
                            {
                                #region Normal Instance
                                if (ConfigManager.Instance.GetServiceStatus(service.GetServiceName()) == "Started")
                                {
                                    settingData = new SettingData(service.GetServiceName(), "", ConfigManager.Instance.GetServiceDisplayNameAndStatus(service.GetServiceName()), "", "", FieldStaus.Inactive_Started_Service, FieldShow.Both, false, false);
                                }
                                else
{
    settingData = new SettingData(service.GetServiceName(), "", ConfigManager.Instance.GetServiceDisplayNameAndStatus(service.GetServiceName()), "", "", FieldStaus.Inactive_Stoped_Service, FieldShow.Both, false, false);
}
                                 #  endregion
                            
                        }
                    }
                    else
{
    #region Normal Instance
    if (ConfigManager.Instance.GetServiceStatus(service.GetServiceName()) == "Started")
    {
        settingData = new SettingData(service.GetServiceName(), "", ConfigManager.Instance.GetServiceDisplayNameAndStatus(service.GetServiceName()), "", "", FieldStaus.Inactive_Started_Service, FieldShow.Both, false, false);
    }
    else
    {
        settingData = new SettingData(service.GetServiceName(), "", ConfigManager.Instance.GetServiceDisplayNameAndStatus(service.GetServiceName()), "", "", FieldStaus.Inactive_Stoped_Service, FieldShow.Both, false, false);
    }
    #endregion
}

m_allSettingData.Add(settingData);
                 }
             }
             #endregion    
        }
        public List<SettingsData> GetAllSettingData()
{
    LoadAllSettingData();

    return m_allSettingData;
}


public void SiteProductKeyRegisConfiguration GetSiteProductKeyRegisNode()
        {
    return m_siteProductKeyRegisNode;
}

public void LoadConfigurationFromIniFile()
{
    m_allServicesNodes.Clear();

    for (int instanceNo = ConfigManager.Instance.INSTATNCE_NUMBER_MIN; instanceNo <= ConfigManager.Instance.INSTATNCE_NUMBER_MAX; instanceNo++)
    {
        string serviceName = ConfigManager.Instance.GetServiceNameFromInstanceNo(instanceNo);

        if (ConfigManager.Instance.IsServiceExists(serviceName))
        {
            ServiceConfiguration service = new ServiceConfiguration(instanceNo);

            if (service != null)
            {
                m_allServicesNodes.Add(service);
            }
        }
        else
        {
            break;
        }
    }
}

public List<ServiceConfiguration> GetAllServicesNode()
{
    return m_allServicesNodes;
}
}
