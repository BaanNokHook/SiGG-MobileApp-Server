using SiGG_MobileApp_Server.Server;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    class ProductKeyManager
    {
        public static readonly string DT_PUBLIC_ENCRYPT_KEY = "CAIAABBmAAAgAAAAq2dkih9yPSZEZ+9YoPYqWd0ID53+QZSamtVrImGZKZY=";
        public static readonly string MT_PUBLIC_ENCRYPT_KEY = "CAIAABBmAAAgAAAAj2Sg3Y/rk41DMa968fcBa4jWbxiHb0m/QHw8qA63Vpk=";

        [DllImport("DTCryptoUtilities.dll", CharSet = CharSet.Unicode)]
        public static extern int VerifySignatureWithPublicKey(string PublicKeyFile
                                                              , string EncryptKey
                                                              , string LicenseFile);

        private static readonly ProductKeyManager instance = new ProductKeyManager();
        public const string LICENSE_DATE_FORMAT = "MM/dd/yyyy";

        private Dictionary<string, string> m_dictProductKeyInfo = new Dictionary<string, string>();
        private List<SettingData> m_allSettingData = new List<SettingData>();

        private iniFileManager m_iniFileMgr;
        private bool m_isMarketTracker;
        private string m_publicKeyFile;
        private string m_licenseKeyFile;
        private bool m_isValidPublicKeyFile;
        private bool m_isValidLicenseKeyFile;

        private bool m_isExpiredProductKeyFile;

        private int m_maxInstances;
        private int m_maxTofOrDtfInputs;
        private int m_maxTofOrDtfInputPerInstance;

        static ProductKeyManager()
        {
        }

        private ProductKeyManager()
        {
        }

        public static ProductKeyManager Instance
        {
            get
            {
                return instance;
            }
        }

        public void Initialise(bool isMarketTracker, string publicKeyFile, string licenseKeyFile)
        {
            m_isMarketTracker = isMarketTracker;

            m_publicKeyFile = publicKeyFile;
            m_isValidPublicKeyFile = ValidPublicKeyFile();

            m_licenseKeyFile = licenseKeyFile;
            m_isValidLicenseKeyFile = VerifySignature();

            m_maxInstances = 0;
            m_maxTofOrDtfInputs = 0;
            m_maxTofOrDtfInputPerInstance = 0;

            if (m_isValidLicenseKeyFile)
            {
                m_iniFileMgr = new iniFileManager(m_licenseKeyFile);
            }
        }

        public void SetNewLicenseKeyFile(string licenseKeyFile)
        {
            m_licenseKeyFile = licenseKeyFile;
            m_isValidPublicKeyFile = true;
            m_isValidLicenseKeyFile = true;
            m_iniFileMgr = new iniFileManager(m_licenseKeyFile);

            LoadAllSettingData();
        }

        public string GetEncryptPublicKey()
        {
            if (m_isMarketTracker)
            {
                return MT_PUBLIC_ENCRYPT_KEY;
            }

            return DT_PUBLIC_ENCRYPT_KEY;
        }

        public bool ValidPublicKeyFile()
        {
            return File.Exists(m_publicKeyFile);
        }

        private bool VerifySignature()
        {
            if (File.Exists(m_licenseKeyFile))
            {
                int res = VerifySignatureWithPublicKey(m_publicKeyFile
                                                       , GetEncryptPublicKey()
                                                       , m_licenseKeyFile);
                if (res == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsValidProductKeyFile()
        {
            return ValidPublicKeyFile() && VerifySignature();
        }

        public bool IsProductKeyFileExist()
        {
            if (File.Exists(m_licenseKeyFile))
            {
                return true;
            }

            return false;
        }

        public bool IsPublicKeyFileExist()
        {
            if (File.Exists(m_publicKeyFile))
            {
                return true;
            }

            return false;
        }

        private bool IsExpiredProductKeyFile(string expirationDate)
        {
            try
            {
                //Current Date
                string currentDate = DateTime.Now.ToString(LICENSE_DATE_FORMAT, DateTimeFormatInfo.InvariantInfo);
                DateTime dCurrentDate = DateTime.ParseExact(currentDate, LICENSE_DATE_FORMAT, DateTimeFormatInfo.InvariantInfo);

                //Expiration_Date from product key 
                DateTime dExpirationDate = DateTime.ParseExact(expirationDate, LICENSE_DATE_FORMAT, DateTimeFormatInfo.InvariantInfo);

                if (dCurrentDate >= dExpirationDate)
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }

            return false;
        }

        private string GetProductKeyStatus(string expirationDate)
        {
            m_isExpiredProductKeyFile = IsExpiredProductKeyFile(expirationDate);

            if (m_isExpiredProductKeyFile)
            {
                return "Expired";
            }

            return "Valid";
        }

        private string ConvertDateStringToDisplayGuiDateFormat(string dateInput)
        {
            try
            {
                DateTime date = DateTime.ParseExact(dateInput, LICENSE_DATE_FORMAT, DateTimeFormatInfo.InvariantInfo);
                return date.ToString("dd MMM yyy", DateTimeFormatInfo.InvariantInfo);
            }
            catch
            {
                return "";
            }
        }

        public int GetTotalCurrentTofOrDtfInputs()
        {
            int count = 0;
            RootConfiguration rootNode = ConfigManager.Instance.GetRootNode();
            ComputerConfiguration computerNode = rootNode.GetComputerNode();
            List<ServiceConfiguration> allServicesNodes = computerNode.GetAllServicesNode();

            for (int i = 0; i < allServicesNodes.Count; i++)
            {
                ServiceConfiguration service = allServicesNodes[i];

                if (service != null)
                {
                    count += service.m_inputsNode.GetAllTcidInputsNode().Count;
                }
            }

            return count;
        }

        public string[] LoadDefaultKey()
        {
            string[] strArray = new string[7];

            strArray[0] = SiteProductKeyRegisConfiguration.KEY_PACKAGE_NAME;
            strArray[1] = SiteProductKeyRegisConfiguration.KEY_PRODUCT_KEY_STATUS;
            strArray[2] = SiteProductKeyRegisConfiguration.KEY_CREATED_DATE;
            strArray[3] = SiteProductKeyRegisConfiguration.KEY_EXPIRATION_DATE;
            strArray[4] = SiteProductKeyRegisConfiguration.KEY_NUMBER_OF_INSTANCES;
            strArray[5] = SiteProductKeyRegisConfiguration.KEY_NUMBER_OF_TOFDTF;
            strArray[6] = SiteProductKeyRegisConfiguration.KEY_MAX_TOFDTF_PER_INSTANCE;

            return strArray;
        }

        public string[] LoadDefaultDisplayName()
        {
            string[] strArray = new string[7];

            strArray[0] = SiteProductKeyRegisConfiguration.DISP_PACKAGE_NAME;
            strArray[1] = SiteProductKeyRegisConfiguration.DISP_PRODUCT_KEY_STATUS;
            strArray[2] = SiteProductKeyRegisConfiguration.DISP_CREATED_DATE;
            strArray[3] = SiteProductKeyRegisConfiguration.DISP_EXPIRATION_DATE;
            strArray[4] = SiteProductKeyRegisConfiguration.DISP_NUMBER_OF_INSTANCES;
            strArray[5] = SiteProductKeyRegisConfiguration.DISP_NUMBER_OF_TOFDTF;
            strArray[6] = SiteProductKeyRegisConfiguration.DISP_MAX_TOFDTF_PER_INSTANCE;

            return strArray;
        }

        public string[] LoadDefaultValueOfKey()
        {
            string[] strArray = new string[7];

            strArray[0] = "-";
            strArray[1] = "Invalid";
            strArray[2] = "-";
            strArray[3] = "-";
            strArray[4] = "-";
            strArray[5] = "-";
            strArray[6] = "-";

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
            string productPgkName = "";
            string productKeyStatus = "";
            string createDate = "";
            string expirationDate = "";
            string numberOfInstance = "";
            string numberOfTofOrDtfInputs = "";
            string maxTofOrDtfInputPerInstance = "";

            m_dictProductKeyInfo.Clear();
            m_allSettingData.Clear();

            if (IsValidProductKeyFile())
            {
                productPgkName = m_iniFileMgr.IniReadValue(SiteProductKeyRegisConfiguration.SECTION_LICENSE, SiteProductKeyRegisConfiguration.KEY_PACKAGE_NAME);

                createDate = m_iniFileMgr.IniReadValue(SiteProductKeyRegisConfiguration.SECTION_LICENSE, SiteProductKeyRegisConfiguration.KEY_CREATED_DATE);
                createDate = ConvertDateStringToDisplayGuiDateFormat(createDate);

                expirationDate = m_iniFileMgr.IniReadValue(SiteProductKeyRegisConfiguration.SECTION_LICENSE, SiteProductKeyRegisConfiguration.KEY_EXPIRATION_DATE);
                productKeyStatus = GetProductKeyStatus(expirationDate);
                expirationDate = ConvertDateStringToDisplayGuiDateFormat(expirationDate);

                string maxInstance = m_iniFileMgr.IniReadValue(SiteProductKeyRegisConfiguration.SECTION_LICENSE, SiteProductKeyRegisConfiguration.KEY_INSTANCE);
                int.TryParse(maxInstance.Trim(), out m_maxInstances);
                numberOfInstance = string.Format("{0} of {1}", GetTotalCurrentInstances().ToString(), m_maxInstances.ToString());

                string maxTofOrDtfInputs = m_iniFileMgr.IniReadValue(SiteProductKeyRegisConfiguration.SECTION_LICENSE, SiteProductKeyRegisConfiguration.KEY_TOFDTF_INPUT);
                int.TryParse(maxTofOrDtfInputs.Trim(), out m_maxTofOrDtfInputs);
                numberOfTofOrDtfInputs = string.Format("{0} of {1}", GetTotalCurrentTofOrDtfInputs().ToString(), m_maxTofOrDtfInputs.ToString());

                maxTofOrDtfInputPerInstance = m_iniFileMgr.IniReadValue(SiteProductKeyRegisConfiguration.SECTION_LICENSE, SiteProductKeyRegisConfiguration.KEY_MAX_TOFDTF_PER_INSTANCE);
                int.TryParse(maxTofOrDtfInputPerInstance.Trim(), out m_maxTofOrDtfInputPerInstance);
            }

            m_dictProductKeyInfo.Add(SiteProductKeyRegisConfiguration.KEY_PACKAGE_NAME, productPgkName);
            m_dictProductKeyInfo.Add(SiteProductKeyRegisConfiguration.KEY_PRODUCT_KEY_STATUS, productKeyStatus);
            m_dictProductKeyInfo.Add(SiteProductKeyRegisConfiguration.KEY_CREATED_DATE, createDate);
            m_dictProductKeyInfo.Add(SiteProductKeyRegisConfiguration.KEY_EXPIRATION_DATE, expirationDate);
            m_dictProductKeyInfo.Add(SiteProductKeyRegisConfiguration.KEY_NUMBER_OF_INSTANCES, numberOfInstance);
            m_dictProductKeyInfo.Add(SiteProductKeyRegisConfiguration.KEY_NUMBER_OF_TOFDTF, numberOfTofOrDtfInputs);
            m_dictProductKeyInfo.Add(SiteProductKeyRegisConfiguration.KEY_MAX_TOFDTF_PER_INSTANCE, maxTofOrDtfInputPerInstance);

            List<SettingData> allSettingData = new List<SettingData>();

            string[] strArrayDefKey = LoadDefaultKey();
            string[] strArrayDefDisp = LoadDefaultDisplayName();
            string[] strArrayDefValofKey = LoadDefaultValueOfKey();
            List<FieldShow> listDefFieldShow = LoadDefaultFieldShow();
            List<bool> listDefRemovable = LoadDefaultRemovable();
            List<bool> listDefSpecialField = LoadDefaultSpecialField();

            AddSettingData(strArrayDefKey
                            , strArrayDefDisp
                            , strArrayDefValofKey
                            , ""
                            , listDefFieldShow
                            , listDefRemovable
                            , listDefSpecialField
                            , m_dictProductKeyInfo
                            , ref allSettingData);

            for (int i = 0; i < allSettingData.Count; i++)
            {
                SettingData settingData = allSettingData[i];
                ConfigManager.Instance.SetFieldStausToMissingRequired(settingData.m_key, settingData.m_value, ref settingData.m_status);

                if (settingData.m_status == FieldStaus.Green)
                {
                    switch (settingData.m_key)
                    {
                        case SiteProductKeyRegisConfiguration.KEY_EXPIRATION_DATE:
                            {
                                if (m_isExpiredProductKeyFile)
                                {
                                    settingData.m_status = FieldStaus.Red;
                                }
                            }
                            break;

                        case SiteProductKeyRegisConfiguration.KEY_NUMBER_OF_INSTANCES:
                            {
                                if (GetTotalCurrentInstances() > m_maxInstances)
                                {
                                    settingData.m_status = FieldStaus.Red;
                                }
                            }
                            break;

                        case SiteProductKeyRegisConfiguration.KEY_NUMBER_OF_TOFDTF:
                            {
                                if (GetTotalCurrentTofOrDtfInputs() > m_maxTofOrDtfInputs)
                                {
                                    settingData.m_status = FieldStaus.Red;
                                }
                            }
                            break;
                    }
                }
            }

            m_allSettingData = ConfigManager.Instance.MakeSettingDataByView(allSettingData);
        }

        public void AddSettingData(string[] strArrayDefKey
                                    , string[] strArrayDefDisp
                                    , string[] strArrayDefValofKey
                                    , string section
                                    , List<FieldShow> listDefFieldShow
                                    , List<bool> listDefRemovable
                                    , List<bool> listDefSpecialField
                                    , Dictionary<string, string> m_dictProductKeyInfo
                                    , ref List<SettingData> allSettingData)
        {
            List<string> listAddSettingData = new List<string>();

            #region Add Setting Data with Default
            for (int i = 0; i < strArrayDefKey.Length; i++)
            {
                string key = "";
                string value = "";

                key = strArrayDefKey[i];

                if (m_dictProductKeyInfo.ContainsKey(key))
                {
                    value = m_dictProductKeyInfo[key];
                }

                if (value.Length > 0)
                {
                    AddSettingData(key
                                    , value
                                    , strArrayDefDisp[i]
                                    , strArrayDefValofKey[i]
                                    , section
                                    , FieldStaus.Green
                                    , listDefFieldShow[i]
                                    , listDefRemovable[i]
                                    , listDefSpecialField[i]
                                    , ref allSettingData);
                }
                else
                {
                    AddSettingData(key
                                    , strArrayDefValofKey[i]
                                    , strArrayDefDisp[i]
                                    , strArrayDefValofKey[i]
                                    , section
                                    , FieldStaus.Red
                                    , listDefFieldShow[i]
                                    , listDefRemovable[i]
                                    , listDefSpecialField[i]
                                    , ref allSettingData);
                }

                listAddSettingData.Add(key);
            }
            #endregion
        }

        public void AddSettingData(string key
                                    , string value
                                    , string display
                                    , string defaultValue
                                    , string section
                                    , FieldStaus status
                                    , FieldShow show
                                    , bool removable
                                    , bool specialField
                                    , ref List<SettingData> allSettingData)
        {
            SettingData settingData = null;

            #region Normal Instance
            settingData = new SettingData(key, value, display, defaultValue, section, status, show, removable, specialField);
            #endregion

            allSettingData.Add(settingData);
        }


        public List<SettingData> GetAllSettingData()
        {
            LoadAllSettingData();
            return m_allSettingData;
        }

        public int GetMaxInstances()
        {
            return m_maxInstances;
        }

        public int GetTotalCurrentInstances()
        {
            return ConfigManager.Instance.GetTotalCurrentInstance();
        }

        public int GetMaxTofOrDtfInputs()
        {
            return m_maxTofOrDtfInputs;
        }

        public int GetMaxTofOrDtfInputPerInstance()
        {
            return m_maxTofOrDtfInputPerInstance;
        }

        public bool IsProductKeyExpired()
        {
            bool ret = false;
            var expirationDate = m_iniFileMgr.IniReadValue(SiteProductKeyRegisConfiguration.SECTION_LICENSE, SiteProductKeyRegisConfiguration.KEY_EXPIRATION_DATE);
            ret = IsExpiredProductKeyFile(expirationDate);

            return ret;
        }

        public bool IsInstanceExceedLimit()
        {
            int totalCurInstance = GetTotalCurrentInstances();
            int maxInstance = GetMaxInstances();

            if (totalCurInstance > maxInstance)
            {
                return true;
            }

            return false;
        }

        public bool IsTofDtfInputExceedLimit()
        {
            int totalCurTofOrDtfInputs = GetTotalCurrentTofOrDtfInputs();
            int maxTofOrDtfInputs = GetMaxTofOrDtfInputs();

            if (totalCurTofOrDtfInputs > maxTofOrDtfInputs)
            {
                return true;
            }

            return false;
        }

        public bool IsTofDtfInputPerInstanceExceedMax()
        {
            int totalInputsNodes = 0;
            int maxTofOrDtfInputPerInstance = GetMaxTofOrDtfInputPerInstance();

            RootConfiguration rootNode = ConfigManager.Instance.GetRootNode();
            ComputerConfiguration computerNode = rootNode.GetComputerNode();
            List<ServiceConfiguration> allServicesNodes = computerNode.GetAllServicesNode();

            for (int i = 0; i < allServicesNodes.Count; i++)
            {
                ServiceConfiguration service = allServicesNodes[i];

                if (service != null)
                {
                    totalInputsNodes = service.m_inputsNode.GetAllTcidInputsNode().Count;

                    if (totalInputsNodes > maxTofOrDtfInputPerInstance)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool VerifyNewSignature(string filePath)
        {
            bool ret = false;
            if (File.Exists(filePath))
            {
                int res = VerifySignatureWithPublicKey(m_publicKeyFile
                                                       , GetEncryptPublicKey()
                                                       , filePath);
                if (res == 0)
                {
                    ret = true;
                }
            }

            return ret;
        }

        public bool IsNewSignatureExpired(string filePath)
        {
            bool ret = false;
            iniFileManager iniFileMgr = new iniFileManager(filePath);

            var expirationDate = iniFileMgr.IniReadValue(SiteProductKeyRegisConfiguration.SECTION_LICENSE, SiteProductKeyRegisConfiguration.KEY_EXPIRATION_DATE);
            ret = IsExpiredProductKeyFile(expirationDate);

            return ret;
        }
    }
}