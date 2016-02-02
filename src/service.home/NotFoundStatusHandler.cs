using Nancy;
using Nancy.ErrorHandling;
using Nancy.Responses;
using Nancy.ViewEngines;

namespace service.home
{
    public class NotFoundStatusHandler : IStatusCodeHandler
    {
        private IViewRenderer viewRenderer;

        public NotFoundStatusHandler(IViewRenderer viewRenderer)
        {
            this.viewRenderer = viewRenderer;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode,
                                      NancyContext context)
        {
            return statusCode == HttpStatusCode.NotFound;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            // loading file instead of using RenderView because views are specific 
            // to the service and i do not want to replciate the 404 page in each service DLL
            var response = new GenericFileResponse("content/404.html", "text/html"); 
            response.StatusCode = statusCode;
            context.Response = response;
        }
    }
}
