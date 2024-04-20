using STEP_DEMO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STEP_DEMO.Helpers;
using System.Web.Security;


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public ActionResult SpecialFactors(tblSpecial_Factor model)
        {
            if (ModelState.IsValid)
            {
                Session["SpecialFactors"] = model;
                return RedirectToAction("TrainingNeed");
            }

            return View(model);
        }


        [CustomAuthorize]
        public ActionResult TrainingNeed()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public ActionResult TrainingNeed(tblTraining_Need model)
        {
            if (ModelState.IsValid)
            {
                Session["TrainingNeed"] = model;
                return RedirectToAction("DisplayAllData");
            }

            return View(model);
        }

        [CustomAuthorize]
        public ActionResult DisplayAllData()
        {
            var kraKpiOutcomes = Session["UserAddedData"] as List<KraKpiOutcomeModel>;
            var specialFactors = Session["SpecialFactors"] as tblSpecial_Factor;
            var trainingNeed = Session["TrainingNeed"] as tblTraining_Need;

            var viewModel = new DisplayAllDataViewModel
            {
                KraKpiOutcomes = kraKpiOutcomes,
                SpecialFactors = specialFactors,
                TrainingNeed = trainingNeed
            };

            return View(viewModel);
        }



    }
}
