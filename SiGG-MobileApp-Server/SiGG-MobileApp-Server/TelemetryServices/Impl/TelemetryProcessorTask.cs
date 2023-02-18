using SiGG_MobileApp_Server.ManagedServices;
using SiGG_MobileApp_Server.ScheduledServices;
using SocketAppServer.CoreServices.Logging;
using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.TelemetryServices.Impl
{
    public class TelemetryProcessorTask : ScheduledTask    
    {
        private ITelemetryServicesProvider telemetry;
        private ITelemetryDataCollector collector;
        private ILoggingService logging;

        public TelemetryProcessorTask()   
            : base("TelemetryProcessor", false, new ScheduledTaskInterval(0, 0, 0, 30), false)
        {
            IServiceMamnager manager = ServiceManager.GetInstance();
            telemetry = manager.GetService<ITelemetryServicesProvider>();
            collector = manager.GetService<ITelemetryDataCollector>();
            logging = manager.GetService<ILoggingService>();   
        }
        private object lck = new object();

        public override void RunTask()
        {
            lock (lck)
            {
                try
                {
                    telemetry.ActionExecutionTime(collector.GetActionExecutions());
                    telemetry.ActionError(collector.GetActionErrors());
                    telemetry.HWUsage(collector.GetHardwareUsages());
                    telemetry.DependencyInjectionExecutiontime(collector.GetDependencyInjector());
                    telemetry.InterceptorExecutionTime(collector.GetInterceptorExecutions());    
                }  
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    if (ex.InnerException != null)
                        msg += $"\n{ex.InnerException.Message}";
                    logging.WriteLog($"TelemetryProcessorTask error: {msg}", ServerLogType.ERROR);   
                }

            }
        }

    }
}