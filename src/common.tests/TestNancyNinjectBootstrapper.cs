using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using Ninject;

namespace common.tests
{
    using lib;
    using lib.logging;
    
    public class TestNancyNinjectBootstrapper : NinjectNancyBootstrapper
    {
        readonly BaseTest test; 
        public TestNancyNinjectBootstrapper(BaseTest test)
        {
            this.test = test;
        }

        protected override void ConfigureApplicationContainer(IKernel existingContainer)
        {
            existingContainer.Load(test.Kernel.GetModules());
        }

        protected override void RequestStartup(IKernel container, IPipelines pipelines, NancyContext context)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                test.FakeLogger.WithLogLevel(LogLevel.Information)
                    .WriteMessage(ctx.Request.Url.ToString());
                return null;
            });

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                test.FakeLogger.WithLogLevel(LogLevel.Information)
                    .WriteMessage("Response.StatusCode:{0}", ctx.Response.StatusCode);
            });

            pipelines.OnError += (ctx, ex) =>
            {
                test.FakeLogger.WithLogLevel(LogLevel.Error).WriteGeneralException(ex);
                return null;
            };

            base.RequestStartup(container, pipelines, context);
        }
    }

}
