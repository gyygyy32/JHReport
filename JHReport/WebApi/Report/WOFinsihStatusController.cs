using JHReport.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JHReport.WebApi.Report
{
    //工单达成情况
    [RoutePrefix("api/WOFinishStatus")]
    public class WOFinsihStatusController : ApiController
    {
        [Route("QueryInfo")]
        [HttpPost]
        public IHttpActionResult QueryInfo(dynamic para)
        {
            IEnumerable<dynamic> res = new ReportService().WOFinishStatusInfo(Convert.ToString(para.wo)
                                                            , Convert.ToString(para.sales)
                                                            , Convert.ToString(para.customer)
                                                            ,Convert.ToString(para.bt)
                                                            ,Convert.ToString(para.et));
            if (res.Count() > 0)
            {
                foreach (var item in res)
                {
                    string a = "s"; a.Split('|');
                    string[] grade = item.gradeqty.Split('|');
                    item.A = grade[0];
                    item.B = grade[1];
                    item.C = grade[2];
                    string[] power = item.testpower.Split('|');
                    item.max = power[0];
                    item.min = power[1];
                    item.avg = power[2];
                }
            }
            return Json(res);
        }
    }
}
