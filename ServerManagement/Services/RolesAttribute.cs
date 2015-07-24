using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServerManagement.Controllers;

namespace ServerManagement.Services
{
    public class RolesAttribute : AuthorizeAttribute
    {
        public RolesAttribute(params int[] roles)
        {
            this.CustomeRoles = roles;
        }

        public int[] CustomeRoles { get; set; }

        //private int AuthorizeUser(AuthorizationContext filterContext)
        //{
        //    if (filterContext.RequestContext.HttpContext != null)
        //    {
        //       // var context = filterContext.RequestContext.HttpContext;
        //        if (BaseController.CurrentSession != null)
        //        {
        //            return BaseController.CurrentSession.Permission;
        //        }

        //        return 0;
        //    }
        //    throw new ArgumentException("filterContext");
        //}

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //if (filterContext == null)
            //    throw new ArgumentException("filterContext");

            //var role = AuthorizeUser(filterContext);
            //if (!CustomeRoles.Contains(role))
            //{           
            //    // denied! redirect to login page or show denied page (403)
            //    filterContext.Result = new HttpUnauthorizedResult();
                
            //}
        }
    }
}