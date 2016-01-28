using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    /// <summary>
    ///  Log Provider Extensions makes the log provider class usable.  
    ///  While Log Provider can be used by many applications the 
    ///  extensions would be custom to an application.
    /// </summary>
    public static class LogProviderExtensions
    {
        public static readonly string PropertyKeyLogLevel = "LogLevel";
        public const string PropertyKeyLogName = "LogName";

        const string KeyValuePairFormat = "{0}:{1};";

        public static ILogProvider WithLogLevel(this ILogProvider provider, LogLevel level)
        {
            provider.WithProperty(PropertyKeyLogLevel, level);
            return provider;
        }

        public static ILogProvider WithLogger(this ILogProvider provider, string logName)
        {
            provider.WithProperty(PropertyKeyLogName, logName);
            return provider;
        }

        public static void WriteGeneralException(this ILogProvider provider, Exception ex)
        {
            provider.Write(getLogName(provider), getLogLevel(provider), null, ex);
        }

        public static void WriteMessage(this ILogProvider provider, string message)
        {
            provider.Write(getLogName(provider), getLogLevel(provider), message, null);
        }

        public static void WriteMessage(this ILogProvider provider, string format, params object[] args)
        {
            string message = string.Format(format, args);

            provider.Write(getLogName(provider), getLogLevel(provider), message, null);
        }

        public static void WriteProperties(this ILogProvider provider)
        {
            provider.Write(getLogName(provider), getLogLevel(provider), convertPropertiestoString(provider), null);
        }

        private static string getLogName(ILogProvider provider)
        {
            return (string) provider.GetPropertyValue(
                PropertyKeyLogName, AppDomain.CurrentDomain.FriendlyName);
        }

        private static LogLevel getLogLevel(ILogProvider provider)
        {
            return (LogLevel) provider.GetPropertyValue(
                PropertyKeyLogLevel, LogLevel.Unknown);
        }

        private static string convertPropertiestoString(ILogProvider provider)
        {
            StringBuilder result = new StringBuilder();

            Dictionary<string, object> props = provider.Properties;

            foreach (string key in props.Keys)
            {
                result.AppendFormat(KeyValuePairFormat, key, props[key].ToString());
            }

            return result.ToString();
        }
    }
}