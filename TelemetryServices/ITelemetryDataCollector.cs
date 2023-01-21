// Satellite-Communication-Server //

using SocketAppServer.TelemetryServices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.TelemetryServices
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
