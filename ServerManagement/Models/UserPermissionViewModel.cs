using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class UserPermissionViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string AliasName { get; set; }

        public bool SuperUser { get; set; }

        public List<SitePermissionViewModel> SitePermission { get; set; }
    }
}