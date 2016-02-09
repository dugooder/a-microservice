using log4net;
using System;
using System.Collections.Generic;

namespace lib.logging
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

            log4net.Util.SystemInfo.NullText = string.Empty;  //TODO: Not sure I like this, all Nulls are "". It does get rid of (null) in the NDC.
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

        /// <summary>
        /// Use the context in two ways.
        /// 1.  using (IDisposable logctx = log.PushContextInfo("message here") { }
        /// 2.  Call Push() try {}  Finally {Pop() }
        ///</summary>
        /// <param name="info">any text you want to appear in the NDC property of the log</param>
        /// <returns>
        /// An object allowing the user of the 'using' statement to enforce poping the 
        /// value off the context stack. Not the normal IDisposale usage; not about releasing memory
        /// </returns>
        public IDisposable PushContextInfo(string info)
        {
            return log4net.NDC.Push(info);
        }

        public string PopContextInfo()
        {
            return log4net.NDC.Pop();
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

        static ILog getLogger(string logName)
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