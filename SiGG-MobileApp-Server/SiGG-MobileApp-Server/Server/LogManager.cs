using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public sealed class LogManager
    {
        private static readonly LogManager instance = new LogManager();

        private readonly object lockWrite = new object();

        private static bool m_isLogToFile;
        private static string m_filePath;

        public static short LOG_CATEGORY_TOF = 1;
        public static short LOG_CATEGORY_DTF = 2;
        public static short LOG_CATEGORY_DTFE = 3;
        public static short LOG_CATEGORY_DATABASE = 4;
        public static short LOG_CATEGORY_BACKFILLING = 5;
        public static short LOG_CATEGORY_SERVICE = 6;

        public static EventLogLevel m_eventLogLevel;

        public enum EventLogLevel
        {
            EventLogLevel_Detail = 2,
            EventLogLevel_Debug = 104
        }

        public enum mxLogSeverity
        {
            mxLogInfo = 0,
            mxLogWarning = 1,
            mxLogError = 2
        };

        static LogManager()
        {
        }

        private LogManager()
        {
            Initialise();
        }

        public static LogManager Instance
        {
            get
            {
                return instance;

            }
        }

        private void Initialise()
        {
            m_isLogToFile = false; //Set value is true, in case need to log file.

            if (m_isLogToFile)
            {
                m_filePath = string.Format(@"{0}\{1}", ConfigManager.Instance.GetServerPath(), "server.log");

                if (File.Exists(m_filePath))
                {
                    File.Delete(m_filePath);
                }
            }
        }

        public void SetEventLogLevel(string logLv)
        {
            m_eventLogLevel = EventLogLevel.EventLogLevel_Detail;

            if (Enum.TryParse(logLv, out m_eventLogLevel) == false)
            {
                m_eventLogLevel = EventLogLevel.EventLogLevel_Detail;
            }
        }

        public EventLogLevel GetEventLogLevel()
        {
            return m_eventLogLevel;
        }

        public void WriteLog(EventLogEntryType type, string message, short category)
        {
            try
            {
                lock (lockWrite)
                {
                    //Prevent Event Log's Exception: Log entry string is too long. A string written to the event log cannot exceed 32766 characters.
                    if (message.Length > 32766)
                    {
                        message = message.Substring(0, 32766 - 1);
                    }

                    #region Log to Console (for debug) & Log to File
                    string logMessage = "";

                    switch (type)
                    {
                        case EventLogEntryType.Information:
                            {
                                logMessage = string.Format("[INFO] {0}", message);
                            }
                            break;

                        case EventLogEntryType.Warning:
                            {
                                logMessage = string.Format("[WARNING] {0}", message);
                            }
                            break;

                        case EventLogEntryType.Error:
                            {
                                logMessage = string.Format("[ERROR] {0}", message);
                            }
                            break;
                    }

                    string dateTimeStamp = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss");
                    logMessage = dateTimeStamp + " " + logMessage;

                    #region Log to Console (for debug)
                    Console.WriteLine(logMessage);
                    #endregion

                    #region Log to File
                    if (m_isLogToFile)
                    {
                        // This text is added only once to the file.
                        if (!File.Exists(m_filePath))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(m_filePath))
                            {
                                sw.WriteLine(logMessage);
                            }
                        }
                        else
                        {
                            // This text is always added, making the file longer over time
                            // if it is not deleted.
                            using (StreamWriter sw = File.AppendText(m_filePath))
                            {
                                sw.WriteLine(logMessage);
                            }
                        }
                    }
                    #endregion
                    #endregion

                    #region Log to EventLog
                    EventLogManager.Instance.WriteEventLogService(message, type, category);
                    #endregion
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("[Error] Cannot WriteLog to file. {0}", ex.Message);
            }
        }
    }
}

