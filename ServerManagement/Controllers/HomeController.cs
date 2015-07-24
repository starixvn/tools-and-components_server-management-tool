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
        private Helper _helper;

        public HomeController()
        {
            _serverService = new ServerService();
            _siteService = new SiteService();
            _serverSiteService = new ServerSiteService();
            _serverSiteResultService = new ServerSiteResultService();
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

                foreach (var server in serverSites)
                {
                    var replyServer = serverSiteResult.Where(x => x.ServerSiteId == server.Id);

                    replyServers.Add(new ReplyServer
                    {
                        ServerIp = serverList.Where(x => x.ServerId == server.ServerId).FirstOrDefault().ServerIp,
                        ReplyTime = _helper.ConvertDateTimeToGMT7(replyServer.Select(x => x.ReplyDate).FirstOrDefault()).ToString(),
                        IsFail = replyServer.Select(x => x.IsResult).FirstOrDefault(),
                        ServerSiteId = server.Id
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

        [HttpGet]
        public JsonResult AutoReloadData()
        {
            var serverSiteList = _serverSiteService.GetServerSite();
            var serverSiteResult = _serverSiteResultService.Replies().OrderByDescending(x => x.ReplyDate).Take(serverSiteList.Count);
            var replyModel = new List<ReplyModel>();

            foreach (var serverReply in serverSiteResult)
            {
                replyModel.Add(new ReplyModel
                {
                    ServerSiteId = serverReply.ServerSiteId,
                    ReplyDate = _helper.ConvertDateTimeToGMT7(serverReply.ReplyDate).ToString(),
                    IsResult = serverReply.IsResult,
                    Id = serverReply.Id
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
    }
}