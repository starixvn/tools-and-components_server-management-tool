using System;
using System.Collections.Generic;
using ServerManagement.Models;
using System.Management;
using Microsoft.Web.Administration;

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
                        ServerId = Convert.ToInt32(reader["id"]),
                        OperatingSystem = reader["operationsystem"].ToString(),
                        Version = reader["version"].ToString(),
                        Manufacturer = reader["Manufacturer"].ToString(),
                        Disk = reader["disk"].ToString(),
                        Memory = reader["Memory"].ToString(),
                        CPU = reader["cpu"].ToString(),
                        HostName = reader["name"].ToString()
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
                var sql = "insert into server('ip') values('" + criteria.ServerIp + "')";
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

        public Result Update(ServerModel criteria)
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

        public Result UpdateSpec(ServerModel criteria, ServerSpecModel serverSpecMode)
        {
            int errorCode = 0;
            try
            {
                var sql = "update server set name='" + serverSpecMode.HostName +
                    "', operationsystem='" + serverSpecMode.OperatingSystem +
                    "', version='" + serverSpecMode.Version +
                    "', Manufacturer='" + serverSpecMode.Manufacturer +
                    "', Disk='" + serverSpecMode.Disk +
                    "', Memory='" + serverSpecMode.Memory +
                    "', CPU='" + serverSpecMode.CPU +
                    "' where id=" + criteria.ServerId;

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

        public List<IISInformationViewModel> GetIISInformation(ServerModel criteria)
        {
            using (ServerManager serverManager = ServerManager.OpenRemote(criteria.ServerIp))
            {
                var iisInformationList = new List<IISInformationViewModel>();

                foreach (var site in serverManager.Sites)
                {
                    var ports = new List<string>();
                    var virtualDirectory = new List<IISInformationViewModel>();

                    foreach (var binding in site.Bindings)
                    {
                        ports.Add(binding.BindingInformation);
                    }

                    if (site.Applications.Count > 0)
                    {
                        foreach (Application app in site.Applications)
                        {
                            virtualDirectory.Add(new IISInformationViewModel
                            {
                                SiteName = app.Path,
                                PhysicalPath = app.VirtualDirectories.Count > 0 ? app.VirtualDirectories["/"].PhysicalPath : string.Empty
                            });
                        }
                    }

                    iisInformationList.Add(new IISInformationViewModel
                    {
                        SiteId = site.Id,
                        SiteName = site.Name,
                        Ports = ports,
                        VirtualDirectory = virtualDirectory,
                        PhysicalPath = site.Applications["/"].VirtualDirectories["/"].PhysicalPath
                    });
                }

                return iisInformationList;
            }
        }
    }
}