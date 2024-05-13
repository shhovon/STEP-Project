using STEP_DEMO.Models;
using STEP_PORTAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STEP_DEMO.Controllers
{
    public class HRController : Controller
    {
        // GET: HR
        public ActionResult Index()
        {
            return View();
        }

        public List<EmployeeInfo> GetEmployeeListByDeptHead(int deptHeadValue, int companyId)
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var employees = db.Database.SqlQuery<EmployeeInfo>("exec prc_GetEmployeeByHR @RegID, @CompID",
                    new SqlParameter("@RegID", deptHeadValue),
                    new SqlParameter("@CompID", companyId)).ToList();

                return employees;
            }
        }

        public List<EmployeeInfo> GetEmployeeListBySearchHR(int deptHeadValue, int companyId, string DepartmentDropdown, string SectionDropdown)
        {
            var SelectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var employees = db.Database.SqlQuery<EmployeeInfo>("exec prc_SearchEmployeeByHR @RegID, @CompID, @DepartmentName, @SectionName, @SESSION_ID",
                    new SqlParameter("@RegID", deptHeadValue),
                    new SqlParameter("@CompID", companyId),
                    new SqlParameter("@DepartmentName", DepartmentDropdown),
                    new SqlParameter("@SectionName", SectionDropdown),
                    new SqlParameter("@SESSION_ID", SelectedTaxPeriod)).ToList();

                return employees;
            }
        }

        private List<CompanyViewModel> GetCompanies()
        {
            List<CompanyViewModel> companies = new List<CompanyViewModel>();
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                companies = db.Company_Information
                              .Select(c => new CompanyViewModel
                              {
                                  ID = c.ID,
                                  Name = c.Name
                              })
                              .ToList();
            }

            return companies;
        }

        public List<DeptSecViewModel> GetDepartmentsAndSections(int companyId)
        {
            List<DeptSecViewModel> departments = new List<DeptSecViewModel>();

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var deptSections = db.Database.SqlQuery<DeptSecViewModel>("exec prc_GetDeptAndSec @CompanyId", new SqlParameter("@CompanyId", companyId)).ToList();

                departments = deptSections
                    .GroupBy(ds => ds.DepartmentName)
                    .Select(g => new DeptSecViewModel
                    {
                        DepartmentName = g.Key,
                        Sections = g.Select(s => s.SectionName).Distinct().ToList()
                    })
                    .ToList();
            }

            return departments;
        }

        [CustomAuthorize]
        public ActionResult ViewEmpListHR()
        {
            List<EmployeeInfo> employees = new List<EmployeeInfo>();
            int deptHeadValue;
            int companyId = 0;

            if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
            {
                employees = GetEmployeeListByDeptHead(deptHeadValue, companyId);
            }

            List<CompanyViewModel> companies = GetCompanies();
            ViewBag.Companies = new SelectList(companies, "ID", "Name");
            List<DeptSecViewModel> departments = GetDepartmentsAndSections(1);
            ViewBag.Departments = departments;

            List<string> topTaxPeriods = new List<string>();
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var last2session = (db.New_Tax_Period
              .OrderByDescending(t => t.TaxPeriod)
              .Take(2).ToList());

                ViewBag.TopTaxPeriods = last2session;

            }

            var SelectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());

            ViewBag.SelectedTaxPeriod = SelectedTaxPeriod;
            string selectedSection = null;
            ViewBag.SelectedSection = selectedSection;

            var model = new EmployeeSessionViewModelClass
            {
                Employees = employees,
                TopTaxPeriods = topTaxPeriods
            };

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(model);
        }

        [HttpPost]
        public ActionResult ViewEmpListHR(int? companyId, string DepartmentDropdown, string SectionDropdown)
        {
            List<EmployeeInfo> employees = new List<EmployeeInfo>();

            if (companyId != null && Session["RegID"] != null)
            {
                int deptHeadValue;
                if (int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
                {
                    employees = GetEmployeeListBySearchHR(deptHeadValue, companyId.Value, DepartmentDropdown, SectionDropdown);
                }
            }

            List<CompanyViewModel> companies = GetCompanies();
            ViewBag.Companies = new SelectList(companies, "ID", "Name", companyId);
            List<DeptSecViewModel> departments = GetDepartmentsAndSections(1);
            ViewBag.Departments = departments;

            List<string> topTaxPeriods = new List<string>();
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var last2session = db.New_Tax_Period
                    .OrderByDescending(t => t.TaxPeriod)
                    .Take(2).ToList();

                ViewBag.TopTaxPeriods = last2session;
                var SelectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());
                ViewBag.SelectedTaxPeriod = SelectedTaxPeriod;
            }

            ViewBag.SelectedSection = SectionDropdown;
            var model = new EmployeeSessionViewModelClass
            {
                Employees = employees,
                TopTaxPeriods = topTaxPeriods
            };

            return View(model);
        }


        [CustomAuthorize]
        public ActionResult AddMarksHR(string regId)
        {
            int RegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
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

                    var kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>
                        ("exec prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                         new SqlParameter("@RegId", RegId),
                         new SqlParameter("@SESSION_ID", Session["SelectedTaxPeriod"])).ToList();

                    ViewBag.KraKpiOutcomeData = kraKpiOutcomeData;
                    ViewBag.RegId = RegId;
                    return View("KraKpiOutcomeViewHR", kraKpiOutcomeData);

                }
            }

            return null;

        }

        [CustomAuthorize]
        [HttpPost]
        public ActionResult AddMarksHR2(int? RegId)
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
                        return View("KraKpiOutcomeViewHR", kraKpiOutcomeData);
/*                        return PartialView("~/Views/HR/KraKpiOutcomeViewHR.cshtml", kraKpiOutcomeData);*/

                    }
                }
            }

            ViewBag.SuccessMessage = "Marks updated successfully!";
            return RedirectToAction("ViewEmpListHR", "HR");
        }


        [CustomAuthorize]
        [HttpPost]
        public ActionResult UpdateMarks(List<KraKpiOutcomeModel> model)
        {
            int regId = Convert.ToInt32(Request.Form["regId"]);
            if (model != null)
            {
                using (DB_STEPEntities db = new DB_STEPEntities())
                {
                    foreach (var item in model)
                    {
                        var outcomeEntity = db.STEPs.FirstOrDefault(o => o.KPI_ID == item.KPI_ID && o.REG_ID == regId && o.KPI_OUTCOME == item.Outcome);
                        if (outcomeEntity != null)
                        {
                            outcomeEntity.Marks_Achieved = item.SelectedMarks.Value;
                            db.Entry(outcomeEntity).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();

                }
            }
            TempData["SuccessMessage"] = "Marks updated successfully!";

            string encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(regId.ToString());
            return RedirectToAction("AddMarksHR", new { regId = encryptedRegId });

            /*return RedirectToAction("AddMarks", new { RegId = regId });*/
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

                var last2session = (db.New_Tax_Period
                                   .OrderByDescending(t => t.TaxPeriod)
                                   .Select(t => t.TaxPeriod)
                                   .Take(2)
                                   .ToList());

                ViewBag.TopTaxPeriods = last2session;
            }

            return View(marksData);
        }

        public ActionResult SaveAttendance(int attendance, string regId)
        {
            int RegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
            int sessionID = (int)Session["SelectedTaxPeriod"];
            DateTime updatedDate = DateTime.Now;

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var existingRecord = db.tbl_StepMaster.FirstOrDefault(s => s.SESSION_ID == sessionID && s.RegId == RegId);

                if (existingRecord != null)
                {
                    existingRecord.Attendance = attendance;
                    existingRecord.Updated_date = DateTime.Now;
                    existingRecord.Updated_by = RegId.ToString();
                }
                else
                {
                    var newRecord = new tbl_StepMaster
                    {
                        SESSION_ID = sessionID,
                        RegId = RegId,
                        Attendance = attendance,
                        Updated_date = DateTime.Now,
                        Updated_by = RegId.ToString()
                    };

                    db.tbl_StepMaster.Add(newRecord);
                }
                    db.SaveChanges();
            }
            TempData["SuccessMessage"] = "Attendance marks saved successfully!";

            string encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(regId.ToString());
            return RedirectToAction("AddMarksHR", new { regId = encryptedRegId });
/*            return RedirectToAction("ViewEmpListHR", "HR");*/
        }

        [HttpPost]
        public ActionResult SaveDiscipline(int discipline, string regId)
        {
            int RegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
            int sessionID = (int)Session["SelectedTaxPeriod"];
            DateTime updatedDate = DateTime.Now;

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var existingRecord = db.tbl_StepMaster.FirstOrDefault(s => s.SESSION_ID == sessionID && s.RegId == RegId);

                if (existingRecord != null)
                {
                    existingRecord.Discipline = discipline;
                    existingRecord.Updated_date = DateTime.Now;
                    existingRecord.Updated_by = RegId.ToString();
                }
                else
                {
                    var newRecord = new tbl_StepMaster
                    {
                        SESSION_ID = sessionID,
                        RegId = RegId,
                        Discipline = discipline,
                        Updated_date = DateTime.Now,
                        Updated_by = RegId.ToString()
                    };

                    db.tbl_StepMaster.Add(newRecord);
                }
                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Discipline marks saved successfully!";
            /*            ViewBag.SuccessMessage = "Discipline marks saved successfully!";*/

            /* return RedirectToAction("ViewEmpListHR", "HR");*/
            string encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(regId.ToString());
            return RedirectToAction("AddMarksHR", new { regId = encryptedRegId });
        }
    }
}