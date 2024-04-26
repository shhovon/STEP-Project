﻿using STEP_DEMO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STEP_DEMO.Helpers;
using System.Web.Security;
using System.Globalization;

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
                var sessionId = Session["selectedTaxPeriod"]?.ToString();

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
        public ActionResult DeleteTraining(string title, string by_when, string type, string status)
        {
            try
            {
                DateTime? byWhen = null;
                if (!string.IsNullOrEmpty(by_when))
                {
                    if (DateTime.TryParse(by_when, out DateTime parsedDate))
                    {
                        byWhen = parsedDate;
                    }
                }


                using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
                {
                    var session = db.tblTraining_Need.FirstOrDefault(tn =>
                        tn.Title == title && tn.By_When == byWhen && tn.Train_Type == type && tn.Status == status);

                    if (session != null)
                    {
                        db.tblTraining_Need.Remove(session);
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Training data not found for deletion.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting training data: " + ex.Message;
            }

            return RedirectToAction("TrainingNeed");
        }



        [CustomAuthorize]
        public ActionResult DisplayAllData()
        {
            var regId = (int)Session["RegId"];

            using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
            {
                var kraKpiData = (from kra in db.KRAs
                                  join kpi in db.KPIs on kra.KRA_ID equals kpi.KRA_ID
                                  where kra.RegId == regId && !string.IsNullOrEmpty(kra.KRA1) && !string.IsNullOrEmpty(kpi.KPI1)
                                  orderby kra.KRA_ID ascending, kpi.KPI_ID ascending
                                  select new KraKpiOutcomeModel
                                  {
                                      KRA_ID = kra.KRA_ID,
                                      KPI_ID = kpi.KPI_ID,
                                      KRA = kra.KRA1,
                                      KPI = kpi.KPI1
                                  }).ToList();

                var stepData = (from s in db.STEPs
                                join k in db.KRAs on s.KRA_ID equals k.KRA_ID
                                join kp in db.KPIs on s.KPI_ID equals kp.KPI_ID
                                where s.REG_ID == regId
                                select new KraKpiViewModel
                                {
                                    KRA = k.KRA1,
                                    KPI = kp.KPI1,
                                    KPI_OUTCOME = s.KPI_OUTCOME,
                                    KRA_ID = k.KRA_ID,
                                    KPI_ID = kp.KPI_ID
                                }).ToList();


                var specialFactors = db.tblSpecial_Factor.FirstOrDefault(m => m.Reg_Id == regId);
                var trainingNeed = db.tblTraining_Need.FirstOrDefault(m => m.Reg_Id == regId);

                var viewModel = new DisplayAllDataViewModel
                {
                    KraKpiData = kraKpiData,
                    StepData = stepData,
                    SpecialFactors = specialFactors,
                    TrainingNeed = trainingNeed
                };

                return View(viewModel);
            }
        }

        [CustomAuthorize]
        [HttpPost] 
        public ActionResult SubmitForApproval()
        {
            var sessionId = (int)Session["selectedTaxPeriod"];
            var regId = (int)Session["RegId"];

            using (EMP_EVALUATIONEntities db = new EMP_EVALUATIONEntities())
            {

                var existingRecord = db.tbl_StepMaster.FirstOrDefault(record =>
                                     record.SESSION_ID == sessionId && record.RegId == regId && record.ApprovalSent == true);

                if (existingRecord != null)
                {
                    return Json(new { success = false, message = "Data already sent for approval!" });
                }

                var newRecord = new tbl_StepMaster
                {
                    SESSION_ID = sessionId,
                    RegId = regId,
                    ApprovalSent = true
                };

                db.tbl_StepMaster.Add(newRecord);
                db.SaveChanges();

            }

            return Json(new { success = true, message = "Data sent for approval successfully!" });
        }


    }
}
