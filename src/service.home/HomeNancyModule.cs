using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Ninject;
using common;
namespace service.home
{
    public class HomeNancyModule : NancyModule
    {
        readonly ILogProvider log;

        [Inject]
        public HomeNancyModule(ILogProvider log) : base("/")
        {
            this.log = log;

            using (this.log.PushContextInfo("home"))
            {
                Get["/"] = parameters =>
                {
                    try
                    {
                        return Response
                            .AsText("OK")
                            .WithStatusCode(HttpStatusCode.OK);
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
