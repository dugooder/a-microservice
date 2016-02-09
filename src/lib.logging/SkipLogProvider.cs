using System;
using System.Collections.Generic;

namespace lib.logging
{
    // Returned by the LogProviderExtensions If() function
    internal sealed class SkipLogProvider : ILogProvider
    {
        public Dictionary<string, object> Properties
        {
            get
            {
                return new Dictionary<string, object>();
            }
        }

        public object GetPropertyValue(string name, object defaultValue)
        {
            return defaultValue;
        }

        public string PopContextInfo()
        {
            return string.Empty;
        }

        public IDisposable PushContextInfo(string info)
        {
            return new FakeIDisposableClass();
        }

        public void Reset()
        {
            // do nothing
        }

        public ILogProvider WithProperty(string name, object value)
        {
            return this;
        }

        public void Write(string logName, LogLevel level, object message, Exception ex)
        {
            // do nothing
        }

        class FakeIDisposableClass : IDisposable
        {
            public void Dispose()
            {
              // do nothing
            }
        }
    }

   
}
