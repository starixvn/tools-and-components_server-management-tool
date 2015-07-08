using System;
using System.Collections.Generic;
using ServerManagement.Models;

namespace ServerManagement.Services
{
    public class ServerService : BaseService
    {
        public List<ServerModel> GetServers()
        {
            var sql = "select * from server";
            var servers = new List<ServerModel>();

            using (var reader = QueryData(sql))
            {
                while (reader.Read())
                {
                    servers.Add(new ServerModel
                    {
                        ServerIp = reader["ip"].ToString(),
                        ServerName = reader["name"].ToString(),
                        ServerId = Convert.ToInt32(reader["id"])
                    });
                }
            }

            return servers;
        }

        public Result Insert(ServerModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "insert into server('ip','name') values('" + criteria.ServerIp + "','" + criteria.ServerName + "')";
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

        public Result Updatet(ServerModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "update server set ip='" + criteria.ServerIp + "', name='" + criteria.ServerName + "' where id=" + criteria.ServerId;
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