using common;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using Nancy.Diagnostics;
using Ninject;

namespace service
{
    public class Bootstrapper : NinjectNancyBootstrapper
    {
        public static string DLLSwithNinjectModulesToLoadMask = "amicroservice*.dll";

        ILogProvider log;
        
        protected override void ConfigureApplicationContainer(IKernel existingContainer)
        {
            existingContainer.Load(DLLSwithNinjectModulesToLoadMask);
        }

        protected override void ApplicationStartup(IKernel container, IPipelines pipelines)
        {
            this.log = container.Get<ILogProvider>();

#if !DEBUG
            DiagnosticsHook.Disable(pipelines);
#endif

            base.ApplicationStartup(container, pipelines);
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get
            {
                return new DiagnosticsConfiguration { Password = @"boy" };
            }
        }

        protected override void RequestStartup(IKernel container, IPipelines pipelines, NancyContext context)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                log.WithLogLevel(LogLevel.Information)
                    .WriteMessage(ctx.Request.Url.ToString());
                return null;
            });

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                log.WithLogLevel(LogLevel.Information)
                    .WriteMessage("Response.StatusCode:{0}", ctx.Response.StatusCode);
            });

            pipelines.OnError += (ctx, ex) =>
            {
                log.WithLogLevel(LogLevel.Error).WriteGeneralException(ex);
                return null;
            };

            base.RequestStartup(container, pipelines, context);
        }
    }
}