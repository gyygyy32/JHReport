using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JHReport.Controllers.Measurement
{
    [RoutePrefix("Measurement")]
    public class MeasurementController : Controller
    {
        [Route("{action=Measure}")]
        // GET: Measure
        public ActionResult Measure()
        {
            return View();
        }
    }
}