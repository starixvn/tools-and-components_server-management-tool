using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ServerManagement.Models;
using ServerManagement.Services;

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
        private ServerSpecService _serverSpecService;
        private WindowsServiceService _windowServiceService;
        private TaskService _taskService;
        private SiteTaskService _siteTaskService;

        public SettingController()
        {
            _userService = new UserService();
            _helper = new Helper();
            _serverService = new ServerService();
            _siteService = new SiteService();
            _serverSiteService = new ServerSiteService();
            _userSiteService = new UserSiteService();
            _serverSpecService = new ServerSpecService();
            _windowServiceService = new WindowsServiceService();
            _taskService = new TaskService();
            _siteTaskService = new SiteTaskService();
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
                result = _serverService.Update(param);
            }
            else
            {
                result = _serverService.Insert(param);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSpecServer(ServerModel param)
        {
            var result = new Result();
            var server = _serverService.GetServers().Where(x => x.ServerId == param.ServerId).First();
            var serverSpecModel = new ServerSpecModel();

            serverSpecModel = _serverSpecService.ComputerSystem(server.ServerIp, ref serverSpecModel);
            serverSpecModel = _serverSpecService.NetworkData(server.ServerIp, ref serverSpecModel);
            serverSpecModel = _serverSpecService.OperatingSystem(server.ServerIp, ref serverSpecModel);
            serverSpecModel = _serverSpecService.VolumeData(server.ServerIp, ref serverSpecModel);
            serverSpecModel = _serverSpecService.ProcessorData(server.ServerIp, ref serverSpecModel);

            if (param.ServerId != 0)
            {
                result = _serverService.UpdateSpec(param, serverSpecModel);
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

        [HttpGet]
        public ActionResult ServiceList()
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var sites = _siteService.GetSites();
            var siteId = sites.First().SiteId;
            var servers = _serverService.GetServers();       
            var serverSites = _serverSiteService.GetServerSite().Where(x => x.SiteId == siteId);
            var serverId = serverSites.First().ServerId;
            var serverIp = serverSites.First().ServerIp;
            var serverSiteId = _serverSiteService.GetServerSite().Where(x => x.SiteId == siteId && x.ServerId == serverId).First().Id;
            var serverIds = serverSites.Select(x => x.ServerId).ToList();

            var model = new ServiceManagementViewModel()
            {
                Sites = sites,
                Servers = servers.Where(x => serverIds.Contains(x.ServerId)).ToList(),
                ServerId = serverId,
                SiteId = siteId,
                Services = _windowServiceService.WindowsService(serverIp, serverSiteId)
            };

            return View(model);
        }

        [HttpGet]
        public JsonResult GetServerBySite(int siteId)
        {
            var serverSites = _serverSiteService.GetServerSite().Where(x => x.SiteId == siteId);

            return Json(serverSites, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ServiceListView(int serverId, int siteId, int filter)
        {
            var server = _serverService.GetServers().Where(x => x.ServerId == serverId).First();
            var serverSite = _serverSiteService.GetServerSite().Where(x => x.ServerId == serverId && x.SiteId == siteId).First();

            var model = new List<ServiceManagementModel>();
            if (filter == 1)
            {
                model = _windowServiceService.WindowsService(server.ServerIp, serverSite.Id).OrderBy(x => x.DisplayName).ToList();
            }
            else
            {
                model = _windowServiceService.WindowsServiceFromDB(server.ServerIp, serverSite.Id).OrderBy(x => x.DisplayName).ToList();
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult InsertService(int siteId, int serverId, string serviceName, string displayName, int status)
        {
            var serverSiteId = _serverSiteService.GetServerSite().First(x => x.ServerId == serverId && x.SiteId == siteId).Id;

            var model = new ServiceManagementModel()
            {
                ServiceSiteId = serverSiteId,
                ServiceName = serviceName,
                DisplayName = displayName,
                Status = status
            };
            var isExisting = _windowServiceService.ExistingWindowService(serviceName, serverSiteId);
            var result = new Result();
            if (isExisting != null)
            {
                result = new Result
                {
                    ErrorCode = -1,
                    ErrorMessage = "The service is existing, please check again."
                };
            }
            else
            {
                result = _windowServiceService.InsertWindowService(model);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveService(int siteId, int serverId, string serviceName)
        {
            var serverSite = _serverSiteService.GetServerSite().Where(x => x.ServerId == serverId && x.SiteId == siteId).First();
            var result = _windowServiceService.RemoveWindowService(serverSite.Id, serviceName);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaskList(TaskModel param)
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var model = _taskService.GetTasks();

            return View(model);
        }


        [HttpGet]
        public ActionResult UpdateTask(int taskId = 0)
        {
            if (!CurrentSession.SuperUser)
            {
                return Redirect("~/Authorization/NoAccess");
            }

            var task = new TaskModel();
            if (taskId != 0)
            {
                task = _taskService.GetTasks().Where(x => x.TaskId == taskId).FirstOrDefault();
            }

            return View(task);
        }

        [HttpPost]
        public ActionResult UpdateTask(TaskModel param)
        {
            var result = new Result();

            if (param.TaskId != 0)
            {
                result = _taskService.Update(param);
            }
            else
            {
                result = _taskService.Insert(param);
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

        private List<SiteTaskModel> SiteTask(int siteId)
        {
            var site = _siteService.GetSites().Where(x => x.SiteId == siteId).FirstOrDefault();
            var task = _taskService.GetTasks();
            var siteTask = _siteTaskService.GetSiteTasks().Where(x => x.SiteId == siteId);

            var siteTaskModel = siteTask.FullOuterJoinJoin(task, st => st.TaskId, t => t.TaskId, (st, t) => new { mySiteTask = st, myTask = t })
                .Select(a => new SiteTaskModel
                {
                    SiteId = a.mySiteTask.SiteId,
                    //TaskName = a.myTask.TaskName,
                    TaskId = a.myTask.TaskId,

                }).ToList();

            return siteTaskModel;
        }
    }
}