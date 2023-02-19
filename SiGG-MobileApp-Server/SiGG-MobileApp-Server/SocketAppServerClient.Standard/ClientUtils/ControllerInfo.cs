using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.SocketAppServerClient.Standard.ClientUtils
{
    public class ControllerInfo
    {
        [JsonProperty]
        public string ControllerName { get; private set; }
        [JsonProperty]
        public string ControllerClass { get; private set; }
        [JsonProperty]
        public List<string> ControllerActions { get; private set; }

        public ControllerInfo()
        {
            ControllerActions = new List<string>();
        }
    }
}
