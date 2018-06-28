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
    }
}