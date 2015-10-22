using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class ReplyModel
    {
        public int Id { get; set; }

        public int ServerSiteId { get; set; }

        public string ReplyDate { get; set; }

        public bool IsResult { get; set; }

        public string BootUpTime { get; set; }

        public int ServerId { get; set; }

        public UnderMaintenanceModel UnderMaintenance { get; set; }
    }
}