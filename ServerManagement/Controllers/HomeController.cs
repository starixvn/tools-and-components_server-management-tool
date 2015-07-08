using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ServerManagement.Models;
using ServerManagement.Services;

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
            var siteList = _siteService.GetSites();
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
                    ReplyServer = replyServers
                });
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
        public JsonResult Reboot(string[] serverIps, string[] serverIds)
        {
            var servers = serverIps;
            var listResult = new Dictionary<string, Result>();
            for (int i = 0; i < serverIps.Count(); i++)
            {
                var result = _helper.RebootServer(serverIps[i]);
                listResult.Add(serverIds[i], result);
            }

            return Json(listResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RebootAll()
        {
            var serverList = _serverService.GetServers();
            var listResult = new Dictionary<string, Result>();

            foreach (var server in serverList)
            {
                var result = _helper.RebootServer(server.ServerIp);
                listResult.Add(server.ServerId.ToString(), result);
            }

            return Json(listResult, JsonRequestBehavior.AllowGet);
        }
    }
}