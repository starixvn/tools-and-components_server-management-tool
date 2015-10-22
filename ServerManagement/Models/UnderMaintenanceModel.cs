using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServerManagement.Models
{
    public class UnderMaintenanceModel
    {
        public bool  Status { get; set; }

        public string TimeFrom { get; set; }

        public string TimeTo { get; set; }

        public UnderMaintenanceModel()
        {
            Status = false;
            var nowHour = DateTime.Today.AddHours(DateTime.Now.Hour);
            TimeFrom = nowHour.ToShortTimeString();
            TimeTo = nowHour.ToShortTimeString();
        }
    }
}
