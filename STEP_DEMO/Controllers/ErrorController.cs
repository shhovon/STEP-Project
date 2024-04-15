using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STEP_DEMO.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            Response.StatusCode = 503;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }
    }
}