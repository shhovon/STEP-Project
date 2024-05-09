using STEP_PORTAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STEP_PORTAL.Helpers;
using System.Web.Security;
using System.Globalization;
using STEP_DEMO.Models;
using System.Data.SqlClient;

namespace STEP_PORTAL.Controllers
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
        public ActionResult SpecialFactors(tblSpecial_Factor model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var regId = (int)Session["RegId"];
                    var sessionId = Session["selectedTaxPeriod"].ToString();

                    using (DB_STEPEntities db = new DB_STEPEntities())
                    {
                        var specialFactor = new tblSpecial_Factor
                        {
                            Reg_Id = regId,
                            Session_Id = int.Parse(sessionId),
                            Description = model.Description
                        };

                        db.tblSpecial_Factor.Add(specialFactor);
                        db.SaveChanges();
                        int? sessionIdInt = int.TryParse(sessionId, out int parsedSessionId) ? parsedSessionId : (int?)null;

                        var taxPeriod = db.New_Tax_Period.Where(t => t.TaxPerID == sessionIdInt).Select(t => t.TaxPeriod).FirstOrDefault();
                        Session["TaxPeriod"] = taxPeriod;

                                                bool success = true;
                        TempData["SuccessMessage"] = success ? "Special Factor saved successfully!" : "";

                        return RedirectToAction("TrainingNeed", "Employee");
                    }

/*                    return Json(new { success = true });*/
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, modelStateErrors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()) });
                }
            }

            return Json(new { success = false, modelStateErrors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()) });
        }

        [HttpGet]
        [CustomAuthorize]
        public ActionResult SpecialFactors()
        {
            try
            {
                var regId = (int)Session["RegId"];
                var sessionId = Session["selectedTaxPeriod"]?.ToString();

/*                if (sessionId != null)*/
                {
                    using (DB_STEPEntities db = new DB_STEPEntities())
                    {
                        int? sessionIdInt = int.TryParse(sessionId, out int parsedSessionId) ? parsedSessionId : (int?)null;
                        int selectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());

                        var addedDescriptions = db.tblSpecial_Factor
                            .Where(sf => sf.Session_Id == sessionIdInt && sf.Reg_Id == regId)
                            .Select(sf => sf.Description)
                            .ToList();

                        var kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                            new SqlParameter("@RegId", regId),
                            new SqlParameter("@SESSION_ID", selectedTaxPeriod)).ToList();

                        if (kraKpiOutcomeData.Count > 0)
                        {
                            Session["ApprovalSent"] = kraKpiOutcomeData[0].ApprovalSent;
                            ViewBag.ApprovalSent = Session["ApprovalSent"];
                        }
                        else
                        {
                            ViewBag.ApprovalSent = false;
                        }

                        var taxPeriod = db.New_Tax_Period.Where(t => t.TaxPerID == sessionIdInt).Select(t => t.TaxPeriod).FirstOrDefault();
                        Session["TaxPeriod"] = taxPeriod;

                        ViewBag.AddedDescriptions = addedDescriptions;
                    }
                }
/*                else
                {
                    ViewBag.ErrorMessage = "Session ID not found.";
                }*/
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error retrieving special factors: " + ex.Message);
            }

            return View(new tblSpecial_Factor());
        }

        [HttpPost]
        [CustomAuthorize]
        public ActionResult TrainingNeed(tblTraining_Need model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var regId = (int)Session["RegId"];
                    var sessionId = Session["selectedTaxPeriod"].ToString();

                    using (DB_STEPEntities db = new DB_STEPEntities())
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

                        int? sessionIdInt = int.TryParse(sessionId, out int parsedSessionId) ? parsedSessionId : (int?)null;
                        var taxPeriod = db.New_Tax_Period.Where(t => t.TaxPerID == sessionIdInt).Select(t => t.TaxPeriod).FirstOrDefault();
                        Session["TaxPeriod"] = taxPeriod;

                        return RedirectToAction("DisplayAllData");
                    }

/*                    return Json(new { success = true });*/
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, modelStateErrors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()) });
                }
            }

            return Json(new { success = false, modelStateErrors = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()) });
        }

        [HttpGet]
        [CustomAuthorize]
        public ActionResult TrainingNeed()
        {
            try
            {
                var regId = (int)Session["RegId"];
                var sessionId = Session["selectedTaxPeriod"]?.ToString();

/*                if (sessionId != null)*/
                {
                    using (DB_STEPEntities db = new DB_STEPEntities())
                    {
                        int? sessionIdInt = int.TryParse(sessionId, out int parsedSessionId) ? parsedSessionId : (int?)null;
                        int selectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());

                        var trainingData = db.tblTraining_Need
                            .Where(tn => tn.Session_Id == sessionIdInt && tn.Reg_Id == regId)
                            .ToList();

                        var kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                            new SqlParameter("@RegId", regId),
                            new SqlParameter("@SESSION_ID", selectedTaxPeriod)).ToList();

                        if (kraKpiOutcomeData.Count > 0)
                        {
                            Session["ApprovalSent"] = kraKpiOutcomeData[0].ApprovalSent;
                            ViewBag.ApprovalSent = Session["ApprovalSent"];
                        }
                        else
                        {
                            ViewBag.ApprovalSent = false;
                        }

                        var taxPeriod = db.New_Tax_Period.Where(t => t.TaxPerID == sessionIdInt).Select(t => t.TaxPeriod).FirstOrDefault();
                        Session["TaxPeriod"] = taxPeriod;

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
        public ActionResult DeleteSession(string description)
        {
            int regId;
            if (!int.TryParse(Session["RegID"].ToString(), out regId))
            {
                return RedirectToAction("DisplayKrasAndKpis", "Home");
            }

            try
            {
                using (DB_STEPEntities db = new DB_STEPEntities())
                {
                    int selectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());
                    var approvalSent = db.prc_GetKraKpiOutcomeData(regId, selectedTaxPeriod)
                                        .Where(data => data.ApprovalSent != null)
                                        .Select(data => data.ApprovalSent)
                                        .Distinct()
                                        .ToList();


                    if (approvalSent.Any(x => x == true))
                    {
                        TempData["ErrorMessage"] = "Could not Delete! You already sent this data for approval.";
                        return RedirectToAction("SpecialFactors");
                    }

                    if (!string.IsNullOrEmpty(description))
                    {
                        var session = db.tblSpecial_Factor.FirstOrDefault(sf => sf.Description == description);
                        if (session != null)
                        {
                            db.tblSpecial_Factor.Remove(session);
                            db.SaveChanges();
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Special factor data not found for deletion.";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Description cannot be empty.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting special factor: " + ex.Message;
            }

            return RedirectToAction("SpecialFactors");
        }


        [HttpPost]
        public ActionResult DeleteTraining(string title, string by_when, string type, string status)
        {
            int regId;
            if (!int.TryParse(Session["RegID"].ToString(), out regId))
            {
                return RedirectToAction("DisplayKrasAndKpis", "Home");
            }

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

                using (DB_STEPEntities db = new DB_STEPEntities())
                {
                    int selectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());
                    var approvalSent = db.prc_GetKraKpiOutcomeData(regId, selectedTaxPeriod).Where(data => data.ApprovalSent != null)
                    .Select(data => data.ApprovalSent)
                    .Distinct()
                    .ToList();

                    if (approvalSent.Any(x => x == true))
                    {
                        TempData["ErrorMessage"] = "Could not Delete! You already sent this data for approval.";
                        return RedirectToAction("TrainingNeed");
                    }

                    var session = db.tblTraining_Need.FirstOrDefault(tn =>
                        tn.Title == (string.IsNullOrEmpty(title) ? tn.Title : title) &&
                        tn.By_When == byWhen &&
                        tn.Train_Type == (string.IsNullOrEmpty(type) ? tn.Train_Type : type) &&
                        tn.Status == (string.IsNullOrEmpty(status) ? tn.Status : status));

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
            int selectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());


            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                ViewBag.ApprovalSent = Session["ApprovalSent"];

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

                var groupedData = kraKpiData.GroupBy(x => x.KRA)
                                .Select(g => new KraKpiViewModel
                                {
                                    KRA = g.Key,
                                    KPIIs = g.Select(x => x.KPI).ToList(),
                                    KPIOutcomes = g.Select(x => x.KPI_OUTCOME).ToList()
                                })
                                .ToList();


                var specialFactors = db.tblSpecial_Factor.Where(m => m.Reg_Id == regId && m.Session_Id == selectedTaxPeriod).ToList();
                var trainingNeed = db.tblTraining_Need.Where(m => m.Reg_Id == regId && m.Session_Id == selectedTaxPeriod).ToList();

                var viewModel = new DisplayAllDataViewModel
                {
                    KraKpiData = kraKpiData,
                    StepData = stepData,
                    SpecialFactors = specialFactors,
                    TrainingNeed = trainingNeed,
                    GroupedData = groupedData
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

            using (DB_STEPEntities db = new DB_STEPEntities())
            {

                /* int selectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());
                                var kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                                    new SqlParameter("@RegId", regId),
                                    new SqlParameter("@SESSION_ID", selectedTaxPeriod)).ToList();

                                if (kraKpiOutcomeData.Count > 0)
                                {
                                    Session["ApprovalSent"] = kraKpiOutcomeData[0].ApprovalSent;
                                    ViewBag.ApprovalSent = Session["ApprovalSent"];
                                }
                                else
                                {
                                    ViewBag.ApprovalSent = false;
                                }*/

                ViewBag.ApprovalSent = Session["ApprovalSent"];

                var existingRecord = db.tbl_StepMaster.FirstOrDefault(record =>
                                     record.SESSION_ID == sessionId && record.RegId == regId && record.ApprovalSent == true);

                if (existingRecord != null)
                {
                    return Json(new { success = false, message = "You already sent this data for approval!" });
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

            return Json(new { success = true, message = "Successfully sent data for approval!" });
        }


    }
}
