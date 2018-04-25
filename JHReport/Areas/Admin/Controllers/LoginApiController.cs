using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JHReport.Areas.Admin.Controllers
{
    public class LoginApiController : ApiController
    {
        public string Login(string id,string pwd)
        {
            return "success";
        }
    }
}
