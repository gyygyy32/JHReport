using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;

namespace JHReport.WebApi.Warehouse
{
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
            return Json(" ");
        }
    }
}