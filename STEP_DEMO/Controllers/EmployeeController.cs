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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SpecialFactors(tblSpecial_Factor model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var regId = (int)Session["RegId"];
                    var sessionId = Session["selectedTaxPeriod"].ToString();

                    using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
                    {
                        var specialFactor = new tblSpecial_Factor
                        {
                            Reg_Id = regId,
                            Session_Id = int.Parse(sessionId),
                            Description = model.Description
                        };

                        db.tblSpecial_Factor.Add(specialFactor);
                        db.SaveChanges();
                    }

                    return RedirectToAction("SpecialFactors");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error adding special factor: " + ex.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        [CustomAuthorize]
        public ActionResult SpecialFactors()
        {
            try
            {
                var regId = (int)Session["RegId"];
                var sessionId = Session["selectedTaxPeriod"]?.ToString(); // Using null propagation operator

                if (sessionId != null)
                {
                    using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
                    {
                        int? sessionIdInt = int.TryParse(sessionId, out int parsedSessionId) ? parsedSessionId : (int?)null;

                        var addedDescriptions = db.tblSpecial_Factor
                            .Where(sf => sf.Session_Id == sessionIdInt)
                            .Select(sf => sf.Description)
                            .ToList();

                        ViewBag.AddedDescriptions = addedDescriptions;
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error retrieving special factors: " + ex.Message);
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public ActionResult TrainingNeed(tblTraining_Need model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var regId = (int)Session["RegId"];
                    var sessionId = Session["selectedTaxPeriod"].ToString();

                    using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
                    {
                        var training = new tblTraining_Need
                        {
                            Reg_Id = regId,
                            Session_Id = int.Parse(sessionId),
                            Title = model.Title,
                            By_When = model.By_When,
                            Train_Type = model.Train_Type,
                            Status = model.Status
                        };

                        db.tblTraining_Need.Add(training);
                        db.SaveChanges();
                    }

                    return RedirectToAction("TrainingNeed");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error adding training data: " + ex.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        [CustomAuthorize]
        public ActionResult TrainingNeed()
        {
            try
            {
                var regId = (int)Session["RegId"];
                var sessionId = Session["selectedTaxPeriod"]?.ToString();

                if (sessionId != null)
                {
                    using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
                    {
                        int? sessionIdInt = int.TryParse(sessionId, out int parsedSessionId) ? parsedSessionId : (int?)null;

                        /*                        var trainingData = db.tblTraining_Need
                                                    .Where(tn => tn.Session_Id == sessionIdInt)
                                                    .Select(tn => new {
                                                        Title = tn.Title,
                                                        By_When = tn.By_When,
                                                        Train_Type = tn.Train_Type,
                                                        Status = tn.Status
                                                    })
                                                    .ToList();*/
                        var trainingData = db.tblTraining_Need
                            .Where(tn => tn.Session_Id == sessionIdInt)
                            .ToList();


                        Session["TrainingData"] = trainingData;
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error retrieving training data: " + ex.Message);
            }

            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSession(string description)
        {
            try
            {
                using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
                {
                    var session = db.tblSpecial_Factor.FirstOrDefault(sf => sf.Description == description);

                    db.tblSpecial_Factor.Remove(session);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting session: " + ex.Message;
            }

            return RedirectToAction("SpecialFactors");
        }  
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTraining(string title, DateTime? by_when, string type, string status)
        {
            try
            {
                using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
                {
                    var session = db.tblTraining_Need.FirstOrDefault(tn =>
                                          tn.Title == title && tn.By_When == by_when && tn.Train_Type == type && tn.Status == status);

                    db.tblTraining_Need.Remove(session);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting training data: " + ex.Message;
            }

            return RedirectToAction("TrainingNeed");
        }

    }
}
