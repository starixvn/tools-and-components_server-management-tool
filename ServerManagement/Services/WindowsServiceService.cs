using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management;
using ServerManagement.Models;
using System.ServiceProcess;

namespace ServerManagement.Services
{
    public class WindowsServiceService : BaseService
    {
        public List<ServiceManagementModel> WindowsService(string ip, int serverSiteId)
        {
            ServiceController[] services = ServiceController.GetServices(ip);
            var model = new List<ServiceManagementModel>();
            var serviceInDB = WindowsServiceFromDB(ip, serverSiteId).Select(x => x.ServiceName);

            foreach (ServiceController service in services)
            {
                model.Add(new ServiceManagementModel
                {
                    DisplayName = service.DisplayName,
                    ServiceName = service.ServiceName,
                    Status = service.Status == ServiceControllerStatus.Stopped ? 1 : 4,
                    InDB = serviceInDB.Contains(service.ServiceName)
                });
            }

            return model;
        }

        public Result StopService(string serviceName, string serverIp)
        {
            try
            {
                var service = GetService(serviceName, serverIp);

                if (service != null && service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                }

                return new Result() { ErrorCode = 0, ErrorMessage = string.Empty };
            }
            catch (Exception ex)
            {
                return new Result() { ErrorCode = -1, ErrorMessage = ex.Message };
            }
        }

        public Result StartService(string serviceName, string serverIp)
        {
            try
            {
                var service = GetService(serviceName, serverIp);

                if (service != null && service.Status == ServiceControllerStatus.Stopped)
                {
                    service.Start();
                }

                return new Result() { ErrorCode = 0, ErrorMessage = string.Empty };
            }
            catch (Exception ex)
            {
                return new Result() { ErrorCode = -1, ErrorMessage = ex.Message };
            }
        }

        public ServiceController GetService(string serviceName, string serverIp)
        {
            ServiceController[] services = ServiceController.GetServices(serverIp);
            return services.FirstOrDefault(x => string.Compare(x.ServiceName, serviceName, true) == 0);
        }

        public List<ServiceManagementModel> WindowsServiceFromDB(string ip, int serverSiteId)
        {
            var sql = "select * from windowservices where serversiteid=" + serverSiteId;
            var services = new List<ServiceManagementModel>();
            var serverService = ServiceController.GetServices(ip).Select(x => new ServiceManagementModel { Status = x.Status == ServiceControllerStatus.Stopped ? 1 : 4, ServiceName = x.ServiceName }).ToList();
            using (var reader = QueryData(sql))
            {
                while (reader.Read())
                {
                    var windowService = serverService.Where(x => x.ServiceName == reader["name"].ToString());

                    services.Add(new ServiceManagementModel
                    {
                        ServiceName = reader["name"].ToString(),
                        DisplayName = reader["displayname"].ToString(),
                        Status = windowService.Count() > 0 ? windowService.First().Status : 1,
                        Id = Convert.ToInt32(reader["id"]),
                        InDB = true
                    });
                }
            }

            return services;
        }

        public List<ServiceManagementModel> GetAllService()
        {
            var sql = "select ws.id, ws.serversiteid, ws.name, ws.displayname, ws.status, sv.ip as ip, s.name as sitename, ss.serverid, ss.siteid from windowservices ws inner join serversite ss on ws.serversiteid = ss.id inner join server sv on ss.serverid = sv.id inner join site s on s.id = ss.siteid";
            var services = new List<ServiceManagementModel>();
            using (var reader = QueryData(sql))
            {
                while (reader.Read())
                {
                    var windowService = GetService(reader["name"].ToString(), reader["ip"].ToString());
                    services.Add(new ServiceManagementModel
                    {
                        ServiceName = reader["name"].ToString(),
                        DisplayName = reader["displayname"].ToString(),
                        Status = windowService != null ? windowService.Status == ServiceControllerStatus.Stopped ? 1 : 4 : 1,
                        Id = Convert.ToInt32(reader["id"]),
                        ServerId = Convert.ToInt32(reader["serverid"]),
                        SiteId = Convert.ToInt32(reader["siteid"]),
                        ServiceSiteId = Convert.ToInt32(reader["serversiteid"]),
                        ServerIp = reader["ip"].ToString(),
                        SiteName = reader["sitename"].ToString()                        
                    });
                }
            }

            return services;
        }

        public Result InsertWindowService(ServiceManagementModel criteria)
        {
            int errorCode = 0;
            try
            {
                var sql = "insert into windowServices('serversiteid','name','displayname','status') values(" + criteria.ServiceSiteId + ",'" + criteria.ServiceName + "','" + criteria.DisplayName + "'," + criteria.Status + ")";
                errorCode = UpdateData(sql);
            }
            catch (Exception ex)
            {
                return new Result
                {
                    ErrorCode = -1,
                    ErrorMessage = ex.Message
                };
            }

            return new Result
            {
                ErrorCode = errorCode,
                ErrorMessage = string.Empty
            };
        }

        public object ExistingWindowService(string serviceName, int serverSiteId)
        {
            try
            {
                var sql = "select * from windowservices where name='" + serviceName + "' and serversiteid=" + serverSiteId;

                return QueryDataScalar(sql);
            }
            catch
            {
                return null;
            }
        }

        public Result RemoveWindowService(int serverSiteId, string serviceName)
        {
            int errorCode = 0;
            try
            {
                var sql = "Delete from windowServices where ServerSiteId=" + serverSiteId + " and name='" + serviceName + "'";
                errorCode = UpdateData(sql);
            }
            catch (Exception ex)
            {
                return new Result
                {
                    ErrorCode = -1,
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