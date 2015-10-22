using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceProcess;

namespace ServerManagement.Models
{
    public class ServiceManagementModel
    {
        public int ServiceSiteId { get; set; }

        public int ServerId { get; set; }

        public string ServerIp { get; set; }

        public string SiteName { get; set; }

        public int SiteId { get; set; }

        public int Id { get; set; }

        public string ServiceName { get; set; }

        public string DisplayName { get; set; }

        public int Status { get; set; }

        public bool InDB { get; set; }
    }
}