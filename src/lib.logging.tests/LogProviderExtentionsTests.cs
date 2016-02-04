using System;
using Xunit;
using Xunit.Abstractions;

namespace lib.logging.tests
{
    using common.tests;

    public class LogProviderExtentionsTests : BaseTest
    {
        FakeLogProvider logger; 
        public LogProviderExtentionsTests(ITestOutputHelper output)  
            : base(output) {
            this.logger = this.FakeLogger as FakeLogProvider;
            this.logger.DetailedOutput = true;
        }

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
            Assert.Equal(ex, logger.LastLogEntry.Exception);
            Assert.Equal(LogLevel.Error, logger.LastLogEntry.LogLevel);
        }

        [Fact]
        public void WriteMessageTest()
        {
            string msg = "Dogs rule! Cats drool!";
            this.FakeLogger.WriteMessage(msg);
            Assert.Equal(msg, logger.LastLogEntry.Message);
            Assert.Equal(LogLevel.Unknown, logger.LastLogEntry.LogLevel);
        }

        [Fact]
        public void WriteFormatMessageTest()
        {
            string msg = "Dogs rule! Cats drool!";
            string fmt = "{0} rule! {1} drool!";
            this.FakeLogger.WithLogLevel(LogLevel.Information).WithLogger("Obvious").WriteMessage(fmt, "Dogs", "Cats");
            Assert.Equal(msg, logger.LastLogEntry.Message);
            Assert.Equal(LogLevel.Information, logger.LastLogEntry.LogLevel);
            Assert.Equal("Obvious", logger.LastLogEntry.LogName);
        }

        [Fact]
        public void WritePropertiesTest()
        {
            this.FakeLogger
                .WithLogLevel(LogLevel.Warning)
                .WithProperty("Data1", "Oliver")
                .WithProperty("Data2", "Olivia")
                .WriteProperties();

            Assert.Equal("LogLevel:Warning;Data1:Oliver;Data2:Olivia;", logger.LastLogEntry.Message);
        }
    }
}