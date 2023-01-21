// DT Software //

using SocketAppServer.ManagedServices;
using System;

namespace SocketAppServer.CoreServices.Logging
{
    public class LoggingServiceImpl : ILoggingService
    {
        private static object lckObj = new object();
        private ILoggerWrapper loggerWrapper = null;
        private ICLIHostService cliHost = null;
        public LoggingServiceImpl()
        {
            cliHost = ServiceManager.GetInstance().GetService<ICLIHostService>();
        }

        internal void WriteLogInternal(ServerLog log)
        {
            if (!cliHost.IsCLIBusy())
                Console.WriteLine($"[{log.EventDate} {log.Type}]: {log.LogText}");
            if (loggerWrapper != null)
                lock (lckObj)
                    loggerWrapper.Register(ref log);
            log = null;
        }

        public void WriteLog(string logText, ServerLogType type = ServerLogType.INFO)
        {
            WriteLogInternal(new ServerLog(logText, type));
        }

        public void WriteLog(string logText, string controller, string action, ServerLogType type = ServerLogType.INFO)
        {
            WriteLogInternal(new ServerLog(logText, controller, action, type));
        }

        public void SetWrapper(ILoggerWrapper loggerWrapper)
        {
            this.loggerWrapper = loggerWrapper;
        }
    }
}
