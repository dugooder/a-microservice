using Ninject;
using Ninject.Parameters;
using System;
using Topshelf;
namespace service
{
    class Program
    {
        static string schema = "http";
        static int portNumber = 9000;
        static string domain = "localhost";

        static void Main(string[] args)
        {
            string appName = typeof(Program).Assembly.GetName().Name;

            using (IKernel k = new StandardKernel())
            {
                k.Load(new common.CommonNinjectModule());
                k.Bind<WindowsService>().To(typeof(WindowsService)).InSingletonScope();

                var host = HostFactory.New(x =>
                {
                    x.AddCommandLineDefinition("schema", v => schema = v);
                    x.AddCommandLineDefinition("port", v => portNumber = int.Parse(v));
                    x.AddCommandLineDefinition("domain", v => domain = v);
                    x.ApplyCommandLine();

                    UriBuilder urlBuilder = new UriBuilder(schema, domain, portNumber);

                    using (WindowsService svc = k.Get<WindowsService>(
                        new ConstructorArgument("uri", urlBuilder.Uri)))
                    {
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
                    }
                });

                host.Run();
            }
        }
    }
}