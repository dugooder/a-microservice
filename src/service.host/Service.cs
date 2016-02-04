using System;
using Nancy.Hosting.Self;
using Ninject;

namespace service
{
    using lib.logging;

    public sealed class Service : IDisposable
    {
        readonly Uri uri;
        readonly ILogProvider log;

        NancyHost host;
        Bootstrapper bootStrapper;
        
        [Inject]
        public Service(ILogProvider log, Uri uri)
        {
            this.log = log;
            this.uri = uri;
        }

        public bool Start()
        { 
            if (host == null)
            {
                HostConfiguration config = new HostConfiguration();

                config.UrlReservations.CreateAutomatically = true;

                bootStrapper = new Bootstrapper();

                host = new NancyHost(bootStrapper, config, uri);
            }

            host.Start();

            log.WithLogLevel(LogLevel.Information)
                 .WriteMessage("Started Host on {0}", uri.ToString());

#if DEBUG
            System.Diagnostics.Process.Start(uri.ToString()  + "health");
#endif
            return true;
        }

        public bool Stop()
        {
            if (host != null)
            {
                host.Stop();
            }

            log.WithLogLevel(LogLevel.Information)
                .WriteMessage("Stopping Host");

            return true;
        }

        public void Dispose()
        {
            if (host != null)
            {
                host.Dispose();
                host = null;
                log.WithLogLevel(LogLevel.Debug)
                    .WriteMessage("Host Dispossed");

            }

            if (bootStrapper != null)
            {
                bootStrapper.Dispose();
                bootStrapper = null;
                log.WithLogLevel(LogLevel.Debug)
                    .WriteMessage("Bootstrapper Dispossed");
            }
        }

    }
}
