using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace lib.repos
{
    using lib.logging;
    using lib.repos.common;

    internal sealed class UserRepository : BaseRepository, IUserRepository
    {
        [Inject]
        public UserRepository(ILogProvider log) : base(log) { }

        public User Create(User entity)
        {
            Context.UseTransaction(true);

            try
            {
                entity.Id =
                    Context.Sql(SQL.Create_User)
                    .Parameter("UserName", entity.UserName)
                    .Parameter("Password", entity.Password)
                    .ExecuteReturnLastId<long>("id");

                foreach (string Claim in entity.Claims)
                {
                    int rows = Context.Sql(SQL.Create_Claim)
                        .Parameter("UserName", entity.UserName)
                        .Parameter("Claim", Claim)
                        .Execute();
                    if (rows < 1)
                    {
                        throw new DataException("Insert into User Claims failed");
                    }
                }

                Context.Commit();

                return entity;
            }
            catch
            {
                Context.Rollback();
                throw;
            }
            finally
            {
                Context.UseTransaction(false);
            }
        }

        public IEnumerable<User> Read()
        {
            Context.UseSharedConnection(true);

            try
            {
                List<User> result = Context
                    .Sql(SQL.Read_All_Users)
                    .QueryMany<User>(delegate (User user, dynamic row)
                    {
                        user.Id = row.id;
                        user.Password = row.password;
                        user.UserName = row.username;
                        List<string> claims = Context
                            .Sql(SQL.Read_Claims_For_User)
                            .Parameter("UserName", user.UserName)
                            .QueryMany<string>();
                        user.Claims.AddRange(claims);
                    });

                return result;
            }
            finally
            {
                Context.UseSharedConnection(false);
            }
        }

        public User Update(User entity)
        {
            // start transaction 
            Context.UseTransaction(true);

            try
            {

                // update user 
                int updatedCnt =
                    Context
                    .Sql(SQL.Update_User)
                    .Parameter("UserName", entity.UserName)
                    .Parameter("Password", entity.Password)
                    .Parameter("Id", entity.Id)
                    .Execute();

                // delete all claims 
                int delCnt =
                    Context
                    .Sql(SQL.Delete_All_User_Claims)
                    .Parameter("UserName", entity.UserName)
                    .Execute();

                // add current set of claims
                int addedCnt = 0;
                foreach (string claim in entity.Claims)
                {
                    addedCnt +=
                        Context.Sql(SQL.Create_Claim)
                         .Parameter("UserName", entity.UserName)
                         .Parameter("Claim", claim)
                         .Execute();
                }

                // compoleted transaction 
                Context.Commit();

                // return updated user
                return entity;
            }
            catch
            {
                Context.Rollback();
                throw;
            }
            finally
            {
                Context.UseTransaction(false);
            }
        }

        public bool Delete(User entity)
        {
            Context.UseTransaction(true);

            try
            {
                int cnt = Context.Sql(SQL.Delete_Claim)
                    .Parameter("UserName", entity.UserName)
                    .Execute();

                cnt += Context.Sql(SQL.Delete_User)
                    .Parameter("UserName", entity.UserName)
                    .Execute();

                Context.Commit();

                return cnt > 0;
            }
            catch
            {
                Context.Rollback();
                throw;
            }
            finally
            {
                Context.UseTransaction(false);
            }
        }

        public User GetByUserName(string userName)
        {

            Context.UseSharedConnection(true);

            try
            {
                User result = Context
                    .Sql(SQL.Read_User_By_UserName)
                    .Parameter("UserName", userName)
                    .QueryMany<User>(delegate (User user, dynamic row)
                    {
                        user.Id = row.id;
                        user.Password = row.password;
                        user.UserName = row.username;
                        List<string> claims = Context
                            .Sql(SQL.Read_Claims_For_User)
                            .Parameter("UserName", user.UserName)
                            .QueryMany<string>();
                        user.Claims.AddRange(claims);
                    }).FirstOrDefault();

                return result;
            }
            finally
            {
                Context.UseSharedConnection(false);
            }
        }

   
    }
}
