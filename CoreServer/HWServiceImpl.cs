// Satellite-Communication-Server //

using SocketAppServer.ManagedServices;
using SocketAppServer.TelemetryServices;
using SocketAppServer.TelemetryServices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices.CoreServer
{
    internal class HWServiceImpl : IHardwareServices
    {
        ITelemetryServicesProvider telemetry = null;
        ICoreServerService coreServer = null;
        public HWServiceImpl()
        {
            IServiceManager manager = ServiceManager.GetInstance();
            telemetry = manager.GetService<ITelemetryServicesProvider>();
            coreServer = manager.GetService<ICoreServerService>();
        }

        public double AverageCPUUsage(double minutes = 3)
        {
            if (telemetry == null)
                return 0;

            DateTime startDate = (minutes == 0
             ? DateTime.Now.AddSeconds(-(minutes * 10))
             : DateTime.Now.AddMinutes(-minutes));
            DateTime endDate = DateTime.Now;
            IEnumerable<HardwareUsage> events = telemetry.GetHardwareUsages(startDate, endDate);

            if (events.Count() > 0)
                return events.Average(ev => ev.CPUUsage);//events.Average(evt => evt.CPUUsage);
            else
                return 0;
        }

        public double AverageMemoryUsage(double minutes = 3)
        {
            if (telemetry == null)
                return 0;

            DateTime startDate = (minutes == 0
             ? DateTime.Now.AddSeconds(-(minutes * 10))
             : DateTime.Now.AddMinutes(-minutes));
            DateTime endDate = DateTime.Now;
            IEnumerable<HardwareUsage> events = telemetry.GetHardwareUsages(startDate, endDate);
            if (events.Count() > 0)
                return events.Average(ev => ev.MemoryUsageMegabytes);// events.Average(evt => evt.MemoryUsageMegabytes);
            else
                return 0;
        }


        public void ReleaseMemory()
        {
            int retrieves = 0;
            while (coreServer.CurrentThreadsCount() > 0 && retrieves < 3)
            {
                Thread.Sleep(1000);
                retrieves += 1;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
