﻿using System;
using System.Collections.Generic;

namespace lib.logging
{
    public interface ILogProvider
    {
        Dictionary<string, object> Properties { get; }

        object GetPropertyValue(string name, object defaultValue);

        ILogProvider WithProperty(string name, object value);

        void Reset();

        IDisposable PushContextInfo(string info);

        string PopContextInfo();

        void Write(string logName, LogLevel level, object message, Exception ex);
    }
}