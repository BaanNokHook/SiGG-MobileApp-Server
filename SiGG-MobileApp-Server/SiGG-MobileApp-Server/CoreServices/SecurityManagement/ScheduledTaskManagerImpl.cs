using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.CoreServices.SecurityManagement
{
    internal class ScheduledTaskManagerImpl : IScheduledTaskManager 
    {
        private List<ScheduledTask> Tasks { get; set; }

        public ScheduledTaskManagerImpl()
        {
            Tasks = new List<ScheduledTask>(10);
        }

        public void AddScheduledTask(ScheduledTask task)
        {
            if (string.IsNullOrEmpty(task.TaskName))
                throw new Exception("Task name is empty");
            if (task.Interval == null)
                throw new Exception("Task interval is null");
            Tasks.Add(task);
        }

        public ScheduledTask GetTaskInfo(string taskName)
        {
            return Tasks.FirstOrDefault(t => t.TaskName.Equals(taskName));
        }

        public List<string> GetTaskList()
        {
            return Tasks.Select(t => t.TaskName).ToList();
        }

        public void RunTaskAsync(ScheduledTask task)
        {
            if (task == null)
                throw new Exception("Task is null.");
            ScheduledTaskExecutorService service = new ScheduledTaskExecutorService();
            service.Execute(task);
        }

        public void RunServerStartupTasks()
        {
            var startupTasks = Tasks.Where(t => t.RunOnServerStart).ToList();
            startupTasks.ForEach(t => RunTaskAsync(t));
        }

        private object lckSyncRun = new object();

        public void RunTaskSync(ScheduledTask task)
        {
            if (task.IsRunning)
                return;
            lock (lckSyncRun)
            {
                task.IsRunning = true;
                task.RunTask();
                task.IsRunning = false;
            }
        }
    }
}