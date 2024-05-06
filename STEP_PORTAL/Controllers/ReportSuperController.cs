﻿using Newtonsoft.Json;
using STEP_DEMO.Models;
using STEP_PORTAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STEP_PORTAL.Controllers
{
    public class ReportSuperController : Controller
    {
        // GET: ReportSuper
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult AddMarks()
        {
            List<EmployeeViewModel> employeeInfo = new List<EmployeeViewModel>();
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                int deptHeadValue;
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
                {
                     employeeInfo = db.Database.SqlQuery<EmployeeViewModel>(
                                "prc_GetEmployeeListByDeptHead @DeptHeadValue",
                                new SqlParameter("@DeptHeadValue","123")).ToList();
                }
            }

            return View(employeeInfo);
        }

        [CustomAuthorize]
        [HttpPost]
        public ActionResult AddMarks(int? RegId)
        {
           // if (!string.IsNullOrEmpty(RegId))
            {
                int deptHeadValue;

                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
                {
                    using (DB_STEPEntities db = new DB_STEPEntities())
                    {
                        var last2session = (db.New_Tax_Period
                                     .OrderByDescending(t => t.TaxPeriod)
                                     .Select(t => t.TaxPeriod).Take(2).ToList());

                        ViewBag.TopTaxPeriods = last2session;

                        string selectedTaxPeriod = Session["SelectedTaxPeriod"] as string;

                        string employeeID = Request.Form["employeeCode"];
                        int regID = (int)Session["RegID"];

                            // insert marks history
                        tblMarksEntryHistory logEntry = new tblMarksEntryHistory
                            {
                                SupervisorID = deptHeadValue,
                                EmployeeID = employeeID,
                                UpdateTime = DateTime.Now,
                                UserIP = GetIPAddress(),

                            };
                            db.tblMarksEntryHistories.Add(logEntry);
                            db.SaveChanges();

                   
                        var userInfo = db.Database.SqlQuery<EmployeeInfo>(
                              "prc_EmployeeInfoByRegID @RegID",
                              new SqlParameter("@RegID", Session["RegID"])).FirstOrDefault();

                        var model = new KraKpiOutcomeModel
                        {
                            EmployeeCode = userInfo.EmployeeCode,
                            Name = userInfo.Name
                        };

                        ViewBag.Designation = userInfo.Designation;

                        /*int? SESSION_ID = db.New_Tax_Period.Where(t => t.TaxPeriod == sessionId).Select(t => t.TaxPerID).FirstOrDefault();*/

                        var kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>
                            ("exec prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                             new SqlParameter("@RegId", RegId),
                             new SqlParameter("@SESSION_ID", Session["SelectedTaxPeriod"])).ToList();

                        ViewBag.KraKpiOutcomeData = kraKpiOutcomeData;
                        return View("KraKpiOutcomeView", kraKpiOutcomeData);

                    }
                }
            }

            ViewBag.SuccessMessage = "Marks updated successfully!";
            return RedirectToAction("ViewEmpList", "DeptHead");
        }


        [CustomAuthorize]
        [HttpPost]
        public ActionResult UpdateMarks(List<KraKpiOutcomeModel> model, List<MarksUpdateModel> marksUpdateModel, int regId)
        {
            if (model != null && marksUpdateModel != null)
            {
                using (DB_STEPEntities db = new DB_STEPEntities())
                {
                    foreach (var item in marksUpdateModel)
                    {
                        var outcomeEntity = db.STEPs.FirstOrDefault(o => o.KPI_OUTCOME == item.Outcome && o.REG_ID == regId);
                        if (outcomeEntity != null)
                        {
                            outcomeEntity.Marks_Achieved = item.Marks;
                            db.Entry(outcomeEntity).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ViewEmpList", "DeptHead");
        }

        protected string GetIPAddress()
        {
            string ipList = (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                   Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }


        [CustomAuthorize]
        [HttpGet]
        public ActionResult ViewMarks()
        {
            int? regId = Session["RegId"] as int?;
            List<KraKpiOutcomeModel> kraKpiOutcomeData;
            using (var db = new DB_STEPEntities())
            {
                var last2session = (db.New_Tax_Period
                                   .OrderByDescending(t => t.TaxPeriod)
                                   .Select(t => t.TaxPeriod).Take(2).ToList());

                ViewBag.TopTaxPeriods = last2session;


                /*int? selectedTaxPeriod = (int)Session["SelectedTaxPeriod"];*/
                int? selectedTaxPeriod = Session["SelectedTaxPeriod"] as int?;
                if (selectedTaxPeriod == null)
                {
                    ViewBag.ErrorMessage = "Choose session first";
                    return View();
                }

                kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
               new SqlParameter("@RegId", regId),
               new SqlParameter("@SESSION_ID", selectedTaxPeriod)).ToList();
            }


            return View(kraKpiOutcomeData);
        }


        // view marks based on employee code

        [HttpPost]
        public ActionResult ViewMarks(int regId)
        {
            List<MarksData> marksData;
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
/*                if (!string.IsNullOrEmpty(regId))*/
                {
                    var userInfo = db.Database.SqlQuery<EmployeeInfo>(
                                    "prc_EmployeeInfoByRegID @RegID",
                                    new SqlParameter("@RegID", regId)).FirstOrDefault();

                    marksData = (from st in db.STEPs
                                 where st.REG_ID == regId
                                 select new MarksData
                                 {
                                     KPI_OUTCOME = st.KPI_OUTCOME,
                                     Marks_Achieved = st.Marks_Achieved ?? 0
                                 }).ToList();

                    ViewBag.EmployeeCode = userInfo.Name;
                    ViewBag.EmployeeName = userInfo.EmployeeCode;
                }
/*                else
                {
                    marksData = new List<MarksData>();
                }*/

                // fetch employee name based on employeeCode
               // string employeeName = GetEmployeeName(employeeCode);


                var last2session = (db.New_Tax_Period
                                   .OrderByDescending(t => t.TaxPeriod)
                                   .Select(t => t.TaxPeriod)
                                   .Take(2)
                                   .ToList());

                ViewBag.TopTaxPeriods = last2session;
            }

            return View(marksData);
        }

        [HttpPost]
        public ActionResult SaveReportSuperComment(string comment, int regId)
        {
            int updatedBy = (int)Session["RegID"];
            int sessionID = (int)Session["SelectedTaxPeriod"];
            string statusType = "Report Super Comment";
            string statusMessage = comment;
            DateTime updatedDate = DateTime.Now;

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var result = db.Database.SqlQuery<StatusResult>(
                    "EXEC prc_UpdateStatus @RegId, @SESSION_ID, @StatusType, @StatusValue, @StatusMessage, @Updated_date, @Updated_by",
                    new SqlParameter("@RegId", regId),
                    new SqlParameter("@SESSION_ID", sessionID),
                    new SqlParameter("@StatusType", statusType),
                    new SqlParameter("@StatusValue", DBNull.Value),
                    new SqlParameter("@StatusMessage", statusMessage),
                    new SqlParameter("@Updated_date", updatedDate),
                    new SqlParameter("@Updated_by", updatedBy)).FirstOrDefault();

                if (result.Status)
                {
                    return Json(new { success = true, message = result.Message });
                }
                else
                {
                    return Json(new { success = false, message = result.Message });
                }

/*                return RedirectToAction("ViewEmpList", "DeptHead");*/

/*                return View();*/
            }
        }  
        
        [HttpPost]
        public ActionResult SaveUserComment(string comment)
        {
            int updatedBy = (int)Session["RegID"];
            int sessionID = (int)Session["SelectedTaxPeriod"];
            string statusType = "Employee Comment";
            string statusMessage = comment;
            DateTime updatedDate = DateTime.Now;

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var result = db.Database.SqlQuery<StatusResult>(
                    "EXEC prc_UpdateStatus @RegId, @SESSION_ID, @StatusType, @StatusValue, @StatusMessage, @Updated_date, @Updated_by",
                    new SqlParameter("@RegId", updatedBy),
                    new SqlParameter("@SESSION_ID", sessionID),
                    new SqlParameter("@StatusType", statusType),
                    new SqlParameter("@StatusValue", DBNull.Value),
                    new SqlParameter("@StatusMessage", statusMessage),
                    new SqlParameter("@Updated_date", updatedDate),
                    new SqlParameter("@Updated_by", updatedBy)).FirstOrDefault();

                if (result.Status)
                {
                    return Json(new { success = true, message = result.Message });
                }
                else
                {
                    return Json(new { success = false, message = result.Message });
                }
            }
        }

        public ActionResult GetReportSuperComment()
        {
            if (Session["SelectedTaxPeriod"] == null)
            {
                ViewBag.ErrorMessage = "Choose session first";
                return Json(new { SupervisorComment = "" }, JsonRequestBehavior.AllowGet);
            }

            int regId = (int)Session["RegID"];
            int sessionID = (int)Session["SelectedTaxPeriod"];

            string supervisorComment = "";

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                supervisorComment = db.tbl_StepMaster
                                        .Where(comment => comment.RegId == regId && comment.SESSION_ID == sessionID)
                                        .Select(comment => comment.Supervisor_Comment)
                                        .FirstOrDefault();
            }

            return Json(new { SupervisorComment = supervisorComment }, JsonRequestBehavior.AllowGet);
        } 
        
        
        public ActionResult GetUserComment()
        {
            if (Session["SelectedTaxPeriod"] == null)
            {
                ViewBag.ErrorMessage = "Choose session first";
                return Json(new { SupervisorComment = "" }, JsonRequestBehavior.AllowGet);
            }

            int regId = (int)Session["RegID"];
            int sessionID = (int)Session["SelectedTaxPeriod"];

            string userComment = "";

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                userComment = db.tbl_StepMaster
                                        .Where(comment => comment.RegId == regId && comment.SESSION_ID == sessionID)
                                        .Select(comment => comment.User_Comment)
                                        .FirstOrDefault();
            }

            return Json(new { UserComment = userComment }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetReportSuperOwnComment(int regId)
        {
            if (Session["SelectedTaxPeriod"] == null)
            {
                ViewBag.ErrorMessage = "Choose session first";
                return Json(new { SupervisorComment = "" }, JsonRequestBehavior.AllowGet);
            }

            int sessionID = (int)Session["SelectedTaxPeriod"];

            string supervisorComment = "";

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                supervisorComment = db.tbl_StepMaster
                                        .Where(comment => comment.RegId == regId && comment.SESSION_ID == sessionID)
                                        .Select(comment => comment.Supervisor_Comment)
                                        .FirstOrDefault();
            }

            return Json(new { SupervisorComment = supervisorComment }, JsonRequestBehavior.AllowGet);
        }


    }
}
