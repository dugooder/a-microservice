using System;
using Nancy;
using Ninject;

namespace service.health
{
    using lib.logging;

    public class HealthNancyModule : NancyModule
    {
        readonly ILogProvider log;

        [Inject]
        public HealthNancyModule(ILogProvider log) : base("/health")
        {
            this.log = log;

            using (log.PushContextInfo("healthcheck"))
            {
                //ie. http://localhost:9000/health/details
                Get["/details"] = parameters =>
            {
                ApplicationHealth appHealth = new ApplicationHealth(log);
                return View["details", appHealth];

            };

                Get["/"] = parameters =>
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
                };
            }

        }
    }
}
