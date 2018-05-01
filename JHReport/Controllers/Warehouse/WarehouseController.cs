using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JHReport.Controllers.Warehouse
{
    [RoutePrefix("Warehouse")]
    public class WarehouseController : Controller
    {
        [Route("StickIn")]
        public ActionResult StockIn()
        {
            return View();
        }
    }
}