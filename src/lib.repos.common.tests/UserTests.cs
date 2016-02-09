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
            // Example service URLs
            // http://localhost/dog/{id}
            // https://localhost/dog/{id}/schedule 
            // http://localhost:9000/cat?name={name}
            testUser = new User("dog", "bone",
               @"^GET:http(s)?:\/\/.*\/dog\/\d.$",
               @"^GET:http(s)?:\/\/.*\/dog\/\d.\/schedule(\/\d.)?$",
               @"^POST:http(s)?:\/\/.*\/cat\?name=.+$");
        }

        [Fact]
        public void TestCreateUser()
        {
            testUser.UserName.Equals("dog");

            testUser.Password.Equals("bone");

            Assert.NotNull(testUser.Claims);

            int currClaims = testUser.Claims.Count() ;

            testUser.Claims.Add("DELETE:http://localhost/fleas");

            testUser.Claims.Count().ShouldEqual(currClaims + 1);
        }

        //[Fact]
        //public void TestHasClaim()
        //{
        //    testUser.HasMatchingClaim("GET", "http://localhost/dog/12").ShouldBeTrue();
        //    testUser.HasMatchingClaim("GET", "hTTps://LOCAlhosT:9000/DOG/12").ShouldBeTrue();
        //    testUser.HasMatchingClaim("GET", "http://localhost/dog/12/aaa").ShouldBeFalse();
        //    testUser.HasMatchingClaim("POST", "http://localhost/dog/12").ShouldBeFalse();
        //    testUser.HasMatchingClaim("GET", "http://localhost/dog/12/").ShouldBeFalse();
        //}

        [Fact]
        public void IsNewTest()
        {
            User user = new User();
            user.IsNew().ShouldBeTrue();
        }
    }
}
