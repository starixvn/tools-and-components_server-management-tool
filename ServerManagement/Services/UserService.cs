using System;
using System.Collections.Generic;
using ServerManagement.Models;

namespace ServerManagement.Services
{
    public class UserService : BaseService
    {
        public List<UserModel> GetUsers()
        {
            var sql = "select * from user";
            var users = new List<UserModel>();

            using (var reader = QueryData(sql))
            {
                while (reader.Read())
                {
                    users.Add(new UserModel
                    {
                        UserId = Convert.ToInt32(reader["Id"]),
                        Username = reader["username"].ToString(),
                        AliasName = reader["aliasname"].ToString(),
                        Email = reader["email"].ToString(),
                        SuperUser = Convert.ToInt32(reader["superuser"]) == 1
                    });
                }
            }

            return users;
        }

        public Result Insert(UserModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "insert into user('username','aliasname','email','superuser') values('" + criteria.Username + "','" + criteria.AliasName + "','" + criteria.Email + "',0)";
                errorCode = UpdateData(sql);
            }
            catch (Exception ex)
            {
                return new Result
                {
                    ErrorCode = errorCode,
                    ErrorMessage = ex.Message
                };
            }

            return new Result
            {
                ErrorCode = errorCode,
                ErrorMessage = string.Empty
            };
        }

        public Result Update(UserModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "update user set username='" + criteria.Username + "', aliasname='" + criteria.AliasName + "',email='" + criteria.Email + "' where id=" + criteria.UserId;
                errorCode = UpdateData(sql);
            }
            catch (Exception ex)
            {
                return new Result
                {
                    ErrorCode = errorCode,
                    ErrorMessage = ex.Message
                };
            }

            return new Result
            {
                ErrorCode = errorCode,
                ErrorMessage = string.Empty
            };
        }

        public Result Delete(int userId)
        {
            int errorCode = 0;
            try
            {
                var sql = "delete from user where id=" + userId;
                errorCode = UpdateData(sql);
            }
            catch (Exception ex)
            {
                return new Result
                {
                    ErrorCode = errorCode,
                    ErrorMessage = ex.Message
                };
            }

            return new Result
            {
                ErrorCode = errorCode,
                ErrorMessage = string.Empty
            };
        }
    }
}