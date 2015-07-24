using System.Linq;
using System.Web.Mvc;
using ServerManagement.Models;
using ServerManagement.Services;
using System.Collections.Generic;

namespace ServerManagement.Controllers
{
    public class SettingController : BaseController
    {
        private UserService _userService;
        private ServerService _serverService;
        private SiteService _siteService;
        private ServerSiteService _serverSiteService;
        private UserSiteService _userSiteService;
        private Helper _helper;

        public SettingController()
        {
            _userService = new UserService();
            _helper = new Helper();
            _serverService = new ServerService();
            _siteService = new SiteService();
            _serverSiteService = new ServerSiteService();
            _userSiteService = new UserSiteService();
        }
        
        [HttpGet]
        public ActionResult SystemList()
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var siteList = _siteService.GetSites();

            return View(siteList);
            
        }

        [HttpGet]
        public ActionResult UpdateSystem(int siteId = 0)
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var site = new SiteModel();
            if (siteId != 0)
            {
                site = _siteService.GetSites().Where(x => x.SiteId == siteId).FirstOrDefault();
            }

            return View(site);
        }

        [HttpPost]
        public ActionResult UpdateSystem(SiteModel param)
        {
            var result = new Result();

            if (param.SiteId != 0)
            {
                result = _siteService.Update(param);
            }
            else
            {
                result = _siteService.Insert(param);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ServerList()
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var serverList = _serverService.GetServers();

            return View(serverList);
        }

        [HttpGet]
        public ActionResult UpdateServer(int serverId = 0)
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var server = new ServerModel();
            if (serverId != 0)
            {
                server = _serverService.GetServers().Where(x => x.ServerId == serverId).FirstOrDefault();
            }

            return View(server);
        }

        [HttpPost]
        public ActionResult UpdateServer(ServerModel param)
        {
            var result = new Result();

            if (param.ServerId != 0)
            {
                result = _serverService.Updatet(param);
            }
            else
            {
                result = _serverService.Insert(param);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SystemServerList()
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var siteServerList = _serverSiteService.GetServerSite();

            return View(siteServerList);
        }

        [HttpGet]
        public ActionResult UpdateSystemServer(int id = 0)
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var model = new UpdateServerSiteViewModel
            {
                Sites = _siteService.GetSites(),
                Servers = _serverService.GetServers(),
                SiteId = 0,
                ServerId = 0,
                Id = id
            };

            if (id != 0)
            {
                var serverSite = _serverSiteService.GetServerSite().Where(x => x.Id == id).FirstOrDefault();
                model.ServerId = serverSite.ServerId;
                model.SiteId = serverSite.SiteId;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateSystemServer(UpdateServerSiteViewModel param)
        {
            var result = new Result();

            if (param.Id != 0)
            {
                result = _serverSiteService.Update(param);
            }
            else
            {
                result = _serverSiteService.Insert(param);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteSystemServer(int id)
        {
            var result = new Result();

            result = _serverSiteService.Delete(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UsersList(UserModel param)
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var model = _userService.GetUsers();

            return View(model);
        }

        [HttpGet]
        public ActionResult UpdateUser(int userId = 0)
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var user = new UserModel();
            if (userId != 0)
            {
                user = _userService.GetUsers().Where(x => x.UserId == userId).FirstOrDefault();
                user.AliasName = user.AliasName;
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult UpdateUser(UserModel param)
        {
            var result = new Result();

            if (param.UserId != 0)
            {
                result = _userService.Update(param);
            }
            else
            {
                result = _userService.Insert(param);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteUser(int userId)
        {
            var result = new Result();

            result = _userService.Delete(userId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserPermission()
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var sites = _siteService.GetSites();
            var siteId = sites.First().SiteId;
            var userPermission = UserPermission(siteId);
            var model = new UserSiteViewModel
            {
                Sites = sites,
                SiteId = sites.Select(x => x.SiteId).FirstOrDefault(),
                UserPermission = userPermission
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult UserPermissionView(int siteId)
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var userPermission = UserPermission(siteId);

            var model = new UserSiteViewModel
            {
                UserPermission = userPermission
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult SetUserPermission(UserSiteModel param)
        {
            var result = new Result();

            var isExisting = _userSiteService.Existing(param);

            if (isExisting)
            {
                result = _userSiteService.Update(param);
            }
            else
            {
                result = _userSiteService.Insert(param);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private List<UserPermissionModel> UserPermission(int siteId)
        {
            var site = _siteService.GetSites().Where(x => x.SiteId == siteId).FirstOrDefault();
            var users = _userService.GetUsers();
            var userSites = _userSiteService.GetUserSite().Where(x => x.SiteId == siteId);
            var userPermission = userSites.FullOuterJoinJoin(users, us => us.UserId, u => u.UserId, (us, u) => new { myUser = u, myUserSite = us })
                .Select(a => new UserPermissionModel
                {
                    UserId = a.myUser.UserId,
                    UserName = a.myUser.Username,
                    Permission = a.myUserSite == null ? 0 : a.myUserSite.SiteId == site.SiteId ? a.myUserSite.Permission : 0,
                    SiteId = a.myUserSite == null ? 0 : a.myUserSite.SiteId
                }).ToList();

            return userPermission;
        }
    }
}