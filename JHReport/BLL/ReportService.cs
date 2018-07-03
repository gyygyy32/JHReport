using System;
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
    }
}