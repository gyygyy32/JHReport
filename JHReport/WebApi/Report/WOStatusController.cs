using JHReport.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JHReport.WebApi.Report
{
    [RoutePrefix("api/WOStatus")]
    public class WOStatusController : ApiController
    {
        [Route("WOStatusInfo")]
        [HttpPost]
        public IHttpActionResult QueryStatus(dynamic para)
        {
           
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = new ReportService().WOStatusInfo(Convert.ToString(para.wo),Convert.ToString(para.sales));
            }
            return Json<dynamic>(res);


        }
    }
}