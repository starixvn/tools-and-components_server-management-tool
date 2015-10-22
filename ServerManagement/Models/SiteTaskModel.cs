using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class SiteTaskModel
    {
        public int TaskId { get; set; }

        public int SiteId { get; set; }

        public int Id { get; set; }

        public bool IsAssigned { get; set; }
    }
}