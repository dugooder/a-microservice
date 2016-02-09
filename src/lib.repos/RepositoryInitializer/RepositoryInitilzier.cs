using Ninject;
namespace lib.repos
{
    using lib.logging;
    using lib.repos.common;

    public abstract class RepositoryInitializerBase : IRepositoryInitializer
    {
        public static string DefaultConnectionStringName = "database";

        protected readonly ILogProvider Log;

        [Inject]
        protected RepositoryInitializerBase(ILogProvider log)
        {
            this.Log = log;
            this.ConnectionStringName = DefaultConnectionStringName;
        }

        public string ConnectionStringName { get; set; }

        public bool IsInitialized { get; protected set; }

        public abstract void Init();

        public object Provider { get; protected set; }

        public virtual void Refresh() { }
    }
}

