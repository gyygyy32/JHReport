﻿using JHReport.Common;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public FileResult ExportExcel()
        {
            return File(ms, "application/vnd.ms-excel", strdate + "Excel.xls");
        }

        // GET: Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Report/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Report/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Report/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Report/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
