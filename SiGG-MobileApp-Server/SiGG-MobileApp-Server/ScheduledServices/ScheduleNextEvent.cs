using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ScheduledServices
{
    internal class ScheduleNextEvent
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
