using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class SiteProductKeyRegisConfiguration
    {
        private List<SettingData> m_allSettingData = new List<SettingData>();

        public const string SECTION_LICENSE = "License";

        public const string KEY_PACKAGE_NAME = "Package_Name";
        public const string DISP_PACKAGE_NAME = "Product Package Name:";

        public const string KEY_PRODUCT_KEY_STATUS = "ProductKeyStatus";
        public const string DISP_PRODUCT_KEY_STATUS = "Site Product Key status:";

        public const string KEY_CREATED_DATE = "Created_Date";
        public const string DISP_CREATED_DATE = "Valid from:";

        public const string KEY_EXPIRATION_DATE = "Expiration_Date";
        public const string DISP_EXPIRATION_DATE = "Expiration Date:";

        public const string KEY_NUMBER_OF_INSTANCES = "NumberOfInstances";
        public const string DISP_NUMBER_OF_INSTANCES = "Number of Instances:";
        public const string KEY_INSTANCE = "Instance";

        public const string KEY_NUMBER_OF_TOFDTF = "NumberOfTofOrDtfInputs";
        public const string DISP_NUMBER_OF_TOFDTF = "Number of TOF/DTF inputs:";
        public const string KEY_TOFDTF_INPUT = "TOFDTF_Input";

        public const string KEY_MAX_TOFDTF_PER_INSTANCE = "Max_TOFDTF_per_Instance";
        public const string DISP_MAX_TOFDTF_PER_INSTANCE = "Maximum TOF/DTF Input per instance:";

        public SiteProductKeyRegisConfiguration()
        {
            LoadAllSettingData();
        }

        public void LoadAllSettingData()
        {
            m_allSettingData.Clear();
            m_allSettingData = ProductKeyManager.Instance.GetAllSettingData();
        }

        public List<SettingData> GetAllSettingData()
        {
            LoadAllSettingData();

            return m_allSettingData;
        }
    }
}
