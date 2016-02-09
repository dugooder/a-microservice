using System.Collections.Generic;
using Nancy.Security;

namespace lib.repos.common
{
    public class User  : Nancy.Security.IUserIdentity
    {
        List<string> Claims_;
        
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> Claims
        {
            get
            {
                if (Claims_ == null)
                {
                    Claims_ = new List<string>();
                }
                return Claims_;
            }
        }

        IEnumerable<string> IUserIdentity.Claims
        {
            get
            {
                return Claims;
            }
        }

        public bool IsNew()
        {
            return this.Id == default(int);
        }

        public User () { }
        public User(string userName, string password, params string[] claim)
        {
            this.UserName = userName;
            this.Password = password;
            this.Claims.AddRange(claim);
        }

    }
}
