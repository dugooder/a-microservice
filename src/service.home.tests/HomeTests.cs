using tests;
using Xunit;
using Xunit.Abstractions;
using Shouldly;
using Nancy.Testing;
using Nancy;

namespace service.home.tests
{
    public class HomeTest : BaseTest
    {
        public HomeTest(ITestOutputHelper testOutput) : base(testOutput) { }

        [Fact]
        public void SomeTest()
        {
            var browser = new Browser(new DefaultNancyBootstrapper());

        }
    }
}
