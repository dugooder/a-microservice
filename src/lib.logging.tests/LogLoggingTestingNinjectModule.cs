using Ninject;

namespace lib.logging.tests
{
    namespace lib.logging
    {
        public sealed class LibLoggingNinjectModule : Ninject.Modules.NinjectModule
        {
            public override void Load()
            {
                Bind<ILogProvider>().To<FakeLogProvider>()
                    .InSingletonScope()
                    .Named("FakeLogger")
                    .BindingConfiguration.IsImplicit = true;
            }
        }
    }
}
