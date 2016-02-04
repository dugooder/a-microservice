﻿using Nancy;
using Ninject;
using Nancy.Responses;

namespace service.home
{
    using lib.logging;

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
                    return View["index"];
                };

                Get["/humans"] = parameters =>
                {
                    var response = new 
                        GenericFileResponse("content/humans.txt", "text/plain");
                    return response.WithStatusCode(HttpStatusCode.OK);
                };
            }
        }
    }
}