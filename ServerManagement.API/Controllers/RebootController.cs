using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using ServerManagement.API.Models;
using Newtonsoft.Json;

namespace ServerManagement.API.Controllers
{
    public class RebootController : BaseController
    {
        [System.Web.Http.HttpPost]
        public Result Post([FromBody] ProxyCriteria Parameter)
        {
            var plainParameter = DecryptProxyCriteria(Parameter);
            var serverCriteria = JsonConvert.DeserializeObject<ServerCriteria>(plainParameter);

            return new Result() { ErrorMessage = serverCriteria.Ips[0].ToString() };
        }
    }
}
