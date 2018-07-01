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

        private string JKSql(string salesorder,string lot)
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
    }
}