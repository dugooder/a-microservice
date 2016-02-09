using System;
using System.Text;
using Nancy;
using Nancy.Security;
using System.Text.RegularExpressions;

namespace lib
{
    public static class NancyExtensions
    {
        public static NancyContextAuthInfo GetBasicUserPassword(this NancyContext ctx)
        {
            NancyContextAuthInfo result = new NancyContextAuthInfo();

            string encodedUsernamePassword = ctx.Request.Headers.Authorization;

            if (!string.IsNullOrWhiteSpace(encodedUsernamePassword))
            {
                try
                {
                    string usernamePassword = Encoding.ASCII.GetString(
                        Convert.FromBase64String(encodedUsernamePassword.Substring(6)));

                    int seperatorIndex = usernamePassword.IndexOf(':');

                    result.UserName = usernamePassword.Substring(0, seperatorIndex);

                    result.Password = usernamePassword.Substring(seperatorIndex + 1);
                }
                catch (Exception innerEx)
                {
                    throw new FormatException("Error in Basic Authorization Header format", innerEx);
                }
            }

            return result;
        }

        public static bool HasMatchingClaim(this IUserIdentity user, string method, string url)
        {
            bool result = false;

            string requestedClaim = string.Format("{0}:{1}", method, url);

            foreach (string claimMask in user.Claims)
            {
                result = Regex.IsMatch(requestedClaim,
                    claimMask, RegexOptions.IgnoreCase);
                if (result) break;
            }

            return result;
        }

        public static void RequireClaimOnUrl(this NancyModule module)
        {
            module.Before.AddItemToEndOfPipeline(requireClaimOnUrl);

        }

        static Response requireClaimOnUrl(NancyContext context)
        {
            Response response = null;

            IUserIdentity user = context.CurrentUser;
            Request req = context.Request;

            if ((user == null) ||
                String.IsNullOrWhiteSpace(user.UserName))
            {
                response = new Response { StatusCode = HttpStatusCode.Unauthorized };
            }
            else if (!user.HasMatchingClaim(req.Method, req.Url.ToString()))
            {
                response = new Response { StatusCode = HttpStatusCode.Unauthorized };
            }

            return response;
        }
    }

    public class NancyContextAuthInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
