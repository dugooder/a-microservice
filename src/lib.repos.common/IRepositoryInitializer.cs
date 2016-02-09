using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.repos.common
{
    public interface IRepositoryInitializer
    {
        string ConnectionStringName { get; set; }
        bool IsInitialized { get; }
        void Init();
        void Refresh();
        object Provider { get;  }
    }
}
