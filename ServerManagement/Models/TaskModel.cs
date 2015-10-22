using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class TaskModel
    {
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public string Action { get; set; }
    }
}