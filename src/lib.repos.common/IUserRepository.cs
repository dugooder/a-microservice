using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.repos.common
{
    public interface IUserRepository
    {
        User GetUser(string userName, string userPass);
    }
}
