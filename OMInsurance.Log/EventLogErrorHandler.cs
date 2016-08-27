using log4net.Core;
using System;
using System.Diagnostics;

namespace OMInsurance.Log
{
    public class EventLogErrorHandler : IErrorHandler
    {
        public void Error(string message)
        {
            string msg = string.Format("Logger failed. {0}", message);
            EventLog.WriteEntry(FileLog.CurrentSource, msg, EventLogEntryType.Error);
        }

        public void Error(string message, Exception e)
        {
            string msg = string.Format("Logger failed. {0}. {1}", message, e.Message);
            EventLog.WriteEntry(FileLog.CurrentSource, msg, EventLogEntryType.Error);
        }

        public void Error(string message, Exception e, ErrorCode errorCode)
        {
            string msg = string.Format("Logger failed. {0}. ErrorCode: {1}. {2}", message, errorCode, e.Message);
            EventLog.WriteEntry(FileLog.CurrentSource, msg, EventLogEntryType.Error);
        }
    }
}
