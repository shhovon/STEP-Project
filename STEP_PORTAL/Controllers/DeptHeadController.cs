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
                var employees = db.Database.SqlQuery<EmployeeInfo>("exec prc_GetTeamMember @RegID, @CompID",
                    new SqlParameter("@RegID", deptHeadValue),
                    new SqlParameter("@CompID", companyId)).ToList();

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
                topTaxPeriods = db.New_Tax_Period
                                .OrderByDescending(t => t.TaxPeriod)
                                .Select(t => t.TaxPeriod)
                                .Take(2)
                                .ToList();
            }

            var model = new EmployeeSessionViewModelClass
            {
                Employees = employees,
                TopTaxPeriods = topTaxPeriods
            };

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
                topTaxPeriods = db.New_Tax_Period
                                .OrderByDescending(t => t.TaxPeriod)
                                .Select(t => t.TaxPeriod)
                                .Take(2)
                                .ToList();

                string selectedTaxPeriod = Request.Form["selectedTaxPeriod"];
                int? sessionID = db.New_Tax_Period.Where(t => t.TaxPeriod == selectedTaxPeriod).Select(t => t.TaxPerID).FirstOrDefault();
                Session["SelectedTaxPeriod"] = sessionID;
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