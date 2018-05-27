using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Mvc;
using Dapper;
namespace JHReport.WebApi.Report
{
    [RoutePrefix("api/QC")]
    public class QCController : ApiController
    {
        [Route("QueryQCInfo")]
        [HttpPost]
        public IHttpActionResult QueryQCInfo(dynamic para)
        {
            string sql = @"SELECT qcv.[aiid]--xuhao
,qcv.[serial_nbr] --组件序列号
      ,qcv.[schedule_nbr]--工单
      ,dfss.descriptions as serial_status--扣留状态
      ,def.[defect_position] --不良位子
      ,def.[remark]--不良描述
      ,def.[according_std] --不良标准
      ,cws.wks_desc  --不良站点
      , case when qcv.visit_type in ('RL','S') then ast.exterior_grade else '' end exterior_grade
      ,qcv.[create_date]
  FROM [mes_main].[dbo].[qc_visit] qcv
  left join [mes_main].[dbo].[df_serial_status] dfss
  on qcv.serial_status=dfss.serial_status
  left join [mes_main].[dbo].[defects] def
  on qcv.[serial_nbr]=def.[serial_nbr]
  left join [mes_main].[dbo].[config_workstation] cws
  on def.[wks_id]=cws.[wks_id]
    left join [mes_main].[dbo].[wo_mfg] wo
  on qcv.[schedule_nbr]=wo.[workorder]
  left join [mes_main].[dbo].[assembly_status] ast
  on qcv.[serial_nbr] =ast.[serial_nbr] 
where qcv.create_date>='" + Convert.ToString( para.begintime)+ "' and qcv.create_date<='"+ Convert.ToString(para.endtime)+"' ";
            sql += String.IsNullOrEmpty(para.serialno.ToString()) ? "" : " and qcv.[serial_nbr] LIKE '" + Convert.ToString(para.serialno) + "%'";
            sql += String.IsNullOrEmpty(para.workshop.ToString()) ? "" : " AND wo.[area_code] = '" + Convert.ToString(para.workshop) + "' ";
            sql += String.IsNullOrEmpty(para.status.ToString()) ? "" : " AND qcv.[visit_type] = '" + Convert.ToString(para.status) + "'";

            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(sql, new { lotid = Convert.ToString(para.lotid) });
            }
            return Json<dynamic>(res);
            //return Json<dynamic>(new { AA = "aa", BB = "cc" });
        }

        [Route("QueryStatus")]
        [HttpPost]
        public IHttpActionResult QueryStatus()
        {
            string sql = @"select * from [mes_main].[dbo].[df_wks_visit_type] vt
where vt.visit_type in ('M','H','RL','S')";
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(sql);
            }
            return Json<dynamic>(res);


        }
        //[Route("QueryStatus")]
        //[HttpPost]
        //public FileResult tt()
        //{
        //    return FileResult();
        //}
    }
}