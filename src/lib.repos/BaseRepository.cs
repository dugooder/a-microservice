using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.repos
{
    using lib.logging; 

    internal abstract class BaseRepository
    {
        protected ILogProvider Log; 

        protected BaseRepository(ILogProvider logger)
        {
            this.Log = logger;
        }
    }
}
