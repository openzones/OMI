using OMInsurance.Entities.Core;
using System;
using System.Diagnostics;
using System.Reflection;

namespace OMInsurance.Log
{
    public static class LogHelper
    {
        public static LogMessage CreateLogMessage(string message, string appName)
        {
            string className = null;
            string methodName = null;
            LogHelper.GetClassAndMethodNames(ref className, ref methodName);
            LogMessage logMessage = new LogMessage(className, methodName, message);
            logMessage.AppName = appName;
            return logMessage;
        }

        public static LogMessage CreateLogMessage(string message, Exception e, string appName)
        {
            string className = null;
            string methodName = null;
            GetClassAndMethodNames(ref className, ref methodName);
            LogMessage logMessage = new LogMessage(className, methodName, message);
            logMessage.AppName = appName;
            if (e != null)
            {
                logMessage.StackTrace = e.StackTrace;
            }
            return logMessage;
        }

        public static void GetClassAndMethodNames(ref string className, ref string methodName)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame;
            MethodBase stackFrameMethod;
            int frameCount = 0;
            do
            {
                frameCount++;
                if (frameCount > stackTrace.FrameCount)
                    break;
                stackFrame = stackTrace.GetFrame(frameCount);
                stackFrameMethod = stackFrame.GetMethod();
                className = stackFrameMethod.ReflectedType.Name;
                methodName = stackFrameMethod.Name;
            }
            while (className.Contains("Log"));
        }
    }
}
