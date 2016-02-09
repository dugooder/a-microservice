using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.repos.common
{
    public interface IRepository<T, IDT> where T : class
    {
        IRepositoryInitializer Initializer { get; set; }

        T Create(T entity);
        IEnumerable<T> Read();
        T Update(T entity);
        bool Delete(T entity);
    }
}
