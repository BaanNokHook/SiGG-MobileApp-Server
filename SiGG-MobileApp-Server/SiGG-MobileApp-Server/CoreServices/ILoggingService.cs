// Sattelite-Communication-Server //

using SocketAppServer.CoreServices.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices
{
    public interface ILoggingService
    {
        void WriteLog(string logText, ServerLogType type = ServerLogType.INFO);
        void WriteLog(string logText, string controller, string action, ServerLogType type = ServerLogType.INFO);
        void SetWrapper(ILoggerWrapper loggerWrapper);
    }
}
