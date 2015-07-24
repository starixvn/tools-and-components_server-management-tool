using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ServerManagement.Models;
using ServerManagement.Services;

namespace ServerManagement.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!HttpContext.User.Identity.IsAuthenticated || CurrentSession == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            base.OnAuthorization(filterContext);
        }

        public static UserPermissionViewModel CurrentSession
        {
            get {
                return SessionService.CurrentSession;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (CurrentSession != null)
            //{
            //    if (CurrentSession.SitePermission == CustomRoles.NoAccess)
            //    {
            //        Redirect("~/Authentication/NoAccess");
            //    }
            //}
            //else
            //{
            //    Redirect("~/Authentication/Login");
            //}

            base.OnActionExecuting(filterContext);
        }
    }
}