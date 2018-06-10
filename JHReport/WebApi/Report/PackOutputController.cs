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
    [RoutePrefix("api/PackOutput")]
    public class PackOutputController : ApiController
    {
        [Route("QueryInfo")]
        [HttpPost]
        public IHttpActionResult QueryInfo(dynamic para)
        {
            string sql = @"SELECT ast.[pallet_nbr]/*托盘编码*/
,ppk.container_nbr/*柜号*/
      ,ast.[serial_nbr] /*组件序列号*/
      ,ast.[workorder]/*工单号*/
      ,ast.[power_grade]/*功率档位*/
      ,ast.current_grade/*电流档位*/
       ,ast.[product_code]/*装配件号*/
       ,ast.[el_grade]/*EL等级*/
      ,ast.[final_grade]/*外观等级*/
     -- ,ast.[packing_date]/*封箱时间*/
      ,ppk.[pack_date]/*封箱时间*/
       ,cpp.[descriptions]/*功率组*/
       ,dps.[pallet_status_desc]/*是否入库*/
        ,dst.[descriptions] shift_type/*班次*/
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
     ,ast.[pack_seq]
     ,ppk.[check_nbr]
     ,ms.[descriptions] as cell_supplier/*电池片供应商*/
     ,'' /*电池片档位*/
      ,[cell_uop] /*电池片效率*/
      ,iv.[pmax]/(cast(REPLACE(cp.[cell_qty],'W','') as  float)*cast(REPLACE([cell_uop],'W','') as  float)) fff /*转换效率*/
      --,cp.[cell_qty]
      ,ab.[eva_supplier_code]/*EVA供应商*/
      ,cmeva.[eva_thickness] /*EVA规格*/
      ,ab.[eva_lot_nbr]/*EVA批号*/
       ,ab.[bks_supplier_code]/*背板供应商*/
       ,cmbks.[bks_thickness] /*bks规格*/
      ,ab.[bks_lot_nbr]/*背板批号*/
      ,ab.[glass_supplier_code]/*玻璃供应商*/
      ,cmglass.glass_thickness_desc /*玻璃规格*/
      ,ab.[glass_lot_nbr]/*电池批号*/
      ,ab.[jbox_supplier_code]/*接线盒供应商*/
      ,cmjbox.jbox_model_desc/*型材规格*/
      ,ab.[jbox_lot_nbr]/*接线盒批号*/
      ,ab.[frame_supplier_code]/*型材供应商*/
      ,cmframe.frame_thickness_desc/*型材规格*/
      ,ab.[frame_lot_nbr]/*型材批号*/
      ,wm.[customer]
      ,ppk.[product_code_original]
  FROM [mes_main].[dbo].[assembly_status] ast
  left join [mes_level2_iface].[dbo].[iv] iv
  on ast.[serial_nbr]=iv.[serial_nbr]
  left join [mes_main].[dbo].[pack_pallets] ppk
  on ast.[pallet_nbr]=ppk.[pallet_nbr]
  left join [mes_main].[dbo].[df_pallet_status] dps
  on ppk.[pallet_status]=dps.[pallet_status]
  left join [mes_main].[dbo].[df_shift_type] dst
  on ppk.[shift_type]=dst.[shift_type]
  left join [mes_main].[dbo].[config_power_group] cpp
  on ppk.[power_grade_group_id]=cpp.[power_grade_group_id]
  left join [mes_main].[dbo].[assembly_basis] ab
  on ast.[serial_nbr]=ab.[serial_nbr]
  left join [mes_main].[dbo].[config_mat_cell] cmc
  on  ab.[cell_part_nbr]=cmc.[part_nbr]
  left join [mes_level4_iface].[dbo].[master_supplier] ms
  on ab.[cell_supplier_code]=ms.[supplier_code]
  left join [mes_main].[dbo].[config_products] cp
  on ab.[product_code]=cp.[product_code]
  left join  [mes_main].[dbo].[wo_mfg] wm
  on ast.[workorder]=wm.[workorder]
  left join [mes_main].[dbo].[config_mat_eva] cmeva
  on ab.[eva_part_nbr]=cmeva.[part_nbr]
  left join [mes_main].[dbo].[config_mat_bks] cmbks
  on ab.[cell_part_nbr]=cmbks.[part_nbr]
  left join [mes_main].[dbo].[config_mat_glass] cmglass
  on ab.glass_part_nbr=cmglass.part_nbr
  left join [mes_main].[dbo].[config_mat_frame] cmframe
  on ab.frame_part_nbr=cmframe.part_nbr
  left join [mes_main].[dbo].[config_mat_jbox] cmjbox
  on ab.jbox_part_nbr=cmjbox.part_nbr
where ppk.[pack_date]>='" + Convert.ToString(para.begintime) + "' and ppk.[pack_date]<='" + Convert.ToString(para.endtime) + "' ";
            //柜号
            sql += String.IsNullOrEmpty(para.containerno.ToString()) ? "" : " and ppk.[container_nbr] LIKE '" + Convert.ToString(para.containerno) + "%'";
            //托盘号
            sql += String.IsNullOrEmpty(para.palletno.ToString()) ? "" : " and ast.[pallet_nbr] LIKE'" + Convert.ToString(para.palletno) + "%' ";
            //组件序列号
            sql += String.IsNullOrEmpty(para.lotno.ToString()) ? "" : " AND ast.[serial_nbr] = '" + Convert.ToString(para.lotno) + "'";
            //报检单号
            sql += String.IsNullOrEmpty(para.checkno.ToString()) ? "" : " AND ppk.[check_nbr]='" + Convert.ToString(para.checkno) + "'";

            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(sql);
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