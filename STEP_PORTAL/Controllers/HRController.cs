

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
            int regId = int.Parse(Session["RegID"].ToString());
            List<CompanyViewModel> companies = new List<CompanyViewModel>();
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var companyInfos = db.Database.SqlQuery<CompanyViewModel>(
                                    "exec prc_GetCompanyByRegID @RegID",
                                    new SqlParameter("@RegId", regId)).ToList();

                foreach (var companyInfo in companyInfos)
                {
                    CompanyViewModel companyViewModel = new CompanyViewModel
                    {
                        ID = companyInfo.ID,
                        Name = companyInfo.Name
                    };
                    companies.Add(companyViewModel);
                }
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
            ViewBag.SelectedDepartment = DepartmentDropdown;
            ViewBag.SelectedSection = SectionDropdown;

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
<<<<<<< HEAD
            int RegId = int.Parse(Session["RegID"].ToString());
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            int EmpRegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
=======
<<<<<<< HEAD
            int RegId = int.Parse(Session["RegID"].ToString());
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            int EmpRegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
=======
<<<<<<< HEAD
            int RegId = int.Parse(Session["RegID"].ToString());
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            int EmpRegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
=======
<<<<<<< HEAD
            int RegId = int.Parse(Session["RegID"].ToString());
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            int EmpRegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
=======
<<<<<<< HEAD
            int RegId = int.Parse(Session["RegID"].ToString());
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            int EmpRegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
=======
/*            int RegId;
            try
            {
                RegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
            }
            catch (FormatException)
            {
                RegId = 0;
            }*/
            int RegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
            int deptHeadValue;
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

                using (DB_STEPEntities db = new DB_STEPEntities())
                {
                    var authResult = db.Database.SqlQuery<StatusResult>(
                                        "exec prc_CheckAuth @RegId, @SESSION_ID, @Type, @EmpRegId",
                                        new SqlParameter("@RegId", RegId),
                                        new SqlParameter("@SESSION_ID", sessionID),
                                        new SqlParameter("@Type", "AddMarksHR"),
                                        new SqlParameter("@EmpRegId", EmpRegId)
                                    ).FirstOrDefault();

                    if (authResult == null || !authResult.Status)
                    {
                        ViewBag.AuthorizationMessage = authResult?.Message ?? "Unauthorized access";
<<<<<<< HEAD
                        TempData["Tag"] = "Unauthorized access";
=======
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                        return RedirectToAction("Dashboard", "Home");
                    }

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
                        SupervisorID = RegId,
                        EmployeeID = employeeID,
                        UpdateTime = DateTime.Now,
                        UserIP = GetIPAddress(),

                    };
                    db.tblMarksEntryHistories.Add(logEntry);
                    db.SaveChanges();


                    var userInfo = db.Database.SqlQuery<EmployeeInfo>(
                          "prc_EmployeeInfoByRegID @RegID",
<<<<<<< HEAD
                          new SqlParameter("@RegID", EmpRegId)).FirstOrDefault();
=======
<<<<<<< HEAD
                          new SqlParameter("@RegID", EmpRegId)).FirstOrDefault();
=======
<<<<<<< HEAD
                          new SqlParameter("@RegID", EmpRegId)).FirstOrDefault();
=======
<<<<<<< HEAD
                          new SqlParameter("@RegID", EmpRegId)).FirstOrDefault();
=======
<<<<<<< HEAD
                          new SqlParameter("@RegID", EmpRegId)).FirstOrDefault();
=======
                          new SqlParameter("@RegID", RegId)).FirstOrDefault();
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

                    Session["EmployeeCodeInd"] = userInfo.EmployeeCode;
                    Session["NameInd"] = userInfo.Name;
                    Session["DesignationInd"] = userInfo.Designation;

                    var model = new KraKpiOutcomeModel
                    {
                        EmployeeCode = userInfo.EmployeeCode,
                        Name = userInfo.Name
                    };

                    ViewBag.Designation = userInfo.Designation;

                    var kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>
                        ("exec prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                         new SqlParameter("@RegId", EmpRegId),
                         new SqlParameter("@SESSION_ID", Session["SelectedTaxPeriod"])).ToList();

                    ViewBag.KraKpiOutcomeData = kraKpiOutcomeData;
                    ViewBag.RegId = EmpRegId;
                    return View("KraKpiOutcomeViewHR", kraKpiOutcomeData);

                }


            return null;

        }

        [CustomAuthorize]
        [HttpPost]
        public ActionResult UpdateMarks(List<KraKpiOutcomeModel> model)
        {
            int regId = Convert.ToInt32(Request.Form["regId"]);
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430


            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();

<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
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

<<<<<<< HEAD
                    var status = DC.UpdateRating(regId, sessionID);

=======

                    var status = DC.UpdateRating(regId, sessionID);

<<<<<<< HEAD
=======

                    db.Database.ExecuteSqlCommand(
                            "exec prc_UpdateRating @RegId, @SESSION_ID",
                            new SqlParameter("@RegId", regId),
                            new SqlParameter("@SESSION_ID", sessionID)
                        );
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

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

        [HttpPost]
        public ActionResult SaveAttendance(int attendance, string regId)
        {
            int RegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
            int sessionID = (int)Session["SelectedTaxPeriod"];
            DateTime updatedDate = DateTime.Now;
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();

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
                    var status = DC.UpdateRating(RegId, sessionID);
            }
            TempData["SuccessMessage"] = "Attendance marks saved successfully!";

<<<<<<< HEAD
=======
<<<<<<< HEAD
            //string encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(regId.ToString());
=======
<<<<<<< HEAD
            //string encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(regId.ToString());
=======
<<<<<<< HEAD
            //string encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(regId.ToString());
=======
<<<<<<< HEAD
            //string encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(regId.ToString());
=======
<<<<<<< HEAD
            //string encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(regId.ToString());
=======
           // string encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(regId.ToString());
>>>>>>> 2fb5633e7882b0cde4cce6838b80756d8d12b3e8
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
            return RedirectToAction("AddMarksHR", new { regId = regId });
/*            return RedirectToAction("ViewEmpListHR", "HR");*/
        }

        [HttpPost]
        public ActionResult SaveDiscipline(int discipline, string regId)
        {
            int RegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
            int sessionID = (int)Session["SelectedTaxPeriod"];
            DateTime updatedDate = DateTime.Now;
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();

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
                var status = DC.UpdateRating(RegId, sessionID);
            }

            TempData["SuccessMessage"] = "Discipline marks saved successfully!";

            /* return RedirectToAction("ViewEmpListHR", "HR");*/
            //string encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(regId.ToString());
            return RedirectToAction("AddMarksHR", new { regId = regId });
        }

        [CustomAuthorize]
        public ActionResult CorpHR()
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

            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult CorpHR(int? companyId, string DepartmentDropdown, string SectionDropdown)
        {
            List<EmployeeInfo> employees = new List<EmployeeInfo>();

            if (companyId.HasValue)
            {
                Session["CompanyId"] = companyId.Value;
            }


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
        public ActionResult FinalRecommendation(string regId)
        {
<<<<<<< HEAD
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();
=======
<<<<<<< HEAD
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();
=======
<<<<<<< HEAD
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();
=======
<<<<<<< HEAD
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();
=======
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
            int RegId = int.Parse(Session["RegID"].ToString());
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            int EmpRegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));         
          
<<<<<<< HEAD
            //List<KraKpiOutcomeModel> kraKpiOutcomeData;
=======
<<<<<<< HEAD
            //List<KraKpiOutcomeModel> kraKpiOutcomeData;
=======
<<<<<<< HEAD
            //List<KraKpiOutcomeModel> kraKpiOutcomeData;
=======
<<<<<<< HEAD
            //List<KraKpiOutcomeModel> kraKpiOutcomeData;
=======
            List<KraKpiOutcomeModel> kraKpiOutcomeData;
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
            using (var db = new DB_STEPEntities())
            {
                var authResult = db.Database.SqlQuery<StatusResult>(
                                    "exec prc_CheckAuth @RegId, @SESSION_ID, @Type, @EmpRegId",
                                    new SqlParameter("@RegId", RegId),
                                    new SqlParameter("@SESSION_ID", sessionID),
                                    new SqlParameter("@Type", "AddMarksCHR"),
                                    new SqlParameter("@EmpRegId", EmpRegId)
                                ).FirstOrDefault();

                if (authResult == null || !authResult.Status)
                {
                    ViewBag.AuthorizationMessage = authResult?.Message ?? "Unauthorized access";
<<<<<<< HEAD
                    TempData["Tag"] = "Unauthorized access";
=======
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                    return RedirectToAction("Dashboard", "Home");
                }

                var last2session = (db.New_Tax_Period
                                   .OrderByDescending(t => t.TaxPeriod)
                                   .Select(t => t.TaxPeriod).Take(2).ToList());

                ViewBag.TopTaxPeriods = last2session;
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

                int nextyearSessionID = sessionID + 1;

                var EmployeeInfo = DC.GetEmployeeInfoByRegID(EmpRegId);
                List<KraKpiOutcomeModel> kraKpiOutcomeData = DC.GetKraKpiOutcomeData(EmpRegId, sessionID);
                List<KraKpiOutcomeModel> nextYearkraKpiOutcomeData = DC.GetKraKpiData(EmpRegId, nextyearSessionID);
                var StepMaster = DC.GetStepMaster(EmpRegId, sessionID);
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

                var userInfo = db.Database.SqlQuery<EmployeeInfo>(
                                "prc_EmployeeInfoByRegID @RegID",
                                new SqlParameter("@RegID", EmpRegId)).FirstOrDefault();

                Session["EmployeeCodeInd"] = userInfo.EmployeeCode;
                Session["NameInd"] = userInfo.Name;
                Session["DesignationInd"] = userInfo.Designation;

                kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
               new SqlParameter("@RegId", EmpRegId),
               new SqlParameter("@SESSION_ID", sessionID)).ToList();

                var nextYeargroupedData = nextYearkraKpiOutcomeData.GroupBy(x => x.KRA_ID)
                                       .Select(g => new KraKpiViewModel
                                       {
                                           KRA = g.First().KRA,
                                           KPIIs = g.Where(x => x.KPI != null).Select(x => x.KPI).ToList(),
                                           Durations = g.Where(x => x.Duration != null)
                                                        .Select(x => x.Duration)
                                                        .ToList()
                                       })
                                       .ToList();

                var comments = from step in db.tbl_StepMaster
                               where step.RegId == EmpRegId
                               select new
                               {
                                   step.Supervisor_Comment,
                                   step.User_Comment,
                                   step.HOD_Comment,
                                   step.HOD_Propose_Promotion,
                                   step.HOD_Propose_Incr
                               };

                var comment = comments.FirstOrDefault();

<<<<<<< HEAD
                var userSL = db.Database.SqlQuery<EmployeeInfo>(
                                "prc_GetEmployeeServiceLength @RegID",
                                new SqlParameter("@RegID", EmpRegId)).FirstOrDefault();
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
                var userSL = db.Database.SqlQuery<EmployeeInfo>("prc_GetEmployeeServiceLength @RegID",
                                new SqlParameter("@RegID", RegId)).FirstOrDefault();

>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                Session["ServiceOfLength"] = userSL.Service_Length;
                ViewBag.RegId = EmpRegId;

                var groupedData = kraKpiOutcomeData.GroupBy(x => x.KRA)
                                                .Select(g => new KraKpiViewModel
                                                {
                                                    KRA = g.Key,
                                                    KPIIs = g.Select(x => x.KPI).ToList(),
                                                    KPIOutcomes = g.Select(x => x.KPIOutcome).ToList(),
                                                    AllRemarks = g.Select(x => x.Remarks).ToList()
                                                })
                                                .ToList();

                var designations = db.Database.SqlQuery<DesignationModel>("prc_GetDesignations").ToList();

                var viewModel = new DisplayAllDataViewModel
                {
                    KraKpiOutcomeData = kraKpiOutcomeData,
                    NextYearKraKpiOutcomeData = nextYearkraKpiOutcomeData,
                    EmployeeInfo = EmployeeInfo,
                    GroupedData = groupedData,
                    NextYearGroupedData = nextYeargroupedData,
                    StepMaster = StepMaster,
                    Designations = designations
                };

                if (comments != null && comment != null)
                {
                    viewModel.SupervisorComment = comment.Supervisor_Comment;
                    viewModel.UserComment = comment.User_Comment;
                    viewModel.HOOcomment = comment.HOD_Comment;
                    viewModel.HOOpromotion = comment.HOD_Propose_Promotion;
                    viewModel.HOOincrement = (int)comment.HOD_Propose_Incr;
                }
                else
                {
                    viewModel.SupervisorComment = null;
                    viewModel.UserComment = null;
                    viewModel.HOOcomment = null;
                    viewModel.HOOpromotion = null;
                    viewModel.HOOincrement = 0;
                }

                return View(viewModel);
            }

        }

        [HttpPost]
        public ActionResult SaveHRComment(string HRComment, string promotion, int incrementValue, DateTime effdate)
        {
            int regId = Convert.ToInt32(Request.Form["regId"]);
            int updatedBy = (int)Session["RegID"];
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            string statusType = "HR Comment";
            string updatedComment = HRComment.Replace(",", " ");
            string statusMessage = $"{updatedComment},{incrementValue},{promotion},{effdate}";
            DateTime updatedDate = DateTime.Now;
            effdate = effdate.Date;

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var result = db.Database.SqlQuery<StatusResult>(
                            "exec prc_UpdateStatus @RegId, @SESSION_ID, @StatusType, @StatusValue, @StatusMessage, @Updated_date, @Updated_by",
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
            }
        }

        public ActionResult GetEmployeeReport(int regId, int comID, string DepartmentDropdown, string SectionDropdown, int sessionId)
        {
            using (var db = new DB_STEPEntities())
            {
                var EmpReportData = db.Database.SqlQuery<EmployeeReportModel>(
                    "exec prc_SearchEmployeeByHR @RegID, @CompID, @DepartmentName, @SectionName, @SESSION_ID",
                    new SqlParameter("@RegID", regId),
                    new SqlParameter("@CompID", comID), 
                    new SqlParameter("@DepartmentName", DepartmentDropdown),
                    new SqlParameter("@SectionName", SectionDropdown),
                    new SqlParameter("@SESSION_ID", sessionId)).ToList();


                return Json(EmpReportData, JsonRequestBehavior.AllowGet);
            }
        }

    }
}