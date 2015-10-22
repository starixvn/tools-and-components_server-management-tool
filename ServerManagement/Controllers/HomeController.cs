using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ServerManagement.Models;
using ServerManagement.Services;
using System;

namespace ServerManagement.Controllers
{
    public class HomeController : BaseController
    {
        private ServerService _serverService;
        private SiteService _siteService;
        private ServerSiteService _serverSiteService;
        private ServerSiteResultService _serverSiteResultService;
        private ServerSpecService _serverSpecService;
        private WindowsServiceService _windowsServiceService;
        private Helper _helper;

        public HomeController()
        {
            _serverService = new ServerService();
            _siteService = new SiteService();
            _serverSiteService = new ServerSiteService();
            _serverSiteResultService = new ServerSiteResultService();
            _serverSpecService = new ServerSpecService();
            _windowsServiceService = new WindowsServiceService();

            _helper = new Helper();
        }

        public ActionResult Index()
        {
            var sitePermission = new List<int>();
            var siteList = new List<SiteModel>();
            if (CurrentSession != null)
            {
                sitePermission = CurrentSession.SitePermission.
                    Where(x => x.Permission > 0).Select(x => x.SiteId).ToList();
                siteList = CurrentSession.SuperUser ?
                    _siteService.GetSites() :
                    _siteService.GetSites().Where(x => sitePermission.Contains(x.SiteId)).ToList();
            }

            var serverList = _serverService.GetServers();
            var serverSiteList = _serverSiteService.GetServerSite();
            var serverSiteResult = _serverSiteResultService.Replies().OrderByDescending(x => x.ReplyDate).Take(serverSiteList.Count);
            var machineViewModel = new List<MachinesViewModel>();

            foreach (var site in siteList)
            {
                var serverSites = serverSiteList.Where(x => x.SiteId == site.SiteId);
                var replyServers = new List<ReplyServer>();

                foreach (var serverSite in serverSites)
                {
                    var replyServer = serverSiteResult.Where(x => x.ServerSiteId == serverSite.Id);

                    replyServers.Add(new ReplyServer
                    {
                        ServerIp = serverList.Where(x => x.ServerId == serverSite.ServerId).FirstOrDefault().ServerIp,
                        ServerId = serverSite.ServerId,
                        ReplyTime = _helper.ConvertDateTimeToGMT7(replyServer.Select(x => x.ReplyDate).FirstOrDefault()).ToString(),
                        IsFail = replyServer.Select(x => x.IsResult).FirstOrDefault(),
                        ServerSiteId = serverSite.Id
                    });
                }

                machineViewModel.Add(new MachinesViewModel
                {
                    System = site.SiteName,
                    Id = site.SiteId,
                    ReplyServer = replyServers
                });
            }

            if (CurrentSession != null)
            {
                ViewBag.AliasName = CurrentSession.AliasName;
            }

            return View(machineViewModel);
        }

        public ActionResult Service()
        {          
            var model = GetService();

            return View(model);
        }

        [HttpGet]
        public ActionResult ServiceListView()
        {
            var model = GetService();

            return View(model);
        }

        [HttpGet]
        public JsonResult AutoReloadData()
        {
            var serverSiteList = _serverSiteService.GetServerSite();                       
            var serverSiteResult = _serverSiteResultService.Replies().OrderByDescending(x => x.ReplyDate).Take(serverSiteList.Count);
            var replyModel = new List<ReplyModel>();

            foreach (var serverReply in serverSiteResult)
            {
                var serverId = serverSiteList.First(x => x.Id == serverReply.ServerSiteId).ServerId;

                replyModel.Add(new ReplyModel
                {
                    ServerSiteId = serverReply.ServerSiteId,
                    ReplyDate = _helper.ConvertDateTimeToGMT7(serverReply.ReplyDate).ToString(),
                    IsResult = serverReply.IsResult,
                    Id = serverReply.Id,
                    ServerId = serverId,
                    BootUpTime = string.Empty, //_helper.ConvertDateTimeToGMT7(_serverSpecService.LastBootUpTime(serverIp)).ToString()
                    UnderMaintenance = GetUnderMaintenance()
                });
            }

            return Json(replyModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Reboot(string[] serverIds)
        {
            var listResult = new Dictionary<string, Result>();
            var sitePermission = new List<string>();
            var siteNoPermission = new List<string>();
            var servers = _serverService.GetServers();
            foreach (var serverId in serverIds)
            {
                var siteId = Convert.ToInt32(serverId.Split(':')[0]);
                var permission = CurrentSession.SitePermission.Where(x => x.SiteId == siteId).Select(x => x.Permission).First();

                if (CurrentSession.SuperUser || permission >= CustomRoles.Admin)
                {
                    sitePermission.Add(serverId);
                }
                else
                {
                    siteNoPermission.Add(serverId);
                }
            }

            foreach (var site in sitePermission)
            {
                var siteId = Convert.ToInt32(site.Split(':')[0]);
                var serverId = Convert.ToInt32(site.Split(':')[1]);
                var serverIp = servers.Where(x => x.ServerId == serverId).First().ServerIp;

                var result = _helper.RebootServer(serverIp);
                listResult.Add(serverIp, result);
            }

            foreach (var site in siteNoPermission)
            {
                var siteId = Convert.ToInt32(site.Split(':')[0]);
                var serverId = Convert.ToInt32(site.Split(':')[1]);
                var serverIp = servers.Where(x => x.ServerId == serverId).First().ServerIp;

                var result = new Result()
                {
                    ErrorCode = -1,
                    ErrorMessage = "No permission in this server"
                };

                listResult.Add(serverIp, result);

            }

            return Json(listResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RebootAll()
        {
            var listResult = new Dictionary<string, Result>();
            var sitePermission = CurrentSession.SitePermission.
                   Where(x => x.Permission > 1).Select(x => x.SiteId).ToList();
            var serverIds = CurrentSession.SuperUser ? 
                _serverSiteService.GetServerSite().Select(x => x.ServerId) :
                _serverSiteService.GetServerSite().Where(x => sitePermission.Contains(x.SiteId)).Select(x => x.ServerId);
            
            var serverList = _serverService.GetServers().Where(x => serverIds.Contains(x.ServerId));

            foreach (var server in serverList)
            {
                var result = _helper.RebootServer(server.ServerIp);
                listResult.Add(server.ServerId.ToString(), result);
            }

            return Json(listResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ViewResult ViewSpec(int serverId)
        {
            var server = _serverService.GetServers().Where(x => x.ServerId == serverId).First();
            server.IISVersion = _serverSpecService.GetIISVersion(server.ServerIp);
            server.DotNetFrameworkVersion = _serverSpecService.GetFrameworkVersion(server.ServerIp);

            return View(server);
        }

        [HttpGet]
        public ViewResult ViewIIS(int serverId)
        {
            var server = _serverService.GetServers().Where(x => x.ServerId == serverId).First();
            var model = _serverService.GetIISInformation(server);

            return View(model);
        }

        [HttpGet]
        public ViewResult ViewService(int serverId, int siteId)
        {
            var serverSiteId = _serverSiteService.GetServerSite().Where(x => x.ServerId == serverId && x.SiteId == siteId).First().Id;
            var serverIp = _serverService.GetServers().Where(x => x.ServerId == serverId).First().ServerIp;

            var model = _windowsServiceService.WindowsServiceFromDB(serverIp, serverSiteId);
            ViewBag.ServerId = serverId;

            return View(model);
        }

        [HttpPost]
        public JsonResult StartService(string serviceName, int serverId)
        {
            var serverIp = _serverService.GetServers().Where(x => x.ServerId == serverId).First().ServerIp;
            var result = _windowsServiceService.StartService(serviceName, serverIp);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StopService(string serviceName, int serverId)
        {
            var serverIp = _serverService.GetServers().Where(x => x.ServerId == serverId).First().ServerIp;
            var result = _windowsServiceService.StopService(serviceName, serverIp);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUnderMaintenanceInfo()
        {
            var underMaintenance = GetUnderMaintenance();

            return Json(underMaintenance, JsonRequestBehavior.AllowGet);
        }

        private List<ServiceManagementModel> GetService()
        {
            if (CurrentSession != null)
            {
                ViewBag.AliasName = CurrentSession.AliasName;
            }

            var sitePermission = new List<int>();
            var siteList = new List<SiteModel>();
            if (CurrentSession != null)
            {
                sitePermission = CurrentSession.SitePermission.
                    Where(x => x.Permission > 0).Select(x => x.SiteId).ToList();
                siteList = CurrentSession.SuperUser ?
                    _siteService.GetSites() :
                    _siteService.GetSites().Where(x => sitePermission.Contains(x.SiteId)).ToList();
            }

            var siteIdList = siteList.Select(x => x.SiteId);

            return _windowsServiceService.GetAllService().Where(x => siteIdList.Contains(x.SiteId)).ToList();
        }
    }
}