using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class ServiceManagementViewModel
    {
        public List<SiteModel> Sites { get; set; }

        public List<ServerModel> Servers { get; set; }

        public List<ServiceManagementModel> Services { get; set; }

        public int ServerId { get; set; }

        public int SiteId { get; set; }

        public string ServiceName { get; set; }

        public bool Status { get; set; }
    }
}