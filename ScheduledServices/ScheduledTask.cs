// Satellite-Communication-Server //

using System;
using System.Threading.Tasks;
using System.Timers;

namespace SocketAppServer.ScheduledServices
{
    public abstract class ScheduledTask
    {
        public bool IsRunning { get; internal set; }
        public bool RunOnServerStart { get; internal set; }
        public string TaskName { get; internal set; }
        public ScheduledTaskInterval Interval { get; internal set; }
        public bool SaveState { get; internal set; }

        Timer timer = new Timer();
        public ScheduledTask(string taskName,
            bool runOnServerStart, ScheduledTaskInterval interval, bool saveState = true)
        {
            TaskName = taskName;
            Interval = interval;
            SaveState = saveState;
            RunOnServerStart = runOnServerStart;

            StartTimer(TryGetLastInterval());
        }

        public ScheduledTaskInterval TryGetLastInterval()
        {
            DateTime? nextEvent = ScheduleNextEventsRepository.Instance.GetNext(TaskName);
            ScheduledTaskInterval nextEventInterval = null;
            if (nextEvent != null)
            {
                var nextDate = (nextEvent - DateTime.Now);
                nextEventInterval = new ScheduledTaskInterval((int)nextDate.Value.Days,
                  (int)nextDate.Value.TotalHours, (int)nextDate.Value.TotalMinutes,
                  (int)nextDate.Value.TotalSeconds);

                if (nextEventInterval.Interval <= 0)
                    nextEvent = null;
            }

            return (nextEvent == null
                ? Interval
                : nextEventInterval);
        }

        private void StartTimer(ScheduledTaskInterval interval)
        {
            if (timer == null)
                timer = new Timer();
            timer.Interval = interval.Interval;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var service = new ScheduledTaskExecutorService();
            service.OnCompleted += Service_OnCompleted;
            service.Execute(this);
        }

        private void Service_OnCompleted(bool result)
        {
            if (timer != null)
            {
                timer.Stop();
                try
                {
                    timer.Dispose();
                }
                catch { }
                timer = null;
            }

            StartTimer(Interval);
        }

        public override string ToString()
        {
            return TaskName;
        }

        public abstract void RunTask();
    }
}
