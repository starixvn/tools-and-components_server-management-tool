using System;
using System.Collections.Generic;
using ServerManagement.Models;

namespace ServerManagement.Services
{
    public class SiteService : BaseService
    {
        public List<SiteModel> GetSites()
        {
            var sql = "select * from site";
            var sites = new List<SiteModel>();

            using (var reader = QueryData(sql))
            {
                while (reader.Read())
                {
                    sites.Add(new SiteModel
                    {
                        SiteId = Convert.ToInt32(reader["Id"]),
                        SiteName = reader["Name"].ToString(),
                        Description = reader["Description"].ToString()
                    });
                }
            }

            return sites;
        }

        public Result Insert(SiteModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "insert into site('name','description') values('" + criteria.SiteName + "','" + criteria.Description + "')";
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

        public Result Update(SiteModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "update site set name='" + criteria.SiteName + "', description='" + criteria.Description + "' where id=" + criteria.SiteId;
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