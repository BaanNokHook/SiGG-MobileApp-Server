using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (System.Environment.UserInteractive)
            {
                if (args.Length > 0)
                {
                    switch (args[0])
                    {
                        case "-install":
                            {
                                if (args.Length >= 3)
                                {
                                    string serviceName = "";
                                    string displayName = "";
                                    string name = "";

                                    if (args[1].IndexOf("-name=", StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        serviceName = "/ServiceName=" + args[1].Substring(6).Trim();
                                    }

                                    if (args[2].IndexOf("-displayname=", StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        displayName = "/DisplayName=" + args[2].Substring(13).Trim();
                                        name = args[2].Substring(13).Trim();
                                    }

#if DEBUG
                                    System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { serviceName, displayName, Assembly.GetExecutingAssembly().Location });
#else
                                    System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { serviceName, displayName, "/LogFile=", "/LogToConsole=true", Assembly.GetExecutingAssembly().Location });
#endif

                                    //EventLogManager.CreateEventSource(name); /*sceaky*/
                                }
                            }
                            break;

                        case "-remove":
                            {
                                if (args.Length >= 3)
                                {
                                    string serviceName = "";
                                    string name = "";

                                    if (args[1].IndexOf("-name=", StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        serviceName = "/ServiceName=" + args[1].Substring(6).Trim();
                                    }

                                    if (args[2].IndexOf("-displayname=", StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        name = args[2].Substring(13).Trim();
                                    }

                                    //Installer must send args[2] when removing service.
                                    //EventLogManager.RemoveEventSource(name); /*sceaky*/

#if DEBUG
                                    System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { serviceName, "/u", Assembly.GetExecutingAssembly().Location });
#else
                                    System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { serviceName, "/u", "/LogFile=", "/LogToConsole=true", Assembly.GetExecutingAssembly().Location });
#endif
                                }
                            }
                            break;

                        case "-debug":
                            {
                                Server archiver = new Server();
                                archiver.OnDebug();
                                Thread.Sleep(Timeout.Infinite);
                            }
                            break;
                    }
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new Server(args)
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
