using System;
using Ninject;
using Xunit.Abstractions;


namespace common.tests
{
    using lib.logging;

    public abstract class BaseTest : IDisposable
    {
        internal protected ILogProvider FakeLogger;
        internal protected ITestOutputHelper TestOutputHelper;
        internal protected IKernel Kernel;

        protected BaseTest(ITestOutputHelper output)
        {
            this.TestOutputHelper = output;
            StandardKernel stdKernel = new Ninject.StandardKernel();

            stdKernel.Load("amicroservice*.dll");

            FakeLogger = stdKernel.Get<ILogProvider>("FakeLogger", 
                new Ninject.Parameters.ConstructorArgument("testOutputHelper", output));

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