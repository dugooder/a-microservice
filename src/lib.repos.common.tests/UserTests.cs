using System;
using System.Linq;
using Xunit;
using Should;
using System.Net.Http;
namespace lib.repos.common.tests
{
    using lib.repos.common;

    public class UserTests
    {
        User testUser;
        public UserTests()
        {
            testUser = new User("dog",
                new UserClaim(HttpMethod.Get, new Uri("http://localhost/dogwalking")),
                new UserClaim(HttpMethod.Post, new Uri("http://localhost/dogwalking"))
                );
        }

        [Fact]
        public void TestCreateUser()
        {
            testUser.UserName.Equals("dog");

            Assert.NotNull(testUser.Claims);

            testUser.Claims.Count().ShouldEqual(2);
            testUser.AddClaim(new UserClaim(HttpMethod.Delete, new Uri("http://localhost/fleas")));

            testUser.Claims.Count().ShouldEqual(3);
        }

        [Fact]
        public void TestHasClaim()
        {
            testUser.HasClaim(HttpMethod.Get, new Uri("http://localhost/dogwalking")).ShouldEqual(true);
            testUser.HasClaim(HttpMethod.Get, new Uri("http://localhost/catwalking")).ShouldEqual(false);
            testUser.HasClaim(HttpMethod.Delete, new Uri("http://localhost/dogwalking")).ShouldEqual(false);
        }
    }
}
