using common;
using Ninject;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace tests
{
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

        // We are not testing log4net just our code so the below are being developed
        // public void PushContextInfoTest() {} 
        // public void PopContextInfoTest() { }
    }
}