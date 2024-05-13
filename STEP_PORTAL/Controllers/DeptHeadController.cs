using STEP_DEMO.Models;
using STEP_PORTAL.Models;
using STEP_PORTAL.Models;
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
        public ActionResult ViewEmpList()
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
        public ActionResult ViewEmpList(int? companyId)
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

    }
}