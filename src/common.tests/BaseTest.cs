using Ninject;
using Xunit.Abstractions;

namespace tests
{
    public abstract class BaseTest
    {
        protected FakeLogProvider FakeLogger;
        protected ITestOutputHelper TestOutputHelper;
        protected IKernel Kernel;

        protected BaseTest(ITestOutputHelper output)
        {
            this.TestOutputHelper = output;

            FakeLogger = new FakeLogProvider(output);

            StandardKernel stdKernel = new Ninject.StandardKernel();

            stdKernel.Load("amicroservice.*.dll");

            this.Kernel = stdKernel;
        }
    }
}