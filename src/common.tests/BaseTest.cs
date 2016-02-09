using System;
using Ninject;
using Xunit.Abstractions;
using System.Linq;

namespace common.tests
{
    using lib.logging;

    public abstract class BaseTest : IDisposable
    {
        internal protected ILogProvider FakeLogger;
        internal protected ITestOutputHelper TestOutputHelper;
        internal protected IKernel Kernel;

        protected BaseTest(ITestOutputHelper output, bool userFakeLogger = true)
        {
            this.TestOutputHelper = output;
            this.Kernel = new Ninject.StandardKernel();
            this.Kernel.Load("amicroservice*.dll");
            if (userFakeLogger)
            {
                FakeLogger = this.Kernel.Get<ILogProvider>("FakeLogger",
                    new Ninject.Parameters.ConstructorArgument("testOutputHelper", output));
                removeRealLogger();
            }
        }

        private void removeRealLogger()
        {
            var binding = Kernel
                .GetBindings(typeof(lib.logging.ILogProvider))
                .FirstOrDefault(b => b.Metadata.Name == null);
            if (binding != null)
            {
                Kernel.RemoveBinding(binding);
            }
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.Kernel != null)
                    {
                        this.Kernel.Dispose();
                        this.Kernel = null;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}