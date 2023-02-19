using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.SocketAppServerClient.Standard.ClientUtils
{
    public class ServerInfo
    {
        [JsonProperty]
        public string MachineName { get; private set; }

        [JsonProperty]
        public string ServerVersion { get; private set; }

        [JsonProperty]
        public bool RequiresAuthentication { get; private set; }

        [JsonProperty]
        public bool IsLoadBanancingServer { get; private set; }

        [JsonProperty]
        public List<ControllerInfo> ServerControllers { get; private set; }

        public ServerInfo()
        {
            ServerControllers = new List<ControllerInfo>();
        }
    }
}
