// Satellite-Communication-Server //

using SocketAppServer.CoreServices;
using SocketAppServer.CoreServices.CoreServer;
using SocketAppServer.ManagedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.Hosting
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
