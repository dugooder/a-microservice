using System;
using Xunit;
using Xunit.Abstractions;
using common;

namespace tests
{
    public class LogProviderExtentionsTests : BaseTest
    {
        public LogProviderExtentionsTests(ITestOutputHelper output) 
            : base(output) { }

        [Fact]
        public void LogLevelTest()
        {
            this.FakeLogger.WithLogLevel(LogLevel.Debug);
            LogLevel actual = (LogLevel) this.FakeLogger.Properties[LogProviderExtensions.PropertyKeyLogLevel];
            Assert.Equal(LogLevel.Debug, actual);
        }

        [Fact]
        public void LoggerTest()
        {
            this.FakeLogger.WithLogger("BigDog");
            string actual = this.FakeLogger.Properties[LogProviderExtensions.PropertyKeyLogName] as string;
            Assert.Equal("BigDog", actual);
        }

        [Fact]
        public void GeneralExceptionTest()
        {
            DuplicateWaitObjectException ex = new DuplicateWaitObjectException("dog");
            this.FakeLogger.WithLogLevel(LogLevel.Error).WriteGeneralException(ex);
            Assert.Equal(ex, this.FakeLogger.LastLogEntry.Exception);
            Assert.Equal(LogLevel.Error, this.FakeLogger.LastLogEntry.LogLevel);
        }

        [Fact]
        public void WriteMessageTest()
        {
            string msg = "Dogs rule! Cats drool!";
            this.FakeLogger.WriteMessage(msg);
            Assert.Equal(msg, this.FakeLogger.LastLogEntry.Message);
            Assert.Equal(LogLevel.Unknown, this.FakeLogger.LastLogEntry.LogLevel);
        }

        [Fact]
        public void WriteFormatMessageTest()
        {
            string msg = "Dogs rule! Cats drool!";
            string fmt = "{0} rule! {1} drool!";
            this.FakeLogger.WithLogLevel(LogLevel.Information).WithLogger("Obvious").WriteMessage(fmt, "Dogs", "Cats");
            Assert.Equal(msg, this.FakeLogger.LastLogEntry.Message);
            Assert.Equal(LogLevel.Information, this.FakeLogger.LastLogEntry.LogLevel);
            Assert.Equal("Obvious", this.FakeLogger.LastLogEntry.LogName);
        }

        [Fact]
        public void WritePropertiesTest()
        {
            this.FakeLogger
                .WithLogLevel(LogLevel.Warning)
                .WithProperty("Data1", "Oliver")
                .WithProperty("Data2", "Olivia")
                .WriteProperties();

            Assert.Equal("LogLevel:Warning;Data1:Oliver;Data2:Olivia;", this.FakeLogger.LastLogEntry.Message);
        }
    }
}