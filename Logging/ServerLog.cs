// Satellite-Communication-Server //

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketAppServer.CoreServices.Logging
{
    public sealed class ServerLog
    {
        [JsonProperty]
        public DateTime EventDate { get; internal set; }

        [JsonProperty]
        public string LogText { get; internal set; }

        [JsonProperty]
        public string ControllerName { get; internal set; }

        [JsonProperty]
        public string ActionName { get; internal set; }

        [JsonProperty]
        public ServerLogType Type { get; internal set; }

        public ServerLog(string logText, ServerLogType type = ServerLogType.INFO)
        {
            EventDate = DateTime.Now;
            LogText = logText;
            Type = type;
        }

        public ServerLog(string logText, string controller, string action, ServerLogType type = ServerLogType.INFO)
        {
            EventDate = DateTime.Now;
            LogText = logText;
            ControllerName = controller;
            ActionName = action;
            Type = type;
        }

        public ServerLog()
        {
            EventDate = DateTime.Now;
        }
    }
}
