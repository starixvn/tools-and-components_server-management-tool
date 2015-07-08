using System;
using System.Collections.Generic;
using ServerManagement.Models;

namespace ServerManagement.Services
{
    public class UserSiteService : BaseService
    {
        public List<UserSiteModel> GetUserSite()
        {
            var sql = "select * from usersite";
            var userSite = new List<UserSiteModel>();

            using (var reader = QueryData(sql))
            {
                while (reader.Read())
                {
                    userSite.Add(new UserSiteModel
                    {
                        SiteId = Convert.ToInt32(reader["siteId"]),
                        UserId = Convert.ToInt32(reader["userId"]),
                        Permission = Convert.ToInt32(reader["permission"])
                    });
                }
            }

            return userSite;
        }

        public Result Insert(UserSiteModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "insert into usersite('siteid','userid','permission') values(" + criteria.SiteId + "," + criteria.UserId + "," + criteria.Permission + ")";
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

        public Result Update(UserSiteModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "update usersite set permission=" + criteria.Permission + " where siteid=" + criteria.SiteId + " and userid=" + criteria.UserId;
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

        public bool Existing(UserSiteModel criteria)
        {
            var sql = "select * from usersite where siteid = " + criteria.SiteId + " and userid=" + criteria.UserId;

            var result = QueryDataScalar(sql);

            return result != null;
        }
    }
}