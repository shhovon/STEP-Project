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

        /* public ActionResult ViewEmpList()
         {
             List<EmployeeViewModel> employees = new List<EmployeeViewModel>();

             using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
             {
                 int deptHeadValue;
                 if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
                 {
                     employees = (from empInfo in db.Employee_Information
                                  join reg in db.tblUser_Registration on empInfo.RegId equals reg.RegId
                                  where reg.DeptHead == deptHeadValue || reg.ReportSuper == deptHeadValue
                                  select new EmployeeViewModel
                                  {
                                      Name = empInfo.Name,
                                      EmployeeCode = empInfo.EmployeeCode,
                                      Department = empInfo.Department,
                                      Section = empInfo.Section,
                                      Designation = empInfo.Designation
                                  }).ToList();
                 }
             }

             return View(employees);
         }
 */
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