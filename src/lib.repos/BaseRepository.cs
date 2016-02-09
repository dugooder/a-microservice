using FluentData;
using Ninject;
using System;
namespace lib.repos
{
    using lib.repos.common;
    using lib.logging;

    internal abstract class BaseRepository : IDisposable
    {
        bool disposedValue = false;
        IDbContext Context_;
        
        protected readonly ILogProvider Log;
        protected IDbContext Context {
            get
            {
                if (Context_ == null)
                {
                    Initializer.Init();
                    IDbProvider dbProvider = Initializer.Provider as IDbProvider;
                    Context_ = new DbContext().ConnectionStringName(
                        Initializer.ConnectionStringName,  dbProvider);
                }
                return Context_;
            }
        }

        [Inject]
        // Property injection used to allow it to change by repo
        public IRepositoryInitializer Initializer { get; set; }

        [Inject]
        protected BaseRepository(ILogProvider logger)
        {
            this.Log = logger;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Context_ != null)
                    {
                        Context_.Dispose();
                        Context_ = null;
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
