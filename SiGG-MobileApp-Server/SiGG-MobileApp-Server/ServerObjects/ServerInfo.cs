using SiGG_MobileApp_Server.ManagedServices;
using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ServerObjects
{
    internal class ServerInfo
    {
        public string MachineName { get; set; }
        public string ServerVersion { get; set; }
        public bool RequiresAuthentication { get; set; }
        public bool IsLoadBanancingServer { get; set; }
        public List<ControllerInfo> ServerControllers { get; set; }

        public ServerInfo()
        {
            IServiceManager manager = ServiceManager.GetInstance();
            ISecurityManagementService securityManagement = manager.GetService<ISecurityManagementService>();
            ICoreServerService coreServer = manager.GetService<ICoreServerService>("realserver");

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            if (coreServer.IsLoadBalanceEnabled())
                IsLoadBanancingServer = true;

            RequiresAuthentication = securityManagement.IsAuthenticationEnabled();
            ServerVersion = version;
            ServerControllers = new List<ControllerInfo>();
            MachineName = Environment.MachineName;
        }
    }
}
