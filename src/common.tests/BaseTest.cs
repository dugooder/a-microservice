using System;
using Ninject;
using Xunit.Abstractions;

namespace tests
{
    public abstract class BaseTest : IDisposable
    {
        internal protected FakeLogProvider FakeLogger;
        internal protected ITestOutputHelper TestOutputHelper;
        internal protected IKernel Kernel;

        protected BaseTest(ITestOutputHelper output)
        {
            this.TestOutputHelper = output;

            FakeLogger = new FakeLogProvider(output);

            StandardKernel stdKernel = new Ninject.StandardKernel();

            stdKernel.Load("amicroservice.*.dll");
            
            this.Kernel = stdKernel;
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