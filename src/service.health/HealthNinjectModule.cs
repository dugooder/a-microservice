using Ninject.Modules;

namespace service.health
{   public class HealthNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IApplicationHealthChecker>().To<ApplicationHealthChecker>();
        }
    }
}
