using Ninject;
using Nancy.Bootstrappers.Ninject;

namespace service
{
    public class Bootstrapper : NinjectNancyBootstrapper
    {
        public static string DLLSwithNinjectModulesToLoadMask = "amicroservice*.dll";

        protected override void ConfigureApplicationContainer(IKernel existingContainer)
        {
            existingContainer.Load(DLLSwithNinjectModulesToLoadMask);
        }
    }
}
