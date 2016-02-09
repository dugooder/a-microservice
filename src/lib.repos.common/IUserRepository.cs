using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.repos.common
{
    public interface IUserRepository : IRepository<User, long>, IDisposable
    {
        User GetByUserName(string userName);
    }
}
