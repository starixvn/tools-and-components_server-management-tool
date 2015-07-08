using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class UserSiteViewModel
    {
        public List<SiteModel> Sites { get; set; }

        public List<UserPermissionModel> UserPermission { get; set; }

        public int SiteId { get; set; }

        public int UserId { get; set; }
    }
}