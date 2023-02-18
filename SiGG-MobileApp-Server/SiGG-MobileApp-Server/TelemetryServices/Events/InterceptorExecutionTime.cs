using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.TelemetryServices.Events
{
    public struct InterceptorExecutionTime
    {
        public string ControllerName { get; private set; }

        public string ActionName { get; private set; }

        public DateTime CollectTime { get; private set; }

        public string ElapsedMs { get; private set; }

        public InterceptorExecutionTime(string controllerName, string actionName,
            long elapsedMs)
        {
            ControllerName = controllerName;
            ActionName = actionName;
            ElapsedMs = $"{elapsedMs} ms";
            CollectTime = DateTime.Now;
        }
    }
}