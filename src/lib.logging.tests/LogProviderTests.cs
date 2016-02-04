using Ninject;
using Ninject.Parameters;   
using Should;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace lib.logging.tests
{
    using common.tests;

    [Collection("Common")]
    public class LogProviderTests : BaseTest
    {
        ILogProvider logger;

        public LogProviderTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            logger = Kernel.Get<ILogProvider>();
        }

        [Fact()]
        public void PropertiesTest()
        {
            logger.WithProperty("color", "red").WithProperty("size", "large");
            Dictionary<string, object> allProps = logger.Properties;

            Assert.Equal(2, allProps.Keys.Count);
            Assert.Equal(allProps["color"].ToString(), "red");
            Assert.Equal(allProps["size"].ToString(), "large");
        }

        [Fact()]
        public void ResetTest()
        {
            ILogProvider provider = logger.WithProperty("color", "red");

            provider.Reset();

            Assert.False(logger.Properties.ContainsKey("color"));
        }

        [Fact()]
        public void WithPropertyTest()
        {
            ILogProvider provider = logger.WithProperty("color", "red");

            Assert.NotNull(provider);

            Assert.Equal("red", logger.Properties["color"]);
        }

       [Fact()]
       public void LogContextTest()
        {
            logger.PopContextInfo().ShouldBeEmpty();
            logger.PushContextInfo("LogContextTest").ShouldNotBeNull();
            logger.WriteMessage("LogContextTest message");
            logger.PopContextInfo().Equals("LogContextTest");
            logger.PopContextInfo().ShouldBeEmpty();
        }

        [Fact()]
        public void LogContextUsingTest()
        {
            logger.PopContextInfo().ShouldBeEmpty();
            using (IDisposable logCt = logger.PushContextInfo("LogContextUsingTest"))
            {
                logger.WriteMessage("LogContextUsingTest message");
                logger.PopContextInfo().Equals("LogContextUsingTest");
            }

            logger.PopContextInfo().ShouldBeEmpty();
        }
    }
}