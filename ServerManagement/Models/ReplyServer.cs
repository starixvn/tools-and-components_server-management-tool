using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class ReplyServer
    {
        public string ServerIp { get; set; }

        public int ServerId { get; set; }

        public string ReplyTime { get; set; }

        public bool IsFail { get; set; }

        public int ServerSiteId { get; set; }
    }
}