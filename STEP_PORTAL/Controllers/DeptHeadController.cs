using STEP_PORTAL.Models;
using STEP_PORTAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace STEP_PORTAL.Controllers
{
    public class DeptHeadController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public List<EmployeeInfo> GetEmployeeListByDeptHead(int deptHeadValue, int companyId)
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {

                var SelectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());


                var employees = db.Database.SqlQuery<EmployeeInfo>("exec prc_GetTeamMember @CompID,@RegID,@SESSION_ID ",
                      new SqlParameter("@CompID", companyId),
                      new SqlParameter("@RegID", deptHeadValue),
                     new SqlParameter("@SESSION_ID", SelectedTaxPeriod)
                    ).ToList();

                return employees;
            }
        }

        private List<CompanyViewModel> GetCompanies()
        {
            List<CompanyViewModel> companies = new List<CompanyViewModel>();
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                companies = db.Company_Information
                              .OrderBy(c => c.Name)
                              .Select(c => new CompanyViewModel
                              {
                                  ID = c.ID,
                                  Name = c.Name
                              })
                              .ToList();

            }

            return companies;
        }

        [CustomAuthorize]
        public ActionResult ViewEmpListRS()
        {
            List<EmployeeInfo> employees = new List<EmployeeInfo>();
            int deptHeadValue;
            int companyId = 0;
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());


            if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
            {
                employees = GetEmployeeListByDeptHead(deptHeadValue, companyId);
            }

            List<CompanyViewModel> companies = GetCompanies();
            ViewBag.Companies = new SelectList(companies, "ID", "Name");

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

            var model = new EmployeeSessionViewModelClass
            {
                Employees = employees              
            };

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(model);
        }

        [HttpPost]
        public ActionResult ViewEmpListRS(int? companyId)
        {
            List<EmployeeInfo> employees = new List<EmployeeInfo>();
            int deptHeadValue;

            if (companyId != null && Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
            {
                employees = GetEmployeeListByDeptHead(deptHeadValue, companyId.Value);
            }
            else
            {
                ViewBag.Message = "Select an unit";
            }

            List<CompanyViewModel> companies = GetCompanies();
            ViewBag.Companies = new SelectList(companies, "ID", "Name", companyId);

            List<string> topTaxPeriods = new List<string>();
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var last2session = (db.New_Tax_Period
                             .OrderByDescending(t => t.TaxPeriod)
                             .Take(2).ToList());

                ViewBag.TopTaxPeriods = last2session;
                var SelectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());
                ViewBag.SelectedTaxPeriod = SelectedTaxPeriod;
            }

            var model = new EmployeeSessionViewModelClass
            {
                Employees = employees,
                TopTaxPeriods = topTaxPeriods
            };

            return View(model);

        }

        public List<EmployeeInfo> GetEmployeeListByHOD(int deptHeadValue, int companyId)
        {
            var SelectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());
            using (DB_STEPEntities db = new DB_STEPEntities())
            {

                var employees = db.Database.SqlQuery<EmployeeInfo>("exec prc_GetHODTeamMember @RegID,@CompID,@SESSION_ID ",
                      new SqlParameter("@RegID", deptHeadValue),
                      new SqlParameter("@CompID", companyId),
                     new SqlParameter("@SESSION_ID", SelectedTaxPeriod)
                    ).ToList();

                return employees;
            }
        }

        [CustomAuthorize]
        public ActionResult ViewEmpListHOD()
        {
            List<EmployeeInfo> employees = new List<EmployeeInfo>();
            int deptHeadValue;
            int companyId = 0;

            if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
            {
                employees = GetEmployeeListByHOD(deptHeadValue, companyId);
            }

            List<CompanyViewModel> companies = GetCompanies();
            ViewBag.Companies = new SelectList(companies, "ID", "Name");

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

            var model = new EmployeeSessionViewModelClass
            {
                Employees = employees,
                TopTaxPeriods = topTaxPeriods
            };

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(model);
        }

        [HttpPost]
        public ActionResult ViewEmpListHOD(int? companyId)
        {
            List<EmployeeInfo> employees = new List<EmployeeInfo>();
            int deptHeadValue;

            if (companyId != null && Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
            {
                employees = GetEmployeeListByHOD(deptHeadValue, companyId.Value);
            }
            else
            {
                ViewBag.Message = "Select an unit";
            }

            List<CompanyViewModel> companies = GetCompanies();
            ViewBag.Companies = new SelectList(companies, "ID", "Name", companyId);

            List<string> topTaxPeriods = new List<string>();
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var last2session = (db.New_Tax_Period
                             .OrderByDescending(t => t.TaxPeriod)
                             .Take(2).ToList());

                ViewBag.TopTaxPeriods = last2session;

                var SelectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());

                ViewBag.SelectedTaxPeriod = SelectedTaxPeriod;

            }

            var model = new EmployeeSessionViewModelClass
            {
                Employees = employees,
                TopTaxPeriods = topTaxPeriods
            };

            return View(model);

        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult ViewEmpMarks(string regId)
        {
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();

            int EmpRegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            int deptHeadValue = int.Parse(Session["RegID"].ToString());
          

<<<<<<< HEAD
            using (var db = new DB_STEPEntities())
            {
                var authResult = DC.CheckAuth(deptHeadValue, sessionID, "AddMarksHOD", EmpRegId);

                if (authResult == null || !authResult.Status)
                {
                    ViewBag.AuthorizationMessage = authResult?.Message ?? "Unauthorized access";
                    return RedirectToAction("Dashboard", "Home");
                }
              

                ViewBag.TopTaxPeriods = DC.GetTax_Period();
                ViewBag.RegId = EmpRegId;
                int nextyearSessionID = sessionID + 1;

=======
            using (var db = new DB_STEPEntities())
            {
                var authResult = DC.CheckAuth(deptHeadValue, sessionID, "AddMarksHOD", EmpRegId);

                if (authResult == null || !authResult.Status)
                {
                    ViewBag.AuthorizationMessage = authResult?.Message ?? "Unauthorized access";
                    return RedirectToAction("Dashboard", "Home");
                }
              
=======
            int RegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            int deptHeadValue = int.Parse(Session["RegID"].ToString());

            List<KraKpiOutcomeModel> kraKpiOutcomeData;
            using (var db = new DB_STEPEntities())
            {
                var authResult = db.Database.SqlQuery<StatusResult>(
                        "exec prc_CheckAuth @RegId, @SESSION_ID, @Type, @EmpRegId",
                        new SqlParameter("@RegId", deptHeadValue),
                        new SqlParameter("@SESSION_ID", sessionID),
                        new SqlParameter("@Type", "AddMarksHOD"),
                        new SqlParameter("@EmpRegId", RegId)
                    ).FirstOrDefault();

                if (authResult == null || !authResult.Status)
                {
                    ViewBag.AuthorizationMessage = authResult?.Message ?? "Unauthorized access";
                    return RedirectToAction("Dashboard", "Home");
                }

                var last2session = (db.New_Tax_Period
                                   .OrderByDescending(t => t.TaxPeriod)
                                   .Select(t => t.TaxPeriod).Take(2).ToList());

                ViewBag.TopTaxPeriods = last2session;

>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065

                ViewBag.TopTaxPeriods = DC.GetTax_Period();
                ViewBag.RegId = EmpRegId;
                int nextyearSessionID = sessionID + 1;

>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
                var EmployeeInfo = DC.GetEmployeeInfoByRegID(EmpRegId);
                List<KraKpiOutcomeModel> kraKpiOutcomeData = DC.GetKraKpiOutcomeData(EmpRegId, sessionID);
                List<KraKpiOutcomeModel> nextYearkraKpiOutcomeData = DC.GetKraKpiData(EmpRegId, nextyearSessionID);
                var StepMaster = DC.GetStepMaster(EmpRegId, sessionID);
                var designations =  DC.GetDesignations();

                var groupedData = kraKpiOutcomeData.GroupBy(x => x.KRA)
                                                .Select(g => new KraKpiViewModel
                                                {
                                                    KRA = g.Key,
                                                    KPIIs = g.Select(x => x.KPI).ToList(),
                                                    KPIOutcomes = g.Select(x => x.KPIOutcome).ToList(),
                                                    AllRemarks = g.Select(x=>x.Remarks).ToList()
                                                })
                                                .ToList();

/*                var nextYeargroupedData = nextYearkraKpiOutcomeData
                    .Where(x => x != null && x.KRA_ID != null)
                    .GroupBy(x => x.KRA_ID)
                    .Select(g => new KraKpiViewModel
                    {
                        KRA = g.First().KRA,
                        KPIIs = g.Where(x => x.KPI != null).Select(x => x.KPI).ToList(), 
                        Durations = g.Where(x => x.Durations != null)
                                                     .SelectMany(x => x.Durations)
                                                     .Where(d => d != null) 
                                                     .ToList()
                                    })
                                    .ToList();*/

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




                var viewModel = new DisplayAllDataViewModel
                {
                    EmployeeInfo = EmployeeInfo,
                    KraKpiOutcomeData = kraKpiOutcomeData,
                    NextYearKraKpiOutcomeData = nextYearkraKpiOutcomeData,
                    GroupedData = groupedData,
                    NextYearGroupedData = nextYeargroupedData,
                    StepMaster = StepMaster,
                    Designations = designations
                };

                return View(viewModel);
            }

        }

        [HttpPost]
        public ActionResult SaveHODComment(string comment, string promotion, int incrementValue)
        {
            int regId = Convert.ToInt32(Request.Form["regId"]);
            int updatedBy = (int)Session["RegID"];
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            string statusType = "HOD Comment";
            string updatedComment = comment.Replace(",", " ");
            string statusMessage = $"{updatedComment},{promotion},{incrementValue}";
            DateTime updatedDate = DateTime.Now;

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

    }
}