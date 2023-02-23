using SiGG_MobileApp_Server.CoreServices.CLIHost;
using SiGG_MobileApp_Server.ManagedServices;
using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.AnAnyExtensionFromFile
{
    public class FunnyCommand : ICLIClient
    {
        public void Activate()
        {
            IServiceManager manager = ServiceManager.GetInstance();
            ILoggingService logging = manager.GetService<ILoggingService>();
            logging.WriteLog("Hi! Im a simple CLI function running inside this beautiful extension loaded dinamically! :)");
        }
    }
}
