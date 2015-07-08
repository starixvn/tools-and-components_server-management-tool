using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ServerManagement.Services;

namespace ServerManagement.Controllers
{
    public class AuthorizationController : Controller
    {
        private AuthenticationService _authenticationService;

        public AuthorizationController()
        {
            _authenticationService = new AuthenticationService();
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
                FormsAuthentication.SetAuthCookie(username, true);

                return Redirect("~/home/index");
            }
            else
            {
                return View();
            }
        }      
    }
}
