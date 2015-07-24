using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ServerManagement.Services;
using ServerManagement.Models;

namespace ServerManagement.Controllers
{
    public class AuthorizationController : Controller
    {
        private AuthenticationService _authenticationService;
        private UserService _userService;
        private UserSiteService _userSiteService;

        public AuthorizationController()
        {
            _authenticationService = new AuthenticationService();
            _userService = new UserService();
            _userSiteService = new UserSiteService();
        }
        //
        // GET: /Authorization/
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var isValidated = _authenticationService.ValidateUser(username, password);
            if (isValidated)
            {
                var users = _userService.GetUsers();

                var isExistingUser = users.Any();
                if (isExistingUser)
                {
                    var user = users.Where(x => x.Username == username).First();
                    var sitePermission = _userSiteService.GetUserSite().Where(x => x.UserId == user.UserId)
                        .Select(y => new SitePermissionViewModel {
                            SiteId = y.SiteId,
                            Permission = y.Permission
                        }).ToList();
                    var userPermission = new UserPermissionViewModel
                    {
                        UserId = user.UserId,
                        UserName = user.Username,
                        AliasName = user.AliasName,
                        SuperUser = user.SuperUser,
                        SitePermission = sitePermission
                    };

                     SessionService.CurrentSession = userPermission;
                    FormsAuthentication.RedirectFromLoginPage(username, true);

                    return Redirect("~/home/index");
                }
                else
                {
                    return Redirect("noaccess");
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult NoAccess()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return Json(new { ErrorCode = 0 }, JsonRequestBehavior.AllowGet);
        }
    }
}
