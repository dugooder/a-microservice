using Topshelf;

namespace service
{
    class Program
    {
        static int portNumber = 9000;

        static void Main(string[] args)
        {
            string appName = "amicroservice";  //TODO: get this from the assembly info or configuration
            
            using (WindowsService svc = new WindowsService(portNumber))
            {
                var host = HostFactory.New(x =>
               {
                   x.AddCommandLineDefinition("port", v => portNumber = int.Parse(v));
                   x.ApplyCommandLine();

                   x.Service<WindowsService>(s =>
                   {
                       s.ConstructUsing(settings => svc);
                       s.WhenStarted(service => service.Start());
                       s.WhenStopped(service => service.Stop());

                       string serviceDescription = string.Format("{0} is running on {1}", appName, portNumber);
                       x.SetDescription(serviceDescription);
                       x.SetDisplayName(appName);
                       x.SetServiceName(appName);
                       x.RunAsNetworkService();
                       x.StartAutomatically();
                   });

               });

                host.Run();
            }
        }
    }
}
