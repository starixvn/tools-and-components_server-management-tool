using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServerManagement.Models;

namespace ServerManagement.Services
{
    public static class SessionService
    {
        private const string CurrentSesionKey = "Fanex.ServerManagement.CurrentSession";

        public static UserPermissionViewModel CurrentSession
        {
            get
            {
                return (UserPermissionViewModel)HttpContext.Current.Session[CurrentSesionKey];
            }

            set
            {
                HttpContext.Current.Session[CurrentSesionKey] = value;
            }
        }

        public static void ClearAll()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Cookies.Clear();
            }
        }
    }
}