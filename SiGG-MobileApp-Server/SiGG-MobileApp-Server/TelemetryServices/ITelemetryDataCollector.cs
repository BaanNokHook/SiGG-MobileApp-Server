using SiGG_MobileApp_Server.TelemetryServices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.TelemetryServices
{
    internal interface ITelemetryDataCollector
    {
        void Collect(object eventObj);   
        
            IEnumerable<ActionError> GetActionErros();
            IEnumerable<ActionExecutionTime> GetActionExecutions();
            IEnumerable<DependencyInjectorExecutionTime> GetDependencyInjectors();
            IEnumerable<HardwareUsage> GetHardwareUsages();
            IEnumerable<InterceptorExecutionTime> GetInterceptorExecutions();
        
    }
}