using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using ServerManagement.API.Models;
using System.Web.Hosting;

namespace ServerManagement.API.Controllers
{
    public class BaseController : ApiController
    {
        protected static string DecryptProxyCriteria(ProxyCriteria criteria)
        {
            var keyPairPath = HostingEnvironment.ApplicationPhysicalPath + @"App_Data\keypair.snk";
            return Utility.Decrypt(criteria.Parameter, keyPairPath);
        }
    }
}
