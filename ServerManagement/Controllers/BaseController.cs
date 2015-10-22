using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ServerManagement.Models;
using ServerManagement.Services;
using Sunplus.UMControllerLibrary;
using System.Configuration;

namespace ServerManagement.Controllers
{
    public class BaseController : Controller
    {
        private UMBusiness _umService = new UMBusiness();

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

        public UnderMaintenanceModel GetUnderMaintenance()
        {
            var underMaintenanceInfo = new UnderMaintenanceModel();
            var dateFrom = underMaintenanceInfo.TimeFrom;
            var dateTo = underMaintenanceInfo.TimeTo;

            underMaintenanceInfo.Status = _umService.isUnderMaintainence(Sites.Agency, ConfigurationManager.AppSettings["UnderMaintenanceUrl"], ref dateFrom, ref dateTo);
            underMaintenanceInfo.TimeFrom = dateFrom;
            underMaintenanceInfo.TimeTo = dateTo;

            return underMaintenanceInfo;
        }
    }
}