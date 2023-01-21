// Satellite-Communication-Server //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.ScheduledServices
{
    internal  class ScheduleNextEvent
    {
        public string TaskName { get; set; }

        public DateTime NextEvent { get; set; }

        public ScheduleNextEvent(string taskName, DateTime nextEvent)
        {
            TaskName = taskName;
            NextEvent = nextEvent;
        }
    }
}
