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
                        Password = reader["password"].ToString(),
                        Email = reader["email"].ToString()
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
                var sql = "insert into user('username','password','email') values('" + criteria.Username + "','" + criteria.Password + "','" + criteria.Email + "')";
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
                var sql = "update user set username='" + criteria.Username + "', password='" + criteria.Password + "',email='" + criteria.Email + "' where id=" + criteria.UserId;
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