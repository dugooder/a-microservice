using System;
using Nancy.Hosting.Self;

namespace service
{
    public sealed class WindowsService : IDisposable
    {
        public const string UrlMask = "http://localhost:{0}";

        readonly int port;

        NancyHost host;
        Bootstrapper bootStrapper;
        
        public WindowsService(int port)
        {
            this.port = port;
        }

        public bool Start()
        {
            if (host == null)
            {
                HostConfiguration config = new HostConfiguration();

                config.UrlReservations.CreateAutomatically = true;

                bootStrapper = new Bootstrapper();

                host = new NancyHost(bootStrapper, config,
                    new Uri(string.Format(UrlMask, port)));
            }

            host.Start();

            return true;
        }

        public bool Stop()
        {
            if (host != null)
            {
                host.Stop();
            }

            return true;
        }

        public void Dispose()
        {
            if (host != null)
            {
                host.Dispose();
                host = null;
            }

            if (bootStrapper != null)
            {
                bootStrapper.Dispose();
                bootStrapper = null;
            }
        }

    }
}
