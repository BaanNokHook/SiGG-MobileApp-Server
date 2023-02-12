using SiGG_MobileApp_Server.ManagedServices;
using SocketAppServer.CoreServices.Logging;
using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ScheduledServices
{
    internal class ScheduledTaskExecutorService : AsyncTask<ScheduledTask, string, bool>
    {
        private ScheduledTask Task { get; set; }

        private ILoggingService logger = null;
        public ScheduledTaskExecutorService()
        {
            logger = ServiceManager.GetInstance().GetService<ILoggingService>();
        }

        public override bool DoInBackGround(ScheduledTask task)
        {
            if (task.IsRunning)
                return false;
            Task = task;
            task.IsRunning = true;
            try
            {
                //     telemetrizar o tempo de execução das tasks
                task.RunTask();
                task.IsRunning = false;
                return true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += $"\n{ex.InnerException.Message}";
                logger.WriteLog($"ScheduledTask execution problem: \nTaskName:{task.TaskName} \nErrorMessage: {msg}", ServerLogType.ERROR);
                task.IsRunning = false;
                return false;
            }
        }

        public override void OnPostExecute(bool result)
        {
            if (Task == null)
                return;
            if (!Task.SaveState)
                return;
            ScheduleNextEventsRepository.Instance.SetNext(Task.TaskName,
                Task.Interval);
        }

        public override void OnPreExecute()
        {
        }

        public override void OnProgressUpdate(string progress)
        {
        }
    }
}
