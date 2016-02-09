using Xunit;
using Xunit.Abstractions;
using Shouldly;

using Ninject;
namespace service.health.tests
{
    using common.tests;
    using service.health;

    public class ApplicationHealthTests : BaseTest
    {
        IApplicationHealthChecker checker;
        public ApplicationHealthTests(ITestOutputHelper testOutput) : base(testOutput)
        {
            checker = Kernel.Get<IApplicationHealthChecker>();
        }

        [Fact]
        public void IsHealthTest()
        {
            checker.IsHealthy().ShouldBe(true);
        }
    }
}
