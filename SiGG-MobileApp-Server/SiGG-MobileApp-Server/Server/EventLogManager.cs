using SiGG_MobileApp_Server.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    class EventLogManager
    {
        private static string DT_EVENTLOG_SYSTEM_LONGNAME = "DTS_SYSTEM";
        private static string DT_EVENTLOG_SYSTEM_SOURCENAME = "DTS_SYSTEM";
        private static string DT_EVENTLOG_LOGNAME = "DTS_INST%d";
        private static string DT_EVENTLOG_SOURCENAME = "Deal Tracker Server %d";

        private static string MT_EVENTLOG_SYSTEM_LOGNAME = "MTS_SYSTEM";
        private static string MT_EVENTLOG_SYSTEM_SOURCENAME = "MTS_SYSTEM";
        private static string MT_EVENTLOG_LOGNAME = "MTS_INST%d";
        private static string MT_EVENTLOG_SOURCENAME = "Market Tracker Server &d";

        private EventLog m_eventLogService = null;

        private static readonly EventLogManager instance = new EventLogManager();

        static EventLogManager()
        {
        }

        private EventLogManager()
        {
            Initialise();
        }

        public static EventLogManager Instance
        {
            get
            {
                return instance;
            }
        }

        private void Initialise()
        {
            var product = ConfigManager.Instance.GetProductName();

            if (product.ToUpper().Contains("MARKET TRACKER"))
            {
                m_eventLogService = new EventLog(MT_EVENTLOG_LOGNAME);
                m_eventLogService.Source = MT_EVENTLOG_SOURCENAME;
            }
            else
            {
                m_eventLogService = new EventLog(DT_EVENTLOG_LOGNAME);
                m_eventLogService.Source = DT_EVENTLOG_SOURCENAME;
            }
        }

        public void WriteEventLogService(string message, EventLogEntryType type, short category = 0)
        {
            m_eventLogService.WriteEntry(message, type, 0, category);
        }

        //https://www.eventsentry.com/blog/2010/11/creating-your-very-own-event-m.html
        public static void CreateEventSource(string name)
        {
            try
            {
                string logName = DT_EVENTLOG_LOGNAME;
                string sourceName = DT_EVENTLOG_SOURCENAME;
                string messageFile = "";

                if (name.Trim().Length > 0)
                {
                    if (name.ToUpper().Contains("MARKET TRACKER"))
                    {
                        logName = MT_EVENTLOG_LOGNAME;
                        sourceName = MT_EVENTLOG_SOURCENAME;
                    }
                }

                string path = Assembly.GetExecutingAssembly().Location;
                var index = path.IndexOf("Archiver.exe", StringComparison.OrdinalIgnoreCase);
                if (index > 0)
                {
                    path = path.Substring(0, index);
                    messageFile = path + "messages.dll";
                }

                // Create the event source if it does not exist.
                if (!EventLog.SourceExists(sourceName))
                {
                    // Create a new event source for the custom event log
                    // named "myNewLog."

                    EventSourceCreationData mySourceData = new EventSourceCreationData(sourceName, logName);

                    // Set the message resource file that the event source references.
                    // All event resource identifiers correspond to text in this file.
                    if (!System.IO.File.Exists(messageFile))
                    {
                        Console.WriteLine("Input message resource file does not exist - {0}",
                            messageFile);
                        messageFile = "";
                    }
                    else
                    {
                        // Set the specified file as the resource
                        // file for message text, category text, and
                        // message parameter strings.

                        mySourceData.MessageResourceFile = messageFile;
                        mySourceData.CategoryResourceFile = messageFile;
                        mySourceData.CategoryCount = 6;
                        mySourceData.ParameterResourceFile = messageFile;

                        Console.WriteLine("Event source message resource file set to {0}",
                            messageFile);
                    }

                    Console.WriteLine("Registering new source for event log.");
                    EventLog.CreateEventSource(mySourceData);

                    //Define max event log size
                    EventLog log = new EventLog(logName);
                    log.MaximumKilobytes = 200000;
                    log.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
                }
                else
                {
                    // Get the event log corresponding to the existing source.
                    logName = EventLog.LogNameFromSourceName(sourceName, ".");
                }

                /* No need to register */
                // Register the localized name of the event log.
                // For example, the actual name of the event log is "myNewLog," but
                // the event log name displayed in the Event Viewer might be
                // "Sample Application Log" or some other application-specific
                // text.
                //EventLog eventLog = new EventLog(logName, ".", sourceName);

                //if (messageFile.Length > 0)
                //{
                //eventLog.RegisterDisplayName(messageFile, 1);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CreateEventSource: " + ex.Message);
            }
        }

        public static void RemoveEventSource(string name)
        {
            try
            {
                string logName = "DT_ARCHIVER";

                if (name.Trim().Length > 0)
                {
                    if (name.ToUpper().Contains("MARKET TRACKER"))
                    {
                        logName = "MT_ARCHIVER";
                    }
                }

                #region Clear Event Log
                foreach (var eventLog in EventLog.GetEventLogs())
                {
                    try
                    {
                        if (eventLog.Log == logName)
                        {
                            eventLog.Clear();
                            eventLog.Dispose();
                            break;
                        }
                    }
                    catch
                    {
                        //Nothing to Do
                    }
                }
                #endregion

                EventLog.Delete(logName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in RemoveEventSource: " + ex.Message);
            }
        }
    }
}
