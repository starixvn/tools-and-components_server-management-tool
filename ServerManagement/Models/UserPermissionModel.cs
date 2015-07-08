using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class UserPermissionModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int SiteId { get; set; }

        public string SiteName { get; set; }

        public int Permission { get; set; }
    }
}