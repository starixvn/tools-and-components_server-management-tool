using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class MachinesViewModel
    {
        public string System { get; set; }

        public List<ReplyServer> ReplyServer { get; set; }
    }
}