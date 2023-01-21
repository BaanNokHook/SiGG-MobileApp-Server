// DT Software //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketAppServer.Hosting;

namespace SocketAppServer
{
    public class SocketServerHost
    {
        public static ISocketServerHostBuilder CreateHostBuilder()
        {
            return new SocketHostBuilderImpl();
        }
    }
}
