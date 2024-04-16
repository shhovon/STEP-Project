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

        [CustomAuthorize]
        public ActionResult ViewEmpList()
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();

            int deptHeadValue;
            if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
            {
                employees = GetEmployeeListByDeptHead(deptHeadValue);
            }

            return View(employees);
        }




    }
}