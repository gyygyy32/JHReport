using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;


namespace JHReport.DAL
{
    public class ReportDal
    {
        static DbUtility dbhelp = new DbUtility(System.Configuration.ConfigurationManager.ConnectionStrings["mesConn"].ToString(), DbProviderType.SqlServer);

        #region 晶科报表

        public DataTable JKQueryInfo(string salesorder, string lot)
        {
            
            return dbhelp.ExecuteDataTable(JKSql(salesorder,lot),null);
        }

        public IEnumerable<dynamic> JKQueryInfoAPI(string salesorder, string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(JKSql(salesorder,lot));
            }
            return res;
        }

        private string JKSql(string salesorder, string lot)
        {
            string sql = @"SELECT
	convert(varchar(100), iv.[wks_visit_date],120) AS TTIME,
	ast.[serial_nbr] AS LOT_NUM,
	wm.[sale_order] AS WORK_ORDER_NO,
	iv.[wks_id] AS DEVICENUM,
	iv.[surf_temp] AS AMBIENTTEMP,
	iv.[Sun_Ref]  AS INTENSITY, -- iv.[SunRef]
	left(cast(iv.FF as DECIMAL(18,6) ) , charindex('.',cast(iv.ff as DECIMAL(18,6) )) + 2) AS FF,
	left(cast(iv.eff as DECIMAL(18,6) ) , charindex('.',cast(iv.eff as DECIMAL(18,6) )) + 2) AS EFF,
	left(cast(iv.pmax as DECIMAL(18,6) ) , charindex('.',cast(iv.pmax as DECIMAL(18,6) )) + 2) AS PM,
	left(cast(iv.isc as DECIMAL(18,6) ) , charindex('.',cast(iv.isc as DECIMAL(18,6) )) + 2) AS ISC,
	left(cast(iv.ipm as DECIMAL(18,6) ) , charindex('.',cast(iv.ipm as DECIMAL(18,6) )) + 2) AS IPM,
	left(cast(iv.voc as DECIMAL(18,6) ) , charindex('.',cast(iv.voc as DECIMAL(18,6) )) + 2) AS VOC,
	left(cast(iv.vpm as DECIMAL(18,6) ) , charindex('.',cast(iv.vpm as DECIMAL(18,6) )) + 2) AS VPM,
	iv.[surf_temp] AS SENSORTEMP,
	left(cast(iv.rs as DECIMAL(18,6) ) , charindex('.',cast(iv.rs as DECIMAL(18,6) )) + 2) AS RS,
	left(cast(iv.rsh as DECIMAL(18,6) ) , charindex('.',cast(iv.rsh as DECIMAL(18,6) )) + 2) AS RSH,
	iv.[temp] AS TEMP,
	iv.[ivfile_path],
	ms.[descriptions] AS SUPPLIER_NAME,
	[cell_uop] AS 电池单片功率
	,
	iv.[pmax] / (
	CAST ( REPLACE( cp.[cell_qty], 'W', '' ) AS FLOAT ) * CAST ( REPLACE( [cell_uop], 'W', '' ) AS FLOAT )) AS DEC_CTM --CTM=测试功率/（单片功率X电池片数量）
	,
	'' AS VC_CELLEFF,
	'' AS RESULT,
	'' AS TYPE,
	'' AS IV_TEST_KEY,
	'' AS PALLET_NO,
	'' AS PRO_LEVEL,
	'' AS CIR_NO,
	'' AS SALE_ORDER_NO 
FROM
	[mes_main].[dbo].[assembly_status] ast
	LEFT JOIN [mes_level2_iface].[dbo].[iv] iv ON ast.[serial_nbr] = iv.[serial_nbr]
	LEFT JOIN [mes_main].[dbo].[pack_pallets] ppk ON ast.[pallet_nbr] = ppk.[pallet_nbr]
	LEFT JOIN [mes_main].[dbo].[df_pallet_status] dps ON ppk.[pallet_status] = dps.[pallet_status]
	LEFT JOIN [mes_main].[dbo].[df_shift_type] dst ON ppk.[shift_type] = dst.[shift_type]
	LEFT JOIN [mes_main].[dbo].[config_power_group] cpp ON ppk.[power_grade_group_id] = cpp.[power_grade_group_id]
	LEFT JOIN [mes_main].[dbo].[assembly_basis] ab ON ast.[serial_nbr] = ab.[serial_nbr]
	LEFT JOIN [mes_main].[dbo].[config_mat_cell] cmc ON ab.[cell_part_nbr] = cmc.[part_nbr]
	LEFT JOIN [mes_level4_iface].[dbo].[master_supplier] ms ON ab.[cell_supplier_code] = ms.[supplier_code]
	LEFT JOIN [mes_main].[dbo].[config_products] cp ON ab.[product_code] = cp.[product_code]
	LEFT JOIN [mes_main].[dbo].[wo_mfg] wm ON ast.[workorder] = wm.[workorder]
	LEFT JOIN [mes_main].[dbo].[config_mat_eva] cmeva ON ab.[eva_part_nbr] = cmeva.[part_nbr]
	LEFT JOIN [mes_main].[dbo].[config_mat_bks] cmbks ON ab.[cell_part_nbr] = cmbks.[part_nbr]
	LEFT JOIN [mes_main].[dbo].[config_mat_glass] cmglass ON ab.glass_part_nbr= cmglass.part_nbr
	LEFT JOIN [mes_main].[dbo].[config_mat_frame] cmframe ON ab.frame_part_nbr= cmframe.part_nbr
	LEFT JOIN [mes_main].[dbo].[config_mat_jbox] cmjbox ON ab.jbox_part_nbr= cmjbox.part_nbr 
WHERE 1=1 ";
            sql += String.IsNullOrEmpty(lot) ? "" : " and iv.[serial_nbr] = '" + lot + "'";
            sql += String.IsNullOrEmpty(salesorder) ? "" : " and wm.[sale_order] = '" + salesorder + "'";
            return sql;
        }
        #endregion

        #region 测试数据明细
        public DataTable TestDataDetailQueryInfo(string lot, string wo, string bt, string et, string workshop)
        {
            return dbhelp.ExecuteDataTable(TestDataDetailSql(lot, wo, bt, et, workshop), null);
        }

        private string TestDataDetailSql(string lot, string wo, string bt, string et, string workshop)
        {
            string sql = @"select 
      iv.[wks_visit_date] as 测试时间/*测试时间*/ 
      ,iv.[serial_nbr] as 组件序列号/*组件序列号*/  
      ,iv.[wks_id] as 机台号/*机台号*/ 
      ,ab.[workorder] as 工单号/*工单号*/  
      ,cmc.cell_uop as 电池片功率/*电池片功率*/
      ,cmc.cell_eff as 电池片效率/*电池片效率*/
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
      ,ab.[product_code] as 组件类型/*组件类型*/
      ,ab.[cell_part_nbr] as 电池料号/*电池料号*/
      ,ab.[cell_lot_nbr]as 电池批号/*电池批号*/
      ,ab.[cell_supplier_code] as 电池供应商/*电池供应商*/
      ,ab.[glass_part_nbr] as 玻璃料号/*玻璃料号*/
      ,ab.[glass_supplier_code] as 玻璃供应商/*玻璃供应商*/
      ,ab.[glass_lot_nbr] as 玻璃批号/*电池批号*/
      ,ab.[eva_part_nbr] as EVA料号/*EVA料号*/
      ,ab.[eva_supplier_code] as EVA供应商/*EVA供应商*/
      ,ab.[eva_lot_nbr] as EVA批号/*EVA批号*/
      ,ab.[bks_part_nbr] as 背板料号/*背板料号*/
      ,ab.[bks_supplier_code] as 背板供应商/*背板供应商*/
      ,ab.[bks_lot_nbr] as 背板批号/*背板批号*/
      ,ab.[frame_part_nbr] as 型材料号/*型材料号*/
      ,ab.[frame_supplier_code] as 型材供应商/*型材供应商*/
      ,ab.[frame_lot_nbr] as 型材批号/*型材批号*/
      ,ab.[jbox_part_nbr] as 接线盒料号/*接线盒料号*/
      ,ab.[jbox_supplier_code] as 接线盒供应商/*接线盒供应商*/
      ,ab.[jbox_lot_nbr] as 接线盒批号/*接线盒批号*/
      ,ab.[huiliu_part_nbr] as 汇流条料号
      ,ab.[huiliu_supplier_code] as 汇流条供应商
      ,ab.[huiliu_lot_nbr] as 汇流条批号

      ,sm.el_grade as EL等级

	  from mes_level2_iface.dbo.iv iv 
	  inner join  [mes_main].[dbo].[assembly_status] sm on iv.serial_nbr = sm.serial_nbr
	  inner join wo_mfg wm on sm.workorder = wm.workorder
	  left join [mes_main].[dbo].[assembly_basis] ab on ab.serial_nbr=sm.serial_nbr
	  left join [mes_main].[dbo].[config_mat_cell]cmc on ab.cell_part_nbr=cmc.[part_nbr]
	  where iv.wks_visit_date>='" + bt+"' and  iv.wks_visit_date<='"+et+"' ";
            sql += String.IsNullOrEmpty(workshop.ToString()) ? "" : " and wm.area_code LIKE '" + Convert.ToString(workshop) + "%'";
            sql += String.IsNullOrEmpty(wo.ToString()) ? "" : " AND wm.workorder = '" + Convert.ToString(wo) + "'";
            sql += String.IsNullOrEmpty(lot.ToString()) ? "" : " AND iv.serial_nbr = '" + Convert.ToString(lot) + "'";
            return sql;
        }
        #endregion


        #region 质量报表
        public DataTable QCQueryInfo(string bt, string et, string lot, string workshop, string status)
        {
            return dbhelp.ExecuteDataTable(QCSql(bt, et, lot, workshop, status),null);
        }

        private string QCSql(string bt,string et,string lot,string workshop,string status)
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
      , convert(varchar(100), qcv.[create_date],120) AS create_date
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
where qcv.create_date>='" + bt + "' and qcv.create_date<='" + et + "' ";
            sql += String.IsNullOrEmpty(lot) ? "" : " and qcv.[serial_nbr] LIKE '" + lot + "%'";
            sql += String.IsNullOrEmpty(workshop) ? "" : " AND wo.[area_code] = '" + workshop + "' ";
            sql += String.IsNullOrEmpty(status) ? "" : " AND qcv.[visit_type] = '" + status + "'";
            return sql;
        }


        #endregion

        #region 包装产量报表
        public DataTable PackOutputQueryInfo(string workshop, string bt, string et, string lot, string container, string pallet, string check)
        {
            return dbhelp.ExecuteDataTable(PackOutputSql(workshop, bt, et, lot, container, pallet, check), null);
        }

        private string PackOutputSql(string workshop,string bt,string et,string lot,string container,string pallet,string check)
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
      ,convert(varchar(100), ppk.[pack_date],120) AS pack_date/*封箱时间*/
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
where ppk.[pack_date]>='" + bt + "' and ppk.[pack_date]<='" + et + "' ";
            //柜号
            sql += String.IsNullOrEmpty(container) ? "" : " and ppk.[container_nbr] LIKE '" + container+ "%'";
            //托盘号
            sql += String.IsNullOrEmpty(pallet) ? "" : " and ast.[pallet_nbr] LIKE'" + pallet + "%' ";
            //组件序列号
            sql += String.IsNullOrEmpty(lot) ? "" : " AND ast.[serial_nbr] = '" + lot + "'";
            //报检单号
            sql += String.IsNullOrEmpty(check) ? "" : " AND ppk.[check_nbr]='" + check + "'";
            return sql;
        }
        #endregion

        #region 组件电子流转单
        //装框线盒查询
        private string FrameBoxSql(string lot)
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
,(select top 1 frame_spec_desc from [mes_main].[dbo].[config_mat_frame] where part_nbr = tcl.part_nbr ) spec
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
where twv.serial_nbr=tcl.serial_nbr
and tcl.wks_id=twv.wks_id
and twv.serial_nbr=ab.serial_nbr
and ab.workorder=wm.workorder
and tcl.part_type=dpt.part_type
and tcl.supplier_code=ms.supplier_code
and twv.shift_type =dst.shift_type
and twv.operator=du.username
and twv.wks_id=cw.wks_id
and cw.process_code=dp.process_code 
and tcl.serial_nbr='" + lot + "'" + " and dp.descriptions='装框' "; //and dpt.[descriptions]='线盒'
            return sql;

        }
        public IEnumerable<dynamic> FrameBoxQueryInfo(string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(FrameBoxSql(lot));
            }
            return res;
        }

        //叠层eva
        private string LaminationEVASql(string lot)
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
,a.eva_width
,a.eva_thickness
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
,[mes_main].[dbo].[config_mat_eva] a
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
and tcl.part_nbr = a.part_nbr
and tcl.serial_nbr='" + lot+"' and dp.descriptions = '叠层' and dpt.[descriptions]='EVA'";
            return sql;
        }
        public IEnumerable<dynamic> LaminationEVAQueryInfo(string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(LaminationEVASql(lot));
            }
            return res;
        }

        //叠层高透EVA
        private string LaminationHighEVASql(string lot)
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
,a.eva_width
,a.eva_thickness
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
,[mes_main].[dbo].[config_mat_eva] a
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
and tcl.part_nbr = a.part_nbr
and tcl.serial_nbr='" + lot+"' and dp.descriptions='叠层' and dpt.[descriptions]='高透EVA'";
            return sql;
        }
        public IEnumerable<dynamic> LaminationHighEVAQueryInfo(string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(LaminationHighEVASql(lot));
            }
            return res;
        }

        //叠层玻璃
        private string LaminationGlassSql(string lot)
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
,a.glass_width_desc
,a.glass_thickness_desc
,a.glass_length_desc
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
,[mes_main].[dbo].[config_mat_glass] a
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
and a.part_nbr = tcl.part_nbr
and tcl.serial_nbr='" + lot+"' and dp.descriptions='叠层' and dpt.[descriptions]='玻璃'";
            return sql;
        }
        public IEnumerable<dynamic> LaminationGlassQueryInfo(string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(LaminationGlassSql(lot));
            }
            return res;
        }
        //叠层背板
        private string LaminationBackSql(string lot)
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
,a.bks_width
,a.bks_thickness
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
,[mes_main].[dbo].[config_mat_bks] a
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
and tcl.part_nbr = a.part_nbr
and tcl.serial_nbr='" + lot+"' and dp.descriptions='叠层' and dpt.[descriptions]='背板'";
            return sql;
        }
        public IEnumerable<dynamic> LaminationBackQueryInfo(string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(LaminationBackSql(lot));
            }
            return res;
        }

        //IV
        private string IVSql(string lot)
        {
            string sql = @"SELECT [serial_nbr]
      ,[wks_id]
      ,[wks_visit_date]
      ,[pmax]
      ,[voc]
      ,[isc]
      ,[ff]
      ,[vpm]
      ,[ipm]
      ,[rs]
      ,[rsh]
      ,[eff]
      ,[env_temp]
      ,[surf_temp]
      ,[temp]
      ,[ivfile_path]
  FROM [mes_level2_iface].[dbo].[iv] iv
   where iv.serial_nbr='"+lot+"'";
            return sql;
        }
        public IEnumerable<dynamic> IVQueryInfo(string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(IVSql(lot));
            }
            return res;
        }

        //包装
        private string PackSql(string lot)
        {
            string sql = @"select AST.[serial_nbr]
      ,AST.[pallet_nbr]
      ,AST.[pack_date]
      ,PPT.wks_id
       From [mes_main].[dbo].[assembly_status] ast
       LEFT JOIN [mes_main].[dbo].[pack_pallets] PPT 
       ON AST.pallet_nbr=PPT.pallet_nbr
       WHERE AST.[serial_nbr]='"+lot+"' ";
            return sql;
        }
        public IEnumerable<dynamic> PackQueryInfo(string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(PackSql(lot));
            }
            return res;
        }

        //测试后EL
        private string ELAfterTestSql(string lot)
        {
            string sql = @"SELECT [serial_nbr]
      ,[wks_id]
      ,[wks_visit_date]
      ,[el_grade]
      ,[process_code]
      ,[el_path]
  FROM [mes_level2_iface].[dbo].[el] el
  where [process_code]='HEL'
  and el.serial_nbr='"+lot+"'";
            return sql;
        }

        public IEnumerable<dynamic> ELAfterTest(string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(ELAfterTestSql(lot));
            }
            return res;
        }

        //层压前EL
        //增加是否返修add by xue lei on 2018-7-31 
        private string ELBeforeLayupSql(string lot)
        {
            string sql = @"SELECT [serial_nbr]
      ,[wks_id]
      ,[wks_visit_date]
      ,[el_grade]
      ,[process_code]
      ,[el_path]
      ,case ( select count(*) from mes_main.dbo.qc_visit as a where a.visit_type='M' and wks_id = 'QC' and serial_nbr  =el.serial_nbr ) 
	   when 0 then 'N'
	   else 'Y' end
	   isRepair
  FROM [mes_level2_iface].[dbo].[el] el
  where el.[process_code]='QEL'
  and el.serial_nbr='" + lot+"'";
            return sql;
        }

        public IEnumerable<dynamic> ELBeforeLayup(string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(ELBeforeLayupSql(lot));
            }
            return res;
        }

        //功率后EL
        private string ELAfterIVSql(string lot)
        {
            string sql = @"SELECT [serial_nbr]
      ,[wks_id]
      ,[wks_visit_date]
      ,[el_grade]
      ,[process_code]
      ,[el_path]
  FROM [mes_level2_iface].[dbo].[el] el
  where el.[process_code]='HEL'
  and el.serial_nbr='" + lot + "'";
            return sql;
        }

        public IEnumerable<dynamic> ELAfterIV(string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(ELAfterIVSql(lot));
            }
            return res;
        }


        //清洗
        private string CleanSql(string lot)
        {
            string sql = @"select
datediff(mi,
(select top 1 a.wks_visit_date from trace_workstation_visit a 
inner join [mes_main].[dbo].[config_workstation] b on a.wks_id = b.wks_id
where b.process_code = 'M45' and a.serial_nbr = ab.serial_nbr order by a.wks_visit_date desc),twv.wks_visit_date) curingtime
,twv.wks_visit_date
,twv.wks_id/*机台号*/
,cw.process_code /*站点编号*/
,dp.descriptions/*站点名称*/
,dst.descriptions shift_type/*班次*/
,du.nickname
 from 
trace_workstation_visit twv /*记录机台*/
,assembly_status ab
,[mes_main].[dbo].[df_shift_type] dst
,[mes_auth].[dbo].[df_user] du
,[mes_main].[dbo].[config_workstation] cw
,[mes_main].[dbo].[df_processes] dp
where twv.serial_nbr=ab.serial_nbr
--and twv.serial_nbr='JYP171202X60000019'
and twv.serial_nbr=ab.serial_nbr
and twv.shift_type =dst.shift_type
and twv.operator=du.username
and twv.wks_id=cw.wks_id
and cw.process_code=dp.process_code 
and ab.serial_nbr='" + lot + "' and dp.descriptions='清洗' order by twv.wks_visit_date desc";
            return sql;
        }

        public IEnumerable<dynamic> CleanQueryInfo(string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(CleanSql(lot));
            }
            return res;
        }

        //层压后检验
        private string QCAfterLayupSql(string lot)
        {
            string sql = @" select * ,(select nickname from [mes_auth].[dbo].[df_user] where username = a.operator) as username,
 (select count(*) from mes_main.dbo.qc_visit  where qc_visit.visit_type = 'H' and wks_id like '%CYHJ%' and serial_nbr = a.serial_nbr) as isHold
  from[mes_main].[dbo].[trace_workstation_visit] as a  where serial_nbr = '" + lot+"' and wks_id like '%CYHJ%' order by wks_visit_date desc; ";
            return sql;
        }
        public IEnumerable<dynamic> QCAfterLayup(string lot)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(QCAfterLayupSql(lot));
            }
            return res;
        }

        #endregion

        #region 工单达成情况

        private string WOFinishStatusSql(string wo, string sales, string customer, string bt, string et)
        {
            string sql = @"exec mes_main.dbo.prWOFinishStatus ";
            sql += string.IsNullOrEmpty(wo) ? " @wo=N''" : " @wo =N'" + wo + "'";
            sql += string.IsNullOrEmpty(sales) ? " ,@sales=N''" : " ,@sales=N'" + sales + "'";
            sql += string.IsNullOrEmpty(customer) ? " ,@customer=N''" : " ,@customer=N'" + customer + "'";
            if (!string.IsNullOrEmpty(bt) && !string.IsNullOrEmpty(et))
            {
                sql += " ,@bt=N'" + bt + "',@et=N'" + et + "'";
            }
            else
            {
                sql += " ,@bt=N'',@et=N''";
            }
            return sql;
        }

        private string WOFinishStatusSql1(string wo,string sales,string customer,string bt,string et)
        {
            string sql = @"select a.create_date
,( select top 1 create_date from mes_main.dbo.assembly_status where workorder = a.workorder order by create_date ) firstlottime
,a.customer
,a.sale_order
,a.workorder
,( select top 1 serial_nbr from mes_main.dbo.assembly_status where workorder = a.workorder order by create_date ) firstlot
,a.product_code
,a.plan_qty
,(select count( a1.serial_nbr) from mes_main.dbo.[trace_workstation_visit]  a1 
inner join mes_main.dbo.config_workstation b on a1.wks_id = b.wks_id  
inner join mes_main.dbo.assembly_status c on a1.serial_nbr = c.serial_nbr
where b.process_code = 'M15' and c.workorder = a.workorder) laminationqty
,a.cell_supplier_desc
,(select top 1 c.cell_uop from mes_main.dbo.trace_componnet_lot a2 
inner join mes_main.dbo.config_workstation b on a2.wks_id = b.wks_id 
inner join [mes_main].[dbo].[config_mat_cell] c on a2.part_nbr = c.part_nbr
inner join mes_main.dbo.assembly_status d on d.serial_nbr = a2.serial_nbr
where b.process_code = 'M10' and d.workorder = a.workorder ) cellgrade
,(select isnull(max( case report_grade when 'A' then ''+ CAST( gradeqty as nvarchar) end),'A:0')  
	   +'|'+isnull(max(case report_grade when 'B' then ''+ CAST( gradeqty as nvarchar) end) ,'B:0')
	   +'|'+isnull(max(case report_grade when 'C' then '' + CAST( gradeqty as nvarchar) end) ,'C:0')	       
from	   	
(
select a1.report_grade, count( a1.report_grade) gradeqty from mes_main.dbo.config_report_grade a1
inner join mes_main.dbo.assembly_status b on a1.exterior_grade = b.final_grade where b.serial_status = 'G' and b.workorder = a.workorder and a1.customer = a.customer
group by a1.report_grade
) t) gradeqty
,(select count(serial_nbr) from mes_main.dbo.assembly_status where serial_status ='S' and workorder = a.workorder) scrapqty 
,(select cast(max(a3.pmax)as nvarchar)+'|'+cast(min(a3.pmax)as nvarchar)+'|'+ cast( avg(a3.pmax) as nvarchar) from mes_level2_iface.dbo.iv a3 inner join mes_main.dbo.assembly_status b on a3.serial_nbr = b.serial_nbr
  where b.workorder = a.workorder) testpower
from mes_main.dbo.wo_mfg a where 1=1 ";
            sql += string.IsNullOrEmpty(wo) ? "" : " and a.workorder = '"+wo+"'";
            sql += string.IsNullOrEmpty(sales) ? "" : " and a.sale_order = '" + sales + "'";
            sql += string.IsNullOrEmpty(customer)?"": " and a.customer = '"+customer+"'";
            if (!string.IsNullOrEmpty(bt) && !string.IsNullOrEmpty(et))
            {
                sql += " and a.create_date between '"+bt+"' and '"+et+"' ";
            }
            return sql;
        }

        public IEnumerable<dynamic> WOFinishStatus(string wo, string sales, string customer, string bt, string et)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(WOFinishStatusSql(wo,sales,customer,bt,et));
            }
            return res;
        }
        #endregion

        #region 工单状态
        //增加订单号查询条件 显示内容增加el等级 modify by xue lei on 2018-10-11
        private string WOStatusSql(string wo,string sales)
        {
            string sql = @"select distinct a.serial_nbr,a.workorder,b.sale_order,convert(varchar(100), c.wks_visit_date,120) wks_visit_date
,e.descriptions process_code,a.el_grade,a.final_grade,d.pmax from mes_main.dbo.assembly_status a 
inner join mes_main.dbo.wo_mfg b on a.workorder = b.workorder
inner join mes_main.dbo.trace_workstation_visit c on a.serial_nbr = c.serial_nbr and c.wks_id like '%HJJ%'
left join [mes_level2_iface].[dbo].[iv] d on a.serial_nbr = d.serial_nbr
left join [mes_main].[dbo].[df_processes] e on a.process_code = e.process_code where 1=1";
            sql += string.IsNullOrEmpty(wo) ? "" : " and a.workorder ='"+wo+"'";
            sql += string.IsNullOrEmpty(sales) ? "" : " and b.sale_order = '"+sales+"'";
//where a.workorder = '" + wo+"'";
            
            return sql;
        }
        public DataTable WOStatusDT(string wo,string sales)
        {
            return dbhelp.ExecuteDataTable(WOStatusSql(wo,sales), null);
        }
        public IEnumerable<dynamic> WOStatus(string wo,string sales)
        {
            IEnumerable<dynamic> res = null;
            using (var conn = Dpperhelper.OpenSqlConnection())
            {
                res = conn.Query(WOStatusSql(wo,sales));
            }
            return res;
        }
        #endregion


    }
}