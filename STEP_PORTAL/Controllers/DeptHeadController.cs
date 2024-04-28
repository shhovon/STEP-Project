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
            using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
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
            using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
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

            return View(employees);
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

            return View(employees);
        }
    }
}