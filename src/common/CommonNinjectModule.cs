namespace common
{
    public sealed class CommonNinjectModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<ILogProvider>().To<LogProvider>().InSingletonScope();
        }
    }
}