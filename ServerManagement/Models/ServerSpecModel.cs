using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class ServerSpecModel
    {
        public string OperatingSystem { get; set; }

        public string Version { get; set; }

        public string Manufacturer { get; set; }

        public string Disk { get; set; }

        public string Memory { get; set; }

        public string CPU { get; set; }

        public string HostName { get; set; }

        public string Ips { get; set; }

        public Version IISVersion { get; set; }

        public string DotNetFrameworkVersion { get; set; }

    }
}