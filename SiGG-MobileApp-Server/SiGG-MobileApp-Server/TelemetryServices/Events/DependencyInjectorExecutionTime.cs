using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.TelemetryServices.Events
{
    public struct DependencyInjectorExecutionTime   
    {
        public string ControllerName { get; private set; }   

        public string ElapsedMs { get; }   

        public DateTime CalledTime { get; private set; }

        public DependencyInjectorExecutionTime(string controllerName,
            long elapsedMs)
        {
            ControllerName = controllerName;
            ElapsedMs = $"{elapsedMs} ms";
            CalledTime = DateTime.Now;
        }
    }
}