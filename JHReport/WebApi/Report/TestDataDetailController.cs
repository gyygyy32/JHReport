using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;

namespace JHReport.WebApi.Report
{
    [RoutePrefix("api/TestDataDetail")]
    public class TestDataDetailController : ApiController
    {
        [Route("QueryData")]
        [HttpPost]
        public IHttpActionResult QueryData(dynamic para)
        {
            string sql = @"select 
      iv.[wks_visit_date]/*测试时间*/
      ,iv.[serial_nbr] /*组件序列号*/
      ,iv.[wks_id]/*机台号*/
      ,ab.[workorder] as workorder/*工单号*/
      ,cmc.cell_uop/*电池片功率*/
      ,cmc.cell_eff/*电池片效率*/
      ,iv.[pmax]
      ,iv.[voc]
      ,iv.[isc]
      ,iv.[ff]
      ,iv.[vpm]
      ,iv.[ipm]
      ,iv.[rs]
      ,iv.[rsh]
      ,iv.[eff]
      ,iv.[env_temp]
      ,iv.[surf_temp]
      ,iv.[temp]
      ,iv.[ivfile_path] 
      ,ab.[product_code]/*组件类型*/
      ,ab.[cell_part_nbr]/*电池料号*/
      ,ab.[cell_lot_nbr]/*电池批号*/
      ,ab.[cell_supplier_code]/*电池供应商*/
      ,ab.[glass_part_nbr]/*玻璃料号*/
      ,ab.[glass_supplier_code]/*玻璃供应商*/
      ,ab.[glass_lot_nbr]/*电池批号*/
      ,ab.[eva_part_nbr]/*EVA料号*/
      ,ab.[eva_supplier_code]/*EVA供应商*/
      ,ab.[eva_lot_nbr]/*EVA批号*/
      ,ab.[bks_part_nbr]/*背板料号*/
      ,ab.[bks_supplier_code]/*背板供应商*/
      ,ab.[bks_lot_nbr]/*背板批号*/
      ,ab.[frame_part_nbr]/*型材料号*/
      ,ab.[frame_supplier_code]/*型材供应商*/
      ,ab.[frame_lot_nbr]/*型材批号*/
      ,ab.[jbox_part_nbr]/*接线盒料号*/
      ,ab.[jbox_supplier_code]/*接线盒供应商*/
      ,ab.[jbox_lot_nbr]/*接线盒批号*/
      ,ab.[huiliu_part_nbr]
      ,ab.[huiliu_supplier_code]
      ,ab.[huiliu_lot_nbr]
      ,ab.[hulian_part_nbr]
      ,ab.[hulian_supplier_code]
      ,ab.[hulian_lot_nbr]
      ,sm.el_grade

	  from mes_level2_iface.dbo.iv iv 
	  inner join  [mes_main].[dbo].[assembly_status] sm on iv.serial_nbr = sm.serial_nbr
	  inner join wo_mfg wm on sm.workorder = wm.workorder
	  left join [mes_main].[dbo].[assembly_basis] ab on ab.serial_nbr=sm.serial_nbr
	  left join [mes_main].[dbo].[config_mat_cell]cmc on ab.cell_part_nbr=cmc.[part_nbr]
	  where iv.wks_visit_date>=@begintime
      and  iv.wks_visit_date<=@endtime ";
            sql += String.IsNullOrEmpty(para.workshop.ToString() ) ? "" : " and wm.area_code LIKE '" + Convert.ToString(para.workshop) + "%'";
            sql += String.IsNullOrEmpty(para.workorder.ToString()) ? "" : " AND wm.workorder = '" + Convert.ToString(para.workorder) + "'";
            sql += String.IsNullOrEmpty(para.serialno.ToString()) ? "" : " AND iv.serial_nbr = '" + Convert.ToString(para.serialno) + "'";
            
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(sql, new { begintime = Convert.ToString(para.begintime),endtime=Convert.ToString( para.endtime) });
            }
            return Json<dynamic>(res);
            //return Json<dynamic>(new { AA = "aa", BB = "cc" });
        }
    }
}
