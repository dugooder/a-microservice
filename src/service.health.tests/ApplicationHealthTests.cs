using tests;
using Xunit;
using Xunit.Abstractions;
using Shouldly;

namespace service.health.tests
{
    public class ApplicationHealthTests : BaseTest
    {
        public ApplicationHealthTests(ITestOutputHelper testOutput) : base(testOutput) { }

        [Fact]
        public void IsHealthTest()
        {
            ApplicationHealth appHealth = new ApplicationHealth(this.FakeLogger);
            appHealth.IsHealthy().ShouldBe(true);
        }

        [Fact]
        public void HealthStatusTest()
        {
            ApplicationHealth appHealth = new ApplicationHealth(this.FakeLogger);
            appHealth.Status.ShouldBe("Bien");
        }
    }
}
