using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ServerManagement.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            base.OnAuthorization(filterContext);
        }
    }
}