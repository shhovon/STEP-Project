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
                    //employeeInfo = (from empInfo in db.Employee_Information
                    //                join reg in db.tblUser_Registration on empInfo.RegId equals reg.RegId
                    //                where reg.ReportSuper == deptHeadValue || reg.DeptHead == deptHeadValue
                    //                select new EmployeeViewModel
                    //                {
                    //                    EmployeeCode = empInfo.EmployeeCode,
                    //                    Name = empInfo.Name,
                    //                    Designation = empInfo.Designation
                    //                }).ToList();


                     employeeInfo = db.Database.SqlQuery<EmployeeViewModel>(
                                "prc_GetEmployeeListByDeptHead @DeptHeadValue",
                                new SqlParameter("@DeptHeadValue","123")).ToList();


                }
            }

            return View(employeeInfo);
        }

        [CustomAuthorize]
        [HttpPost]
        public ActionResult AddMarks(int RegId)
        {
           // if (!string.IsNullOrEmpty(RegId))
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

/*                        ViewBag.EmployeeCode = userInfo.EmployeeCode;
                        ViewBag.EmployeeName = userInfo.Name;*/
                        ViewBag.Designation = userInfo.Designation;


                        var kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>
                            ("exec prc_GetKraKpiOutcomeData @RegId", new SqlParameter("@RegId", RegId)).ToList();

                        ViewBag.KraKpiOutcomeData = kraKpiOutcomeData;
                        return View("KraKpiOutcomeView", kraKpiOutcomeData);

                    }
                }
            }

            /*return RedirectToAction("AddMarks");*/
            ViewBag.SuccessMessage = "Marks updated successfully!";
            return RedirectToAction("ViewEmpList", "DeptHead");
        }

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

        //private string GetEmployeeName(string employeeCode)
        //{
        //    string employeeName = string.Empty;

        //    using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
        //    {
        //        var employee = db.Employee_Information.FirstOrDefault(e => e.EmployeeCode == employeeCode);
        //        if (employee != null)
        //        {
        //            employeeName = employee.Name;
        //        }
        //    }

        //    return employeeName;
        //}


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
        public ActionResult ViewMarks(int regId)
        {
            List<MarksData> marksData;
            using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
            {
/*                if (!string.IsNullOrEmpty(regId))*/
                {
                    var userInfo = db.Database.SqlQuery<EmployeeInfo>(
                                    "prc_EmployeeInfoByRegID @RegID",
                                    new SqlParameter("@RegID", regId)).FirstOrDefault();

                    /* var regId = (from emp in db.Employee_Information
                                                     join st in db.STEPs on emp.RegId equals st.REG_ID
                                                     where emp.EmployeeCode == employeeCode
                                                     select st.REG_ID).FirstOrDefault();*/

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

        //public ActionResult GetRegId(string employeeCode)
        //{
        //    using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
        //    {
        //        var regId = (from emp in db.Employee_Information
        //                     join st in db.STEPs on emp.RegId equals st.REG_ID
        //                     where emp.EmployeeCode == employeeCode
        //                     select st.REG_ID).FirstOrDefault();

        //        return Json(regId, JsonRequestBehavior.AllowGet);
        //    }
        //}

    }
}
