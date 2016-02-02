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
                k.Bind<Service>().To(typeof(Service)).InSingletonScope();

                var host = HostFactory.New(x =>
                {
                    x.AddCommandLineDefinition("schema", v => schema = v);
                    x.AddCommandLineDefinition("port", v => portNumber = int.Parse(v));
                    x.AddCommandLineDefinition("domain", v => domain = v);
                    x.ApplyCommandLine();

                    UriBuilder urlBuilder = new UriBuilder(schema, domain, portNumber);

                    using (Service svc = k.Get<Service>(
                        new ConstructorArgument("uri", urlBuilder.Uri)))
                    {
                        x.Service<Service>(s =>
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