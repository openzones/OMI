using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using OMInsurance.Configuration;
using OMInsurance.Entities.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OMInsurance.Log
{
    public static class FileLog
    {
        private const string DefaultLoggerName = "DefaultLogger";

        private static ILog _logger = null;
        private static bool _firstError = true;
        private static string _appName = string.Empty;
        private static object _initializationLocker = new object();

        public static string CurrentSource
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName);
            }
        }

        private static RollingFileAppender CreateFileAppender(string appName)
        {
            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = "%d{yyyy-MM-dd HH:mm:ss.fff} %-5p [%C{1}] %m%n";
            layout.ActivateOptions();

            RollingFileAppender appender = new RollingFileAppender();
            string path = ConfiguraionProvider.LogPath;
            string fileSizeInMb = ConfiguraionProvider.FileSizeInMb;
            appender.AppendToFile = true;
            appender.File = Path.Combine(Path.Combine(path, appName), string.Format("{0}.log", appName));
            appender.ImmediateFlush = true;
            appender.Layout = layout;
            appender.MaximumFileSize = string.Format("{0}MB", fileSizeInMb);
            appender.MaxSizeRollBackups = 2;
            appender.RollingStyle = RollingFileAppender.RollingMode.Size;
            appender.StaticLogFileName = true;
            appender.LockingModel = new FileAppender.MinimalLock();

            LevelRangeFilter levelFilter = new LevelRangeFilter();
            levelFilter.LevelMin = Level.Info;
            levelFilter.LevelMax = Level.Fatal;

            appender.ErrorHandler = new EventLogErrorHandler();
            appender.AddFilter(levelFilter);
            appender.ActivateOptions();

            return appender;
        }
        
        public static void Initialize(string componentName)
        {
            if (_logger != null)
            {
                return;
            }

            lock (_initializationLocker)
            {
                _appName = componentName;
                if (_logger != null)
                {
                    return;
                }

                try
                {
                    InitForComponent(componentName);
                }
                catch (Exception e)
                {
                    string msg = string.Format("Log.Initialize() threw an exception: {0}{1}.", Environment.NewLine, e.ToString());
                    EventLog.WriteEntry(CurrentSource, msg, EventLogEntryType.Error);
                    throw;
                }
            }
        }
        
        private static void InitForComponent(string componentName)
        {
            _appName = componentName;

            Hierarchy hierarchy = log4net.LogManager.GetRepository() as Hierarchy;
            Logger log = hierarchy.Root;

            log.Level = Level.All;
            log.AddAppender(CreateFileAppender(componentName));
            _logger = LogManager.GetLogger("root");
            hierarchy.Configured = true;
        }

        private static Level GetLevel(string levelName)
        {
            switch (levelName.ToLower())
            {
                case "debug":
                    return Level.Debug;
                case "info":
                    return Level.Info;
                case "warn":
                    return Level.Warn;
                case "error":
                    return Level.Error;
                case "fatal":
                    return Level.Fatal;
            }
            return Level.Info;
        }

        private static void EventLogWriteErrorEntry(Exception e, string source)
        {
            if (_firstError)
            {
                _firstError = false;
                EventLog.WriteEntry(source, e.ToString(), EventLogEntryType.Error);
            }
        }

        private static void EventLogWriteErrorEntry(Exception e)
        {
            EventLogWriteErrorEntry(e, CurrentSource);
        }

        private static void EventLogWriteErrorEntry(string message, string source)
        {
            if (_firstError)
            {
                _firstError = false;
                EventLog.WriteEntry(source, message, EventLogEntryType.Error);
            }
        }

        private static void EventLogWriteErrorEntry(string message)
        {
            EventLogWriteErrorEntry(message, CurrentSource);
        }

        #region Debug Log

        public static void Debug(string message)
        {
            DebugBase(LogHelper.CreateLogMessage(message, _appName), null);
        }

        public static void Debug(string message, Exception e)
        {
            DebugBase(LogHelper.CreateLogMessage(message, _appName), e);
        }

        public static void Debug(Exception e)
        {
            DebugBase(LogHelper.CreateLogMessage(e.Message, _appName), e);
        }

        public static void Debug(LogMessage message, Exception e)
        {
            DebugBase(message, e);
        }

        private static void DebugBase(LogMessage message, Exception e)
        {
            try
            {
                if (_logger != null)
                {
                    if (e != null)
                    {
                        _logger.Debug(message, e);
                    }
                    else
                    {
                        _logger.Debug(message);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogWriteErrorEntry(ex);
            }
        }

        #endregion //Debug Log

        #region Info Log

        public static void Info(string message)
        {
            InfoBase(LogHelper.CreateLogMessage(message, _appName), null);
        }

        public static void Info(string message, Exception e)
        {
            InfoBase(LogHelper.CreateLogMessage(message, _appName), e);
        }

        public static void Info(Exception e)
        {
            InfoBase(LogHelper.CreateLogMessage(e.Message, _appName), e);
        }

        public static void Info(LogMessage message, Exception e)
        {
            InfoBase(message, e);
        }

        private static void InfoBase(LogMessage message, Exception e)
        {
            try
            {
                message.Severity = LogMessageSeverity.Info;
                if (_logger != null)
                {
                    if (e != null)
                    {
                        _logger.Info(message, e);
                    }
                    else
                    {
                        _logger.Info(message);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogWriteErrorEntry(ex);
            }
        }

        #endregion //Info Log

        #region Warning Log

        public static void Warning(string message)
        {
            WarningBase(LogHelper.CreateLogMessage(message, _appName), null);
        }

        public static void Warning(string message, Exception e)
        {
            WarningBase(LogHelper.CreateLogMessage(message, _appName), e);
        }

        public static void Warning(Exception e)
        {
            WarningBase(LogHelper.CreateLogMessage(e.Message, _appName), e);
        }

        public static void Warning(LogMessage message, Exception e)
        {
            WarningBase(message, e);
        }

        private static void WarningBase(LogMessage message, Exception e)
        {
            try
            {
                message.Severity = LogMessageSeverity.Warning;
                if (_logger != null)
                {
                    if (e != null)
                    {
                        _logger.Warn(message, e);
                    }
                    else
                    {
                        _logger.Warn(message);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogWriteErrorEntry(ex);
            }
        }

        #endregion //Warning Log

        #region Error Log

        public static void Error(string message)
        {
            ErrorBase(LogHelper.CreateLogMessage(message, _appName), null);
        }

        public static void Error(string message, Exception e)
        {
            ErrorBase(LogHelper.CreateLogMessage(message, _appName), e);
        }

        public static void Error(Exception e)
        {
            ErrorBase(LogHelper.CreateLogMessage(e.Message, _appName), e);
        }

        public static void Error(LogMessage message, Exception e)
        {
            ErrorBase(message, e);
        }

        private static void ErrorBase(LogMessage message, Exception e)
        {
            try
            {
                message.Severity = LogMessageSeverity.Error;
                if (_logger != null)
                {
                    if (_logger.IsErrorEnabled)
                    {
                        if (e != null)
                        {
                            _logger.Error(message, e);
                        }
                        else
                        {
                            _logger.Error(message);
                        }
                    }
                }
                else
                {
                    EventLogWriteErrorEntry(e);
                }
            }
            catch (Exception ex)
            {
                EventLogWriteErrorEntry(ex);
            }
        }

        #endregion //Error Log

        #region Fatal Log

        public static void Fatal(string message)
        {
            FatalBase(LogHelper.CreateLogMessage(message, _appName), null);
        }

        public static void Fatal(string message, Exception e)
        {
            FatalBase(LogHelper.CreateLogMessage(message, _appName), e);
        }

        public static void Fatal(Exception e)
        {
            FatalBase(LogHelper.CreateLogMessage(e.Message, _appName), e);
        }

        public static void Fatal(LogMessage message, Exception e)
        {
            FatalBase(message, e);
        }

        private static void FatalBase(LogMessage message, Exception e)
        {
            try
            {
                message.Severity = LogMessageSeverity.Fatal;
                if (_logger != null)
                {
                    if (_logger.IsFatalEnabled)
                    {
                        if (e != null)
                        {
                            _logger.Fatal(message, e);
                        }
                        else
                        {
                            _logger.Fatal(message);
                        }
                    }
                }
                else
                {
                    EventLogWriteErrorEntry("Logger does not exist.");
                }
            }
            catch (Exception ex)
            {
                EventLogWriteErrorEntry(ex);
            }
        }

        #endregion //Fatal Log

        #region Stack trace

        public static string GetStackTraceString()
        {
            StringBuilder sb = new StringBuilder(512);
            var frames = new System.Diagnostics.StackTrace(true).GetFrames();
            for (int i = 1; i < frames.Length; i++) /* Ignore current WriteStackTrace method...*/
            {
                StackFrame currFrame = frames[i];
                string fileName = currFrame.GetFileName();
                int lineNumber = currFrame.GetFileLineNumber();
                bool debugMode = fileName != null;
                MethodBase method = currFrame.GetMethod();
                if (method.ReflectedType != null)
                {
                    // Managed code
                    string arguments = GetArgumentsString(method);
                    string reflectionTypeName = method.ReflectedType.FullName;
                    if (debugMode)
                    {
                        sb.AppendFormat("at {0}.{1}({2}) in {3}:line {4}\n", reflectionTypeName, method.Name, arguments, fileName, lineNumber);
                    }
                    else
                    {
                        sb.AppendFormat("at {0}.{1}({2}) +0x{3:x}\n", reflectionTypeName, method.Name, arguments, currFrame.GetILOffset());
                    }
                }
                else
                {
                    if (debugMode)
                    {
                        sb.AppendFormat("at {0} in {1}:line {2}n", method.Name, fileName, lineNumber);
                    }
                    else
                    {
                        sb.AppendFormat("at {0} +0x{1:x}\n", method.Name, currFrame.GetNativeOffset());
                    }
                }
            }
            return sb.ToString();
        }

        private static string GetArgumentsString(MethodBase method)
        {
            return string.Join(
                    ", ",
                    method.GetParameters()
                        .Select(p => string.Format("{0} {1}", p.ParameterType.Name, p.Name)));
        }

        #endregion

    }
}