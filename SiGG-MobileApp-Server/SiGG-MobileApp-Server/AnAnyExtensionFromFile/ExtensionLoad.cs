using SiGG_MobileApp_Server.EFI;
using SiGG_MobileApp_Server.ManagedServices;
using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.AnAnyExtensionFromFile
{
    public class ExtensionLoad : IExtensibleFrameworkInterface
    {
        public string ExtensionName => "An any custom extension";
        public string ExtensionVersion => "1.0.0.0";
        public string ExtensionPublisher => "marcos8154@gmail.com";
        public string MinServerVersion => "2.0.26";

        public void Load(IServiceManager manager)
        {
            ILoggingService logging = manager.GetService<ILoggingService>();
            logging.WriteLog("Hello! Im a file-based extension loaded dinamically from SocketAppServer! :)");


            ICLIHostService cliHost = manager.GetService<ICLIHostService>();
            cliHost.RegisterCLICommand("file-x", "Run a funny function inside this beautifull extension :)", new FunnyCommand());
        }
    }
}
