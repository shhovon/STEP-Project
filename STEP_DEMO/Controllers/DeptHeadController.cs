using STEP_DEMO.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STEP_DEMO.Controllers
{
    public class DeptHeadController : Controller
    {
        // GET: DeptHead
        public ActionResult Index()
        {
            return View();
        }
        public List<EmployeeViewModel> GetEmployeeListByDeptHead(int deptHeadValue)
        {
            using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
            {
                var employees = db.Database.SqlQuery<EmployeeViewModel>("exec prc_GetEmployeeListByDeptHead @DeptHeadValue", new SqlParameter("@DeptHeadValue", deptHeadValue)).ToList();
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
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();

            int deptHeadValue;
            if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
            {
                employees = GetEmployeeListByDeptHead(deptHeadValue);
            }

            List<CompanyViewModel> companies = GetCompanies();
            ViewBag.Companies = new SelectList(companies, "ID", "Name");

            return View(employees);
        }

        [HttpPost]
        public ActionResult ViewEmpList(int? companyId)
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();

            if (companyId != null)
            {
                employees = GetEmployeesByCompany(companyId.Value);
            }
            else
            {
                ViewBag.Message = "Select an unit";
            }

            List<CompanyViewModel> companies = GetCompanies();
            ViewBag.Companies = new SelectList(companies, "ID", "Name", companyId);

            return View(employees);
        }


        private List<EmployeeViewModel> GetEmployeesByCompany(int companyId)
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();

            using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
            {
                employees = db.Database.SqlQuery<EmployeeViewModel>("prc_GetEmployeesByCompany @CompanyId",
                    new SqlParameter("CompanyId", companyId)
                ).ToList();
            }

            return employees;
        }

    }
}