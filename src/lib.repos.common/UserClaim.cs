using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace lib.repos.common
{
    public class UserClaim
    {
        public HttpMethod Method { get; set; }
        public Uri Url { get; set; }
        public UserClaim() { }

        public UserClaim(HttpMethod method, Uri url)
        {
            this.Method = method;
            this.Url = url;
        }
    }
}
