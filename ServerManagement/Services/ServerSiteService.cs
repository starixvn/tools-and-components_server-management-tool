using System;
using System.Collections.Generic;
using ServerManagement.Models;

namespace ServerManagement.Services
{
    public class ServerSiteService : BaseService
    {
        public List<SystemServerViewModel> GetServerSite()
        {
            var sql = "select ss.id as serversiteid, ss.ServerId as serverid, ss.SiteId as siteid, sv.ip, s.Name from serversite ss inner join server sv on ss.ServerId = sv.Id inner join site s on ss.SiteId = s.Id";
            var serversites = new List<SystemServerViewModel>();

            using (var reader = QueryData(sql))
            {
                while (reader.Read())
                {
                    serversites.Add(new SystemServerViewModel
                    {
                        SiteId = Convert.ToInt32(reader["siteid"]),
                        ServerId = Convert.ToInt32(reader["serverid"]),
                        Id = Convert.ToInt32(reader["serversiteid"]),
                        ServerIp = reader["ip"].ToString(),
                        SiteName = reader["Name"].ToString()
                    });
                }
            }

            return serversites;
        }

        public Result Insert(UpdateServerSiteViewModel criteria)
        {
            var isExisting = false;
            isExisting = Existing(criteria);
            if (isExisting)
            {
                return new Result
                {
                    ErrorCode = -1,
                    ErrorMessage = "The site is added to server. Please check."
                };
            }

            int errorCode = 0;
            try
            {
                var sql = "insert into serversite('serverid','siteid') values(" + criteria.ServerId + "," + criteria.SiteId + ")";
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

        public Result Update(UpdateServerSiteViewModel criteria)
        {
            var isExisting = false;
            isExisting = Existing(criteria);
            if (isExisting)
            {
                return new Result
                {
                    ErrorCode = -1,
                    ErrorMessage = "The site is added to server. Please check."
                };
            }

            int errorCode = 0;
            try
            {
                var sql = "update serversite set serverid =" + criteria.ServerId + ", siteid=" + criteria.SiteId + " where id=" + criteria.Id;
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

        public Result Delete(int id)
        {
            int errorCode = 0;
            try
            {
                var sql = "delete from serversite where id=" + id;
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

        private bool Existing(UpdateServerSiteViewModel criteria)
        {
            var sql = "select * from serversite where serverid = " + criteria.ServerId + " and siteid = " + criteria.SiteId;

            var result = QueryDataScalar(sql);

            return result != null;
        }
    }
}