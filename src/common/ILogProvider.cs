using System;
using System.Collections.Generic;

namespace common
{
    public enum LogLevel
    {
        Unknown,
        Debug,
        Information,
        Warning,
        Error
    }

    public interface ILogProvider
    {
        Dictionary<string, object> Properties { get; }

        object GetPropertyValue(string name, object defaultValue);

        ILogProvider WithProperty(string name, object value);

        void Reset();

        void PushContextInfo(string info);

        void PopContextInfo();

        void Write(string logName, LogLevel level, object message, Exception ex);
    }
}