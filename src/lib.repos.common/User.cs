using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace lib.repos.common
{
    public class User
    {
        List<UserClaim> claims;
        public IEnumerable<UserClaim> Claims
        {
            get
            {
                return claims;
            }

        }
        public string UserName { get; protected set; }

        public User(string userName, params UserClaim[] claims)
        {
            this.claims = new List<UserClaim>();
            this.UserName = userName.Clone() as string;
            for (int i = 0; i < claims.Length; i++)
            {
                this.claims.Add(claims[i]);
            }
        }

        public bool HasClaim(HttpMethod method, Uri url)
        {
            return claims.Find(delegate (UserClaim c)
            {
                return method == c.Method &&
                       url == c.Url;
            }) != null;
        }

        public void AddClaim(UserClaim claim)
        {
            claims.Add(claim);
        }
    }
}
