using Newtonsoft.Json;
using STEP_DEMO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STEP_DEMO.Controllers
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
            using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
            {
                int deptHeadValue;
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
                {
                    // fetch employee information for the dropdown list
                    employeeInfo = (from empInfo in db.Employee_Information
                                    join reg in db.tblUser_Registration on empInfo.RegId equals reg.RegId
                                    where reg.ReportSuper == deptHeadValue || reg.DeptHead == deptHeadValue
                                    select new EmployeeViewModel
                                    {
                                        EmployeeCode = empInfo.EmployeeCode,
                                        Name = empInfo.Name,
                                        Designation = empInfo.Designation
                                    }).ToList();

                }
            }

            return View(employeeInfo);
        }

        [CustomAuthorize]
        [HttpPost]
        public ActionResult AddMarks(string employeeCode)
        {
            if (!string.IsNullOrEmpty(employeeCode))
            {
                int deptHeadValue;
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
                {
                    using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
                    {
                        var last2session = (db.New_Tax_Period
                                     .OrderByDescending(t => t.TaxPeriod)
                                     .Select(t => t.TaxPeriod).Take(2).ToList());

                        ViewBag.TopTaxPeriods = last2session;

                        string employeeID = Request.Form["employeeCode"];

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

                        var employeeCodeParam = new SqlParameter("@EmployeeCode", employeeCode);
                        var deptHeadValueParam = new SqlParameter("@DeptHeadValue", deptHeadValue);

                        // fetch employee name based on employeeCode
                        string employeeName = GetEmployeeName(employeeCode);
                        ViewBag.EmployeeCode = employeeCode;
                        ViewBag.EmployeeName = employeeName;

                        var kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>("exec prc_GetKraKpiOutcomeData @EmployeeCode, @DeptHeadValue", employeeCodeParam, deptHeadValueParam).ToList();
                        ViewBag.KraKpiOutcomeData = kraKpiOutcomeData;
                        return View("KraKpiOutcomeView", kraKpiOutcomeData);
                    }
                }
            }

            /*return RedirectToAction("AddMarks");*/
            ViewBag.SuccessMessage = "Marks updated successfully!";
            return RedirectToAction("ViewEmpList", "DeptHead");
        }

        /* [CustomAuthorize]
         [HttpPost]
         public ActionResult UpdateMarks(List<string> outcomes, Dictionary<string, int> marks)
         {
             using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
             {
                 if (outcomes != null && marks != null)
                 {
                     foreach (var outcome in outcomes)
                     {
                         if (marks.TryGetValue(outcome, out var mark))
                         {
                             var outcomeEntity = db.STEPs.FirstOrDefault(o => o.KPI_OUTCOME == outcome);

                             if (outcomeEntity != null)
                             {
                                 outcomeEntity.Marks_Achieved = mark;
                                 db.Entry(outcomeEntity).State = EntityState.Modified;
                             }
                         }
                     }

                     db.SaveChanges();

                     TempData["SuccessMessage"] = "Marks updated successfully!";
                 }
             }
             return RedirectToAction("ViewEmpList", "DeptHead");
             //return RedirectToAction("AddMarks");
         }*/


        public class MarksUpdateModel
        {
            public Dictionary<string, int> Marks { get; set; }
        }

        [CustomAuthorize]
        [HttpPost]
        public ActionResult UpdateMarks(MarksUpdateModel model)
        {
            if (model != null && model.Marks != null)
            {
                using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
                {
                    foreach (var outcome in model.Marks.Keys)
                    {
                        var mark = model.Marks[outcome];
                        var outcomeEntity = db.STEPs.FirstOrDefault(o => o.KPI_OUTCOME == outcome);

                        if (outcomeEntity != null)
                        {
                            outcomeEntity.Marks_Achieved = mark;
                            db.Entry(outcomeEntity).State = EntityState.Modified;
                        }
                    }

                    db.SaveChanges();                    
                }               
            }


/*            return RedirectToAction("AddMarks");*/
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

        private string GetEmployeeName(string employeeCode)
        {
            string employeeName = string.Empty;

            using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
            {
                var employee = db.Employee_Information.FirstOrDefault(e => e.EmployeeCode == employeeCode);
                if (employee != null)
                {
                    employeeName = employee.Name;
                }
            }

            return employeeName;
        }


        [CustomAuthorize]
        [HttpGet]
        public ActionResult ViewMarks()
        {
            int? regId = Session["RegId"] as int?;
            List<MarksData> marksData;
            using (var db = new EMP_EVALUATIONEntities())
            {
                var last2session = (db.New_Tax_Period
                                   .OrderByDescending(t => t.TaxPeriod)
                                   .Select(t => t.TaxPeriod).Take(2).ToList());

                ViewBag.TopTaxPeriods = last2session;

                marksData = db.STEPs
                        .Where(s => s.REG_ID == regId)
                        .Select(s => new MarksData
                        {
                            KPI_OUTCOME = s.KPI_OUTCOME,
                            Marks_Achieved = s.Marks_Achieved ?? 0
                        })
                        .ToList();
            }

            return View(marksData);
        }

        // view marks based on employee code

        [HttpPost]
        public ActionResult ViewMarks(string employeeCode)
        {
            List<MarksData> marksData;
            using (var db = new EMP_EVALUATIONEntities())
            {
                if (!string.IsNullOrEmpty(employeeCode))
                {
                    var regId = (from emp in db.Employee_Information
                                 join st in db.STEPs on emp.RegId equals st.REG_ID
                                 where emp.EmployeeCode == employeeCode
                                 select st.REG_ID).FirstOrDefault();

                    marksData = (from st in db.STEPs
                                 where st.REG_ID == regId
                                 select new MarksData
                                 {
                                     KPI_OUTCOME = st.KPI_OUTCOME,
                                     Marks_Achieved = st.Marks_Achieved ?? 0
                                 }).ToList();
                }
                else
                {
                    marksData = new List<MarksData>();
                }

                var last2session = (db.New_Tax_Period
                                   .OrderByDescending(t => t.TaxPeriod)
                                   .Select(t => t.TaxPeriod)
                                   .Take(2)
                                   .ToList());

                ViewBag.TopTaxPeriods = last2session;
            }

            return View(marksData);
        }

        public ActionResult GetRegId(string employeeCode)
        {
            using (var db = new EMP_EVALUATIONEntities())
            {
                var regId = (from emp in db.Employee_Information
                             join st in db.STEPs on emp.RegId equals st.REG_ID
                             where emp.EmployeeCode == employeeCode
                             select st.REG_ID).FirstOrDefault();

                return Json(regId, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
