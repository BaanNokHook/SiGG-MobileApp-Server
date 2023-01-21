// Satellite-Communication-Server //

using SocketAppServer.CoreServices;
using SocketAppServer.LoadBalancingServices;
using SocketAppServer.ManagedServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace SocketAppServer.ServerObjects
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
