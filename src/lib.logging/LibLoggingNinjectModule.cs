namespace lib.logging
{
    public sealed class LibLoggingNinjectModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<ILogProvider>().To<LogProvider>().InSingletonScope();
        }
    }
}