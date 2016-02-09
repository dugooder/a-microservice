using Ninject.Modules;
namespace lib.repos
{
    using lib.repos.common;
    using lib.repos.SQLite;

    public class LibReposNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepositoryInitializer>().To<SQLiteRepositoryInitializer>().InSingletonScope();

            Bind<IUserRepository>().To<UserRepository>();
        }
    }
}
