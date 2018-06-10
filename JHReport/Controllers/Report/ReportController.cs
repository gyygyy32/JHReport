﻿using JHReport.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace JHReport.Controllers
{
    [RoutePrefix("Report")]
    public class ReportController : Controller
    {
        [Route("{action=Runcard}")]
        // GET: Report
        public ActionResult RunCard()
        {
            return View();
        }
        //[Authorize]
        //[MyFilter2Attribute]
        [Route("{action=QC}")]
        public ActionResult QC()
        {
            return View();
        }
        [Route("{action=TestDataDetail}")]
        public ActionResult TestDataDetail()
        {
            return View();
        }
        [Route("{action=PackOutput}")]
        public ActionResult PackOutput()
        {
            return View();
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


        [Route("{action=ExportToExcel}")]
        public FileContentResult ExportToExcel()
        {
            List<Student> lstStudent = StaticDataOfStudent.ListStudent;
            string[] columns = { "ID", "Name", "Age" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(lstStudent, "", false, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "MyStudent.xlsx");
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
