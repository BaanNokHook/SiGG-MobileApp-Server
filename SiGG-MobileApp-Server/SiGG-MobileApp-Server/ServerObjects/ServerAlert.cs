using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ServerObjects
{
    public class ServerAlert
    {
        public DateTime Date { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Message { get; set; }

        public ServerAlert(string controller, string action,
            string message)
        {
            Date = DateTime.Now;
            Controller = controller;
            Action = action;
            Message = message;
        }

        public ServerAlert()
        {
            Date = DateTime.Now;
        }
    }

    public class ServerAlertManager
    {
        private static object readLock = new object();
        internal static List<ServerAlert> Load()
        {
            lock (readLock)
            {
                string file = @".\ServerAlerts.json";
                try
                {
                    if (!File.Exists(file))
                        return new List<ServerAlert>();

                    string txt = File.ReadAllText(file);
                    List<ServerAlert> json = JsonConvert.DeserializeObject<List<ServerAlert>>(txt, AppServerConfigurator.SerializerSettings);
                    return json;
                }
                catch
                {
                    //file was corrupted
                    File.Delete(file);
                    return new List<ServerAlert>();
                }
            }
        }

        private static object writeLock = new object();
        public static void CreateAlert(ServerAlert alert)
        {
            lock (writeLock)
            {
                string file = @".\ServerAlerts.json";
                var alerts = Load();

                if (alerts.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nYour server has {alerts.Count} alerts!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                alerts.Add(alert);
                File.WriteAllText(file, JsonConvert.SerializeObject(alerts, AppServerConfigurator.SerializerSettings));
            }
        }
    }
}
