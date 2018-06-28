using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace JHReport.DAL
{
    public class ReportDal
    {
        static DbUtility dbhelp = new DbUtility(System.Configuration.ConfigurationManager.ConnectionStrings["mesConn"].ToString(), DbProviderType.SqlServer);
        public DataTable JKQueryInfo(string salesorder, string lot)
        {
            string sql = @"SELECT iv.[wks_visit_date] AS TTIME, ast.[serial_nbr] AS LOT_NUM, wm.[sale_order] AS WORK_ORDER_NO, iv.[wks_id] AS DEVICENUM, iv.[env_temp] AS AMBIENTTEMP
	, '' AS INTENSITY, iv.[ff] AS FF, iv.[eff] AS FF, iv.[pmax] AS PM, iv.[isc] AS ISC
	, iv.[ipm] AS IPM, iv.[voc] AS VOC, iv.[vpm] AS VPM, iv.[surf_temp] AS SENSORTEMP, iv.[rs] AS RS
	, iv.[rsh] AS RSH, iv.[eff] AS EFF, iv.[temp] AS TEMP, iv.[ivfile_path], '' AS DEC_CTM
	, ast.[pack_seq], ppk.[check_nbr], ms.[descriptions] AS SUPPLIER_NAME, '', [cell_uop]
	, iv.[pmax] / (CAST(REPLACE(cp.[cell_qty], 'W', '') AS float) * CAST(REPLACE([cell_uop], 'W', '') AS float)) AS VC_CELLEFF
FROM [mes_main].[dbo].[assembly_status] ast
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
	LEFT JOIN [mes_main].[dbo].[config_mat_glass] cmglass ON ab.glass_part_nbr = cmglass.part_nbr
	LEFT JOIN [mes_main].[dbo].[config_mat_frame] cmframe ON ab.frame_part_nbr = cmframe.part_nbr
	LEFT JOIN [mes_main].[dbo].[config_mat_jbox] cmjbox ON ab.jbox_part_nbr = cmjbox.part_nbr
WHERE 1=1 ";
            sql += String.IsNullOrEmpty(lot) ? "" : " and iv.[serial_nbr] = '" + lot + "'";
            sql += String.IsNullOrEmpty(lot) ? "" : " and wm.[sale_order] = '" + salesorder + "'";
            return dbhelp.ExecuteDataTable(sql,null);
        }
    }
}