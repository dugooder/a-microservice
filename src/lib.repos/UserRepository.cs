using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace lib.repos
{
    using lib.logging;
    using lib.repos.common;

    internal class UserRepository : BaseRepository, IUserRepository
    {
        [Inject]
        public UserRepository(ILogProvider log) : base(log) { }
       
        public User GetUser(string userName, string userPass)
        {
            Log.WithLogLevel(LogLevel.Information).WriteMessage("Getting user {0}", userName);
            //            throw new NotImplementedException();
            return null;
        }
    }
}
