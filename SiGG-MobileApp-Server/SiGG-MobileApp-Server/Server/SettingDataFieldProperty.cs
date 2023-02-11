using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class SettingDataFieldProperty
    {
        public string m_displayName;
        public string m_defaultValue;
        public FieldStaus m_status;
        public FieldShow m_show;
        public bool m_removable;

        public SettingDataFieldProperty(string displayName,
                                       string defaultValue,
                                       FieldStaus status,
                                       FieldShow show,
                                       bool removable)
        {
            m_displayName = displayName;
            m_defaultValue = defaultValue;
            m_status = status;
            m_show = show;
            m_removable = removable;
        }
    }
}
