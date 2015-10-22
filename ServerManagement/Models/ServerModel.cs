using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class ServerModel : ServerSpecModel
    {
        public int ServerId { get; set; }

        public string ServerIp { get; set; }

        public string ServerName { get; set; }
    }
}