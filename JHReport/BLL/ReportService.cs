﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using JHReport.DAL;

namespace JHReport.BLL
{
    public class ReportService
    {
        public DataTable JKQueryInfo(string saleorder, string lot)
        {
            return new ReportDal().JKQueryInfo(saleorder,lot);
        }

        public IEnumerable<dynamic> JKQueryInfoAPI(string saleorder, string lot)
        {
            return new ReportDal().JKQueryInfoAPI(saleorder, lot);
        }

        public DataTable TestDataDetailQueryInfo(string lot, string wo, string bt, string et, string workshop)
        {
            return new ReportDal().TestDataDetailQueryInfo(lot, wo, bt, et, workshop);
        }

        public DataTable QCQueryInfo(string bt, string et, string lot, string workshop, string status)
        {
            return new ReportDal().QCQueryInfo( bt,  et,  lot,  workshop,  status);
        }

        public DataTable PackOutputQueryInfo(string workshop, string bt, string et, string lot, string container, string pallet, string check)
        {
            return new ReportDal().PackOutputQueryInfo(workshop, bt, et, lot, container, pallet, check);
        }

        #region 组件电子流转单
        //装框线盒
        public IEnumerable<dynamic> FrameBoxQueryInfo(string lot)
        {
            return new ReportDal().FrameBoxQueryInfo(lot);

        }
        //叠层背板
        public IEnumerable<dynamic> LaminationBackQueryInfo(string lot)
        {
            return new ReportDal().LaminationBackQueryInfo(lot);
        }
        //叠层EVA
        public IEnumerable<dynamic> LaminationEVAQueryInfo(string lot)
        {
            return new ReportDal().LaminationEVAQueryInfo(lot);
        }
        //叠层玻璃
        public IEnumerable<dynamic> LaminationGlassQueryInfo(string lot)
        {
            return new ReportDal().LaminationGlassQueryInfo(lot);
        }
        //叠层高透玻璃
        public IEnumerable<dynamic> LaminationHighEVAQueryInfo(string lot)
        {
            return new ReportDal().LaminationHighEVAQueryInfo(lot);
        }

        //层压前EL
        public IEnumerable<dynamic> ELBeforeLayupQueryInfo(string lot)
        {
            return new ReportDal().ELBeforeLayup(lot);
        }
        //测试后EL
        public IEnumerable<dynamic> ELAfterTestQueryInfo(string lot)
        {
            return new ReportDal().ELAfterTest(lot);
        }
        //清洗
        public IEnumerable<dynamic> CleanQueryInfo(string lot)
        {
            return new ReportDal().CleanQueryInfo(lot);
        }
        //IV
        public IEnumerable<dynamic> IVQueryInfo(string lot)
        {
            return new ReportDal().IVQueryInfo(lot);
        }
        //包装
        public IEnumerable<dynamic> PackQueryInfo(string lot)
        {
            return new ReportDal().PackQueryInfo(lot);
        }
        //层压后检验
        public IEnumerable<dynamic> QCAfterLayupInfo(string lot)
        {
            return new ReportDal().QCAfterLayup(lot);
        }
        //功率后EL
        public IEnumerable<dynamic> ELAfterIVInfo(string lot)
        {
            return new ReportDal().ELAfterIV(lot);
        }

        #endregion

        #region 工单达成情况
        public IEnumerable<dynamic> WOFinishStatusInfo(string wo, string sales, string customer, string bt, string et)
        {
            return new ReportDal().WOFinishStatus(wo, sales, customer, bt, et);
        }
        #endregion

        #region 工单状态
        public IEnumerable<dynamic> WOStatusInfo(string wo,string sales)
        {
            return new ReportDal().WOStatus(wo,sales);
        }
        public DataTable WOStatusInfoDT(string wo,string sales)
        {
            return new ReportDal().WOStatusDT(wo,sales);
        }
        #endregion
    }
}