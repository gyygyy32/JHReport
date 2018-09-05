using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using JHReport.BLL;

namespace JHReport.WebApi.Report
{
    [RoutePrefix("api/RunCard")]
    public class RunCardController : ApiController
    {

        [Route("QueryLotID")]
        [HttpPost]
        public IHttpActionResult QueryLotID(dynamic lotid)
        {
            string sql = @"select ast.[serial_nbr]/*组件编号*/
      ,ast.[pallet_nbr]/*托盘号*/
      ,ast.[workorder]/*工单*/
      ,ast.[product_code]/*产品类型*/
      ,ast.[schedule_nbr]
      ,ast.[serial_status]
      ,ast.[exterior_grade]
      ,ast.[el_grade]
      ,ast.[final_grade]
      ,ast.[process_idx]
      ,ast.[process_code]/*站点编号*/
      ,proce.descriptions as process_name/*站点名称*/
      ,ast.[create_date]/*投产日期--add by xue lei on 2018-7-31*/
      ,ast.[ny_result]/*耐压数据--add by xue lei on 2018-7-31*/ 
      , ( select top 1 create_time   FROM [mes_level2_iface].[dbo].[YD9860_Data] where barcode = ast.serial_nbr order by create_time desc) nytime
      ,ast.power_grade /*功率挡位--add by xue lei on 2018-7-31*/
      ,ast.current_grade /*电流挡位--add by xue lei on 2018-7-31*/
        ,ast.exterior_grade /*组件等级--add by xue lei on 2018-7-31*/
      --,ast.[last_update]
     -- ,ast.[pack_date]
      ,ast.[power_grade]
      ,ast.[current_grade]
     -- ,ast.[packrule_string]
      ,ast.[pack_seq]
      ,mfg.[cell_supplier_desc]
      ,mfg.area_code /*车间编号*/
      ,da.descriptions as workshop_name /*车间*/
      ,mfg.[workorder_type] /*工单类型编码*/
      ,mfg.[mod_color] /*组件颜色--add by xue lei on 2018-7-31*/
      ,mfg.sale_order /*订单号--add by xue lei on 2018-7-31*/ 
      ,wot.descriptions as workorder_type_name/*工单类型*/
      ,cus.[cust_desc]/*客户代码*/
from [mes_main].[dbo].[assembly_status] ast 
left join wo_mfg mfg
on ast.workorder=mfg.workorder
left join df_wo_status wo
on  mfg.wo_status=wo.wo_status
left join [mes_level4_iface].[dbo].[customers] cus
on mfg.[customer]=cus.[cust_id]
left join [mes_main].[dbo].[df_processes] proce
on ast.[process_code]=proce.[process_code]
left join [mes_main].[dbo].[df_wo_type] wot
on mfg.[workorder_type]=wot.[workorder_type]
left join df_areas da
on mfg.area_code=da.area_code
and da.active_flag='1'
where 
ast.serial_nbr=@lotid;";
            IEnumerable<dynamic> res =null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(sql, new { lotid = Convert.ToString(lotid.lotid) });
            }
            return Json<dynamic>(res);
            //return Json<dynamic>(new { AA = "aa", BB = "cc" });
        }
        [Route("WeldStationInfo")]
        [HttpGet]
        public IHttpActionResult WeldStationInfo(string lotid)
        {
            string sql = @"select tcl.serial_nbr /*组件序列号*/
,twv.schedule_nbr/*工单号*/
,tcl.wks_visit_date/*过站时间*/
,dpt.[descriptions] part_type/*材料类型*/
,tcl.part_nbr/*材料料号*/
,ms.descriptions supplier_code/*材料供应商*/
,tcl.lot_nbr/*材料批次号*/
,DU.nickname operator/*材料供应商*/
,twv.wks_id/*机台号*/
,cw.process_code /*站点编号*/
,dp.descriptions/*站点名称*/
,dst.descriptions shift_type/*班次*/
,a.cell_uop /*单片功率*/
 from trace_componnet_lot tcl/*记录材料*/
,trace_workstation_visit twv /*记录机台*/
,assembly_status ab
,wo_mfg wm
,[mes_level4_iface].[dbo].[df_part_type] dpt
,[mes_level4_iface].[dbo].[master_supplier] ms
,[mes_main].[dbo].[df_shift_type] dst
,[mes_auth].[dbo].[df_user] du
,[mes_main].[dbo].[config_workstation] cw
,[mes_main].[dbo].[df_processes] dp
,mes_main.dbo.config_mat_cell a
where twv.serial_nbr=tcl.serial_nbr
and tcl.wks_id=twv.wks_id
--and twv.serial_nbr='JYP171202X60000019'
and twv.serial_nbr=ab.serial_nbr
and ab.workorder=wm.workorder
and tcl.part_type=dpt.part_type
and tcl.supplier_code=ms.supplier_code
and twv.shift_type =dst.shift_type
and twv.operator=du.username
and twv.wks_id=cw.wks_id
and cw.process_code=dp.process_code 
and tcl.serial_nbr=@lotid
and dp.descriptions='焊接'
and tcl.part_nbr = a.part_nbr ;";
            object res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(sql, new { lotid = lotid });
            }
            return Json(res);//
        }

        //装框接线盒
        [Route("FrameBox")]
        [HttpGet]
        public IHttpActionResult FrameBox(string lotid)
        {
           IEnumerable<dynamic> res= new ReportService().FrameBoxQueryInfo(lotid);
            return Json(res);
        }
        //叠层EVA
        [Route("LaminationEVA")]
        [HttpGet]
        public IHttpActionResult LaminationEVA(string lotid)
        {
            IEnumerable<dynamic> res = new ReportService().LaminationEVAQueryInfo(lotid);
            return Json(res);
        }
        //叠层高透EVA
        [Route("LaminationHighEVA")]
        [HttpGet]
        public IHttpActionResult LaminationHighEVA(string lotid)
        {
            IEnumerable<dynamic> res = new ReportService().LaminationHighEVAQueryInfo(lotid);
            return Json(res);
        }

        //叠层玻璃
        [Route("LaminationGlass")]
        [HttpGet]
        public IHttpActionResult LaminationGlass(string lotid)
        {
            IEnumerable<dynamic> res = new ReportService().LaminationGlassQueryInfo(lotid);
            return Json(res);
        }

        //叠层背板
        [Route("LaminationBack")]
        [HttpGet]
        public IHttpActionResult LaminationBack(string lotid)
        {
            IEnumerable<dynamic> res = new ReportService().LaminationBackQueryInfo(lotid);
            return Json(res);
        }

        //IV
        [Route("IV")]
        [HttpGet]
        public IHttpActionResult IV(string lotid)
        {
            IEnumerable<dynamic> res = new ReportService().IVQueryInfo(lotid);
            return Json(res);
        }

        //包装
        [Route("Pack")]
        [HttpGet]
        public IHttpActionResult Pack(string lotid)
        {
            IEnumerable<dynamic> res = new ReportService().PackQueryInfo(lotid);
            return Json(res);
        }

        //测试后EL
        [Route("ELAfterTest")]
        [HttpGet]
        public IHttpActionResult ELAfterTest(string lotid)
        {
            IEnumerable<dynamic> res = new ReportService().ELAfterTestQueryInfo(lotid);
            return Json(res);
        }

        //层压前EL
        [Route("ELBeforeLayup")]
        [HttpGet]
        public IHttpActionResult ELBeforeLayup(string lotid)
        {
            IEnumerable<dynamic> res = new ReportService().ELBeforeLayupQueryInfo(lotid);
            return Json(res);
        }
        //功率后EL
        [Route("ELAfterIV")]
        [HttpGet]
        public IHttpActionResult ELAfterIV(string lotid)
        {
            IEnumerable<dynamic> res = new ReportService().ELAfterIVInfo(lotid);
            return Json(res);
        }
        //清洗
        [Route("Clean")]
        [HttpGet]
        public IHttpActionResult Clean(string lotid)
        {
            IEnumerable<dynamic> res = new ReportService().CleanQueryInfo(lotid);
            List<dynamic> list = res.ToList<dynamic>();
           
            if (list.Count > 0)
            {
                list[0].curingtime = Math.Round(Convert.ToDouble(list[0].curingtime) / 60, 2, MidpointRounding.AwayFromZero);
                if (list[0].curingtime != null) list[0].curingtime = list[0].curingtime + "H";
                list[0].testitem = "dsdsa";
                return Json(res);
            }
            //Math.Round((list[0].curingtime) / 60, 2,MidpointRounding.AwayFromZero)
            return Json(res);
            
        }

        //层压后检验
        [Route("QCAfterLayup")]
        [HttpGet]
        public IHttpActionResult QCAfterLayup(string lotid)
        {
            IEnumerable<dynamic> res = new ReportService().QCAfterLayupInfo(lotid);
            return Json(res);
        }

        

    }
}
