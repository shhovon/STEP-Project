using STEP_DEMO.Models;
using System;
using System.Collections.Generic;
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
                        var employeeCodeParam = new SqlParameter("@EmployeeCode", employeeCode);
                        var deptHeadValueParam = new SqlParameter("@DeptHeadValue", deptHeadValue);

                        var kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>("exec prc_GetKraKpiOutcomeData @EmployeeCode, @DeptHeadValue", employeeCodeParam, deptHeadValueParam).ToList();
                        ViewBag.KraKpiOutcomeData = kraKpiOutcomeData;
                        return View("KraKpiOutcomeView", kraKpiOutcomeData);
                    }
                }
            }

            return RedirectToAction("AddMarks");
        }

        [CustomAuthorize]
        [HttpPost]
        public ActionResult UpdateMarks(List<string> outcomes, Dictionary<string, int> marks)
        {
            using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
            {

                int regId;
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out regId))
                {
                    // insert marks history
                    tblMarksEntryHistory logEntry = new tblMarksEntryHistory
                    {
                        SupervisorID = regId,
                        UpdateTime = DateTime.Now,
                        UserIP = GetIPAddress(),

                    };
                    db.tblMarksEntryHistories.Add(logEntry);
                    db.SaveChanges();
                }
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
                            }
                        }
                    }

                    db.SaveChanges();

/*                    TempData["SuccessMessage"] = "Marks updated successfully!";*/
                }
            }

            return RedirectToAction("AddMarks");
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
        public ActionResult ViewMarks()
        {
            int? regId = Session["RegId"] as int?;
            List<MarksData> marksData;
            using (var db = new EMP_EVALUATIONEntities())
            {
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

    }
}
