using SiGG_MobileApp_Server.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.NetFxTestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Client.Configure("localhost", 4050, Encoding.UTF8, 1000000);
            Handle();
            */
            Console.ForegroundColor = ConsoleColor.White;
            SocketServerHost.CreateHostBuilder()
                   .UseStartup<Startup>()
                   .Run();
        }
    }
}
