using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STEP_DEMO.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee

        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult SpecialFactors()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult TrainingNeed()
        {
            return View();
        }
    }
}