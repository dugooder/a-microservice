using System;
using Nancy;
using Ninject;
using Nancy.Security;
namespace service.health
{
    using lib;
    using lib.logging;

    public sealed class HealthNancyModule : NancyModule
    {
        readonly ILogProvider log;
        readonly IApplicationHealthChecker healthChecker;

        [Inject]
        public HealthNancyModule(ILogProvider log, IApplicationHealthChecker checker) : base("/health")
        {
            // Make a module secure by using the below functions
            //this.RequiresHttps();
            //this.RequiresAuthentication();
            //this.RequireClaimOnUrl();  // To authorized make sure "^GET:http(s)?:\/\/.*\/health(\/)?$" is a claim.

            this.log = log;
            this.healthChecker = checker;

            Get["/"] = parameters =>
            {
                using (log.PushContextInfo("healthcheck"))
                {
                    bool isHealthy = false;
                    try
                    {
                        isHealthy = this.healthChecker.IsHealthy();
                    }
                    catch (Exception e)
                    {
                        isHealthy = false;
                        log.WithLogLevel(LogLevel.Error).WriteGeneralException(e);
                    }

                    HttpStatusCode statusCode = isHealthy ? HttpStatusCode.OK : HttpStatusCode.ServiceUnavailable;

                    return Response.AsText(statusCode.ToString())
                                   .WithStatusCode(statusCode)
                                   .WithContentType("text/plain")
                                   .WithHeader("Content-Disposition", "inline")
                                   .WithHeader("Cache-Control", "no-cache");
                }
            };
        }
    }
}
