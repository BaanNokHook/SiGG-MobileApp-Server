// Sattelite-Communication-Server //

using SocketAppServer.ScheduledServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices
{
    public interface IScheduledTaskManager
    {
        void AddScheduledTask(ScheduledTask task);

        ScheduledTask GetTaskInfo(string taskName);

        void RunTaskAsync(ScheduledTask task);

        void RunTaskSync(ScheduledTask task);

        void RunServerStartupTasks();

        List<string> GetTaskList();
    }
}
