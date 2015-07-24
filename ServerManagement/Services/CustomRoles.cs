using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Services
{
    public static class CustomRoles
    {
        public const int Admin = 2;
        public const int View = 1;
        public const int NoAccess = 0;
        public const int SuperUser = 3;
    }
}