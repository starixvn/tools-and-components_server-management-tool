using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerManagement.Models
{
    public class UpdateServerSiteViewModel
    {
        public List<SiteModel> Sites { get; set; }

        public List<ServerModel> Servers { get; set; }

        public int SiteId { get; set; }

        public int ServerId { get; set; }

        public int Id { get; set; }
    }
}