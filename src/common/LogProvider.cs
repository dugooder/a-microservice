using log4net;
using System;
using System.Collections.Generic;

namespace common
{
    /// <summary>
    /// Log Provider hides the underlining logger implementation (log4net) and 
    /// provide a basic class that is intended to be extended.  Extension methods 
    /// like WriteException or WriteSessionExpiration() are expected to be written.
    /// 
    /// Using a small class and extentions keeps the unit testing fake class small and manageable.
    /// </summary>
    internal sealed class LogProvider : ILogProvider
    {
        static Dictionary<string, ILog> loggerCache = new Dictionary<string, ILog>();

        Dictionary<string, object> props;

        public LogProvider()
        {
            props = new Dictionary<string, object>();

            log4net.Config.XmlConfigurator.Configure(); // expects log4net config in EXE app.config

            PushContextInfo(string.Empty); // gets rid if (null) in NDC in file}
        }

        public Dictionary<string, object> Properties
        {
            get
            {
                return props;
            }
        }

        public void Reset()
        {
            props.Clear();
        }

        public object GetPropertyValue(string name, object defaultValue)
        {
            if (props.ContainsKey(name))
            {
                return props[name];
            }
            else
            {
                return defaultValue;
            }
        }

        public ILogProvider WithProperty(string name, object value)
        {
            props[name] = value;

            return this;
        }

        public void PushContextInfo(string info)
        {
            log4net.NDC.Push(info);
        }

        public void PopContextInfo()
        {
            log4net.NDC.Pop();
        }

        public void Write(string logName, LogLevel level, object message, Exception ex)
        {
            ILog logger = getLogger(logName);

            switch (level)
            {
                case LogLevel.Debug:
                    logger.Debug(message, ex);
                    break;
                case LogLevel.Error:
                    logger.Error(message, ex);
                    break;
                case LogLevel.Information:
                    logger.Info(message, ex);
                    break;
                case LogLevel.Warning:
                    logger.Warn(message, ex);
                    break;
                default:  //  LogLevel.Uknown
                    logger.Info(message, ex);
                    break;
            }

            Reset();
        }

        private static ILog getLogger(string logName)
        {
            ILog result = null;

            if (loggerCache.ContainsKey(logName))
            {
                result = loggerCache[logName];
            }
            else
            {
                result = LogManager.GetLogger(logName);
                loggerCache[logName] = result;
            }
            return result;
        }
    }
}