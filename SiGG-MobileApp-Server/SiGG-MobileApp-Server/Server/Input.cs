using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class Input     //base class (parent) 
    {
        public int m_instanceNo;
        public iniFileManager m_iniFileMgr;
        public string m_inputKey;
        public string m_section;
        public InputsConfiguration.INPUT_TYPE m_inputType;

        public string m_name;
        public string m_tcid;
        public string m_cacheSize;
        public string m_marketfeedLog;
        public string m_heartBeat;
        public string m_heartbeatRecord;
        public string m_heartbeatInterval;
        public string m_alias;
        public string m_reRequestEnable;
        public string m_reRequestTimeout;

        public RecordMaps m_recordMaps;
        public FieldOverrides m_fieldOverrides;

        public const string KEY_INPUT_CACHE_SIZE = "InputCacheSize";
        public const string KEY_MARKETFEED_LOGGING = "MarketfeedLogging";
        public const string KEY_HEART_BEAT = "Heartbeat";
        public const string KEY_HEART_BEAT_RECORD = "HeartbeatRecord";
        public const string KEY_HEART_BEAT_INTERVAL = "HeartbeatInterval";
        public const string KEY_HEART_BEAT_TIMEOUT = "HeartbeatTimeout";
        public const string KEY_TCID = "TCID";
        public const string KEY_ALIAS = "Alias";
        public const string KEY_REREQUEST_ENABLE = "ReRequestEnable";
        public const string KEY_REREQUEST_TIMEOUT = "ReRequestTimeout";

        public Input(int instanceNo, iniFileManager iniFileMgr, string inputKey, InputsConfiguration.INPUT_TYPE inputType)
        {
            m_instanceNo = instanceNo;
            m_iniFileMgr = iniFileMgr;
            m_inputKey = inputKey;
            m_inputType = inputType;
            m_section = string.Format("{0}\\{1}", InputsConfiguration.SECTION_INPUTS, m_inputKey);

            Initialise();
        }

        public virtual void Initialise()
        {
            //Cache Size of each Inputs in Recordstore. 
            m_cacheSize = m_iniFileMgr.IniReadValue(m_section, KEY_INPUT_CACHE_SIZE);

            //MARKETFEEDLOGGING
            m_marketfeedLog = m_iniFileMgr.IniReadValue(m_section, KEY_MARKETFEED_LOGGING);

            //HEARTBEAT
            m_heartBeat = m_iniFileMgr.IniReadValue(m_section, KEY_HEART_BEAT);

            //HEARTBEAT RECORD NAME
            m_heartbeatRecord = m_iniFileMgr.IniReadValue(m_section, KEY_HEART_BEAT_RECORD);

            //HEARTBEAT INTERVAL
            m_heartbeatInterval = m_iniFileMgr.IniReadValue(m_section, KEY_HEART_BEAT_INTERVAL);

            //TCID (must be upper case and 4 chars)
            m_tcid = m_iniFileMgr.IniReadValue(m_section, KEY_TCID).ToUpper();

            //Alias (must be upper case and 4 chars)
            m_alias = m_iniFileMgr.IniReadValue(m_section, KEY_ALIAS).ToUpper();

            //load ReRequest
            m_reRequestEnable = m_iniFileMgr.IniReadValue(m_section, KEY_REREQUEST_ENABLE);
            m_reRequestTimeout = m_iniFileMgr.IniReadValue(m_section, KEY_REREQUEST_TIMEOUT);

            m_recordMaps = new RecordMaps(m_instanceNo, m_iniFileMgr, m_inputKey);
            m_fieldOverrides = new FieldOverrides(m_instanceNo, m_iniFileMgr, m_inputKey);
        }

        public iniFileManager GetIniFileManager()
        {
            return m_iniFileMgr;
        }
    }
}
