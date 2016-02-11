using Xunit;
using Xunit.Abstractions;
using Shouldly;
using Nancy;
using Nancy.Testing;
using Ninject;
namespace service.health.tests
{
    using common.tests;
    using service.health;

    public class ApplicationHealthTests : BaseTest
    {
        TestNancyNinjectBootstrapper bs;

        public ApplicationHealthTests(ITestOutputHelper testOutput) : base(testOutput)
        {
            bs = new TestNancyNinjectBootstrapper(this);
        }

        [Fact]
        public void IsHealthTest()
        {
            var browser = new Browser(bs);

            var result = browser.Get("/health", with =>
            {
                with.HttpRequest();
                with.BasicAuth("NextCheckME", "IamOpenToWorld");
            });

            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
