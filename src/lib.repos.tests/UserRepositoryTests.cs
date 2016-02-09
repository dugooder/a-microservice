using System;
using Ninject;
using Should;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace lib.repos.tests
{
    using global::common.tests;
    using lib.repos.common;

    public class UserRepositoryTests : BaseTest
    {

        public UserRepositoryTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void CRUD_Test()
        {

            IRepositoryInitializer repoInitr = Kernel.Get<IRepositoryInitializer>();
            repoInitr.ConnectionStringName = "UserRepository_CRUD_Test";
            repoInitr.Refresh();  
            using (IUserRepository repo = Kernel.Get<IUserRepository>())
            {
                string testsUserName = "Create_Delete_Test_UnitTest";
                User u1 = new User();
                u1.UserName = testsUserName;

                u1.Password = "Dog";
                u1.Claims.Add("http://yahoo");
                u1.Claims.Add("http://google");

                u1 = repo.Create(u1);

                u1.Id.ShouldBeGreaterThan(0);

                IEnumerable<User> userList = repo.Read();
                userList.ShouldNotBeNull();
                User createdUser = userList.FirstOrDefault(delegate (User u)
                {
                    return string.CompareOrdinal(u.UserName, testsUserName) == 0;
                });

                createdUser.Id.ShouldBeGreaterThan(0);
                createdUser.Claims.Count.ShouldEqual(2);

                u1.Password = "Cat";
                u1.Claims.Remove("http://yahoo");
                u1.Claims.Add("http://top.st");
                u1.Claims.Add("http://webcrawler.com");
                repo.Update(u1);

                User updatedUser = repo.Read().FirstOrDefault(delegate (User u)
                {
                    return string.CompareOrdinal(u.UserName, testsUserName) == 0;
                });

                updatedUser.Id.ShouldEqual(createdUser.Id);
                updatedUser.Password.ShouldEqual("Cat");
                updatedUser.Claims.Count.ShouldEqual(3);
                updatedUser.Claims[0].ShouldEqual("http://google");
                updatedUser.Claims[1].ShouldEqual("http://top.st");
                updatedUser.Claims[2].ShouldEqual("http://webcrawler.com");

                repo.Delete(u1).ShouldBeTrue();

                userList = null;
                userList = repo.Read();
                User shouldBeNullUser = userList.Select(
                    delegate (User u)
                    {
                        return u.UserName == testsUserName ? u : null;
                    }).FirstOrDefault();
                Assert.Null(shouldBeNullUser);
            }
        }

        [Fact]
        public void GetByUserNameTest()
        {
            IRepositoryInitializer repoInitr = Kernel.Get<IRepositoryInitializer>();
            repoInitr.ConnectionStringName = "UserRepository_GetByUserNameTest";
            repoInitr.Refresh();
            using (IUserRepository repo = Kernel.Get<IUserRepository>())
            {
                string testsUserName = "GetByUserNameTest";
                User u1 = new User();
                u1.UserName = testsUserName;
                u1.Claims.Add("gold");
                u1.Claims.Add("frankincense");
                u1.Claims.Add("myrrh");
                u1.Password = "magi";
                repo.Create(u1);

                User fetchedUser = repo.GetByUserName(u1.UserName);

                fetchedUser.Id.ShouldEqual(u1.Id);
                fetchedUser.Password.ShouldEqual(u1.Password);
                fetchedUser.Claims.Count.ShouldEqual(u1.Claims.Count);
                fetchedUser.Claims[2].ShouldEqual(u1.Claims[2]);

                repo.Delete(u1);
            }
        }
    }
}
