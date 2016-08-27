using OMInsurance.Entities.Core;
using System;
using System.Diagnostics;
using System.IO;

namespace OMInsurance.Log
{
    public static class AsyncLogger
    {
        private static LogMessageQueue messageQueue = new LogMessageQueue();
        private static string applicationName = string.Empty;

        public static void Initialize(string appName)
        {
            applicationName = appName;
        }

        public static string CurrentSource
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName);
            }
        }

        #region Debug Log

        public static void Debug(string message, Exception e)
        {
            DebugBase(message, e);
        }

        public static void Debug(string message)
        {
            DebugBase(message, null);
        }

        public static void Debug(Exception e)
        {
            DebugBase(e.Message, e);
        }

        private static void DebugBase(string message, Exception e)
        {
            LogMessage msg = LogHelper.CreateLogMessage(message, e, applicationName);
            try
            {
                if (messageQueue != null)
                {
                    msg.Severity = LogMessageSeverity.Debug;
                    messageQueue.EnqueueEvent(msg);
                }
            }
            catch (Exception ex)
            {
                FileLog.Debug(message, e);
                FileLog.Fatal(ex);
            }
        }

        #endregion //Debug Log
        
        #region Info Log

        public static void Info(string message, Exception e)
        {
            InfoBase(message, e);
        }

        public static void Info(string message)
        {
            InfoBase(message, null);
        }

        public static void Info(Exception e)
        {
            InfoBase(e.Message, e);
        }

        private static void InfoBase(string message, Exception e)
        {
            LogMessage msg = LogHelper.CreateLogMessage(message, e, applicationName);
            try
            {
                if (messageQueue != null)
                {
                    msg.Severity = LogMessageSeverity.Info;
                    messageQueue.EnqueueEvent(msg);
                }
            }
            catch (Exception ex)
            {
                FileLog.Info(message, e);
                FileLog.Fatal(ex);
            }
        }

        #endregion //Info Log
        
        #region Warning Log

        public static void Warning(string message, Exception e)
        {
            WarningBase(message, e);
        }

        public static void Warning(string message)
        {
            WarningBase(message, null);
        }

        public static void Warning(Exception e)
        {
            WarningBase(e.Message, e);
        }

        private static void WarningBase(string message, Exception e)
        {
            LogMessage msg = LogHelper.CreateLogMessage(message, e, applicationName);
            try
            {
                if (messageQueue != null)
                {
                    msg.Severity = LogMessageSeverity.Warning;
                    messageQueue.EnqueueEvent(msg);
                }
            }
            catch (Exception ex)
            {
                FileLog.Warning(message, e);
                FileLog.Fatal(ex);
            }
        }

        #endregion //Warning Log
        
        #region Error Log

        public static void Error(string message, Exception e)
        {
            ErrorBase(message, e);
        }

        public static void Error(string message)
        {
            ErrorBase(message, null);
        }

        public static void Error(Exception e)
        {
            ErrorBase(e.Message, e);
        }

        private static void ErrorBase(string message, Exception e)
        {
            LogMessage msg = LogHelper.CreateLogMessage(message, e, applicationName);
            try
            {
                if (messageQueue != null)
                {
                    msg.Severity = LogMessageSeverity.Error;
                    messageQueue.EnqueueEvent(msg);
                }
            }
            catch (Exception ex)
            {
                FileLog.Error(message, e);
                FileLog.Fatal(ex);
            }
        }

        #endregion //Error Log

        #region Fatal Log

        public static void Fatal(string message, Exception e)
        {
            FatalBase(message, e);
        }

        public static void Fatal(string message)
        {
            FatalBase(message, null);
        }

        public static void Fatal(Exception e)
        {
            FatalBase(e.Message, e);
        }

        private static void FatalBase(string message, Exception e)
        {
            LogMessage msg = LogHelper.CreateLogMessage(message, e, applicationName);
            try
            {
                if (messageQueue != null)
                {
                    msg.Severity = LogMessageSeverity.Fatal;
                    messageQueue.EnqueueEvent(msg);
                }
            }
            catch (Exception ex)
            {
                FileLog.Fatal(message, e);
                FileLog.Fatal(ex);
            }
        }

        #endregion //Error Log
    }
}
