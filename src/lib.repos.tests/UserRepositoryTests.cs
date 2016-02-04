using System;
using Xunit;
using Xunit.Abstractions;
using Ninject;
using Should;
using System.Net.Http;
namespace lib.repos.tests
{
    using global::common.tests;
    using lib.repos.common;

    public class UserRepositoryTests : BaseTest
    {
        IUserRepository userRepo;
        public UserRepositoryTests(ITestOutputHelper output) : base(output)
        {
            userRepo = Kernel.Get<IUserRepository>();
        }

        [Fact]
        public void TestGetUser()
        {
            User u1 = userRepo.GetUser("oliver", "olivia_is_my_hero");
            Assert.NotNull(u1);
            u1.UserName.ShouldEqual("oliver");
            u1.HasClaim(HttpMethod.Get, new Uri("http://localhost/food"));
        }
    }
}
