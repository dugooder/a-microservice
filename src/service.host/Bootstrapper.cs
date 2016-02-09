using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using Nancy.Diagnostics;
using Ninject;
using System;
using System.Text;

namespace service
{
    using lib;
    using lib.logging;
    using lib.repos;
    using lib.repos.common;
    

    //public static class NancyModuleExtension
    //{
    //    public static void ServiceRequiresAuthentication(this NancyModule module)
    //    {
    //        module.Before.AddItemToEndOfPipeline(serviceRequiresAuthentication);
    //    }

    //    staticResponse serviceRequiresAuthentication(NancyContext context)
    //    {
    //        Response response = null;

    //        Nancy.Security.IUserIdentity currentUser = context.CurrentUser;

    //        // if User is null
    //        // if User is not null && context URL does not pattern match to one of the claims
    //        if ((currentUser == null) || 
    //            (String.IsNullOrWhiteSpace(currentUser.UserName)) ||
    //            ()
    //        {
    //            response = new Response { StatusCode = HttpStatusCode.Unauthorized };
    //        }

    //        return response;
    //    }
    //}

    public class Bootstrapper : NinjectNancyBootstrapper
    {
        public static string DLLSwithNinjectModulesToLoadMask = "amicroservice*.dll";

        ILogProvider log;
        
        protected override void ConfigureApplicationContainer(IKernel existingContainer)
        {
            RepositoryInitializerBase.DefaultConnectionStringName = "db";
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
            setCurrentUser(container, pipelines, context);
                        
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

        static void setCurrentUser(IKernel container, IPipelines pipelines, NancyContext context)
        {
            NancyContextAuthInfo info = context.GetBasicUserPassword();
            if (!string.IsNullOrWhiteSpace(info.UserName))
            {
                using (IUserRepository repo = container.Get<IUserRepository>())
                {
                    User user = repo.GetByUserName(info.UserName);
                    if (user != null)
                    {
                        if (string.CompareOrdinal(user.Password, info.Password) != 0)
                        {
                            user.Claims.Clear();
                        }
                        context.CurrentUser = user;
                    }
                }
            }
        }
    }
}