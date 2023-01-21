// DT Software //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.TelemetryServices.Events
{
    public struct ActionError
    {
        public string ControllerName { get; private set; }
        public string ActionName { get; private set; }
        public DateTime CalledTime { get; private set; }
        public string Error { get; private set; }

        public ActionError(string controllerName, string actionName, string error)
        {
            ControllerName = controllerName;
            ActionName = actionName;
            Error = error;
            CalledTime = DateTime.Now;
        }
    }
}
