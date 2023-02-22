using SiGG_MobileApp_Server.CoreServices.CoreServer;
using SiGG_MobileApp_Server.ManagedServices;
using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.NetFxTestServer
{
    public class Startup : AppServerConfigurator
    {
        public override void ConfigureServices(IServiceManager serviceManager)
        {
            ICoreServerService coreServer = serviceManager.GetService<ICoreServerService>("realserver");
            coreServer.EnableBasicServerProcessorMode(typeof(BasicServer));
        }

        public override ServerConfiguration GetServerConfiguration()
        {
            return new ServerConfiguration(Encoding.ASCII, 1001);
        }
    }
}
