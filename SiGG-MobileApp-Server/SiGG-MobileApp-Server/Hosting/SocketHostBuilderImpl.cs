using SiGG_MobileApp_Server.CoreServices.CoreServer;
using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Hosting
{
    internal class SocketHostBuilderImpl : ISocketServerHostBuilder
    {
        private AppServerConfigurator configurator;
        public void Run()
        {
            IServiceManager manager = configurator.Services;
            configurator.ConfigureServices(manager);

            ServerConfiguration config = configurator.GetServerConfiguration();

            ICoreServerService server = manager.GetService<ICoreServerService>("realserver");
            server.SetConfiguration(config);
            server.Start();
        }

        public ISocketServerHostBuilder UseStartup<TStartup>() where TStartup : class
        {
            Type t = typeof(TStartup);
            configurator = (AppServerConfigurator)Activator.CreateInstance(t);
            return this;
        }
    }
}
