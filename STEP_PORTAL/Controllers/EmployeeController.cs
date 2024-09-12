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
using System.Data.SqlClient;
using STEP_PORTAL.Models;

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
                    int sessionId = int.Parse(Session["SelectedTaxPeriod"].ToString());

                    using (DB_STEPEntities db = new DB_STEPEntities())
                    {
                        var specialFactor = new tblSpecial_Factor
                        {
                            Reg_Id = regId,
                            Session_Id = sessionId,
                            Description = model.Description,
                            Created_date = DateTime.Now,
                            Created_By = regId.ToString(),
                            Updated_by = regId.ToString(),
                            Updated_date = DateTime.Now
                        };

                        db.tblSpecial_Factor.Add(specialFactor);
                        db.SaveChanges();
                        int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
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
                int sessionId = int.Parse(Session["SelectedTaxPeriod"].ToString());

                /*                if (sessionId != null)*/
                {
                    using (DB_STEPEntities db = new DB_STEPEntities())
                    {

                        int selectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());

                        var addedDescriptions = db.tblSpecial_Factor
                            .Where(sf => sf.Session_Id == sessionId && sf.Reg_Id == regId)
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

                        var taxPeriod = db.New_Tax_Period.Where(t => t.TaxPerID == sessionId).Select(t => t.TaxPeriod).FirstOrDefault();
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
                    int sessionId = int.Parse(Session["SelectedTaxPeriod"].ToString());

                    using (DB_STEPEntities db = new DB_STEPEntities())
                    {
                        var training = new tblTraining_Need
                        {
                            Reg_Id = regId,
                            Session_Id = sessionId,
                            Title = model.Title,
                            By_When = model.By_When,
                            Train_Type = model.Train_Type,
                            Status = model.Status,
                            Created_date = DateTime.Now,
                            Created_By = regId.ToString(),
                            Updated_by = regId.ToString(),
                            Updated_date = DateTime.Now
                        };

                        db.tblTraining_Need.Add(training);
                        db.SaveChanges();

                        var taxPeriod = db.New_Tax_Period.Where(t => t.TaxPerID == sessionId).Select(t => t.TaxPeriod).FirstOrDefault();
                        Session["TaxPeriod"] = taxPeriod;

                        return RedirectToAction("KraKpiNextYear", "Home");
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
                int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());

                /*                if (sessionId != null)*/
                {
                    using (DB_STEPEntities db = new DB_STEPEntities())
                    {
                        int selectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());

                        var trainingData = db.tblTraining_Need
                            .Where(tn => tn.Session_Id == sessionID && tn.Reg_Id == regId)
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

                        var taxPeriod = db.New_Tax_Period.Where(t => t.TaxPerID == sessionID).Select(t => t.TaxPeriod).FirstOrDefault();
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

                var stepData = (from s in db.STEPs
                                join k in db.KRAs on s.KRA_ID equals k.KRA_ID
                                join kp in db.KPIs on s.KPI_ID equals kp.KPI_ID
                                where s.REG_ID == regId && s.SESSION_ID == selectedTaxPeriod
                                select new KraKpiViewModel
                                {
                                    KRA = k.KRA1,
                                    KPI = kp.KPI1,
                                    KPI_OUTCOME = s.KPI_OUTCOME,
                                    KRA_ID = k.KRA_ID,
                                    KPI_ID = kp.KPI_ID
                                }).ToList();


                var kraKpiData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                   new SqlParameter("RegId", regId),
                   new SqlParameter("SESSION_ID", selectedTaxPeriod)).ToList();

                var groupedData = kraKpiData.GroupBy(x => x.KRA_ID)
                                    .Select(g => new KraKpiViewModel
                                    {
                                        KRA_ID = g.Key,
                                        KRA = g.First().KRA,
                                        KPIIs = g.Select(x => x.KPI).ToList(),
                                        KPIOutcomes = g.Select(x => x.KPIOutcome).ToList()
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
            int sessionId = int.Parse(Session["SelectedTaxPeriod"].ToString());
            var regId = (int)Session["RegId"];

            using (DB_STEPEntities db = new DB_STEPEntities())
            {

                ViewBag.ApprovalSent = Session["ApprovalSent"];

                string ApprovalSent = Session["ApprovalSent"]?.ToString();

                int RegID = int.Parse(Session["RegID"].ToString());
                int updatedBy = (int)Session["RegID"];
                int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
                string statusType = "ApprovalSent";
                string statusMessage = "";
                DateTime updatedDate = DateTime.Now;

                var result = db.Database.SqlQuery<StatusResult>(
                    "EXEC prc_UpdateStatus @RegId, @SESSION_ID, @StatusType, @StatusValue, @StatusMessage, @Updated_date, @Updated_by",
                    new SqlParameter("@RegId", regId),
                    new SqlParameter("@SESSION_ID", sessionID),
                    new SqlParameter("@StatusType", statusType),
                    new SqlParameter("@StatusValue", DBNull.Value),
                    new SqlParameter("@StatusMessage", statusMessage),
                    new SqlParameter("@Updated_date", updatedDate),
                    new SqlParameter("@Updated_by", updatedBy)).FirstOrDefault();

             
                if (result != null)
                {
                    if (result.Status)
                    {
                        Session["ApprovalSent"] = true;
                        //ViewBag.ApprovalSent = true;
                        return Json(new { success = true, message = result.Message });
                    }
                    else
                    {
                        return Json(new { success = false, message = result.Message });
                    }
                }

            }

            return Json(new { success = true, message = "Successfully sent data for approval!" });
        }


    }
}
