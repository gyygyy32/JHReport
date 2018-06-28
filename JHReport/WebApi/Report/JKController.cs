using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JHReport.BLL;

namespace JHReport.WebApi.Report
{
    //晶科报表
    [RoutePrefix("api/JK")]
    public class JKController : ApiController
    {
        [Route("QueryInfo")]
        [HttpPost]
        public IHttpActionResult QueryInfo(dynamic para)
        {
            var res = new ReportService().JKQueryInfoAPI(Convert.ToString(para.salesorder), Convert.ToString(para.lot));
            return Json(res);
        }
    }
}