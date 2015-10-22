using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class IISInformationViewModel
    {
        public Int64 SiteId { get; set; }

        public string SiteName { get; set; }

        public string PhysicalPath { get; set; }

        public List<string> Ports { get; set; }

        public string State { get; set; }

        public string HostHeader { get; set; }

        public List<IISInformationViewModel> VirtualDirectory { get; set; }
    }
}