using System;
namespace OMInsurance.Entities.Core
{
    public class LogMessage
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string Message { get; set; }
        public string AppName { get; set; }
        public LogMessageSeverity Severity { get; set; }
        public string StackTrace { get; set; }
        public DateTime? CreateDate { get; set; }

        public LogMessage()
        {
        }

        public LogMessage(LogMessage msg)
        {
            ClassName = msg.ClassName;
            MethodName = msg.MethodName;
            Message = msg.Message;
            AppName = msg.AppName;
            Severity = msg.Severity;
            StackTrace = msg.StackTrace;
            CreateDate = msg.CreateDate;
        }

        public LogMessage(string className, string methodName, string message)
        {
            ClassName = className;
            MethodName = methodName;
            Message = message;
            CreateDate = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1} : {2}", ClassName, MethodName, Message);
        }
    }

    public enum LogMessageSeverity
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }
}
