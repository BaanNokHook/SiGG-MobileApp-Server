using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiGG_MobileApp_Server.CoreServices.EFIManagement
{
    internal class EFIManagerImpl : IEFIManager
    {
        private List<IExtensibleFrameworkInterface> Extension { get; set; }  
        private ILoggingService logger = null
        
        public EFIManagerImpl()
        {
            Extensions = new List<IntensibleFrameworkInterface>(5);
            logger = ServiceManager.GetInstance().GetService<ILoggingService>();
        }

        public void AddExtension(IExtensibleFrameworkInterface extension)
        {
            Extensions.Add(extension);
        }

        public void LoadAll()
        {
            IServiceManager manager = ServiceManager.GetInstance();
            ICoreServerService coreServer = manager.GetService<ICoreServerService>();
            double serverVersion;
            double.TryParse(coreServer.GetServerVersion(), out serverVersion);

            foreach (IExtensibleFrameworkInterface extension in Extensions)
            {
                try
                {
                    if (string.IsNullOrEmpty(extension.ExtensionName))
                        throw new Exception($"Cannot be load unknown extension from assembly '{extension.GetType().Assembly.FullName}'");
                    if (string.IsNullOrEmpty(extension.ExtensionVersion))
                        throw new Exception($"Cannot be read extension version for '{extension.ExtensionName}'");
                    if (string.IsNullOrEmpty(extension.ExtensionPublisher))
                        throw new Exception($"Cannot be load unknown publisher extension for '{extension.ExtensionName}'");

                    double minServerVersion = double.Parse(extension.MinServerVersion);
                    if (serverVersion < minServerVersion)
                        throw new Exception($"The extension '{extension.ExtensionName}' could not be loaded because it requires server v{extension.MinServerVersion}");

                    Console.ForegroundColor = ConsoleColor.Green;
                    logger.WriteLog($"      => Loading extension '{extension.ExtensionName}'");
                    logger.WriteLog($"      => version {extension.ExtensionVersion}");
                    logger.WriteLog($"      => by {extension.ExtensionPublisher}");
                    extension.Load(manager);
                    logger.WriteLog($"      => Extension '{extension.ExtensionName}' successfully loaded");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    logger.WriteLog($"Extension '{extension.ExtensionName}' fail to load: {ex.Message}", Logging.ServerLogType.ERROR);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        public void AddExtensionFromDisk(string extensionPath)
        {
            try
            {
                IExtensibleFrameworkInterface efi = null;

                Assembly assembly = Assembly.LoadFile(extensionPath);
                Type[] types = assembly.GetTypes();

                for (int i = 0; i < types.Length; i++)
                {
                    if (types[i].GetInterface(typeof(IExtensibleFrameworkInterface).FullName) != null)
                    {
                        efi = (IExtensibleFrameworkInterface)Activator.CreateInstance(types[i]);
                        break;
                    }
                }

                AddExtension(efi);
            }
            catch (Exception ex)
            {
                throw new Exception("An implementation for 'IExtensibleFrameworkInterface' could not be found in the specified assembly.");
            }
        }
    }
}
