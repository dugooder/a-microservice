using System;
using common;
using Nancy;
using Ninject;

namespace service.health
{
    public class HealthServiceNancyModule : NancyModule
    {
        readonly ILogProvider log;

        [Inject]
        public HealthServiceNancyModule(ILogProvider log) : base("/health")
        {
            this.log = log;

            Get["/"] = parameters =>
            {
                using (log.PushContextInfo("healthcheck"))
                {
                    try
                    {
                        ApplicationHealth appHealth = new ApplicationHealth(log);
                        if (appHealth.IsHealthy())
                        {
                            return Response
                                .AsText("OK")
                                .WithStatusCode(HttpStatusCode.OK);
                        }
                        else
                        {
                            return Response
                                .AsText("Not Healthy")
                                .WithStatusCode(HttpStatusCode.ServiceUnavailable);
                        }
                    }
                    catch (Exception e)
                    {
                        return Response
                            .AsText(e.Message)
                            .WithStatusCode(HttpStatusCode.InternalServerError);
                    }
                }
            };
        }

    }
}
