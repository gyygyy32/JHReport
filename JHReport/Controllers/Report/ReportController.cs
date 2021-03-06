﻿using Dapper;
using JHReport.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using JHReport.BLL;



namespace JHReport.Controllers
{
    [RoutePrefix("Report")]
    public class ReportController : Controller
    {
        static DbUtility dbhelp = new DbUtility(System.Configuration.ConfigurationManager.ConnectionStrings["mesConn"].ToString(), DbProviderType.SqlServer);

        [Route("Runcard")]
        // GET: Report
        public ActionResult RunCard()
        {
            return View();
        }

        [Route("Runcard01")]
        public ActionResult RunCardNew()
        {
            return View("~/Views/Report/RunCard01.cshtml");
        }

        //[Authorize]
        //[MyFilter2Attribute]
        [Route("QC")]
        public ActionResult QC()
        {
            return View();
        }
        [Route("TestDataDetail")]
        public ActionResult TestDataDetail()
        {
            return View();
        }
        [Route("PackOutput")]
        public ActionResult PackOutput()
        {
            return View();
        }
        [Route("JK")]
        public ActionResult JK()
        {
            return View();
        }
        [Route("WOFinishStatus")]
        public ActionResult WOFinishStatus()
        {
            return View();
        }
        [Route("WOStatus")]
        public ActionResult WOStatus()
        {
            return View();
        }
        [Route("WOStatusExcel/{wo=}/{sales=}")]
        [HttpGet]
        public FileContentResult WOStatusToExcel(string wo,string sales)
        {
            DataTable dt = new ReportService().WOStatusInfoDT(wo=="Null"?"":wo,sales=="Null"?"":sales);
            //string[] AryColnameChinese = { "工单号","订单号","组件条码","投产时间","当前站别","组件等级","实测功率" };

            dt.Columns[0].ColumnName = "组件条码";
            dt.Columns[1].ColumnName = "工单号";
            dt.Columns[2].ColumnName = "订单号";
            dt.Columns[3].ColumnName = "投产时间";
            dt.Columns[4].ColumnName = "当前站别";
            dt.Columns[5].ColumnName = "EL等级";
            dt.Columns[6].ColumnName = "组件等级";
            dt.Columns[7].ColumnName = "实测功率";



            return ExportExcel(dt, "WOStatus");
        }



        //质量报表导出excel
        [Route("QCExcel/{bt}/{et=}/{lot=}/{workshop=}/{status=}")]
        [HttpGet]
        public FileContentResult QCExportToExcel(string bt, string et, string lot, string workshop, string status)
        {
            DataTable dt = new ReportService().QCQueryInfo(bt == "Null" ? "" : bt
                                                                     , et == "Null" ? "" : et
                                                                     , lot == "Null" ? "" : lot
                                                                     , workshop == "Null" ? "" : workshop
                                                                     , status == "Null" ? "" : status);
            return ExportExcel(dt, "QCReport");
        }

        //晶科报表导出excel
        [Route("JKExcel/{salesorder=}/{lot=}")]
        [HttpGet]
        public FileContentResult JKExportExcel(string salesorder, string lot)
        {
            DataTable dt = new ReportService().JKQueryInfo(salesorder == "Null" ? "" : salesorder, lot == "Null" ? "" : lot);
            return ExportExcel(dt, "JKReport");
        }

        //测试数据明细导出excel
        [Route("TestDataDetailExcel/{lot=}/{wo=}/{bt=}/{et=}/{workshop=}")]

        [HttpGet]
        public FileContentResult TestDataDetailExportExcel(string lot, string wo, string bt, string et, string workshop)
        {
            DataTable dt = new ReportService().TestDataDetailQueryInfo(lot == "Null" ? "" : lot
                                                                     , wo == "Null" ? "" : wo
                                                                     , bt == "Null" ? "" : bt
                                                                     , et == "Null" ? "" : et
                                                                     , workshop == "Null" ? "" : workshop);
            return ExportExcel(dt, "TestDataDetailReport");
        }

        //包装产量报表导出excel
        [Route("PackoutputExcel/{workshop}/{bt}/{et}/{lot}/{container}/{pallet}/{check}")]
        [HttpGet]
        public FileContentResult PackOutputExportExcel(string workshop, string bt, string et, string lot, string container, string pallet, string check)
        {
            DataTable dt = new ReportService().PackOutputQueryInfo(workshop == "Null" ? "" : workshop
                                                                     , bt == "Null" ? "" : bt
                                                                     , et == "Null" ? "" : et
                                                                     , lot == "Null" ? "" : lot
                                                                     , container == "Null" ? "" : container
                                                                     , pallet == "Null" ? "" : pallet
                                                                     , check == "Null" ? "" : check);
            return ExportExcel(dt, "PackOutputReport");
        }


        public FileContentResult ExportExcel01(DataTable dt, string filename,string[] colname)
        {
            byte[] content = ExcelExportHelper.ExportExcel(dt, "", false, colname);
            return File(content, ExcelExportHelper.ExcelContentType, filename + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
        }
        public FileContentResult ExportExcel(DataTable dt, string filename)
        {
            List<string> listColName = new List<string>();
            foreach (DataColumn item in dt.Columns)
            {
                listColName.Add(item.ColumnName.ToString());
            }

            byte[] content = ExcelExportHelper.ExportExcel(dt, "", false, listColName.ToArray());
            return File(content, ExcelExportHelper.ExcelContentType, filename + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
        }


        /// <summary>  
        /// 转换为一个DataTable  
        /// </summary>  
        /// <typeparam name="TResult"></typeparam>  
        ///// <param name="value"></param>  
        /// <returns></returns>  
        public static DataTable ToDataTable<TResult>(IEnumerable<TResult> value) where TResult : class
        {
            //创建属性的集合  
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口  
            Type type = typeof(TResult);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列  
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in value)
            {
                //创建一个DataRow实例  
                DataRow row = dt.NewRow();
                //给row 赋值  
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable  
                dt.Rows.Add(row);
            }
            return dt;
        }
        public FileResult ExportExcel()
        {
            var sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            var lstTitle = new List<string> { "编号", "姓名", "年龄", "创建时间" };
            foreach (var item in lstTitle)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
            }
            sbHtml.Append("</tr>");

            for (int i = 0; i < 1000; i++)
            {
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", i);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>屌丝{0}号</td>", i);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", new Random().Next(20, 30) + i);
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", DateTime.Now);
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("</table>");

            //第一种:使用FileContentResult  
            byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
            return File(fileContents, "application/ms-excel", "fileContents.xls");

            //第二种:使用FileStreamResult  
            //var fileStream = new MemoryStream(fileContents);
            //return File(fileStream, "application/ms-excel", "fileStream.xls");

            //第三种:使用FilePathResult  
            //服务器上首先必须要有这个Excel文件,然会通过Server.MapPath获取路径返回.  
            //var fileName = Server.MapPath("~/Files/fileName.xls");
            //return File(fileName, "application/ms-excel", "fileName.xls");
        }

    }



    public class StudentViewModel
    {
        public List<Student> ListStudent
        {
            get
            {
                return StaticDataOfStudent.ListStudent;
            }
        }
    }

    public class StaticDataOfStudent
    {
        public static List<Student> ListStudent
        {
            get
            {
                return new List<Student>()
                {
                new Student(){ID=1,Name="曹操",Sex="男",Email="caocao@163.com",Age=24},
                new Student(){ID=2,Name="李易峰",Sex="女",Email="lilingjie@sina.com.cn",Age=24},
                new Student(){ID=3,Name="张三丰",Sex="男",Email="zhangsanfeng@qq.com",Age=224},
                new Student(){ID=4,Name="孙权",Sex="男",Email="sunquan@163.com",Age=1224},
                };
            }
        }
    }
    public class Student
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }
    }


}
