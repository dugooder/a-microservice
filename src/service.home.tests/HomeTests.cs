using Xunit;
using Xunit.Abstractions;
using Shouldly;
using Nancy.Testing;
using Nancy;

namespace service.home.tests
{
    using common.tests;

    public class HomeTest : BaseTest
    {
        TestNancyNinjectBootstrapper bs;

        public HomeTest(ITestOutputHelper testOutput) : base(testOutput)
        {
            bs = new TestNancyNinjectBootstrapper(this);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (bs != null)
                {
                    bs.Dispose();
                    bs = null;
                }
            }
        }

        [Fact]
        public void HomePageTest()
        {
            var browser = new Browser(bs);

            var result = browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public void HumansInfoTest()
        {
            var browser = new Browser(bs);

            var result = browser.Get("/humans", with =>
            {
                with.HttpRequest();
            });

            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            result.ContentType.ShouldBe("text/plain");
            result.BodyAsText().ShouldContain("colophon", Case.Insensitive);
        }

        [Fact]
        public void NotFoundPageTest()
        {
            var browser = new Browser(bs);

            var result = browser.Get("/whereismycar", with =>
            {
                with.HttpRequest();
            });

            result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
