using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;

namespace JHReport.WebApi.Warehouse
{
    [RoutePrefix("api/StockIn")]
    public class StockInController : ApiController
    {
        public IHttpActionResult QueryCondition(string PalletID, dynamic ConditionSetting)
        {
            //查询托盘信息带出符合条件的库位
            string sql = "select * from mes_main.dbo.pack_pallets where [pallet_nbr]=@PalletID;";
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
               var res = conn.Query(sql, new { PalletID=PalletID }).Single();
            }
            return Json("");
        }
        [Route("QueryStock")]
        [HttpPost]
        public IHttpActionResult QueryStock()
        {
            string sql = " select * from warehouse.dbo.tstock;";
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                var res = conn.Query(sql);
                return Json<dynamic>(res);
            }
        }
        [Route("QueryStorageLocation")]
        [HttpPost]
        public IHttpActionResult QueryStorageLocation(string stock)
        {
            string sql = "select * from warehouse.dbo.tstoragelocation where stockcodeid = @stock;";
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                var res = conn.Query(sql);
                return Json<dynamic>(res);
            }
        }


    }
}
