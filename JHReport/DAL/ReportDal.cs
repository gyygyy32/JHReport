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
       convert(varchar(100), iv.[wks_visit_date],120) as wks_visit_date /*测试时间*/
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
      ,el.el_grade
      from mes_level2_iface.dbo.iv iv  /*测试数据*/
,[mes_main].[dbo].[assembly_basis] ab
,[mes_main].[dbo].[config_mat_cell]cmc
--,[mes_main].[dbo].[config_mat_eva] cme
,[mes_level2_iface].[dbo].[el] el
,wo_mfg wm
where iv.[serial_nbr]=ab.serial_nbr
and ab.cell_part_nbr=cmc.[part_nbr]
--and ab.eva_part_nbr=cme.part_nbr
and iv.[serial_nbr]=el.[serial_nbr]
and ab.workorder=wm.workorder and iv.wks_visit_date>='" + bt + "' and  iv.wks_visit_date<='" + et+"'";
            sql += String.IsNullOrEmpty(workshop.ToString()) ? "" : " and wm.area_code LIKE '" + Convert.ToString(workshop) + "%'";
            sql += String.IsNullOrEmpty(wo.ToString()) ? "" : " AND ab.workorder = '" + Convert.ToString(wo) + "'";
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



    }
}