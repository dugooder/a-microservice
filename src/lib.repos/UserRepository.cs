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
    using System.Net.Http;
    internal class UserRepository : BaseRepository, IUserRepository
    {
        [Inject]
        public UserRepository(ILogProvider log) : base(log) { }

        public User GetUser(string userName, string userPass)
        {
            User result = null;

            Log.WithLogLevel(LogLevel.Information).WriteMessage("Getting user {0}", userName);

            List<User> userList = loadDummyUsers();
            foreach (User user in userList)
            {
                if (string.CompareOrdinal(user.UserName, userName) == 0)
                {
                    result = user;
                }
            }

            return result;
        }
        
        // This is a template afterall
        List<User> loadDummyUsers()
        {
            List<User> result = new List<User>(); 
            User u1 = new User("oliver",
                new UserClaim(HttpMethod.Get, new Uri("http://localhost/food")),
                new UserClaim(HttpMethod.Post, new Uri("http://localhost/food")));
            result.Add(u1);
            User u2 = new User("laura",
                new UserClaim(HttpMethod.Get, new Uri("http://localhost/shop")),
                new UserClaim(HttpMethod.Post, new Uri("http://localhost/shop")));
            result.Add(u2);
            return result;
        }
    }
}
