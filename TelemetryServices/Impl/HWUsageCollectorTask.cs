// Satellite-Communication-Server //

using SocketAppServer.CoreServices;
using SocketAppServer.ManagedServices;
using SocketAppServer.ScheduledServices;
using SocketAppServer.TelemetryServices.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketAppServer.TelemetryServices.Impl
{
    internal class HWUsageCollectorTask : ScheduledTask
    {
        private ITelemetryDataCollector telemetry;
        private ICoreServerService coreServer;
        public HWUsageCollectorTask()
            : base("HWUsageCollector", true, new ScheduledTaskInterval(0, 0, 0, 1), false)
        {
            IServiceManager manager = ServiceManager.GetInstance();
            telemetry = manager.GetService<ITelemetryDataCollector>();
            coreServer = manager.GetService<ICoreServerService>();
        }

        public override void RunTask()
        {
            double cpu = GetCPU();
            double memory = GetMemory();

            if (telemetry != null)
                telemetry.Collect(new HardwareUsage(cpu, memory, coreServer.CurrentThreadsCount()));
        }

        private double GetMemory()
        {
            double memory = GC.GetTotalMemory(false);
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = memory;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            //   return $"{len.ToString("N3")}{sizes[order]}";
            return double.Parse(len.ToString("N3"));
        }

        private double GetCPU()
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            Thread.Sleep(500);
            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
            return double.Parse((cpuUsageTotal * 100).ToString("N2"));

            //return $"{(cpuUsageTotal * 100).ToString("N2")}%";
        }
    }
}
