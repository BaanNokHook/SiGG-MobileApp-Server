using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public enum FieldShow
    {
        OnlyAdvance,
        Both,
        NotShow
    }

    public enum FieldStaus
    {
        Green,
        Yellow,
        Pink,
        Grey,
        Red,
        Red_Duplicate,
        RecordMaps,
        FieldOverrides,
        Inactive_Stoped_Service,
        Inactive_Started_Service,
        Inactive_Stoped_MasterSlaveService,
        Inactive_Started_MasterSlaveService,
        Inactive_Global,
        Inactive_Inputs,
        Inactive_RecordMaps,
        Inactive_RecordMap,
        Inactive_Prefix,
        Inactive_Flags,
        Inactive_FieldOverrides,
        Inactive_Outputs,
        Inactive_Output_Socket,
        Inactive_Output_DTAPP,
        Inactive_User_Logon,
        Inactive_MasterSlave,
        Inactive_ErrorLog,
        Inactive_RecordCache,
        Inactive_InputTcidNode,
        Inactive_InputDtf,
        Inactive_InputTof,
        Disable_Global,
        Disable_Outputs,
        Disable_ErrorLog,
        Disable_RecordCache,
        NotSet
    }

    public class SettingData
    {
        public string m_key;
        public string m_value;
        public string m_display;
        public string m_defaultValue;
        public string m_section;
        public FieldStaus m_status;
        public FieldShow m_show;
        public bool m_removable;
        public bool m_specialField; //Special Field isn't show on dialog

        public SettingData(string key, string value, string display, string defaultValue, string section, FieldStaus status, FieldShow show, bool removable, bool isSpecialField)
        {
            m_key = key;
            m_value = value;
            m_display = display;
            m_defaultValue = defaultValue;
            m_section = section;
            m_status = status;
            m_show = show;
            m_removable = removable;
            m_specialField = isSpecialField;
        }
    }
}
