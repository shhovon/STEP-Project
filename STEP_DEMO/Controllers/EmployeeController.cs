﻿using STEP_DEMO.Models;
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
                using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
                {
                    db.tblSpecial_Factor.Add(model);
                    db.SaveChanges();
                }
                    
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
                var regID = (int)Session["RegID"];

                model.Reg_Id = regID;


                using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
                {
                    db.tblTraining_Need.Add(model);
                    db.SaveChanges();
                }

                Session["TrainingNeed"] = model;
                return RedirectToAction("DisplayAllData");
            }

            return View(model);
        }

        [CustomAuthorize]
        public ActionResult DisplayAllData()
        {
            ViewBag.ClearSubmissionStatus = true;

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
