/*using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;*/
using iTextSharp.text;
using iTextSharp.text.pdf;
using STEP_PORTAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using iText.Layout.Element;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;
using STEP_DEMO.Models;

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
            int RegId = int.Parse(Session["RegID"].ToString());
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            int EmpRegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));

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
                    TempData["Tag"] = "Unauthorized access";
                    return RedirectToAction("Dashboard", "Home");
                }

                // fetch attendance and discipline
                var record = db.tbl_StepMaster.FirstOrDefault(s => s.SESSION_ID == sessionID && s.RegId == EmpRegId);
                if (record != null)
                {
                    ViewBag.AttendanceValue = record.Attendance;
                    ViewBag.DisciplineValue = record.Discipline;
                }
                else
                {
                    ViewBag.AttendanceValue = null;
                    ViewBag.DisciplineValue = null;
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
                      new SqlParameter("@RegID", EmpRegId)).FirstOrDefault();

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


            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();

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

                    var status = DC.UpdateRating(regId, sessionID);


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
            TempData["AttendanceValue"] = attendance;
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
            TempData["DisciplineValue"] = discipline;
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

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

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
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();
            int RegId = int.Parse(Session["RegID"].ToString());
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            int EmpRegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));

            //List<KraKpiOutcomeModel> kraKpiOutcomeData;
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
                    TempData["Tag"] = "Unauthorized access";
                    return RedirectToAction("Dashboard", "Home");
                }

                var last2session = (db.New_Tax_Period
                                   .OrderByDescending(t => t.TaxPeriod)
                                   .Select(t => t.TaxPeriod).Take(2).ToList());

                ViewBag.TopTaxPeriods = last2session;

                int nextyearSessionID = sessionID + 1;

                var EmployeeInfo = DC.GetEmployeeInfoByRegID(EmpRegId);
                List<KraKpiOutcomeModel> kraKpiOutcomeData = DC.GetKraKpiOutcomeData(EmpRegId, sessionID);
                List<KraKpiOutcomeModel> nextYearkraKpiOutcomeData = DC.GetKraKpiData(EmpRegId, nextyearSessionID);
                var StepMaster = DC.GetStepMaster(EmpRegId, sessionID);

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

                var specialFactors = db.tblSpecial_Factor.Where(m => m.Reg_Id == EmpRegId && m.Session_Id == sessionID).ToList();
                var trainingNeed = db.tblTraining_Need.Where(m => m.Reg_Id == EmpRegId && m.Session_Id == sessionID).ToList();

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

                var userSL = db.Database.SqlQuery<EmployeeInfo>(
                                "prc_GetEmployeeServiceLength @RegID",
                                new SqlParameter("@RegID", EmpRegId)).FirstOrDefault();
                Session["ServiceOfLength"] = userSL.Service_Length;
                ViewBag.RegId = EmpRegId;

                var groupedData = kraKpiOutcomeData.GroupBy(x => x.KRA_ID)
                                    .Select(g => new KraKpiViewModel
                                    {
                                        KRA_ID = g.Key,
                                        KRA = g.First().KRA,
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
                    SpecialFactors = specialFactors,
                    TrainingNeed = trainingNeed,
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

        [CustomAuthorize]
        public ActionResult StepReportPdf(string regId)
        {
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();
            int RegId = int.Parse(Session["RegID"].ToString());
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            int EmpRegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));

            using (var db = new DB_STEPEntities())
            {
                var EmployeeInfo = DC.GetEmployeeInfoByRegID(EmpRegId);
                var userInfo = db.Database.SqlQuery<EmployeeInfo>(
                                "prc_EmployeeInfoByRegID @RegID",
                                new SqlParameter("@RegID", EmpRegId)).FirstOrDefault();
                var userSL = db.Database.SqlQuery<EmployeeInfo>(
                                "prc_GetEmployeeServiceLength @RegID",
                                new SqlParameter("@RegID", EmpRegId)).FirstOrDefault();
                ViewBag.RegId = EmpRegId;
                var designations = db.Database.SqlQuery<DesignationModel>("prc_GetDesignations").ToList();

                List<KraKpiOutcomeModel> kraKpiOutcomeData = DC.GetKraKpiOutcomeData(EmpRegId, sessionID);
                kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                              new SqlParameter("@RegId", EmpRegId),
                              new SqlParameter("@SESSION_ID", sessionID)).ToList();

                var groupedData = kraKpiOutcomeData.GroupBy(x => x.KRA_ID)
                                    .Select(g => new KraKpiViewModel
                                    {
                                        KRA_ID = g.Key,
                                        KRA = g.First().KRA,
                                        KPIIs = g.Select(x => x.KPI).ToList(),
                                        KPIOutcomes = g.Select(x => x.Outcome).ToList(),
                                        Marks = g.Select(x => x.Marks_Achieved).ToList(),
                                        AVG_Marks_List = g.Select(x => x.AVG_Marks_Achieved).ToList(),
                                        KPI_AVG_List = g.Select(x => x.KPI_AVG).ToList(),
                                        Attendance = g.Select(x => x.Attendance).ToList(),
                                        Discipline = g.Select(x => x.Discipline).ToList()
                                    })
                                    .ToList();

                string htmlContent = GenerateHtmlContent(userInfo, userSL, groupedData);

                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    /* document.Add(new iTextSharp.text.Paragraph("SUCCESS THROUGH EFFECTIVE PERFORMANCE (STEP)"));
                       document.Add(new iTextSharp.text.Paragraph("ID NUMBER: " + userInfo.EmployeeCode));
                       document.Add(new iTextSharp.text.Paragraph("NAME: " + userInfo.Name));
                       document.Add(new iTextSharp.text.Paragraph("JOB TITLE: " + userInfo.Designation));
                       document.Add(new iTextSharp.text.Paragraph("GRADE: NOT APPLICABLE"));
                       document.Add(new iTextSharp.text.Paragraph("DEPARTMENT: " + userInfo.Department));
                       document.Add(new iTextSharp.text.Paragraph("SUB - DEPARTMENT: " + userInfo.Section));
                       document.Add(new iTextSharp.text.Paragraph("UNIT: _________"));
                       document.Add(new iTextSharp.text.Paragraph("COST CENTRE: NOT APPLICABLE"));
                       document.Add(new iTextSharp.text.Paragraph("DATE OF JOINING: " + userInfo.JoiningDate.ToString("dd/MM/yyyy")));
                       document.Add(new iTextSharp.text.Paragraph("DATE OF CONFIRM.: " + userInfo.ConfirmDate.ToString("dd/MM/yyyy")));
                       document.Add(new iTextSharp.text.Paragraph("LENGTH OF SERVICE: " + userSL.Service_Length));
                       document.Add(new iTextSharp.text.Paragraph("REVIEW DATE FROM: _________ TO: _________"));*/

                    using (StringReader sr = new StringReader(htmlContent))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                    }

                    document.Close();
                    writer.Close();

                    byte[] bytes = ms.ToArray();
                    ms.Close();

                    string fileName = $"StepReport_{userInfo.EmployeeCode}.pdf";
                    return File(bytes, "application/pdf", fileName);
                }
            }
        }

        private string GenerateHtmlContent(EmployeeInfo userInfo, EmployeeInfo userSL, List<KraKpiViewModel> groupedData)
        {
            string htmlContent = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'></meta>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'></meta>
            <title>SUCCESS THROUGH EFFECTIVE PERFORMANCE (STEP)</title>
 <style>
    body {{
        font - family: Arial, sans-serif;
        line-height: 1.6;
        color: #333;
        max-width: 1000px;
        margin: 0 auto;
        padding: 20px;
    }}
    .header {{
        text - align: center;
        margin-bottom: 20px;
    }}
    .step {{
        font - size: 24px;
        font-weight: bold;
    }}
    .confidential {{
        color: red;
        font-weight: bold;
        font-size: 18px;
    }}
    h1 {{
        font - size: 20px;
        text-align: center;
        margin-bottom: 30px;
    }}
    h2, h3 {{
        font - size: 16px;
        margin-top: 30px;
        margin-bottom: 15px;
    }}
    table {{
        width: 100%;
        border-collapse: collapse;
        margin-bottom: 20px;
    }}
    th, td {{
        border: 1px solid #000;
        padding: 8px;
        text-align: left;
        font-size: 14px;
    }}
    th {{
        background - color: #f2f2f2;
    }}
    .container {{
        display: flex;
        justify-content: space-between;
    }}
    .personal-info, .hr-info {{
        width: 48%;
    }}
    .personal-info p {{
        margin: 5px 0;
        font-size: 14px;
    }}
    .assessment, .recommendation {{
        margin - top: 30px;
    }}
    .assessment h3, .recommendation h3 {{
        font - size: 16px;
        margin-bottom: 10px;
    }}
    .assessment p, .recommendation p {{
        margin: 5px 0;
        font-size: 14px;
    }}
    .kra-table {{
        border - collapse: collapse;
        width: 100%;
    }}
    .kra-table th, .kra-table td {{
        border: 1px solid black;
        padding: 8px;
        text-align: left;
        vertical-align: top;
    }}
    .kra-table th {{
        background - color: #f2f2f2;
    }}
    .kra-table td:first-child {{
        text - align: center;
    }}
    .kra-table td:nth-child(3) {{
        width: 20%;
    }}
    .kra-table td:nth-child(4) {{
        width: 15%;
    }}
    .kra-table td:nth-child(5) {{
        width: 25%;
    }}
    .kra-table td:last-child {{
        width: 10%;
    }}
</style>

        </head>
        <body>
            <div class='header'>
                <div style='float:right;' class='step'>STEP</div>
                <div style='float:left;' class='confidential'>Confidential</div>
                <h3>SUCCESS THROUGH EFFECTIVE PERFORMANCE (STEP)</h3>
            </div>

           <div class='container'>
            <div class='personal-info'>
                <p>ID NUMBER: {userInfo.EmployeeCode}</p>
                <p>NAME: {userInfo.Name}</p>
                <p>JOB TITLE: {userInfo.Designation}</p>
                <p>GRADE: NOT APPLICABLE</p>
                <p>DEPARTMENT: {userInfo.Department}</p>
                <p>SUB - DEPARTMENT: {userInfo.Section}</p>
                <p>UNIT: _________</p>
                <p>COST CENTRE: NOT APPLICABLE</p>
                <p>DATE OF JOINING: {userInfo.JoiningDate.ToString("dd/MM/yyyy")}</p>
                <p>DATE OF CONFIRM: {userInfo.ConfirmDate.ToString("dd/MM/yyyy")}</p>
                <p>LENGTH OF SERVICE: {userSL.Service_Length}</p>
                <p>REVIEW DATE FROM: _________ TO: _________</p>
            </div>

            <div class='hr-info'>
            <div class='assessment'>
                <h3>ASSESSMENT OF LAST YEAR (To be completed by HR)</h3>
                <p>INCREMENTS: _________</p>
                <p>PROMOTION: _________</p>
                <p>PRESENT SALARY: _________</p>
            </div>

            <div class='recommendation'>
                <h3>RECOMMENDATION FOR CURRENT YEAR (To be completed by HR after final assessment)</h3>
                <p>INCREMENTS: _________</p>
                <p>PROMOTION TO GRADE: _________</p>
                <p>PROPOSED SALARY: _________</p>
            </div>
           </div>
          </div>

            <h2>1. PERFORMANCE OBJECTIVES REVIEW OF THE YEAR 2023-2024</h2>
            <h3>1.1 ACHIEVEMENT AGAINST AGREED OBJECTIVES</h3>

            <table class='kra-table'>
                <thead>
                    <tr>
                        <th>SL</th>
                        <th>KEY RESULT AREAS (KRA) (Task and Personal Objectives)</th>
                        <th>KEY PERFORMANCE INDICATORS (KPI) (Determine how KRA's to be measured)</th>
                        <th>KPI OUTCOME</th>
                        <th>PERFORMANCE ACHIEVEMENTS (Achievements against agreed KRA/KPIs)</th>
                        <th>Total Achieved</th>
                    </tr>
                </thead>
                <tbody>";

            int counter = 1;
            foreach (var group in groupedData)
            {
                int kpiCount = group.KPIIs.Count;
                for (int i = 0; i < kpiCount; i++)
                {
                    var kpi = group.KPIIs[i];
                    var outcome = group.KPIOutcomes[i];
                    var Marks = group.Marks[i];
                    var AVG_Marks_List = group.AVG_Marks_List[i];
                    var Total_Kpi_Avg = group.KPI_AVG_List[i];

                    htmlContent += $@"
                    <tr>
                        {(i == 0 ? $"<td rowspan='{kpiCount}'>{counter++}</td><td rowspan='{kpiCount}'>{group.KRA}</td>" : string.Empty)}
                        <td>{kpi}</td>
                        <td>{outcome}</td>
                        <td style='text-align:center'>{Marks}</td>
                        {(i == 0 ? $"<td rowspan='{kpiCount}' style='text-align:center'>{AVG_Marks_List}</td>" : string.Empty)}
                    </tr>";
                }
            }


            htmlContent += $@"
                </tbody>
            </table>
            <p>Average Cumulative Score of KRA's (KRA1+KRA2+KRA3+KRA4+KRA5): 0 </p>

            <h3>1.2. Performance Assessment (Rate relevant rating)</h3>
                <table>
                    <thead>
                        <tr>
                            <th>Attendance</th>
                            <th>Discipline</th>
                        </tr>
                    </thead>
                    <tbody>";


            foreach (var group in groupedData)
            {
                var Attendance = group.Attendance[0];
                var Discipline = group.Discipline[0];

                    htmlContent += $@"
                    <tr>                 
                        <td>{Attendance}</td>
                        <td>{Discipline}</td>    
                    </tr>";
            }

            htmlContent += $@"
                        </tbody>
                    </table>
                    </body>
                    </html>";

            return htmlContent;
        }
}
}  