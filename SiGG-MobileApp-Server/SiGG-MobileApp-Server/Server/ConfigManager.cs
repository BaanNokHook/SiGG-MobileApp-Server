using SiGG_MobileApp_Server.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public sealed class ConfigManager
    {
        private static readonly ConfigManager instance - new ConfigManager();

        private static Server m_server;

        private Dictionary<String, int> m_dictServiceInstance = new Dictionary<string, int>();

        private RootConfiguration m_RootNode;

        private List<SettingData> m_ServiceNode = new List<SettingData>();7

        private static List<string> m_serviceName = new List<string>();

        public string NODE_NAME_DTF = "DTF";
        public string NODE_NAME_TOF = "TOF";

        public string KEY_MMC_TYPE = "MMC_TYPE";

        public string SITE_PRODUCT_KEY_REGIS_NODE_NAME = "SiteProductKeyRegistration";
        public string PRODUCT_PROPERTIES_NODE_NAME = "ProductProperties";

        public int INSTANCE_NUMBER_MIN = 1;
        public int INSTANCE_NUMBER_MAX = 10;

        public int SLAVE_INSTANCE_NUMBER_MIN = 1;
        public int SLAVE_INSTANCE_NUMBER_MAX = 4;

        public const string FIELD_STATUS_CUSTOM = "Custom";
        public const string FIELD_STATUS_CUSTOM_NO_VALUE = "Custom - no value";   
        public const string FIELD_STATUS_DEFAULT = "Default";
        public const string FIELD_STATUS_DISABLED = "Disabled";
        public const string FIELD_STATUS_MISSING_REQUIRED = "Missing - required";
        public const string FIELD_STATUS_DUPLICATE = "Duplicate";   

        public string DIRECTORY_SITE_PRODUCT_KEY
        {
            get { return "Site Product Key"; } 
        }

        public string DIRECTORY_PRODUCT_PROPERTIES
        {
            get { return "Product Properties";  }
        }  

        public string SEVER_INI_FILE_NAME
        {
            get { return "dtserver.int"; }  
        }


        private string _DTSERVER_PRODUCTNAME;  

        public string DTSERVER_PRODUCTNAME
        {
            get
            {
                if (_DTSERVER_PRODUCTNAME == null)
                {
                    _DTSERVER_PRODUCTNAME = "Deal Tracker";   
                }
                return _DTSERVER_PRODUCTNAME;   
            }

            set { _DTSERVER_PRODUCTNAME = value; }

        }
        private string _MTSERVER_PRODUCTNAME;   

        public string MTSERVER_PRODUCTNAME
        {
            get
            {
                if (_MTSERVER_PRODUCTNAME == null)
                {
                    _MTSERVER_PRODUCTNAME = "Market Tracker";
                }
                return _MTSERVER_PRODUCTNAME;
            }

            set { _MTSERVER_PRODUCTNAME = value; }
        }

        private string _DTSERVER_PRODUCTNAME_SHORT;
public string DTSERVER_PRODUCTNAME_SHORT
{
    get
    {
        if (_DTSERVER_PRODUCTNAME_SHORT == null)
        {
            _DTSERVER_PRODUCTNAME_SHORT = "DT Server";
        }
        return _DTSERVER_PRODUCTNAME_SHORT;
    }

    set { _DTSERVER_PRODUCTNAME_SHORT = value; }
}
private string _MTSERVER_PRODUCTNAME_SHORT;
public string MTSERVER_PRODUCTNAME_SHORT
{
    get
    {
        if (_MTSERVER_PRODUCTNAME_SHORT == null)
        {
            _MTSERVER_PRODUCTNAME_SHORT = "MT Server";
        }
        return _MTSERVER_PRODUCTNAME_SHORT;
    }

    set { _MTSERVER_PRODUCTNAME_SHORT = value; }
}

public string _DTSERVER_PRODUCTNAME_FULL;
public string DTSERVER_PRODUCTNAME_FULL
{
    get
    {
        if (_DTSERVER_PRODUCTNAME_FULL == null)
        {
            _DTSERVER_PRODUCTNAME_FULL = "Deal Tracker Server";
        }
        return _DTSERVER_PRODUCTNAME_FULL;
    }

    set { _DTSERVER_PRODUCTNAME_FULL = value; }
}
public string _MTSERVER_PRODUCTNAME_FULL;
public string MTSERVER_PRODUCTNAME_FULL
{
    get
    {
        if (_MTSERVER_PRODUCTNAME_FULL == null)
        {
            _MTSERVER_PRODUCTNAME_FULL = "Market Tracker Server";
        }
        return _MTSERVER_PRODUCTNAME_FULL;
    }

    set { _MTSERVER_PRODUCTNAME_FULL = value; }
}

private string _DTSERVER_SERVICENAME;
public string DTSERVER_PREFIX_SERVICENAME
{
    get
    {
        if (_DTSERVER_SERVICENAME == null)
        {
            _DTSERVER_SERVICENAME = "DTServer";
        }
        return _DTSERVER_SERVICENAME;
    }

    set { _DTSERVER_SERVICENAME = value; }
}
private string _MTSERVER_SERVICENAME;
public string MTSERVER_PREFIX_SERVICENAME
{
    get
    {
        if (_MTSERVER_SERVICENAME == null)
        {
            _MTSERVER_SERVICENAME = "MTServer";
        }
        return _MTSERVER_SERVICENAME;
    }

    set { _MTSERVER_SERVICENAME = value; }
}

private string _COMPUTER_NAME;
public string COMPUTER_NAME
{
    get { return _COMPUTER_NAME; }
    set { _COMPUTER_NAME = value; }
}

private bool m_isMarketTracker;
private string m_productName;
private string m_productShortName;
private string m_productFullName;
private string m_prefixServiceName;

public enum _PROCESS_TYPES
{
    PROCESS_TYPE_MMC_INIT_LOAD = 0,
    PROCESS_TYPE_MASTERSLAVECONFIGDLG_LOAD,
    PROCESS_TYPE_MASTERSLAVECONFIGDLG_CONFRIM,
    PROCESS_TYPE_STARTSERVICE,
    PROCESS_TYPE_ADDREMOVE_INSTANCE,
    PROCESS_TYPE_UPDATE_LICENSE_KEY,
}

public enum _ENABLED_SLAVE_COMBOBOX
{
    ENABLED_SLAVE_COMBOBOX_0 = 0,
    ENABLED_SLAVE_COMBOBOX_1,
    ENABLED_SLAVE_COMBOBOX_2,
    ENABLED_SLAVE_COMBOBOX_3,
    ENABLED_SLAVE_COMBOBOX_4,
};

public const int PERMITTED_TOF_DTF_FOR_2INSTANCES = 24;

public string PUBLICKEY_FILE_NOT_EXIST_MSG
{
    get { return "The Public Key file does not exist."; }
}

public string PRODUCTKEY_FILE_NOT_EXIST_MSG
{
    get { return "The Site Product Key file does not exist."; }
}

public string PRODUCTKEY_INVALID_MSG
{
    get { return "Invalid Site Product Key file: "; }
}

public string INVALIDE_SIGNATRUE
{
    get { return PRODUCTKEY_INVALID_MSG + "Invalid Signature."; }
}

public string PRODUCTKEY_EXPIRED_MSG
{
    get
    {
        string errMessage = "The Site Product Key file is expired.";
        errMessage += "\n\nFor more information,";
        errMessage += "\nplease see Site Product Key Registration page in Refinitiv " + GetProductFullName() + " Manager.";
        return errMessage;
    }
}

public string PRODUCTKEY_EXPIRED_PERIOD_MSG(string day)
{
    return "The Site Product Key file will be expired in " + day + " day(s).\n\nFor more information,\nplease see Site Product Key Registration page in Refinitiv " + GetProductFullName() + " Manager.";
}

public string INSTANCE_EXCEED_MSG
{
    get { return "Number of instances exceeded the Site Product Key file limit.\n\nFor more information,\nplease see Site Product Key Registration page in Refinitiv " + GetProductFullName() + " Manager."; }
}

public string TOTAL_TOFDTF_EXCEED_MSG
{
    get { return "Number of TOF/DTF inputs exceeded the Site Product Key file limit.\n\nFor more information,\nplease see Site Product Key Registration page in Refinitiv " + GetProductFullName() + " Manager."; }
}

public string TOFDTF_PER_INSTANCE_EXCEED_MSG
{
    get { return "Maximum number of TOF/DTF inputs per instance exceeded.\n\nFor more information,\nplease see Site Product Key Registration page in Refinitiv " + GetProductFullName() + " Manager."; }
}

public string INSTANCE_TOFDTF_INPUT_EXCEED_MSG(string instance, string maxInstance)
{
    return "Maximum number of TOF/DTF instance(s) exceeded (Attempted to add " + instance + " instances but maximum number of instance is " + maxInstance + ".).\nNo more TOF/DTF input can be added.\n\nFor more information,\nplease see Site Product Key Registration page in Refinitiv " + GetProductFullName() + " Manager.";
}

public string MSG_HEADER_MASTERSLAVE
{
    get { return "Master-Slave Setting"; }
}
public string MSG_HEADER_START_SERVICE = "Start Service";

public string ERRMSG_MASTERSLAVE_CANNOT_OPEN_SETTING_DLG = "Cannot open the Master-Slave Setting dialog because the '{0}' service is running.\n\nPlease stop the service before setting Master-Slave option.";
public const string ERRMSG_MASTERSLAVE_AUTOSETDISABLE = "\n\n{0} is currently configured with Master-Slave option enabled so {1} will disable Master-Slave option automatically.";
public const string ERRMSG_MASTERSLAVE_INVALID_CONFIGURE = "The configuration file (.ini ) contains invalid for Master-Slave option.\n{0} will set to turn off Master-Slave option and set all slave instances to default automatically.";
public const string ERRMSG_MASTERSLAVE_INVALID_SITEPRODUCTKEY = "{0} does not allow setting Master-Slave option because the site product key have TOF/DTF instance less than 2 instances.";
public const string ERRMSG_MASTERSLAVE_TURNOFF_MASTERSLAVE_OPT = "\n{0} will set to turn off Master-Slave option and set all slave instances to default automatically.";
public const string ERRMSG_MASTERSLAVE_MUSTHAVE_SLAVE_LEAST_ONE = "{0} must have at least 1 slave instance configured in Master-Slave option.";
public const string ERRMSG_MASTERSLAVE_PLEASE_RECHECK_SERVICE = "\n\nPlease re-check it in master instance before restarting the service.";
public const string ERRMSG_MASTERSLAVE_NOT_ALLOW_MASTER_IS_SLAVE_SETTINGDLG = "The configuration file (.ini ) contains invalid slave instance number.\n{0} does not allow setting master instance as slave instance [SlaveInstance{1} is '{2}'].\n{3} will set all slave instances to default in Master-Slave Setting dialog and will set to turn off Master-Slave option automatically.\n\nPlease re-check it before configure Master-Slave option.";
public const string ERRMSG_MASTERSLAVE_NOT_ALLOW_MASTER_IS_SLAVE = "{0} does not allow setting master instance as slave instance [SlaveInstance{1} is '{2}'].";
public const string ERRMSG_MASTERSLAVE_PLEASE_RECHECK_ADDREMOVEINTS = "\n\nPlease re-check it in master instance before adding/removing {0} instance(s).";
public const string ERRMSG_MASTERSLAVE_INVALID_SLAVE_INI = "The configuration file (.ini ) contains invalid slave instance number [SlaveInstance{0} is '{1}'].";
public const string ERRMSG_MASTERSLAVE_INVALID_SLAVE_BOUNDARY_SETTINGDLG = "The configuration file (.ini ) contains invalid slave instance number [SlaveInstance{0} is '{1}'].\n{2} will set all slave instances to default in Master-Slave Setting dialog and will set to turn off Master-Slave option automatically.\n\nPlease re-check it before configure Master-Slave option.";
public const string ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_INSTANCE_SETTINGDLG = "The configuration file (.ini ) contains invalid slave instance number.\n{0} does not allow setting Master-Slave option with duplicate slave instance.\n[SlaveInstance{1} and SlaveInstance{2} are '{3}'].\n{4} will set all slave instances to default in Master-Slave Setting dialog and will set to turn off Master-Slave option automatically.\n\nPlease re-check it before configure Master-Slave option.";
public const string ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_INSTANCE = "{0} does not allow setting Master-Slave option with duplicate slave instance.\n[SlaveInstance{1} and SlaveInstance{2} are '{3}'].\n\nPlease choose different slave instance.";
public const string ERRMSG_MASTERSLAVE_PLEASE_RECHECK = "\n{0} will set to turn off Master-Slave option and set all slave instances to default automatically.";
public const string ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_TCID_SETTING = "{0} does not allow setting Master-Slave option with duplicate TCID.\n['{1}' TCID in {2} is duplicate with '{3}' TCID in {4}].\n\nPlease remove or edit one '{5}' TCID.";
public const string ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_TCID_SERVICE = "{0} does not allow starting master service instance with duplicate TCID in Master-Slave option.\n['{1}' TCID in {2} is duplicate with '{3}' TCID in {4}].\n\nPlease remove or edit one '{5}' TCID before restarting the service.";
public const string ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_ALIAS_SETTING = "{0} does not allow setting Master-Slave option with duplicate alias name.\n['{1}' alias name in {2} is duplicate with '{3}' alias name in {4}].\n\nPlease remove or edit one '{5}' alias name.";
public const string ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_ALIAS_SERVICE = "{0} does not allow starting master service instance with duplicate alias name in Master-Slave option.\n['{1}' alias name in {2} is duplicate with '{3}' alias name in {4}].\n\nPlease remove or edit one '{5}' alias name before restarting the service.";
public const string ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_ALIASAndTCID_SETTING = "{0} does not allow setting Master-Slave option with duplicate alias name with TCID.\n['{1}' alias name in {2} duplicate with '{3}' TCID {4}].\n\nPlease remove or edit one.";
public const string ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_ALIASAndTCID_SERVICE = "{0} does not allow starting master service instance with duplicate alias name with TCID in Master-Slave option.\n['{1}' alias name in {2} duplicate with '{3}' TCID {4}].\n\nPlease remove or edit one before restarting the service.";
public const string ERRMSG_MASTERSLAVE_SLAVE_MUST_NOT_HAVE_OUTPUT = "The slave instance must not have output in Master-Slave option {0}";
public const string ERRMSG_MASTERSLAVE_NOT_ALLOW_SLAVESERVICERUN_SETTING = "{0} does not allow running slave service instance ['{1}' service is running] while setting Master-Slave option.\n\nPlease stop '{2}' service before setting Master-Slave option.";
public const string ERRMSG_MASTERSLAVE_NOT_ALLOW_SLAVESERVICERUN_SERVICE = "{0} does not allow running slave service instance ['{1}' service is running] while starting master service instance.\n\nPlease stop '{2}' service before starting master service instance.";
public const string ERRMSG_MASTERSLAVE_MASTER_MUST_NOT_HAVE_DTAAPPOUTPUT = "{0} does not allow setting DT Application output in Master-Slave option [{1} {2}].\n\nPlease remove all DTApplication outputs.";

public string ERRMAG_DUPLICATE_IPOUTPUT_STARTSERVICE = "{0} does not allow setting duplicate The TCP/IP or DT Application outputs.\n\nThe port [{1}] duplicated with {2} in {3} and {4} in {5}.\n\nPlease remove or edit one before restarting the service.";

static ConfigManager()
{
}

private ConfigManager()
{
    Initialise();
}

public static ConfigManager Instance
{
    get
    {
        return instance;
    }
}

private void Initialise()
{
    COMPUTER_NAME = Environment.MachineName.ToString();

    if (RegistryHelpers.GetDisplayName().IndexOf(MTSERVER_PRODUCTNAME_FULL) >= 0)
    {
        m_isMarketTracker = true;

        m_productName = MTSERVER_PRODUCTNAME;
        m_productShortName = MTSERVER_PRODUCTNAME_SHORT;
        m_productFullName = MTSERVER_PRODUCTNAME_FULL;
        m_prefixServiceName = MTSERVER_PREFIX_SERVICENAME;
    }
    else
    {
        m_isMarketTracker = false;

        m_productName = DTSERVER_PRODUCTNAME;
        m_productShortName = DTSERVER_PRODUCTNAME_SHORT;
        m_productFullName = DTSERVER_PRODUCTNAME_FULL;
        m_prefixServiceName = DTSERVER_PREFIX_SERVICENAME;
    }

    for (int instanceNo = INSTATNCE_NUMBER_MIN; instanceNo <= INSTATNCE_NUMBER_MAX; instanceNo++)
    {
        m_dictServiceInstance.Add(GetServiceNameFromInstanceNo(instanceNo), instanceNo);
    }
}

public void LoadAllConfigurations()
{
    m_RootNode = new RootConfiguration();
}

public void SetServer(Server server)
{
    m_server = server;
}

public Server GetServer()
{
    return m_server;
}

public RootConfiguration GetRootNode()
{
    return m_RootNode;
}

public int GetTotalCurrentInstance()
{
    int iTotalCurrentInstance = 0;

    for (int instanceNo = INSTATNCE_NUMBER_MIN; instanceNo <= INSTATNCE_NUMBER_MAX; instanceNo++)
    {
        string serviceName = GetServiceNameFromInstanceNo(instanceNo);

        if (IsServiceExists(serviceName))
        {
            iTotalCurrentInstance++;
        }
        else
        {
            break;
        }
    }

    return iTotalCurrentInstance;
}

public List<SettingData> MakeSettingDataByView(List<SettingData> allSettingData)
{
    List<SettingData> settingDataLists = new List<SettingData>();

    for (int i = 0; i < allSettingData.Count; i++)
    {
        SettingData settingData = allSettingData[i];

        if ((settingData.m_show == FieldShow.OnlyAdvance)
            || (settingData.m_show == FieldShow.Both))
        {
            settingDataLists.Add(settingData);
        }
    }

    return settingDataLists;
}


public ServiceConfiguration GetServiceNode(string serviceName)
{
    ServiceConfiguration service = null;

    try
    {
        RootConfiguration rootNode = GetRootNode();
        if (rootNode != null)
        {
            ComputerConfiguration computerNode = rootNode.GetComputerNode();
            List<ServiceConfiguration> allServicesNodes = computerNode.GetAllServicesNode();

            for (int instanceNo = 0; instanceNo < allServicesNodes.Count; instanceNo++)
            {
                service = allServicesNodes[instanceNo];

                if (service.GetServiceName() == serviceName)
                {
                    break;
                }
            }
        }
    }
    catch
    {
        service = null;
    }

    return service;
}

public string GetComputerName()
{
    return COMPUTER_NAME;
}

public bool IsMarketTracker()
{
    return m_isMarketTracker;
}

public string GetProductName()
{
    return m_productName;
}

public string GetProductShortName()
{
    return m_productShortName;
}

public string GetProductFullName()
{
    return m_productFullName;
}

public string GetPrefixServiceName()
{
    return m_prefixServiceName;
}

public int GetInstanceNoFromServiceName(string serviceName)
{
    if (m_dictServiceInstance.ContainsKey(serviceName))
    {
        return m_dictServiceInstance[serviceName];
    }

    return 0;
}

public int GetInstanceNoFromServiceDisplayName(string ServiceDisplayName)
{
    int instanceNo = 0;

    if (ServiceDisplayName != MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE)
    {
        string temp = ServiceDisplayName;
        temp = temp.Substring(GetProductFullName().Length, temp.Length - GetProductFullName().Length);

        int.TryParse(temp, out instanceNo);
    }

    return instanceNo;
}

public string GetServiceNameFromServiceDisplayName(string serviceDisplayName)
{
    string temp = serviceDisplayName;
    int lastindex = temp.IndexOf("{");
    string productFullName = GetProductFullName();
    if (lastindex > 0 && lastindex > productFullName.Length)
    {
        int length = lastindex - productFullName.Length;
        temp = temp.Substring(GetProductFullName().Length, length);
    }
    else
    {
        temp = temp.Substring(GetProductFullName().Length, temp.Length - GetProductFullName().Length);
    }
    temp = temp.Trim();
    int instanceNo = 0;
    int.TryParse(temp, out instanceNo);

    return GetServiceNameFromInstanceNo(instanceNo);
}

public string GetServiceNameFromInstanceNo(int instanceNo)
{
    return string.Format("{0}{1}", GetPrefixServiceName(), instanceNo.ToString("00"));
}

public string GetMasterInstanceServiceName()
{
    int instanceNo = INSTATNCE_NUMBER_MIN;
    return string.Format("{0}{1}", GetPrefixServiceName(), instanceNo.ToString("00"));
}

private string GetApplicationDataPath()
{
    string strReturn = RegistryHelpers.GetDataLocation();
    return strReturn;
}

public string GetServerPath()
{
    string strReturn = "";
    strReturn = GetApplicationDataPath();
    strReturn += @"Refinitiv\";
    strReturn += RegistryHelpers.GetDisplayName().Substring(0, (RegistryHelpers.GetDisplayName().Length - 6));
    return strReturn;
}

public string GetIniFilePath(int instanceNo)
{
    string strReturn = "";
    strReturn = string.Format(@"{0}\{1} {2}\{3}", GetServerPath(), GetProductFullName(), instanceNo.ToString("00"), SEVER_INI_FILE_NAME);
    return strReturn;
}

public string GetProductKeyPath()
{
    string strReturn = "";
    strReturn = string.Format(@"{0}\{1}", GetServerPath(), DIRECTORY_SITE_PRODUCT_KEY);
    return strReturn;
}

public string GetProductKeyFilePath()
{
    string strReturn = "";
    strReturn = string.Format(@"{0}\{1}\{2} {3}", GetServerPath(), DIRECTORY_SITE_PRODUCT_KEY, GetProductFullName(), "Site Product Key.key");
    return strReturn;
}

public string GetPublicKeyFilePath()
{
    string strReturn = RegistryHelpers.GetInstallLocation() + "PublicKey.dat";
    return strReturn;
}

public string GetProductPropertiesPath()
{
    string strReturn = "";
    strReturn = string.Format(@"{0}\{1}", GetServerPath(), DIRECTORY_PRODUCT_PROPERTIES);
    return strReturn;
}

public string GetProductPropertiesIniFilePath()
{
    string strReturn = "";
    strReturn = string.Format(@"{0}\{1}\{2}", GetServerPath(), DIRECTORY_PRODUCT_PROPERTIES, "ProductProperties.ini");
    return strReturn;
}

public bool IsServiceExists(string serviceName)
{
    try
    {
        ServiceController[] services = ServiceController.GetServices();

        var service = services.FirstOrDefault(s => string.Equals(s.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase));
        return service != null;
    }
    catch
    {
        //...
    }

    return false;
}

public string GetServiceDisplayName(string serviceName)
{
    try
    {
        ServiceController[] services = ServiceController.GetServices();

        var service = services.FirstOrDefault(s => string.Equals(s.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase));

        if (service != null)
        {
            return service.DisplayName;
        }
    }
    catch
    {
        //
    }

    return "Unknown";
}

public string GetServiceDisplayNameAndStatus(string serviceName)
{
    try
    {
        string serviceDisplayName = GetServiceDisplayName(serviceName);
        string serviceStatus = GetServiceStatus(serviceName);
        if (serviceDisplayName != "Unknown")
        {
            serviceDisplayName += " {" + serviceStatus + "}";

            //If not Slave, check status is Start Pending or Stop Pending
            bool isEnableMasterSlave = IsEnableMasterSlave();
            var currentInstanceNo = GetInstanceNoFromServiceName(serviceName);
            var slaveInstances = GetAllSlaveInstance();

            if (!slaveInstances.Exists(item => item.Equals(currentInstanceNo.ToString())))
            {
                if (serviceStatus.Equals("Start Pending", StringComparison.OrdinalIgnoreCase))
                {
                    //SetWaitingServiceStartTimer(serviceName, Server.TIMER_FOR_START_STOP_SERVICE);
                }
                else if (serviceStatus.Equals("Stop Pending", StringComparison.OrdinalIgnoreCase))
                {
                    //SetWaitingForServiceStoppedTimer(serviceName, Server.TIMER_FOR_START_STOP_SERVICE);
                }
            }
            return serviceDisplayName;
        }
    }
    catch
    {
        //...
    }

    return "Unknown";
}

public string GetServiceDisplayNameFromInstanceNo(int instanceNo)
{
    return string.Format("{0} {1}", GetProductFullName(), instanceNo.ToString("00"));
}

public string GetServiceStatus(string serviceName)
{
    try
    {
        //Check whether the service is slave
        bool isEnableMasterSlave = IsEnableMasterSlave();
        bool isSlave = false;
        if (isEnableMasterSlave)
        {
            var slaveInstances = GetAllSlaveInstance();
            int instanceNo = GetInstanceNoFromServiceName(serviceName);

            isSlave = slaveInstances.Exists(item => item.Equals(instanceNo.ToString()));
        }

        //Slave status is from Master Slave (instance1)
        if (isSlave)
        {
            serviceName = GetServiceNameFromInstanceNo(INSTATNCE_NUMBER_MIN);
        }

        ServiceController service = new ServiceController(serviceName);
        if (service != null)
        {
            switch (service.Status)
            {
                case ServiceControllerStatus.Running:
                    return "Started";
                case ServiceControllerStatus.Stopped:
                    return "Stopped";
                case ServiceControllerStatus.Paused:
                    return "Paused";
                case ServiceControllerStatus.StopPending:
                    return "Stop Pending";
                case ServiceControllerStatus.StartPending:
                    return "Start Pending";
                default:
                    return "Status Changing";
            }
        }
    }
    catch
    {
        //...
    }

    return "Unknown";
}

public bool IsServiceRunningWithMasterSlave(string serviceName)
{
    //Check whether the service is slave
    bool isEnableMasterSlave = IsEnableMasterSlave();
    bool isSlave = false;
    if (isEnableMasterSlave)
    {
        var slaveInstances = GetAllSlaveInstance();
        int instanceNo = GetInstanceNoFromServiceName(serviceName);

        isSlave = slaveInstances.Exists(item => item.Equals(instanceNo.ToString()));
    }

    //Slave status is from Master Slave (instance1)
    if (isSlave)
    {
        serviceName = GetServiceNameFromInstanceNo(INSTATNCE_NUMBER_MIN);
    }

    return IsServiceRunning(serviceName);
}

public bool IsServiceRunning(string serviceName)
{
    try
    {
        ServiceController service = new ServiceController(serviceName);
        if (service != null)
        {
            switch (service.Status)
            {
                case ServiceControllerStatus.StartPending:
                case ServiceControllerStatus.ContinuePending:
                case ServiceControllerStatus.Running:
                    return true;

                case ServiceControllerStatus.PausePending:
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.StopPending:
                case ServiceControllerStatus.Stopped:
                default:
                    return false;
            }
        }
    }
    catch
    {
        //...
    }

    return false;
}

public bool HasServiceRunning()
{
    RootConfiguration rootNode = GetRootNode();
    ComputerConfiguration computerNode = rootNode.GetComputerNode();
    List<ServiceConfiguration> allServicesNodes = computerNode.GetAllServicesNode();

    bool ret = false;
    if (allServicesNodes.Count > 0)
    {
        foreach (var service in allServicesNodes)
        {
            if (IsServiceRunning(service.GetServiceName()))
            {
                ret = true;
                break;
            }
        }
    }

    return ret;
}

public bool IsServiceStoppedWithMasterSlave(string serviceName)
{
    //Check whether the service is slave
    bool isEnableMasterSlave = IsEnableMasterSlave();
    bool isSlave = false;
    if (isEnableMasterSlave)
    {
        var slaveInstances = GetAllSlaveInstance();
        int instanceNo = GetInstanceNoFromServiceName(serviceName);

        isSlave = slaveInstances.Exists(item => item.Equals(instanceNo.ToString()));
    }

    //Slave status is from Master Slave (instance1)
    if (isSlave)
    {
        serviceName = GetServiceNameFromInstanceNo(INSTATNCE_NUMBER_MIN);
    }

    return IsServiceStopped(serviceName);
}

public bool IsServiceStopped(string serviceName)
{
    bool ret = false;
    try
    {
        ServiceController[] services = ServiceController.GetServices();

        var service = services.FirstOrDefault(s => string.Equals(s.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase));
        if (service != null)
        {
            if (service.Status == ServiceControllerStatus.Stopped)
                ret = true;
        }
    }
    catch
    {
        //...
    }

    return ret;
}

public string GetServiceStartName(string serviceName)
{
    string username = "";

    System.Management.SelectQuery sQuery = new System.Management.SelectQuery(string.Format("select name, startname from Win32_Service where name = '{0}'", serviceName));
    using (System.Management.ManagementObjectSearcher mgmtSearcher = new System.Management.ManagementObjectSearcher(sQuery))
    {
        foreach (System.Management.ManagementObject service in mgmtSearcher.Get())
        {
            username = service["startname"].ToString();
            break;
        }
    }

    return username;
}

public bool IsEnableMasterSlave()
{
    bool isEnableMasterSlave = false;
    ServiceConfiguration masterService = GetServiceNode(GetMasterInstanceServiceName());

    if (masterService == null)
    {
        iniFileManager iniFileMgr = new iniFileManager(GetIniFilePath(INSTATNCE_NUMBER_MIN));
        string enableMasterSlave = iniFileMgr.IniReadValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_ENABLE_MASTER_SLAVE);

        if (enableMasterSlave == "TRUE")
        {
            isEnableMasterSlave = true;
        }
    }
    else
    {
        isEnableMasterSlave = masterService.m_globalNode.GetMasterSlaveNode().IsEnableMasterSlave();
    }

    return isEnableMasterSlave;
}

public bool IsSlaveInstance(string serviceName)
{
    bool isSlaveInstance = false;
    List<string> listSlaveInstance = GetAllSlaveInstance();

    for (int j = 0; j < listSlaveInstance.Count; j++)
    {
        int slaveInstanceNo = int.Parse(listSlaveInstance[j]);

        if (slaveInstanceNo == 0)
        {
            continue;
        }
        else
        {
            string slaveServiceName = GetServiceNameFromInstanceNo(slaveInstanceNo);

            if (slaveServiceName == serviceName)
            {
                isSlaveInstance = true;
                break;
            }
        }
    }

    return isSlaveInstance;
}

public List<string> GetAllSlaveInstance()
{
    List<string> listSlaveInstance = new List<string>();

    iniFileManager iniFileMgr = new iniFileManager(GetIniFilePath(INSTATNCE_NUMBER_MIN));

    string slaveInstance = "";
    slaveInstance = iniFileMgr.IniReadValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_1);
    listSlaveInstance.Add(slaveInstance);

    slaveInstance = iniFileMgr.IniReadValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_2);
    listSlaveInstance.Add(slaveInstance);

    slaveInstance = iniFileMgr.IniReadValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_3);
    listSlaveInstance.Add(slaveInstance);

    slaveInstance = iniFileMgr.IniReadValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_4);
    listSlaveInstance.Add(slaveInstance);

    return listSlaveInstance;
}

public FieldStaus GetNormalIconNodeFieldStaus(string key)
{
    FieldStaus status = FieldStaus.NotSet;

    switch (key)
    {
        case MasterSlaveConfiguration.SECTION_MASTER_SLAVE:
            status = FieldStaus.Inactive_MasterSlave;
            break;

        case GlobalConfiguration.SECTION_GLOBAL:
            status = FieldStaus.Inactive_Global;
            break;

        case InputsConfiguration.SECTION_INPUTS:
            status = FieldStaus.Inactive_Inputs;
            break;

        case OutputsConfiguration.SECTION_OUTPUTS:
            status = FieldStaus.Inactive_Outputs;
            break;

        case ErrorLogConfiguration.SECTION_ERRORLOG:
            status = FieldStaus.Inactive_ErrorLog;
            break;

        case RecordCacheConfiguration.SECTION_RECORD_CACHE:
            status = FieldStaus.Inactive_RecordCache;
            break;
    }

    return status;
}

public FieldStaus GetSlaveIconNodeFieldStaus(string key)
{
    FieldStaus status = FieldStaus.NotSet;

    switch (key)
    {
        case GlobalConfiguration.SECTION_GLOBAL:
            status = FieldStaus.Disable_Global;
            break;

        case InputsConfiguration.SECTION_INPUTS:
            status = FieldStaus.Inactive_Inputs;
            break;

        case OutputsConfiguration.SECTION_OUTPUTS:
            status = FieldStaus.Disable_Outputs;
            break;

        case ErrorLogConfiguration.SECTION_ERRORLOG:
            status = FieldStaus.Disable_ErrorLog;
            break;

        case RecordCacheConfiguration.SECTION_RECORD_CACHE:
            status = FieldStaus.Disable_RecordCache;
            break;
    }

    return status;
}

public FieldStaus GetSlaveFieldStaus(string key)
{
    FieldStaus status = FieldStaus.NotSet;

    switch (key)
    {
        case ErrorLogConfiguration.KEY_SYSTEM:
        case ErrorLogConfiguration.KEY_RECORDSTORE:
        case ErrorLogConfiguration.KEY_INPUTS:
        case ErrorLogConfiguration.KEY_OUTPUTS:
        case ErrorLogConfiguration.KEY_NAME_RESOLUTION:
        case ErrorLogConfiguration.KEY_INITIALISATION:
        case ErrorLogConfiguration.KEY_MARKET_FEED_LOGGING:
        case RecordCacheConfiguration.KEY_SIZE:
        case RecordCacheConfiguration.KEY_RE_REQUEST_ENABLE:
        case RecordCacheConfiguration.KEY_RE_REQUEST_TIMEOUT:
        case RecordCacheConfiguration.KEY_FORCE_SNAPSHOT_TICKET_REQ:
            status = FieldStaus.Grey;
            break;

        case RecordMaps.SECTION_RECORDMAPS:
            status = FieldStaus.RecordMaps;
            break;

        case FieldOverrides.SECTION_FIELD_OVERRIDES:
            status = FieldStaus.FieldOverrides;
            break;

        default:
            status = FieldStaus.Green;
            break;
    }

    return status;
}

public void AddSettingData(bool isForField
                           , bool isCheckForMasterSlave
                           , string[] strArrayReadIni
                           , string[] strArrayDefKey
                           , string[] strArrayDefDisp
                           , string[] strArrayDefValofKey
                           , string section
                           , List<FieldShow> listDefFieldShow
                           , List<bool> listDefRemovable
                           , List<bool> listDefSpecialField
                           , iniFileManager iniFileMgr
                           , string serviceName
                           , ref List<SettingData> allSettingData)
{
    List<string> listAddSettingData = new List<string>();

    string key = "";
    string value = "";

    if (isForField)
    {
        #region For Field
        #region Add Setting Data with Default
        for (int i = 0; i < strArrayDefKey.Length; i++)
        {
            bool isFound = false;

            for (int j = 0; j < strArrayReadIni.Length; j++)
            {
                iniFileMgr.SplitKeyAndValue(strArrayReadIni[j], ref key, ref value);

                if (strArrayDefKey[i].Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    isFound = true;

                    break;
                }
            }

            if (isFound)
            {
                if (value.Length > 0)
                {
                    AddSettingData(isForField
                                    , isCheckForMasterSlave
                                    , key
                                    , value
                                    , strArrayDefDisp[i]
                                    , strArrayDefValofKey[i]
                                    , section
                                    , FieldStaus.Green
                                    , listDefFieldShow[i]
                                    , listDefRemovable[i]
                                    , listDefSpecialField[i]
                                    , serviceName
                                    , ref allSettingData);
                }
                else
                {
                    value = strArrayDefValofKey[i];

                    AddSettingData(isForField
                                    , isCheckForMasterSlave
                                    , key
                                    , value
                                    , strArrayDefDisp[i]
                                    , strArrayDefValofKey[i]
                                    , section
                                    , FieldStaus.Red
                                    , listDefFieldShow[i]
                                    , listDefRemovable[i]
                                    , listDefSpecialField[i]
                                    , serviceName
                                    , ref allSettingData);
                }
            }
            else
            {
                key = strArrayDefKey[i];
                value = strArrayDefValofKey[i];

                AddSettingData(isForField
                                , isCheckForMasterSlave
                                , key
                                , value
                                , strArrayDefDisp[i]
                                , strArrayDefValofKey[i]
                                , section
                                , FieldStaus.Red
                                , listDefFieldShow[i]
                                , listDefRemovable[i]
                                , listDefSpecialField[i]
                                , serviceName
                                , ref allSettingData);
            }

            listAddSettingData.Add(key);
        }
        #endregion

        #region Add Setting Data with Custom
        for (int i = 0; i < strArrayReadIni.Length; i++)
        {
            bool isFound = false;
            key = "";
            value = "";

            iniFileMgr.SplitKeyAndValue(strArrayReadIni[i], ref key, ref value);

            for (int j = 0; j < listAddSettingData.Count; j++)
            {
                if (key == listAddSettingData[j])
                {
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
            {
                FieldStaus status = FieldStaus.NotSet;

                if (IsSlaveInstance(serviceName))
                {
                    status = FieldStaus.Grey;
                }
                else
                {
                    status = FieldStaus.Yellow;

                    if (value.Trim().Length == 0)
                    {
                        status = FieldStaus.Pink;
                    }
                }

                AddSettingData(isForField
                                , isCheckForMasterSlave
                                , key
                                , value.Trim()
                                , key
                                , ""
                                , section
                                , status
                                , FieldShow.Both
                                , true
                                , false
                                , serviceName
                                , ref allSettingData);
            }
        }
        #endregion
        #endregion
    }
    else
    {
        #region For Node Tree View
        for (int i = 0; i < strArrayReadIni.Length; i++)
        {
            key = strArrayReadIni[i];

            AddSettingData(isForField
                            , isCheckForMasterSlave
                            , key
                            , value
                            , strArrayDefDisp[i]
                            , strArrayDefValofKey[i]
                            , section
                            , FieldStaus.NotSet
                            , FieldShow.Both
                            , true
                            , false
                            , serviceName
                            , ref allSettingData);
        }
        #endregion
    }
}

public void AddSettingData(bool isForField
                           , bool isCheckForMasterSlave
                           , string key
                           , string value
                           , string display
                           , string defaultValue
                           , string section
                           , FieldStaus status
                           , FieldShow show
                           , bool removable
                           , bool specialField
                           , string serviceName
                           , ref List<SettingData> allSettingData)
{
    if (key == KEY_MMC_TYPE)
    {
        return;
    }

    SettingData settingData = null;

    if (isCheckForMasterSlave)
    {
        #region Check for Master-Slave
        if (IsEnableMasterSlave())
        {
            if (serviceName == GetMasterInstanceServiceName())
            {
                #region Master Instance
                FieldStaus prevStatus = status;

                if (section != ErrorLogConfiguration.SECTION_ERRORLOG)
                {
                    status = GetNormalIconNodeFieldStaus(key);

                    if (status == FieldStaus.NotSet)
                    {
                        status = prevStatus;
                    }
                }

                settingData = new SettingData(key, value, display, defaultValue, section, status, show, removable, specialField);
                #endregion
            }
            else
            {
                if (IsSlaveInstance(serviceName))
                {
                    #region Slave Instance
                    if (status != FieldStaus.Grey)
                    {
                        status = FieldStaus.NotSet;

                        if (isForField)
                        {
                            status = GetSlaveFieldStaus(key);
                        }
                        else
                        {
                            status = GetSlaveIconNodeFieldStaus(key);
                        }
                    }

                    if (status != FieldStaus.NotSet)
                    {
                        settingData = new SettingData(key, value, display, defaultValue, section, status, show, removable, specialField);
                    }
                    #endregion
                }
                else
                {
                    if (isForField)
                    {
                        #region Normal Instance
                        settingData = new SettingData(key, value, display, defaultValue, section, status, show, removable, specialField);
                        #endregion
                    }
                    else
                    {
                        #region for Treeview Node
                        status = GetNormalIconNodeFieldStaus(key);
                        settingData = new SettingData(key, value, display, defaultValue, section, status, show, removable, specialField);
                        #endregion
                    }
                }
            }
        }
        else
        {
            if (isForField)
            {
                #region Normal Instance
                settingData = new SettingData(key, value, display, defaultValue, section, status, show, removable, specialField);
                #endregion
            }
            else
            {
                #region for Treeview Node
                status = GetNormalIconNodeFieldStaus(key);
                settingData = new SettingData(key, value, display, defaultValue, section, status, show, removable, specialField);
                #endregion
            }
        }
        #endregion
    }
    else
    {
        #region Normal Instance
        settingData = new SettingData(key, value, display, defaultValue, section, status, show, removable, specialField);
        #endregion
    }

    if (settingData != null)
        allSettingData.Add(settingData);
}

public string GetListViewStatus(FieldStaus status)
{
    switch (status)
    {
        case FieldStaus.Yellow:
            return FIELD_STATUS_CUSTOM;

        case FieldStaus.Pink:
            return FIELD_STATUS_CUSTOM_NO_VALUE;

        case FieldStaus.Red:
            return FIELD_STATUS_MISSING_REQUIRED;

        case FieldStaus.Red_Duplicate:
            return FIELD_STATUS_DUPLICATE;

        case FieldStaus.Grey:
            return FIELD_STATUS_DISABLED;

        default:
            return "";
    }
}

public void SetFieldStausToMissingRequired(string key, string value, ref FieldStaus status)
{
    switch (key)
    {
        case RecordCacheConfiguration.KEY_SIZE:
            {
                if (value.Trim().Length == 0)
                {
                    status = FieldStaus.Red;
                }
            }
            break;
    }
}

public SettingData GetSettingData(bool isForField, string serviceName, string key, string value, string display, string defaultValue, string section, SettingDataFieldProperty settingProperty)
{
    SettingData settingData = null;

    if (IsEnableMasterSlave())
    {
        if (serviceName == GetMasterInstanceServiceName())
        {
            #region Master Instance
            settingData = new SettingData(key, value, display, defaultValue, section, settingProperty.m_status, settingProperty.m_show, settingProperty.m_removable, false);
            #endregion
        }
        else
        {
            if (IsSlaveInstance(serviceName))
            {
                #region Slave Instance
                FieldStaus status = FieldStaus.NotSet;

                if (isForField)
                {
                    status = GetSlaveFieldStaus(key);
                }
                else
                {
                    status = GetSlaveIconNodeFieldStaus(key);
                }

                if (status != FieldStaus.NotSet)
                {
                    settingData = new SettingData(key, value, display, defaultValue, section, status, settingProperty.m_show, settingProperty.m_removable, false);
                }
                #endregion
            }
            else
            {
                #region Normal Instance
                settingData = new SettingData(key, value, display, defaultValue, section, settingProperty.m_status, settingProperty.m_show, settingProperty.m_removable, false);
                #endregion
            }
        }
    }
    else
    {
        #region Normal Instance
        settingData = new SettingData(key, value, display, defaultValue, section, settingProperty.m_status, settingProperty.m_show, settingProperty.m_removable, false);
        #endregion
    }

    return settingData;
}

public void DoDisableService(int instanceNo)
{
    if ((instanceNo < INSTATNCE_NUMBER_MIN) || (instanceNo > INSTATNCE_NUMBER_MAX))
    {
        return;
    }

    int totalCurrentInstance = ProductKeyManager.Instance.GetTotalCurrentInstances();
    if (instanceNo <= totalCurrentInstance)
    {
        var svc = new ServiceController(GetServiceNameFromInstanceNo(instanceNo));
        ServiceHelper.ChangeStartMode(svc, ServiceStartMode.Disabled);
    }
}

public void DoEnableAllInstanceAutoStartServices(List<int> listSlaveInstanceNo)
{
    for (int index = 0; index < listSlaveInstanceNo.Count; index++)
    {
        DoEnableServiceToAutoStart(listSlaveInstanceNo[index]);
    }
}

public void DoEnableServiceToAutoStart(int instanceNo)
{
    if ((instanceNo < INSTATNCE_NUMBER_MIN) || (instanceNo > INSTATNCE_NUMBER_MAX))
    {
        return;
    }

    int totalCurrentInstance = ProductKeyManager.Instance.GetTotalCurrentInstances();
    if (instanceNo <= totalCurrentInstance)
    {
        var svc = new ServiceController(GetServiceNameFromInstanceNo(instanceNo));
        ServiceHelper.ChangeStartMode(svc, ServiceStartMode.Automatic);
    }
}

public void SetMasterSlaveOption(bool isEnableMasterSlave, iniFileManager iniFileMgr, bool mustReloadMasterSlaveConfigure)
{
    int iSlaveInstance1 = 0;
    int.TryParse(iniFileMgr.IniReadValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_1), out iSlaveInstance1);

    int iSlaveInstance2 = 0;
    int.TryParse(iniFileMgr.IniReadValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_2), out iSlaveInstance2);

    int iSlaveInstance3 = 0;
    int.TryParse(iniFileMgr.IniReadValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_3), out iSlaveInstance3);

    int iSlaveInstance4 = 0;
    int.TryParse(iniFileMgr.IniReadValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_4), out iSlaveInstance4);

    if (isEnableMasterSlave)
    {
        if (iSlaveInstance1 > 0)
        {
            DoDisableService(iSlaveInstance1);
        }

        if (iSlaveInstance2 > 0)
        {
            DoDisableService(iSlaveInstance2);
        }

        if (iSlaveInstance3 > 0)
        {
            DoDisableService(iSlaveInstance3);
        }

        if (iSlaveInstance4 > 0)
        {
            DoDisableService(iSlaveInstance4);
        }

        iniFileMgr.IniWriteValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_ENABLE_MASTER_SLAVE, "TRUE");
    }
    else
    {
        if (iSlaveInstance1 > 0)
        {
            DoEnableServiceToAutoStart(iSlaveInstance1);
        }

        if (iSlaveInstance2 > 0)
        {
            DoEnableServiceToAutoStart(iSlaveInstance2);
        }

        if (iSlaveInstance3 > 0)
        {
            DoEnableServiceToAutoStart(iSlaveInstance3);
        }

        if (iSlaveInstance4 > 0)
        {
            DoEnableServiceToAutoStart(iSlaveInstance4);
        }

        iniFileMgr.IniWriteValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_ENABLE_MASTER_SLAVE, "FALSE");

        iniFileMgr.IniWriteValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_1, "0");
        iniFileMgr.IniWriteValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_2, "0");
        iniFileMgr.IniWriteValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_3, "0");
        iniFileMgr.IniWriteValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_4, "0");

        UpdateMasterInstanceOutputAccessAllowedListAfterTurnOffMasterSlaveOption();
    }
}

private bool IsValidateSiteProductKeyForCanSettingMasterSlaveOption(_PROCESS_TYPES enProcessType, iniFileManager iniFileMgr)
{
    string errCaption = string.Format("{0} Manager", GetProductFullName());
    string errMsg = "";

    //check this in case the site product key can setting Master-Slave Option
    if (ProductKeyManager.Instance.GetMaxTofOrDtfInputs() < PERMITTED_TOF_DTF_FOR_2INSTANCES)
    {
        if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
        {
            errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());
        }
        else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_UPDATE_LICENSE_KEY)
        {
            errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_SITEPRODUCTKEY, GetProductFullName());
            errMsg += string.Format(ERRMSG_MASTERSLAVE_TURNOFF_MASTERSLAVE_OPT, GetProductFullName());
        }
        else
        {
            errCaption = MSG_HEADER_MASTERSLAVE;
            errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_SITEPRODUCTKEY, GetProductFullName());
        }

        //To set disable Master-Slave option
        SetMasterSlaveOption(false, iniFileMgr, true);

        //MessageBox.Show(errMsg, errCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    return true;
}

public int GetTotalEnableSlaveCombobox(int totalActiveInstance)
{
    //Check enable/disable slave combobox
    switch (totalActiveInstance)
    {
        case 1:
            return (int)_ENABLED_SLAVE_COMBOBOX.ENABLED_SLAVE_COMBOBOX_0;

        case 2:
            return (int)_ENABLED_SLAVE_COMBOBOX.ENABLED_SLAVE_COMBOBOX_1;

        case 3:
            return (int)_ENABLED_SLAVE_COMBOBOX.ENABLED_SLAVE_COMBOBOX_2;

        case 4:
            return (int)_ENABLED_SLAVE_COMBOBOX.ENABLED_SLAVE_COMBOBOX_3;

        default:
            return (int)_ENABLED_SLAVE_COMBOBOX.ENABLED_SLAVE_COMBOBOX_4;
    }
}

public bool IsDuplicateSlaveInstance(List<string> listSlaveServiceDisplayName, ref int iDuplicateSlaveInstanceCombobox1, ref int iDuplicateSlaveInstanceCombobox2, ref int iDuplicateSlaveInstance)
{
    bool isDuplicate = false;
    int iSlaveDup1 = 0;
    int iSlaveDup2 = 0;

    for (int i = 0; i < listSlaveServiceDisplayName.Count; i++)
    {
        int iTotalCount = -1;
        iSlaveDup2 = 0;
        iSlaveDup1++;

        for (int j = 0; j < listSlaveServiceDisplayName.Count; j++)
        {
            iSlaveDup2++;
            if (listSlaveServiceDisplayName[i].Equals(listSlaveServiceDisplayName[j]))
            {
                //ignore in case 0 (None)
                if (!listSlaveServiceDisplayName[i].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
                {
                    iTotalCount++;
                }
            }

            if (iTotalCount > 0)
            {
                iDuplicateSlaveInstanceCombobox1 = iSlaveDup1;
                iDuplicateSlaveInstanceCombobox2 = iSlaveDup2;

                string temp = listSlaveServiceDisplayName[i];
                temp = temp.Substring(GetProductFullName().Length, temp.Length - GetProductFullName().Length);

                iDuplicateSlaveInstance = 0;
                int.TryParse(temp, out iDuplicateSlaveInstance);

                break;
            }

        }

        if (iTotalCount > 0)
        {
            isDuplicate = true;
            break;
        }
    }

    return isDuplicate;
}

public bool IsAllSlaveIsService(List<string> listSlaveServiceDisplayName, ref int iInvalidSlaveInstance, ref string errInstanceFullName)
{
    for (int i = 0; i < listSlaveServiceDisplayName.Count; i++)
    {
        //ignore in case 0 (None)
        if (!listSlaveServiceDisplayName[i].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
        {
            string serviceName = GetServiceNameFromServiceDisplayName(listSlaveServiceDisplayName[i]);

            if (!IsServiceExists(serviceName))
            {
                iInvalidSlaveInstance = GetInstanceNoFromServiceName(serviceName);
                errInstanceFullName = listSlaveServiceDisplayName[i];
                return false;
            }
        }
    }

    return true;
}

public void SetDictAllInputForVerifyData(string serviceDisplayName, ref Dictionary<string, List<TcidInputNode>> dictAllInputForVerifyData)
{
    List<TcidInputNode> listTcidInputNode = new List<TcidInputNode>();

    string serviceName = GetServiceNameFromServiceDisplayName(serviceDisplayName);
    ServiceConfiguration service = GetServiceNode(serviceName);

    if (service != null)
    {
        List<SettingData> listSettingDataTcidInput = service.m_inputsNode.GetAllSettingData();

        TcidInputNode tcidInputNode = null;

        for (int tcidInputNodeIndex = 0; tcidInputNodeIndex < listSettingDataTcidInput.Count; tcidInputNodeIndex++)
        {
            tcidInputNode = service.m_inputsNode.GetTcidInputNode(listSettingDataTcidInput[tcidInputNodeIndex].m_key);
            listTcidInputNode.Add(tcidInputNode);
        }

        dictAllInputForVerifyData.Add(serviceDisplayName, listTcidInputNode);
    }
}

public void SetDictAllOutputForVerifyData(string serviceDisplayName, ref Dictionary<string, List<Output>> dictAllOutputForVerifyData)
{
    string serviceName = GetServiceNameFromServiceDisplayName(serviceDisplayName);
    ServiceConfiguration service = GetServiceNode(serviceName);

    if (service != null)
    {
        List<Output> allOutput = service.m_outputsNode.GetAllOutput();

        if (!dictAllOutputForVerifyData.ContainsKey(serviceDisplayName))
        {
            dictAllOutputForVerifyData.Add(serviceDisplayName, allOutput);
        }
    }
}

public bool HaveOutput(string serviceDisplayName, Dictionary<string, List<Output>> dictAllOutputForVerifyData)
{
    if (dictAllOutputForVerifyData.ContainsKey(serviceDisplayName))
    {
        List<Output> allOutput = dictAllOutputForVerifyData[serviceDisplayName];

        if (allOutput.Count > 0)
        {
            return true;
        }
    }

    return false;
}

public bool IsAllSlaveInstanceHaveOutput(_PROCESS_TYPES enProcessType, List<string> listSlaveServiceDisplayName, Dictionary<string, List<Output>> dictAllOutputForVerifyData)
{
    string errMsgCaption = string.Format("{0} Manager", GetProductFullName());
    string errMsg = "";
    string adviceErrMsg = "";
    string invalidInstanceErrMsg = "";
    string serviceDiaplayName = "";

    for (int instanceIndex = 0; instanceIndex < listSlaveServiceDisplayName.Count; instanceIndex++)
    {
        serviceDiaplayName = listSlaveServiceDisplayName[instanceIndex];

        //ignore in case 0 (None)
        if (!serviceDiaplayName.Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
        {
            if (HaveOutput(serviceDiaplayName, dictAllOutputForVerifyData))
            {
                switch (enProcessType)
                {
                    case _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_LOAD:
                        {
                            errMsgCaption = MSG_HEADER_MASTERSLAVE;
                            adviceErrMsg = string.Format("\n{0} will set to turn off Master-Slave option.", GetProductFullName());
                        }
                        break;

                    case _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_CONFRIM:
                        {
                            errMsgCaption = MSG_HEADER_MASTERSLAVE;
                            adviceErrMsg = "\n\nPlease remove all slave output or choose other slave instance.";
                        }
                        break;

                    case _PROCESS_TYPES.PROCESS_TYPE_STARTSERVICE:
                        {
                            errMsgCaption = MSG_HEADER_START_SERVICE;
                            adviceErrMsg = ERRMSG_MASTERSLAVE_PLEASE_RECHECK_SERVICE;
                        }
                        break;
                }

                if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
                {
                    errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());
                }
                else if ((enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_CONFRIM) || (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_STARTSERVICE))
                {
                    int iSlaveInstanceIndex = 1;
                    for (int j = 0; j < listSlaveServiceDisplayName.Count; j++)
                    {
                        if (serviceDiaplayName.Equals(listSlaveServiceDisplayName[j]))
                        {
                            break;
                        }

                        iSlaveInstanceIndex++;
                    }

                    invalidInstanceErrMsg = string.Format("[SlaveInstance{0} is {1}]. {2}", iSlaveInstanceIndex, serviceDiaplayName, adviceErrMsg);
                    errMsg = string.Format(ERRMSG_MASTERSLAVE_SLAVE_MUST_NOT_HAVE_OUTPUT, invalidInstanceErrMsg);
                }

                //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
    }

    return false;
}

bool IsAllSlaveInstanceStopService(_PROCESS_TYPES enProcessType, List<string> listSlaveServiceDisplayName)
{
    string errMsgCaption = string.Format("{0} Manager", GetProductFullName());
    string errMsg = "";
    string serviceDiaplayName = "";

    for (int instanceIndex = 0; instanceIndex < listSlaveServiceDisplayName.Count; instanceIndex++)
    {
        serviceDiaplayName = listSlaveServiceDisplayName[instanceIndex];

        //ignore in case 0 (None)
        if (!serviceDiaplayName.Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
        {
            string serviceName = GetServiceNameFromServiceDisplayName(serviceDiaplayName);

            if (IsServiceRunning(serviceName))
            {
                if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
                {
                    errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());
                }
                else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_LOAD || enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_CONFRIM)
                {
                    errMsgCaption = MSG_HEADER_MASTERSLAVE;

                    errMsg = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_SLAVESERVICERUN_SETTING, GetProductFullName(), serviceDiaplayName, serviceDiaplayName);
                }
                else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_STARTSERVICE)
                {
                    errMsgCaption = MSG_HEADER_START_SERVICE;

                    errMsg = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_SLAVESERVICERUN_SERVICE, GetProductFullName(), serviceDiaplayName, serviceDiaplayName);
                }

                //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    return true;
}

public bool HaveDTAppOutput(string setviceDisplayName, Dictionary<string, List<Output>> dictAllOutputForVerifyData)
{
    if (dictAllOutputForVerifyData.ContainsKey(setviceDisplayName))
    {
        List<Output> allOutput = dictAllOutputForVerifyData[setviceDisplayName];

        for (int i = 0; i < allOutput.Count; i++)
        {
            Output output = allOutput[i];

            if (output.m_outputType == OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_DTAPP)
            {
                return true;
            }
        }
    }

    return false;
}

public bool TurnOffMasterSlaveOption()
{
    string instance1 = GetIniFilePath(INSTATNCE_NUMBER_MIN);
    iniFileManager iniFileMgr = new iniFileManager(instance1);

    string enableMasterSlave = iniFileMgr.IniReadValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_ENABLE_MASTER_SLAVE);

    if (enableMasterSlave.Equals("TRUE", StringComparison.CurrentCultureIgnoreCase))
    {
        iniFileMgr.IniWriteValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_ENABLE_MASTER_SLAVE, "FALSE");
        iniFileMgr.IniWriteValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_1, "0");
        iniFileMgr.IniWriteValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_2, "0");
        iniFileMgr.IniWriteValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_3, "0");
        iniFileMgr.IniWriteValue(MasterSlaveConfiguration.SECTION_MASTER_SLAVE, MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_4, "0");

        UpdateMasterInstanceOutputAccessAllowedListAfterTurnOffMasterSlaveOption();
        return true;
    }

    return false;
}

public void UpdateMasterInstanceOutputAccessAllowedListAfterTurnOffMasterSlaveOption()
{
    string serviceName = GetServiceNameFromInstanceNo(INSTATNCE_NUMBER_MIN);
    ServiceConfiguration service = GetServiceNode(serviceName);

    if (service != null)
    {
        iniFileManager iniFileMgr = service.GetIniFileManager();

        List<TcidInputNode> listTcidInputNode = new List<TcidInputNode>();
        List<SettingData> listSettingDataTcidInput = service.m_inputsNode.GetAllSettingData();

        TcidInputNode tcidInputNode = null;

        for (int tcidInputNodeIndex = 0; tcidInputNodeIndex < listSettingDataTcidInput.Count; tcidInputNodeIndex++)
        {
            tcidInputNode = service.m_inputsNode.GetTcidInputNode(listSettingDataTcidInput[tcidInputNodeIndex].m_key);
            listTcidInputNode.Add(tcidInputNode);
        }

        List<Output> allOutput = service.m_outputsNode.GetAllOutput();

        for (int i = 0; i < allOutput.Count; i++)
        {
            Output output = allOutput[i];

            switch (output.m_outputType)
            {
                case OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_SOCKET:
                    {
                        switch (output.m_accessAllowList)
                        {
                            case "ALL":
                            case "ALL_TOF":
                            case "ALL_DTF":
                                break;

                            default:
                                {
                                    if (output.m_accessAllowList.Length == 0)
                                    {
                                        iniFileMgr.IniWriteValue(output.m_section, Output.KEY_ACCESS_ALLOWED_LIST, "ALL");
                                    }
                                    else
                                    {
                                        bool isFound = false;
                                        var arrOption = output.m_accessAllowList.Split(',');

                                        for (int j = 0; j < arrOption.Length; j++)
                                        {
                                            string tcid = "";

                                            int index = arrOption[j].IndexOf("_");
                                            if (index != -1)
                                            {
                                                tcid = arrOption[j].Substring(0, index);

                                                for (int k = 0; k < listTcidInputNode.Count; k++)
                                                {
                                                    tcidInputNode = listTcidInputNode[k];

                                                    if (tcidInputNode.m_inputNodeName.Equals(tcid))
                                                    {
                                                        isFound = true;
                                                    }
                                                }
                                            }
                                        }

                                        if (!isFound)
                                        {
                                            iniFileMgr.IniWriteValue(output.m_section, Output.KEY_ACCESS_ALLOWED_LIST, "ALL");
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }
        }
    }
}

public void DoValidateMasterSlaveOption()
{
    if (TurnOffMasterSlaveOption())
    {
        string errMsgCaption = string.Format("{0} Manager", GetProductFullName());
        string errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());
        //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

public bool IsValidateConfigureForMasterSlaveOption(_PROCESS_TYPES enProcessType, List<string> listSlaveServiceDisplayName, iniFileManager iniFileMgr)
{
    bool isEnableMasterSlave = false;

    if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_CONFRIM)
    {
        isEnableMasterSlave = true;
    }
    else
    {
        isEnableMasterSlave = IsEnableMasterSlave();
    }

    if (isEnableMasterSlave)
    {
        //Validate configure for Master-Slave option rules:
        //1.Validate Site Product Key for can setting Master-Slave Option
        //2.Check AllowInstance must not equal INSTANCE_MASTER
        //3.Must be Slave instance least one in Master-Slave Mode
        //4.TotalActiveInstance must be more than 1 instance
        //5.Check Slave instance configure
        //6.All Slave Configure not be Master instance	
        //7.Check boundary of slave instance
        //8.Check duplicate slave instance
        //9.All Slave must be service
        //10.Verify Input config of all instance must have not duplicate TCID
        //11.Verify Input config of all instance must have not duplicate Alias name
        //12.Verify Input config of all instance must have not duplicate between Alias name and TCID
        //13.Check Slave instance does not have output
        //14.Verify all Slave Instance must be stop service
        //15.Not Allow DTApplication outputs in Master-Slave Mode

        string errMsgCaption = string.Format("{0} Manager", GetProductFullName());
        string errMsg = "";

        #region 1.Validate Site Product Key for can setting Master-Slave Option
        if (!IsValidateSiteProductKeyForCanSettingMasterSlaveOption(enProcessType, iniFileMgr))
        {
            return false;
        }
        #endregion

        #region  2.Check AllowInstance must not equal INSTANCE_MASTER
        int iAllowInstance = ProductKeyManager.Instance.GetMaxInstances();

        if (iAllowInstance == INSTATNCE_NUMBER_MIN)
        {
            if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
            {
                errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_UPDATE_LICENSE_KEY)
            {
                errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());
                errMsg += string.Format(ERRMSG_MASTERSLAVE_TURNOFF_MASTERSLAVE_OPT, GetProductFullName());
            }

            //To set disable Master-Slave option
            SetMasterSlaveOption(false, iniFileMgr, true);

            //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        #endregion

        #region 3.Must be Slave instance least one in Master-Slave Mode
        bool haveSlaveInstance = false;

        for (int i = 0; i < listSlaveServiceDisplayName.Count; i++)
        {
            if (!listSlaveServiceDisplayName[i].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
            {
                haveSlaveInstance = true;
                break;
            }
        }

        if (!haveSlaveInstance)
        {
            if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
            {
                errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());

                //To set disable Master-Slave option
                SetMasterSlaveOption(false, iniFileMgr, true);
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_STARTSERVICE)
            {
                errMsgCaption = MSG_HEADER_START_SERVICE;

                string msg1 = string.Format(ERRMSG_MASTERSLAVE_MUSTHAVE_SLAVE_LEAST_ONE, GetProductFullName());

                errMsg = string.Format("{0}{1}", msg1, ERRMSG_MASTERSLAVE_PLEASE_RECHECK_SERVICE);
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_ADDREMOVE_INSTANCE)
            {
                errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());
            }
            else if ((enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_LOAD) || (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_CONFRIM))
            {
                errMsgCaption = MSG_HEADER_MASTERSLAVE;

                errMsg = string.Format(ERRMSG_MASTERSLAVE_MUSTHAVE_SLAVE_LEAST_ONE, GetProductFullName());
            }

            //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        #endregion

        #region 4.TotalActiveInstance must be more than 1 instance
        //Get Total active instance
        int iTotalActiveInstance = ProductKeyManager.Instance.GetTotalCurrentInstances();

        if (iTotalActiveInstance <= INSTATNCE_NUMBER_MIN)
        {
            if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
            {
                errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());

                //To set disable Master-Slave option
                SetMasterSlaveOption(false, iniFileMgr, true);
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_STARTSERVICE)
            {
                errMsgCaption = MSG_HEADER_START_SERVICE;
                errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());
            }
            else
            {
                errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());
            }

            //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        #endregion

        #region 5.Check Slave instance configure
        bool bIsValidSlaveConfigure = true;
        int iTotalEnableSlaveCombobox = GetTotalEnableSlaveCombobox(iTotalActiveInstance);
        if (iTotalEnableSlaveCombobox == (int)_ENABLED_SLAVE_COMBOBOX.ENABLED_SLAVE_COMBOBOX_1)
        {
            if ((!listSlaveServiceDisplayName[1].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
                || (!listSlaveServiceDisplayName[2].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
                || (!listSlaveServiceDisplayName[3].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE)))
            {
                bIsValidSlaveConfigure = false;
            }
        }
        else if (iTotalEnableSlaveCombobox == (int)_ENABLED_SLAVE_COMBOBOX.ENABLED_SLAVE_COMBOBOX_2)
        {
            if ((!listSlaveServiceDisplayName[2].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
                || (!listSlaveServiceDisplayName[3].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE)))
            {
                bIsValidSlaveConfigure = false;
            }
        }
        else if (iTotalEnableSlaveCombobox == (int)_ENABLED_SLAVE_COMBOBOX.ENABLED_SLAVE_COMBOBOX_3)
        {
            if (!listSlaveServiceDisplayName[3].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
            {
                bIsValidSlaveConfigure = false;
            }
        }

        if (!bIsValidSlaveConfigure)
        {
            if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
            {
                errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_UPDATE_LICENSE_KEY)
            {
                errMsgCaption = string.Format("{0} Manager", GetProductFullName());

                errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());
                errMsg += string.Format(ERRMSG_MASTERSLAVE_TURNOFF_MASTERSLAVE_OPT, GetProductFullName());
            }

            //To set disable Master-Slave option
            SetMasterSlaveOption(false, iniFileMgr, true);

            //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        #endregion

        int iSlaveInstanceIndex = INSTATNCE_NUMBER_MIN;

        #region 6.All Slave Configure not be Master instance
        for (int i = 0; i < listSlaveServiceDisplayName.Count; i++)
        {
            if (listSlaveServiceDisplayName[i].Equals(GetServiceDisplayNameFromInstanceNo(INSTATNCE_NUMBER_MIN)))
            {
                if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
                {
                    errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());

                    //To set disable Master-Slave option
                    SetMasterSlaveOption(false, iniFileMgr, true);
                }
                else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_LOAD)
                {
                    errMsgCaption = MSG_HEADER_MASTERSLAVE;
                    errMsg = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_MASTER_IS_SLAVE_SETTINGDLG, GetProductFullName(), iSlaveInstanceIndex.ToString(), GetServiceDisplayNameFromInstanceNo(INSTATNCE_NUMBER_MIN), GetProductFullName());
                }

                else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_ADDREMOVE_INSTANCE)
                {
                    string notAllMasterIsSlave = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_MASTER_IS_SLAVE, GetProductFullName(), iSlaveInstanceIndex.ToString(), GetServiceDisplayNameFromInstanceNo(INSTATNCE_NUMBER_MIN));
                    string plsRecheck = string.Format(ERRMSG_MASTERSLAVE_PLEASE_RECHECK_ADDREMOVEINTS, GetProductFullName());
                    errMsg = notAllMasterIsSlave + plsRecheck;
                }

                //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            iSlaveInstanceIndex++;
        }
        #endregion

        #region 7.Check boundary of slave instance
        iSlaveInstanceIndex = SLAVE_INSTATNCE_NUMBER_MIN;

        for (int i = 0; i < listSlaveServiceDisplayName.Count; i++)
        {
            //ignore in case 0 (None)
            if (!listSlaveServiceDisplayName[i].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
            {
                int instanceNo = GetInstanceNoFromServiceDisplayName(listSlaveServiceDisplayName[i]);

                if ((instanceNo <= INSTATNCE_NUMBER_MIN) || (instanceNo > iTotalActiveInstance))
                {
                    if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
                    {
                        errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());

                        //To set disable Master-Slave option
                        SetMasterSlaveOption(false, iniFileMgr, true);
                    }
                    else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_ADDREMOVE_INSTANCE)
                    {
                        string invaildConfig = string.Format(ERRMSG_MASTERSLAVE_INVALID_SLAVE_INI, iSlaveInstanceIndex.ToString(), GetServiceDisplayNameFromInstanceNo(instanceNo));
                        errMsg = invaildConfig + string.Format(ERRMSG_MASTERSLAVE_PLEASE_RECHECK_ADDREMOVEINTS, GetProductFullName());
                    }
                    else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_STARTSERVICE)
                    {
                        errMsgCaption = MSG_HEADER_START_SERVICE;
                        string invaildConfig = string.Format(ERRMSG_MASTERSLAVE_INVALID_SLAVE_INI, iSlaveInstanceIndex, GetServiceDisplayNameFromInstanceNo(instanceNo));
                        errMsg = invaildConfig + ERRMSG_MASTERSLAVE_PLEASE_RECHECK_SERVICE;
                    }
                    else if ((enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_LOAD) || (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_CONFRIM))
                    {
                        errMsgCaption = MSG_HEADER_MASTERSLAVE;
                        errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_SLAVE_BOUNDARY_SETTINGDLG, iSlaveInstanceIndex, GetServiceDisplayNameFromInstanceNo(instanceNo), GetProductFullName());
                    }

                    //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            iSlaveInstanceIndex++;
        }
        #endregion

        #region 8.Check duplicate slave instance
        int iDuplicateSlaveInstanceValue1 = 0;
        int iDuplicateSlaveInstanceValue2 = 0;
        int iDuplicateSlaveInstance = 0;

        if (IsDuplicateSlaveInstance(listSlaveServiceDisplayName, ref iDuplicateSlaveInstanceValue1, ref iDuplicateSlaveInstanceValue2, ref iDuplicateSlaveInstance))
        {
            if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
            {
                errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());

                //To set disable Master-Slave option
                SetMasterSlaveOption(false, iniFileMgr, true);
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_LOAD)
            {
                errMsgCaption = MSG_HEADER_MASTERSLAVE;
                errMsg = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_INSTANCE_SETTINGDLG, GetProductFullName(), iDuplicateSlaveInstanceValue1.ToString(), iDuplicateSlaveInstanceValue2.ToString(), GetServiceDisplayNameFromInstanceNo(iDuplicateSlaveInstance), GetProductFullName());
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_CONFRIM)
            {
                errMsgCaption = MSG_HEADER_MASTERSLAVE;
                errMsg = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_INSTANCE, GetProductFullName(), iDuplicateSlaveInstanceValue1.ToString(), iDuplicateSlaveInstanceValue2.ToString(), GetServiceDisplayNameFromInstanceNo(iDuplicateSlaveInstance));
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_ADDREMOVE_INSTANCE)
            {
                string strInvaildConfig = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_INSTANCE, GetProductFullName(), iDuplicateSlaveInstanceValue1.ToString(), iDuplicateSlaveInstanceValue2.ToString(), GetServiceDisplayNameFromInstanceNo(iDuplicateSlaveInstance));
                errMsg = strInvaildConfig + string.Format(ERRMSG_MASTERSLAVE_PLEASE_RECHECK_ADDREMOVEINTS, GetProductFullName());
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_STARTSERVICE)
            {
                errMsgCaption = MSG_HEADER_START_SERVICE;
                errMsg = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_INSTANCE, GetProductFullName(), iDuplicateSlaveInstanceValue1.ToString(), iDuplicateSlaveInstanceValue2.ToString(), GetServiceDisplayNameFromInstanceNo(iDuplicateSlaveInstance));
            }

            //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        #endregion

        #region 9.All Slave must be service
        int invalidSlaveInstance = 0;
        string errInstanceFullName = "";
        if (!IsAllSlaveIsService(listSlaveServiceDisplayName, ref invalidSlaveInstance, ref errInstanceFullName))
        {
            if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
            {
                //To set disable Master-Slave option
                SetMasterSlaveOption(false, iniFileMgr, true);
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_LOAD)
            {
                errMsgCaption = MSG_HEADER_MASTERSLAVE;
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_CONFRIM)
            {
                errMsgCaption = MSG_HEADER_MASTERSLAVE;
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_ADDREMOVE_INSTANCE)
            {
                errMsgCaption = string.Format("{0} Manager", GetProductFullName());
            }
            else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_STARTSERVICE)
            {
                errMsgCaption = MSG_HEADER_START_SERVICE;

                //To set disable Master-Slave option
                SetMasterSlaveOption(false, iniFileMgr, true);
            }

            string strInvaildConfig = string.Format(ERRMSG_MASTERSLAVE_INVALID_SLAVE_INI, invalidSlaveInstance.ToString(), errInstanceFullName);
            errMsg = strInvaildConfig + string.Format(ERRMSG_MASTERSLAVE_PLEASE_RECHECK, GetProductFullName());

            //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        #endregion

        List<string> listAllMasterSlaveInstanceNo = new List<string>();

        Dictionary<string, List<TcidInputNode>> dictAllInputForVerifyData = new Dictionary<string, List<TcidInputNode>>();

        #region Prepare data for validate Input 
        #region Do Dictionary Input for Verify Data
        //For Master Instance
        SetDictAllInputForVerifyData(GetServiceDisplayNameFromInstanceNo(INSTATNCE_NUMBER_MIN), ref dictAllInputForVerifyData);

        //For Slave Instance
        for (int instanceIndex = 0; instanceIndex < listSlaveServiceDisplayName.Count; instanceIndex++)
        {
            //ignore in case 0 (None)
            if (!listSlaveServiceDisplayName[instanceIndex].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
            {
                SetDictAllInputForVerifyData(listSlaveServiceDisplayName[instanceIndex], ref dictAllInputForVerifyData);
            }
        }
        #endregion

        #region Get All Master-Slave Instance No List
        listAllMasterSlaveInstanceNo.Add(GetServiceDisplayNameFromInstanceNo(INSTATNCE_NUMBER_MIN));
        for (int i = 0; i < listSlaveServiceDisplayName.Count; i++)
        {
            //ignore in case 0 (None)
            if (!listSlaveServiceDisplayName[i].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
            {
                listAllMasterSlaveInstanceNo.Add(listSlaveServiceDisplayName[i]);
            }
        }
        #endregion
        #endregion

        #region 10.Verify Input config of all instance must have not duplicate TCID
        List<string> listTcid = new List<string>();
        Dictionary<string, int> dictTcidToInstanceNo = new Dictionary<string, int>();

        if (dictAllInputForVerifyData.Count > 0)
        {
            bool isDuplicate = false;
            int instanceNoDup1 = 0;
            int instanceNoDup2 = 0;
            string tcidDup = "";

            for (int indexInstanceNo = 0; indexInstanceNo < listAllMasterSlaveInstanceNo.Count; indexInstanceNo++)
            {
                List<TcidInputNode> listTcidInputNode = null;
                if (dictAllInputForVerifyData.ContainsKey(listAllMasterSlaveInstanceNo[indexInstanceNo]))
                {
                    listTcidInputNode = dictAllInputForVerifyData[listAllMasterSlaveInstanceNo[indexInstanceNo]];

                    for (int indexTcidInputNode = 0; indexTcidInputNode < listTcidInputNode.Count; indexTcidInputNode++)
                    {
                        TcidInputNode tcidInputNode = listTcidInputNode[indexTcidInputNode];

                        if (tcidInputNode.m_inputNodeName.Trim().Length > 0)
                        {
                            if (!dictTcidToInstanceNo.ContainsKey(tcidInputNode.m_inputNodeName))
                            {
                                listTcid.Add(tcidInputNode.m_inputNodeName);
                                dictTcidToInstanceNo.Add(tcidInputNode.m_inputNodeName, tcidInputNode.m_instanceNo);
                            }
                            else
                            {
                                //assign instanceNoDup1
                                if (dictTcidToInstanceNo.ContainsKey(tcidInputNode.m_inputNodeName))
                                {
                                    instanceNoDup1 = dictTcidToInstanceNo[tcidInputNode.m_inputNodeName];
                                }

                                if (tcidInputNode.m_instanceNo == instanceNoDup1)
                                {
                                    continue;
                                }

                                //assign instanceNoDup2
                                instanceNoDup2 = tcidInputNode.m_instanceNo;

                                //assign tcidDup
                                tcidDup = tcidInputNode.m_inputNodeName;

                                isDuplicate = true;
                                break;
                            }
                        }
                    }

                    if (isDuplicate)
                    {
                        if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
                        {
                            errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());

                            //To set disable Master-Slave option
                            SetMasterSlaveOption(false, iniFileMgr, true);
                        }
                        else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_LOAD || enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_CONFRIM)
                        {
                            errMsgCaption = MSG_HEADER_MASTERSLAVE;

                            errMsg = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_TCID_SETTING, GetProductFullName(), tcidDup, GetServiceDisplayNameFromInstanceNo(instanceNoDup1), tcidDup, GetServiceDisplayNameFromInstanceNo(instanceNoDup2), tcidDup);
                        }
                        else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_STARTSERVICE) //Validate config for Master-Slave option when start service
                        {
                            errMsgCaption = MSG_HEADER_START_SERVICE;

                            errMsg = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_TCID_SERVICE, GetProductFullName(), tcidDup, GetServiceDisplayNameFromInstanceNo(instanceNoDup1), tcidDup, GetServiceDisplayNameFromInstanceNo(instanceNoDup2), tcidDup);
                        }

                        //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
        }
        #endregion

        #region 11.Verify Input config of all instance must have not duplicate Alias name
        List<string> listAlias = new List<string>();
        Dictionary<string, int> dictAliasToInstanceNo = new Dictionary<string, int>();

        if (dictAllInputForVerifyData.Count > 0)
        {
            bool isDuplicate = false;
            int instanceNoDup1 = 0;
            int instanceNoDup2 = 0;
            string aliasDup = "";

            for (int indexInstanceNo = 0; indexInstanceNo < listAllMasterSlaveInstanceNo.Count; indexInstanceNo++)
            {
                List<TcidInputNode> listTcidInputNode = null;
                if (dictAllInputForVerifyData.ContainsKey(listAllMasterSlaveInstanceNo[indexInstanceNo]))
                {
                    listTcidInputNode = dictAllInputForVerifyData[listAllMasterSlaveInstanceNo[indexInstanceNo]];

                    for (int indexTcidInputNode = 0; indexTcidInputNode < listTcidInputNode.Count; indexTcidInputNode++)
                    {
                        TcidInputNode tcidInputNode = listTcidInputNode[indexTcidInputNode];

                        if (tcidInputNode.m_hasTofInput)
                        {
                            InputTof inputTof = (InputTof)tcidInputNode.m_tofInput;

                            if (inputTof.m_alias.Trim().Length > 0)
                            {
                                if (!dictAliasToInstanceNo.ContainsKey(inputTof.m_alias))
                                {
                                    listAlias.Add(inputTof.m_alias);
                                    dictAliasToInstanceNo.Add(inputTof.m_alias, tcidInputNode.m_instanceNo);
                                }
                                else
                                {
                                    //assign instanceNoDup1
                                    if (dictAliasToInstanceNo.ContainsKey(inputTof.m_alias))
                                    {
                                        instanceNoDup1 = dictAliasToInstanceNo[inputTof.m_alias];
                                    }

                                    if (tcidInputNode.m_instanceNo == instanceNoDup1)
                                    {
                                        continue;
                                    }

                                    //assign instanceNoDup2
                                    instanceNoDup2 = tcidInputNode.m_instanceNo;

                                    //assign aliasDup
                                    aliasDup = inputTof.m_alias;

                                    isDuplicate = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (isDuplicate)
                    {
                        if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
                        {
                            errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());

                            //To set disable Master-Slave option
                            SetMasterSlaveOption(false, iniFileMgr, true);
                        }
                        else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_LOAD || enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_CONFRIM)
                        {
                            errMsgCaption = MSG_HEADER_MASTERSLAVE;

                            errMsg = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_ALIAS_SETTING, GetProductFullName(), aliasDup, GetServiceDisplayNameFromInstanceNo(instanceNoDup1), aliasDup, GetServiceDisplayNameFromInstanceNo(instanceNoDup2), aliasDup);
                        }
                        else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_STARTSERVICE) //Validate config for Master-Slave option when start service
                        {
                            errMsgCaption = MSG_HEADER_START_SERVICE;

                            errMsg = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_ALIAS_SERVICE, GetProductFullName(), aliasDup, GetServiceDisplayNameFromInstanceNo(instanceNoDup1), aliasDup, GetServiceDisplayNameFromInstanceNo(instanceNoDup2), aliasDup);
                        }

                        //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
        }
        #endregion

        #region 12.Verify Input config of all instance must have not duplicate between Alias name and TCID
        if ((listTcid.Count != 0) || (listAlias.Count != 0))
        {
            bool isDuplicate = false;
            int instanceNoDupOfAlias = 0;
            int instanceNoDupOfTcid = 0;
            string tcidDup = "";
            string aliasDup = "";

            //Check duplicate between Alias name and TCID
            for (int i = 0; i < listAlias.Count; i++)
            {
                aliasDup = listAlias[i];

                for (int j = 0; j < listTcid.Count; j++)
                {
                    tcidDup = listTcid[j];

                    if (aliasDup.Equals(tcidDup))
                    {
                        //Assign instanceNoDupOfAlias
                        if (dictAliasToInstanceNo.ContainsKey(aliasDup))
                        {
                            instanceNoDupOfAlias = dictAliasToInstanceNo[aliasDup];
                        }

                        //Assign instanceNoDupOfTcid
                        if (dictTcidToInstanceNo.ContainsKey(tcidDup))
                        {
                            instanceNoDupOfTcid = dictTcidToInstanceNo[tcidDup];
                        }

                        if (instanceNoDupOfAlias == instanceNoDupOfTcid)
                        {
                            //Not duplicate
                            continue;
                        }
                        else
                        {
                            isDuplicate = true;
                            break;
                        }
                    }
                }

                if (isDuplicate)
                {
                    if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
                    {
                        errMsg = string.Format(ERRMSG_MASTERSLAVE_INVALID_CONFIGURE, GetProductFullName());

                        //To set disable Master-Slave option
                        SetMasterSlaveOption(false, iniFileMgr, true);
                    }
                    else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_LOAD || enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MASTERSLAVECONFIGDLG_CONFRIM)
                    {
                        errMsgCaption = MSG_HEADER_MASTERSLAVE;

                        errMsg = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_ALIASAndTCID_SETTING, GetProductFullName(), aliasDup, GetServiceDisplayNameFromInstanceNo(instanceNoDupOfAlias), tcidDup, GetServiceDisplayNameFromInstanceNo(instanceNoDupOfTcid));
                    }
                    else if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_STARTSERVICE) //Validate config for Master-Slave option when start service
                    {
                        errMsgCaption = MSG_HEADER_START_SERVICE;

                        errMsg = string.Format(ERRMSG_MASTERSLAVE_NOT_ALLOW_DUPLICATE_ALIASAndTCID_SERVICE, GetProductFullName(), aliasDup, GetServiceDisplayNameFromInstanceNo(instanceNoDupOfAlias), tcidDup, GetServiceDisplayNameFromInstanceNo(instanceNoDupOfTcid));
                    }

                    //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        #endregion

        Dictionary<string, List<Output>> dictAllOutputForVerifyData = new Dictionary<string, List<Output>>();

        #region Prepare data for validate Output 
        //For Master
        SetDictAllOutputForVerifyData(GetServiceDisplayNameFromInstanceNo(INSTATNCE_NUMBER_MIN), ref dictAllOutputForVerifyData);

        //For Slave
        for (int instanceIndex = 0; instanceIndex < listSlaveServiceDisplayName.Count; instanceIndex++)
        {
            //ignore in case 0 (None)
            if (!listSlaveServiceDisplayName[instanceIndex].Equals(MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE))
            {
                SetDictAllOutputForVerifyData(listSlaveServiceDisplayName[instanceIndex], ref dictAllOutputForVerifyData);
            }
        }
        #endregion

        #region 13.Check Slave instance does not have output
        if (IsAllSlaveInstanceHaveOutput(enProcessType, listSlaveServiceDisplayName, dictAllOutputForVerifyData))
        {
            return false;
        }
        #endregion

        #region 14.Verify all Slave Instance must be stop service
        if (!IsAllSlaveInstanceStopService(enProcessType, listSlaveServiceDisplayName))
        {
            if (enProcessType == _PROCESS_TYPES.PROCESS_TYPE_MMC_INIT_LOAD)
            {
                //To set disable Master-Slave option
                SetMasterSlaveOption(false, iniFileMgr, true);
            }

            return false;
        }
        #endregion

        #region 15.Not Allow DTApplication outputs in Master-Slave Mode
        if (HaveDTAppOutput(GetServiceDisplayNameFromInstanceNo(INSTATNCE_NUMBER_MIN), dictAllOutputForVerifyData))
        {
            errMsg = string.Format(ERRMSG_MASTERSLAVE_MASTER_MUST_NOT_HAVE_DTAAPPOUTPUT, GetProductFullName(), GetProductFullName(), INSTATNCE_NUMBER_MIN.ToString());

            //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        #endregion
    }

    return true;
}

private bool IsCurrentInstanceIsBeMasterOrSlaveInstance(string serviceName, List<string> listMasterSlaveServiceName)
{
    for (int i = 0; i < listMasterSlaveServiceName.Count; i++)
    {
        if (listMasterSlaveServiceName[i] == serviceName)
        {
            return true;
        }
    }

    return false;
}

public bool IsValidateMasterSlaveStartServiceProcess(string serviceName, List<string> listSlaveServiceName)
{
    //Master-Slave rules:
    //1.If Enable Master-Slave option, All slave service cannot start service by itself, this process must be start at master instance only 

    if (IsEnableMasterSlave())
    {
        if (IsCurrentInstanceIsBeMasterOrSlaveInstance(serviceName, listSlaveServiceName))
        {
            //1.If Enable Master-Slave option, All slave service cannot start service by itself, this process must be start at Master Instance only
            if (!serviceName.Equals(GetServiceNameFromInstanceNo(INSTATNCE_NUMBER_MIN))) //Check slave is not master
            {
                string errMsgCaption = MSG_HEADER_START_SERVICE;
                string errMsg = string.Format("{0} does not allow slave instance [{1}] to start the service.\n\nPlease start the service at the Master instance [{2}].", GetProductFullName(), GetServiceDisplayName(serviceName), GetServiceDisplayNameFromInstanceNo(INSTATNCE_NUMBER_MIN));

                //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    return true;
}

public bool IsValidateSiteProductKey(bool bCheckForMasterSlave, bool IsEnableMasterSlaveOpt)
{
    string errMsgCaption = string.Format("{0} Manager", GetProductFullName());
    string errMsg = "";

    if (!ProductKeyManager.Instance.IsPublicKeyFileExist())
    {
        //Check the public key file is exist?
        errMsg = PUBLICKEY_FILE_NOT_EXIST_MSG;

        if (bCheckForMasterSlave && IsEnableMasterSlaveOpt)
        {
            string errMsgMasterSlaveAutoSetDisable = string.Format(ERRMSG_MASTERSLAVE_AUTOSETDISABLE, GetProductFullName(), GetProductFullName());
            errMsg += errMsgMasterSlaveAutoSetDisable;
        }

        //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
    else if (!ProductKeyManager.Instance.IsProductKeyFileExist())
    {
        //Check the product key file is exist?
        errMsg = PRODUCTKEY_FILE_NOT_EXIST_MSG;

        if (bCheckForMasterSlave && IsEnableMasterSlaveOpt)
        {
            string errMsgMasterSlaveAutoSetDisable = string.Format(ERRMSG_MASTERSLAVE_AUTOSETDISABLE, GetProductFullName(), GetProductFullName());
            errMsg += errMsgMasterSlaveAutoSetDisable;
        }

        //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
    else if (!ProductKeyManager.Instance.IsValidProductKeyFile())
    {
        //Check the product key file is valid?
        errMsg = INVALIDE_SIGNATRUE;

        if (bCheckForMasterSlave && IsEnableMasterSlaveOpt)
        {
            string errMsgMasterSlaveAutoSetDisable = string.Format(ERRMSG_MASTERSLAVE_AUTOSETDISABLE, GetProductFullName(), GetProductFullName());
            errMsg += errMsgMasterSlaveAutoSetDisable;
        }

        //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
    else if (ProductKeyManager.Instance.IsProductKeyExpired())
    {
        //Check the product key file is expired?
        errMsg = PRODUCTKEY_EXPIRED_MSG;

        if (bCheckForMasterSlave && IsEnableMasterSlaveOpt)
        {
            string errMsgMasterSlaveAutoSetDisable = string.Format(ERRMSG_MASTERSLAVE_AUTOSETDISABLE, GetProductFullName(), GetProductFullName());
            errMsg += errMsgMasterSlaveAutoSetDisable;
        }

        //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
    else if (ProductKeyManager.Instance.IsInstanceExceedLimit())
    {
        //Check total of instances exceed product key file limit?
        errMsg = INSTANCE_EXCEED_MSG;

        if (bCheckForMasterSlave && IsEnableMasterSlaveOpt)
        {
            string errMsgMasterSlaveAutoSetDisable = string.Format(ERRMSG_MASTERSLAVE_AUTOSETDISABLE, GetProductFullName(), GetProductFullName());
            errMsg += errMsgMasterSlaveAutoSetDisable;
        }

        //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
    else if (ProductKeyManager.Instance.IsTofDtfInputExceedLimit())
    {
        //Check total of TOF/DTF inputs exceed product key file limit?
        errMsg = TOTAL_TOFDTF_EXCEED_MSG;

        if (bCheckForMasterSlave && IsEnableMasterSlaveOpt)
        {
            string errMsgMasterSlaveAutoSetDisable = string.Format(ERRMSG_MASTERSLAVE_AUTOSETDISABLE, GetProductFullName(), GetProductFullName());
            errMsg += errMsgMasterSlaveAutoSetDisable;
        }

        //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
    else if (ProductKeyManager.Instance.IsTofDtfInputPerInstanceExceedMax())
    {
        //Check total of TofDtf inputs of each instance exceed maximum limit? 
        errMsg = TOFDTF_PER_INSTANCE_EXCEED_MSG;

        if (bCheckForMasterSlave && IsEnableMasterSlaveOpt)
        {
            string errMsgMasterSlaveAutoSetDisable = string.Format(ERRMSG_MASTERSLAVE_AUTOSETDISABLE, GetProductFullName(), GetProductFullName());
            errMsg += errMsgMasterSlaveAutoSetDisable;
        }

        //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    return true;
}

public bool IsValidateSiteProductKeyForAddInput(ServiceConfiguration service, string tcid)
{
    string caption = "Add Input";

    if (!ProductKeyManager.Instance.IsPublicKeyFileExist())
    {
        //Check the public key file is exist?
        //MessageBox.Show(PUBLICKEY_FILE_NOT_EXIST_MSG, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
    else if (!ProductKeyManager.Instance.IsProductKeyFileExist())
    {
        //Check the product key file is exist?
        //MessageBox.Show(PRODUCTKEY_FILE_NOT_EXIST_MSG, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
    else if (!ProductKeyManager.Instance.IsValidProductKeyFile())
    {
        //Check the product key file is valid?
        //MessageBox.Show(PRODUCTKEY_INVALID_MSG, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
    else if (ProductKeyManager.Instance.IsProductKeyExpired())
    {
        //Check the product key file is expired?
        //MessageBox.Show(PRODUCTKEY_EXPIRED_MSG, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
    else if (ProductKeyManager.Instance.IsInstanceExceedLimit())
    {
        //Check total of instances exceed product key file limit?
        //MessageBox.Show(INSTANCE_EXCEED_MSG, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
    else if (IsInputExceedInstanceLimitForAddInput(service, tcid))
    {
        //Check total of TofDtf input for each instance exceed product key file limit?
        //MessageBox.Show(TOFDTF_PER_INSTANCE_EXCEED_MSG, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    return true;
}

private bool IsInputExceedInstanceLimitForAddInput(ServiceConfiguration service, string tcid)
{
    bool ret = false;

    if (service != null)
    {
        var inputsNodes = service.m_inputsNode.GetAllTcidInputsNode();

        if ((inputsNodes.Count >= ProductKeyManager.Instance.GetMaxTofOrDtfInputPerInstance())
            || (inputsNodes.Count >= ProductKeyManager.Instance.GetMaxTofOrDtfInputs()))
        {
            ret = true;
            foreach (var node in inputsNodes)
            {
                if (node.m_inputNodeName.Length >= 4)
                {
                    if (node.m_inputNodeName.Substring(0, 4).Equals(tcid, StringComparison.OrdinalIgnoreCase))
                    {
                        ret = false;
                        break;
                    }
                }
            }
        }
    }

    return ret;
}

private bool HaveDuplicateOutputIpPortNumberWhenStartService(List<ServiceConfiguration> listServiceConfiguration, ref string dupServiceName, ref string outputNameDup1, ref int instanceNoDup1, ref string outputNameDup2, ref int instanceNoDup2)
{
    Dictionary<string, DuplicateOutputAt> dictOutputIpPortNumber = new Dictionary<string, DuplicateOutputAt>();

    for (int indexService = 0; indexService < listServiceConfiguration.Count; indexService++)
    {
        ServiceConfiguration serviceConfiguration = listServiceConfiguration[indexService];
        List<Output> allOutput = serviceConfiguration.m_outputsNode.GetAllOutput();

        Output output = null;

        for (int indexOutput = 0; indexOutput < allOutput.Count; indexOutput++)
        {
            output = allOutput[indexOutput];

            if ((output.m_outputType == OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_SOCKET)
                || (output.m_outputType == OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_DTAPP))
            {
                string outputName = "";
                string value = "";

                List<SettingData> listSettingData = null;

                switch (output.m_outputType)
                {
                    case OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_SOCKET:
                        {
                            OutputSocket outputSock = (OutputSocket)output;
                            listSettingData = outputSock.GetAllSettingData();
                        }
                        break;

                    case OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_DTAPP:
                        {
                            OutputDTApp outputDtApp = (OutputDTApp)output;
                            listSettingData = outputDtApp.GetAllSettingData();
                        }
                        break;
                }

                for (int i = 0; i < listSettingData.Count; i++)
                {
                    SettingData settingData = listSettingData[i];

                    if (settingData.m_key == OutputSocket.KEY_PORT_NUMBER)
                    {
                        value = settingData.m_value.Trim();
                        break;
                    }
                }

                string len = OutputsConfiguration.SECTION_OUTPUTS + @"\";
                outputName = output.m_section.Substring((len.Length), (output.m_section.Length - len.Length));

                if (!dictOutputIpPortNumber.ContainsKey(value))
                {
                    DuplicateOutputAt duplicateOutputAt = new DuplicateOutputAt(outputName, output.m_instanceNo, output.m_outputType, value);

                    dictOutputIpPortNumber.Add(value, duplicateOutputAt);
                }
                else
                {
                    //Duplicate 
                    dupServiceName = value;

                    DuplicateOutputAt duplicateOutputAt = dictOutputIpPortNumber[value];

                    outputNameDup1 = duplicateOutputAt.m_outputName;
                    instanceNoDup1 = duplicateOutputAt.m_instanceNo;

                    outputNameDup2 = outputName;
                    instanceNoDup2 = output.m_instanceNo;

                    return true;
                }
            }
        }
    }

    return false;
}

public bool IsValidateBasicConfigureRule()
{
    string errMsgCaption = "";
    string errMsg = "";

    RootConfiguration rootNode = GetRootNode();
    ComputerConfiguration computerNode = rootNode.GetComputerNode();
    List<ServiceConfiguration> listServiceConfiguration = computerNode.GetAllServicesNode();

    string dupServiceName = "";
    string outputNameDup1 = "";
    string outputNameDup2 = "";
    int instanceNoDup1 = 0;
    int instanceNoDup2 = 0;

    #region 1.Check Not duplicate IP Port output
    if (HaveDuplicateOutputIpPortNumberWhenStartService(listServiceConfiguration, ref dupServiceName, ref outputNameDup1, ref instanceNoDup1, ref outputNameDup2, ref instanceNoDup2))
    {
        errMsg = string.Format(ERRMAG_DUPLICATE_IPOUTPUT_STARTSERVICE, GetProductFullName(), dupServiceName, outputNameDup1, GetServiceDisplayNameFromInstanceNo(instanceNoDup1), outputNameDup2, GetServiceDisplayNameFromInstanceNo(instanceNoDup2));

        errMsgCaption = MSG_HEADER_START_SERVICE;
        //MessageBox.Show(errMsg, errMsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
    #endregion

    return true;
}

public void GetlistSlaveServiceDisplayName(ref List<int> listSlaveInstanceNo, ref List<string> listSlaveServiceDisplayName)
{
    string serviceName = GetServiceNameFromInstanceNo(INSTATNCE_NUMBER_MIN);
    ServiceConfiguration service = GetServiceNode(serviceName);

    if (service != null)
    {
        MasterSlaveConfiguration masterSlaveNode = service.m_globalNode.GetMasterSlaveNode();
        List<SettingData> listSettingData = masterSlaveNode.GetAllSettingData();

        for (int i = 0; i < listSettingData.Count; i++)
        {
            SettingData settingData = listSettingData[i];

            switch (settingData.m_key)
            {
                case MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_1:
                case MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_2:
                case MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_3:
                case MasterSlaveConfiguration.KEY_SLAVE_INSTANCE_4:
                    {
                        string serviceDisplayName = MasterSlaveConfiguration.MASTERSLAVE_DEFAULT_SLAVEINSTANCE;

                        int instanceNo = 0;
                        int.TryParse(settingData.m_value, out instanceNo);

                        listSlaveInstanceNo.Add(instanceNo);

                        if (instanceNo != 0)
                        {
                            serviceDisplayName = GetServiceDisplayNameFromInstanceNo(instanceNo);
                        }

                        listSlaveServiceDisplayName.Add(serviceDisplayName);
                    }
                    break;
            }
        }
    }
}

public bool IsValidateMasterSlave(string serviceName)
{
    if (IsEnableMasterSlave())
    {
        ServiceConfiguration service = GetServiceNode(serviceName);

        if (service != null)
        {
            List<string> listMasterSlaveServiceName = new List<string>();
            List<string> listSlaveServiceName = new List<string>();
            List<int> listSlaveInstanceNo = new List<int>();
            List<string> listSlaveServiceDisplayName = new List<string>();

            #region Prepare Data for validate
            GetlistSlaveServiceDisplayName(ref listSlaveInstanceNo, ref listSlaveServiceDisplayName);

            listMasterSlaveServiceName.Add(GetServiceNameFromInstanceNo(INSTATNCE_NUMBER_MIN));

            for (int i = 0; i < listSlaveInstanceNo.Count; i++)
            {
                if ((listSlaveInstanceNo[i] > INSTATNCE_NUMBER_MIN) && (listSlaveInstanceNo[i] <= INSTATNCE_NUMBER_MAX))
                {
                    listMasterSlaveServiceName.Add(GetServiceNameFromInstanceNo(listSlaveInstanceNo[i]));

                    listSlaveServiceName.Add(GetServiceNameFromInstanceNo(listSlaveInstanceNo[i]));
                }
            }
            #endregion

            if (IsCurrentInstanceIsBeMasterOrSlaveInstance(serviceName, listMasterSlaveServiceName))
            {
                //Master-Slave rules:
                //1.Validate configure for Master-Slave option
                //2.Validate Master-Slave Option when start service

                if ((IsValidateConfigureForMasterSlaveOption(_PROCESS_TYPES.PROCESS_TYPE_STARTSERVICE, listSlaveServiceDisplayName, service.GetIniFileManager()))
                    && (IsValidateMasterSlaveStartServiceProcess(serviceName, listMasterSlaveServiceName)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    return true;
}

private void DoAccessAllowedList(List<ServiceConfiguration> listServiceConfiguration, List<int> listAllInstance)
{
    bool canSetAllDtf = false;
    bool canSetAllTof = false;
    List<string> allTcidInput = new List<string>();

    for (int indexService = 0; indexService < listServiceConfiguration.Count; indexService++)
    {
        ServiceConfiguration service = GetServiceNode(listServiceConfiguration[indexService].GetServiceName());

        if (service != null)
        {
            bool thisInstanceIsMasterSlave = false;

            for (int i = 0; i < listAllInstance.Count; i++)
            {
                if (service.GetInstanceNo() == listAllInstance[i])
                {
                    thisInstanceIsMasterSlave = true;
                    break;
                }
            }

            if (!thisInstanceIsMasterSlave)
            {
                continue;
            }

            #region Get All Inputs
            List<TcidInputNode> listTcidInputNode = service.m_inputsNode.GetAllTcidInputsNode();

            for (int indexTcidInputNode = 0; indexTcidInputNode < listTcidInputNode.Count; indexTcidInputNode++)
            {
                TcidInputNode tcidInputNode = listTcidInputNode[indexTcidInputNode];

                if (tcidInputNode.m_hasDtfInput)
                {
                    allTcidInput.Add(tcidInputNode.m_inputNodeName + "_DTF");
                }

                if (tcidInputNode.m_hasTofInput)
                {
                    allTcidInput.Add(tcidInputNode.m_inputNodeName + "_TOF");
                }
            }
            #endregion
        }
    }

    #region Can set ALL_DTF
    for (int i = 0; i < allTcidInput.Count; i++)
    {
        int pos = allTcidInput[i].IndexOf("_DTF");
        if (pos > 0)
        {
            canSetAllDtf = true;
            break;
        }
    }
    #endregion

    #region Can set ALL_TOF
    for (int i = 0; i < allTcidInput.Count; i++)
    {
        int pos = allTcidInput[i].IndexOf("_TOF");
        if (pos > 0)
        {
            canSetAllTof = true;
            break;
        }
    }
    #endregion

    for (int indexService = 0; indexService < listServiceConfiguration.Count; indexService++)
    {
        ServiceConfiguration service = GetServiceNode(listServiceConfiguration[indexService].GetServiceName());

        if (service != null)
        {
            bool thisInstanceIsMasterSlave = false;

            for (int i = 0; i < listAllInstance.Count; i++)
            {
                if (service.GetInstanceNo() == listAllInstance[i])
                {
                    thisInstanceIsMasterSlave = true;
                    break;
                }
            }

            if (!thisInstanceIsMasterSlave)
            {
                continue;
            }

            #region Check Output Access Allow List
            List<Output> allOutput = service.m_outputsNode.GetAllOutput();

            for (int indexOutput = 0; indexOutput < allOutput.Count; indexOutput++)
            {
                Output output = allOutput[indexOutput];
                iniFileManager iniFileMgr = output.GetIniFileManager();

                switch (output.m_accessAllowList)
                {
                    case "ALL":
                        {
                            continue;
                        }

                    case "ALL_TOF":
                        {
                            if (!canSetAllTof)
                            {
                                iniFileMgr.IniWriteValue(output.m_section, Output.KEY_ACCESS_ALLOWED_LIST, "ALL");
                            }
                        }
                        break;

                    case "ALL_DTF":
                        {
                            if (!canSetAllDtf)
                            {
                                iniFileMgr.IniWriteValue(output.m_section, Output.KEY_ACCESS_ALLOWED_LIST, "ALL");
                            }
                        }
                        break;

                    default:
                        {
                            bool isFound = false;

                            var arrOption = output.m_accessAllowList.Split(',');

                            for (int i = 0; i < arrOption.Length; i++)
                            {
                                for (int j = 0; j < allTcidInput.Count; j++)
                                {
                                    if (arrOption[i] == allTcidInput[j])
                                    {
                                        isFound = true;
                                        break;
                                    }
                                }
                            }

                            if (!isFound)
                            {
                                iniFileMgr.IniWriteValue(output.m_section, Output.KEY_ACCESS_ALLOWED_LIST, "ALL");
                            }
                        }
                        break;
                }
            }
            #endregion
        }
    }
}

public void DoCheckAccessAllowedList()
{
    List<int> listAllInstance = new List<int>();

    RootConfiguration rootNode = GetRootNode();
    ComputerConfiguration computerNode = rootNode.GetComputerNode();
    List<ServiceConfiguration> listServiceConfiguration = computerNode.GetAllServicesNode();

    if (IsEnableMasterSlave())
    {
        listAllInstance.Add(INSTATNCE_NUMBER_MIN);
        List<string> listAllSlaveInstance = GetAllSlaveInstance();

        #region Get All Instance
        for (int i = 0; i < listAllSlaveInstance.Count; i++)
        {
            int instanceNo = 0;
            string slaveInst = listAllSlaveInstance[i];
            int.TryParse(slaveInst, out instanceNo);

            if ((instanceNo > INSTATNCE_NUMBER_MIN) && (instanceNo <= INSTATNCE_NUMBER_MAX))
            {
                listAllInstance.Add(instanceNo);
            }
        }
        #endregion
    }
    else
    {
        #region Get All Instance
        for (int i = 0; i < listServiceConfiguration.Count; i++)
        {
            int instanceNo = listServiceConfiguration[i].GetInstanceNo();

            if ((instanceNo >= INSTATNCE_NUMBER_MIN) && (instanceNo <= INSTATNCE_NUMBER_MAX))
            {
                listAllInstance.Add(instanceNo);
            }
        }
        #endregion
    }

    DoAccessAllowedList(listServiceConfiguration, listAllInstance);
}

public List<TcidInputNode> LoadOutputFilter(ref Dictionary<string, TcidInputNode> dictTcidInputNode, ServiceConfiguration m_service)
{
    List<TcidInputNode> listTcidInputNode = new List<TcidInputNode>();
    List<TcidInputNode> listTcidInputNodeTmp = new List<TcidInputNode>();
    List<TcidInputNode> listTcidInputNodeTmp2 = m_service.m_inputsNode.GetAllTcidInputsNode();

    for (int i = 0; i < listTcidInputNodeTmp2.Count; i++)
    {
        listTcidInputNodeTmp.Add(listTcidInputNodeTmp2[i]);
    }

    //Check this instance is Master and Enable Master Slave option 
    if (m_service.GetInstanceNo() == INSTATNCE_NUMBER_MIN)
    {
        if (IsEnableMasterSlave())
        {
            List<ServiceConfiguration> listSlaveService = new List<ServiceConfiguration>();
            List<string> listSlaveInstanceTmp = GetAllSlaveInstance();

            #region Get Input From Slave Instance
            for (int i = 0; i < listSlaveInstanceTmp.Count; i++)
            {
                //ignore in case 0 (None)
                if (listSlaveInstanceTmp[i].Trim() != "0")
                {
                    int instanceNo = 0;
                    int.TryParse(listSlaveInstanceTmp[i], out instanceNo);

                    if (instanceNo != 0)
                    {
                        ServiceConfiguration slaveService = GetServiceNode(GetServiceNameFromInstanceNo(instanceNo));
                        listSlaveService.Add(slaveService);
                    }
                }
            }
            #endregion

            #region Assign Input to listTcidInputNode
            for (int i = 0; i < listSlaveService.Count; i++)
            {
                ServiceConfiguration slaveService = listSlaveService[i];

                List<TcidInputNode> listSlaveTcidInputNodeTmp = slaveService.m_inputsNode.GetAllTcidInputsNode();

                for (int j = 0; j < listSlaveTcidInputNodeTmp.Count; j++)
                {
                    listTcidInputNodeTmp.Add(listSlaveTcidInputNodeTmp[j]);
                }
            }
            #endregion
        }
    }

    var sortedlistTcidInputNode = listTcidInputNodeTmp.OrderBy(key => key.m_inputNodeName);

    foreach (TcidInputNode tcidInputNode in sortedlistTcidInputNode)
    {
        if (!dictTcidInputNode.ContainsKey(tcidInputNode.m_inputNodeName))
        {
            dictTcidInputNode.Add(tcidInputNode.m_inputNodeName, tcidInputNode);

            listTcidInputNode.Add(tcidInputNode);
        }
    }

    return listTcidInputNode;
}

public string GetDefaultOutputName(ServiceConfiguration service)
{
    string outputName = "output";
    string defaultOutputName = "";

    List<SettingData> listSettingData = service.m_outputsNode.GetAllSettingData();

    bool isMaxDefaultNo = false;
    bool isFound = false;

    for (int i = 1; i <= 99; i++)
    {
        isFound = false;

        defaultOutputName = string.Format("{0}{1}", outputName, i.ToString("00"));

        for (int j = 0; j < listSettingData.Count; j++)
        {
            if (defaultOutputName.Equals(listSettingData[j].m_key, StringComparison.CurrentCultureIgnoreCase))
            {
                isFound = true;
                break;
            }
        }

        if (!isFound)
            break;

        if (i == 99)
            isMaxDefaultNo = true;
    }

    if (!isFound)
    {
        if (isMaxDefaultNo)
        {
            defaultOutputName = outputName;
        }
    }

    return defaultOutputName;
}

public void SortingInputSection(iniFileManager iniFileMgr)
{
    if (iniFileMgr != null)
    {
        //Load existing inputs
        string[] strArray = iniFileMgr.GetAllKeysInSection(InputsConfiguration.SECTION_INPUTS);

        //Get Current Input
        Dictionary<string, string> inputPair = new Dictionary<string, string>();
        for (int i = 0; i < strArray.Length; ++i)
        {
            var pair = strArray[i].Split('=');
            if (pair.Length >= 2)
            {
                string key = pair[0];
                string value = pair[1];

                inputPair.Add(key, value);

                //Clear section
                iniFileMgr.IniWriteValue(InputsConfiguration.SECTION_INPUTS, key, null);
            }
        }

        //Sorting
        var sortedInputPair = inputPair.OrderBy(key => key.Key);

        //Write ini
        foreach (var input in sortedInputPair)
        {
            iniFileMgr.IniWriteValue(InputsConfiguration.SECTION_INPUTS, input.Key, input.Value);
        }
    }
}

public void SortingOutputSection(iniFileManager iniFileMgr)
{
    string[] strArray = iniFileMgr.GetAllKeysInSection(OutputsConfiguration.SECTION_OUTPUTS);

    //Get Current Output
    Dictionary<string, string> outputPair = new Dictionary<string, string>();
    for (int i = 0; i < strArray.Length; ++i)
    {
        var pair = strArray[i].Split('=');
        if (pair.Length >= 2)
        {
            string key = pair[0];
            string value = pair[1];

            outputPair.Add(key, value);

            //Clear section
            iniFileMgr.IniWriteValue(OutputsConfiguration.SECTION_OUTPUTS, key, null);
        }
    }

    //Sorting
    var sortedOutputPair = outputPair.OrderBy(key => key.Value);

    //Write ini
    foreach (var output in sortedOutputPair)
    {
        iniFileMgr.IniWriteValue(OutputsConfiguration.SECTION_OUTPUTS, output.Key, output.Value);
    }
}

public void SortingOutputUserLogonsSection(string section, iniFileManager iniFileMgr)
{
    string[] strArray = iniFileMgr.GetAllKeysInSection(section);

    //Get Current Output
    Dictionary<string, string> outputPair = new Dictionary<string, string>();
    for (int i = 0; i < strArray.Length; ++i)
    {
        var pair = strArray[i].Split('=');
        if (pair.Length >= 2)
        {
            string key = pair[0];
            string value = pair[1];

            outputPair.Add(key, value);

            //Clear section
            iniFileMgr.IniWriteValue(section, key, null);
        }
    }

    //Sorting
    var sortedOutputPair = outputPair.OrderBy(key => key.Value);

    //Write ini
    foreach (var output in sortedOutputPair)
    {
        iniFileMgr.IniWriteValue(section, output.Key, output.Value);
    }
}

public bool IsValidPortNumber(int port)
{
    if (port < 1024 || port > 65535)
    {
        string message = "The port must be value between 1024 and 65535.\n\nPlease enter a valid value.";
        string caption = "Invalid Value";

        //MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }

    return true;
}

private OutputsConfiguration.OUTPUT_TYPE GetOutputTypeFromMMCType(string mmcType)
{
    switch (mmcType)
    {
        case OutputsConfiguration.OUTPUT_TYPE_SOCKET:
            return OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_SOCKET;

        case OutputsConfiguration.OUTPUT_TYPE_DTAPP:
            return OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_DTAPP;

        default:
            return OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_NOTSET;
    }
}

public bool OutputNameIsAlreadyUsed(ServiceConfiguration service, string outputName)
{
    List<Output> allOutput = service.m_outputsNode.GetAllOutput();

    for (int indexOutput = 0; indexOutput < allOutput.Count; indexOutput++)
    {
        Output output = allOutput[indexOutput];

        if (output.m_key.Equals(outputName, StringComparison.CurrentCultureIgnoreCase))
        {
            //MessageBox.Show("The configured output name is either missing or already in used. Please enter other output name.", "Missing name", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return true;
        }
    }

    return false;
}

public bool OutputPortNumberIsAlreadyUsed(string serviceName, string outputName, int port)
{
    RootConfiguration rootNode = GetRootNode();

    if (rootNode != null)
    {
        ComputerConfiguration computerNode = rootNode.GetComputerNode();
        List<ServiceConfiguration> allServicesNodes = computerNode.GetAllServicesNode();

        for (int instanceNo = 0; instanceNo < allServicesNodes.Count; instanceNo++)
        {
            ServiceConfiguration service = allServicesNodes[instanceNo];
            List<Output> allOutput = service.m_outputsNode.GetAllOutput();
            List<SettingData> listSettingData = new List<SettingData>();

            for (int indexOutput = 0; indexOutput < allOutput.Count; indexOutput++)
            {
                Output output = allOutput[indexOutput];

                if (output != null)
                {
                    switch (output.m_outputType)
                    {
                        case OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_SOCKET:
                        case OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_DTAPP:
                            {
                                if (output.m_portNumber == port)
                                {
                                    if ((output.m_key.Equals(outputName, StringComparison.CurrentCultureIgnoreCase))
                                        && (serviceName.Equals(service.GetServiceName(), StringComparison.CurrentCultureIgnoreCase)))
                                    {
                                        continue;
                                    }

                                    string outputTypeName = "";

                                    if (output.m_outputType == OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_SOCKET)
                                    {
                                        outputTypeName = "TCP/IP";
                                    }
                                    else if (output.m_outputType == OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_DTAPP)
                                    {
                                        outputTypeName = "DT Application";
                                    }

                                    string message = string.Format("The port [{0}] is currently configured on one of {1} {2} outputs [{3} : {4}].\n\nPlease select another port.", port.ToString(), GetProductFullName(), outputTypeName, service.GetServiceName(), output.m_key);
                                    string caption = "Invalid Value";

                                    //MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return true;
                                }
                            }
                            break;
                    }
                }
            }
        }
    }

    return false;
}

public bool OutputPortNumberIsAlreadyUsedForShow(int port)
{
    RootConfiguration rootNode = GetRootNode();

    if (rootNode != null)
    {
        ComputerConfiguration computerNode = rootNode.GetComputerNode();
        List<ServiceConfiguration> allServicesNodes = computerNode.GetAllServicesNode();

        int count = 0;

        for (int instanceNo = 0; instanceNo < allServicesNodes.Count; instanceNo++)
        {
            ServiceConfiguration service = allServicesNodes[instanceNo];
            List<Output> allOutput = service.m_outputsNode.GetAllOutput();
            List<SettingData> listSettingData = new List<SettingData>();

            for (int indexOutput = 0; indexOutput < allOutput.Count; indexOutput++)
            {
                Output output = allOutput[indexOutput];

                if (output != null)
                {
                    switch (output.m_outputType)
                    {
                        case OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_SOCKET:
                        case OutputsConfiguration.OUTPUT_TYPE.OUTPUT_TYPE_DTAPP:
                            {
                                if (output.m_portNumber == port)
                                {
                                    count++;

                                    if (count > 1)
                                    {
                                        return true;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }
    }

    return false;
}

public string GetStringOfInteger(string value)
{
    string ret = "";

    int iValue = -1;
    if (int.TryParse(value, out iValue))
    {
        ret = iValue.ToString();
    }

    return ret;
}

public bool IsNumeric(string value)
{
    return value.All(char.IsNumber);
}
}
}
