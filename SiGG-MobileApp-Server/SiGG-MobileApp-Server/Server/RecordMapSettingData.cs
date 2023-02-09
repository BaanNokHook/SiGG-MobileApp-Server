using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class RecordMapSettingData
    {
        public string m_nodeName;
        public string m_prefix;
        public string m_input;
        public string m_flags;
        public string m_section;

        public RecordMapSettingData(string nodeName, string prefix, string input, string flags, string section)
        {
            m_nodeName = nodeName;
            m_prefix = prefix;
            m_input = input;
            m_flags = flags;
            m_section = section;
        }
    }
}
