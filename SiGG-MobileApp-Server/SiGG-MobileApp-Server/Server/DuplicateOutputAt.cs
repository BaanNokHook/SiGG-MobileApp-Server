using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class DuplicateOutputAt
    {
        public int m_instanceNo;
        public OutputsConfiguration.OUTPUT_TYPE m_outputType;
        public string m_outputName;
        public string m_value;

        public DuplicateOutputAt(string outputName, int instanceNo, OutputsConfiguration.OUTPUT_TYPE outputType, string value)
        {
            m_outputName = outputName;
            m_instanceNo = instanceNo;
            m_outputType = outputType;
            m_value = value;
        }
    }
}
