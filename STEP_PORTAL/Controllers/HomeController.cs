using STEP_PORTAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STEP_PORTAL.Helpers;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data.Entity.SqlServer;
using STEP_DEMO.Models;

namespace STEP_PORTAL.Controllers
{
    public class HomeController : Controller
    {
        [CustomAuthorize]
        [HttpPost]
        public ActionResult Index(KraKpiViewModel model)
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                int regId;
                int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out regId))
                {

                    var userInfo = db.Database.SqlQuery<EmployeeInfo>(
                              "prc_EmployeeInfoByRegID @RegID",
                              new SqlParameter("@RegID", Session["RegID"])).FirstOrDefault();


                    ViewBag.ApprovalSent = Session["ApprovalSent"];


                    for (int i = 0; i < model.KRAs.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(model.KRAs[i]))
                        {
                            KRA kra = new KRA
                            {
                                KRA1 = model.KRAs[i],
                                RegId = regId,
                                Section_Name = userInfo.Section,
                                SessionId = sessionID,
                                Created_By = regId.ToString(),
                                Created_date = DateTime.Now,
                                Updated_date = DateTime.Now,
                                Updated_by = regId.ToString()
                            };

                            db.KRAs.Add(kra);
                            db.SaveChanges();

                            if (model.KPIs != null && model.KPIs.Count > i && model.KPIs[i] != null)
                            {
                                foreach (var kpiName in model.KPIs[i])
                                {
                                    if (!string.IsNullOrEmpty(kpiName))
                                    {
                                        KPI kpi = new KPI
                                        {
                                            KPI1 = kpiName,
                                            KRA_ID = kra.KRA_ID,
                                            Created_date = DateTime.Now,
                                            Created_By = regId.ToString(),
                                            Updated_by = regId.ToString(),
                                            Updated_date = DateTime.Now
                                        };
                                        db.KPIs.Add(kpi);
                                    }
                                    db.SaveChanges();
                                    //TempData["Message"] = "Successfully Saved";
                                }
                            }
                        }
                    }
                }

/*                var kraKpiData = (from kra in db.KRAs
                                  join kpi in db.KPIs on kra.KRA_ID equals kpi.KRA_ID
                                  orderby kra.KRA_ID descending, kpi.KPI_ID descending
                                  select new { KRA = kra.KRA1, KPI = kpi.KPI1 }).Take(5).ToList();*/
                var kraKpiData = (from kra in db.KRAs
                                  join kpi in db.KPIs on kra.KRA_ID equals kpi.KRA_ID
                                  orderby kra.KRA_ID descending, kpi.KPI_ID descending
                                  select new { KRA_ID = kra.KRA_ID, KRA = kra.KRA1, KPI = kpi.KPI1 }).Take(5).ToList();

                // grouping KPIs by KRA
                var groupedData = kraKpiData.GroupBy(x => x.KRA_ID)
                                            .Select(g => new KraKpiViewModel
                                            {
                                                KRA_ID = g.Key,
                                                KRA = g.First().KRA,
                                                KPIIs = g.Select(x => x.KPI).ToList()
                                            })
                                            .ToList();
                var lastFiveKRAs = db.KRAs.OrderByDescending(k => k.KRA_ID).Take(5).ToList();
                ViewBag.KraKpiData = groupedData;
            }

            return RedirectToAction("ViewKraKpi", "Home");
        }

        [CustomAuthorize]
        public ActionResult Index()
        {
            ViewBag.ApprovalSent = Session["ApprovalSent"];
            var viewModel = new KraKpiViewModel
            {
                KRAs = new List<string>(),
            };
            return View(viewModel);
        }

        // next year 

        [CustomAuthorize]
        public ActionResult KraKpiNextYear()
        {
            ViewBag.ApprovalSent = Session["ApprovalSent"];
            var viewModel = new KraKpiViewModel();

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                int regId;
                int NextYrSessionID = (int.Parse(Session["SelectedTaxPeriod"].ToString()) + 1);
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out regId))
                {
                    //var kraKpiData = (from kra in db.KRAs
                    //                  join kpi in db.KPIs on kra.KRA_ID equals kpi.KRA_ID
                    //                  where kra.RegId == regId && kra.SessionId == NextYrSessionID
                    //                  orderby kra.KRA_ID
                    //                  select new { kra.KRA_ID, kra.KRA1, kra.Duration, kpi.KPI_ID, kpi.KPI1 }).ToList();

                    var kraKpiDataPrc = db.Database.SqlQuery<KraKpiViewModel>(
                                      "prc_GetKraKpiData @RegID, @SessionID",
                                      new SqlParameter("@RegID", Session["RegID"]),
                                      new SqlParameter("@SessionID", NextYrSessionID)).ToList();


                    viewModel.DefaultKRAs = Enumerable.Repeat("", 5).ToList();
                    viewModel.DefaultKPIs = Enumerable.Repeat(Enumerable.Repeat("", 3).ToList(), 5).ToList();
                    viewModel.DefaultDurations = Enumerable.Repeat((DateTime?)null, 5).ToList();


                    viewModel.SessionId = kraKpiDataPrc.Select(k => k.SessionId).FirstOrDefault();


                    viewModel.KRA_IDs = viewModel.KRA_IDs ?? new List<int>();
                    viewModel.KRAs = viewModel.KRAs ?? new List<string>();
                    viewModel.KPI_IDs = viewModel.KPI_IDs ?? new List<List<int>>();
                    viewModel.KPIs = viewModel.KPIs ?? new List<List<string>>();
                    viewModel.Durations = viewModel.Durations ?? new List<DateTime?>();
                    


                    viewModel.KRA_IDs = kraKpiDataPrc.Select(k => k.KRA_ID).Distinct().ToList();
                    viewModel.KRAs = kraKpiDataPrc.GroupBy(k => k.KRA_ID).Select(g => g.First().KRA).ToList();
                    viewModel.Durations = kraKpiDataPrc.GroupBy(k => k.KRA_ID).Select(g => g.First().Duration).ToList();
                    viewModel.KPIs = kraKpiDataPrc.GroupBy(k => k.KRA_ID)
                                                   .Select(g => g.Select(k => k.KPI).ToList())
                                                   .ToList();
                    viewModel.KPI_IDs = kraKpiDataPrc.GroupBy(k => k.KRA_ID)
                                                     .Select(g => g.Select(k => k.KPI_ID).ToList())
                                                     .ToList();
                }
            }

            return View(viewModel);
        }


        [CustomAuthorize]
        [HttpPost]
        public ActionResult KraKpiNextYear(KraKpiViewModel model)
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                int regId;
                int NextYrSessionID = (int.Parse(Session["SelectedTaxPeriod"].ToString()) + 1);
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out regId))
                {
                    ViewBag.ApprovalSent = Session["ApprovalSent"];

                    if (model.KRAs != null && model.Durations != null)
                    {
                        // Validation for minimum KPIs
                        for (int i = 0; i < model.KRAs.Count; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(model.KRAs[i]))
                            {
                                int nonEmptyKpiCount = model.KPIs[i].Count(kpi => !string.IsNullOrWhiteSpace(kpi));
                                if (nonEmptyKpiCount < 2)
                                {
                                    return View(model);
                                }
                            }
                        }

                        // Get all existing KRAs for this user and session
                        var existingKRAs = db.KRAs.Where(k => k.RegId == regId && k.SessionId == NextYrSessionID).ToList();

                        for (int i = 0; i < model.KRAs.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(model.KRAs[i]))
                            {
                                // If KRA is empty and it exists in the database, delete it and its associated KPIs
                                if (model.KRA_IDs.Count > i && model.KRA_IDs[i] != 0)
                                {
                                    var kraToDelete = existingKRAs.FirstOrDefault(k => k.KRA_ID == model.KRA_IDs[i]);
                                    if (kraToDelete != null)
                                    {
                                        var kpisToDelete = db.KPIs.Where(k => k.KRA_ID == kraToDelete.KRA_ID);
                                        db.KPIs.RemoveRange(kpisToDelete);
                                        db.KRAs.Remove(kraToDelete);
                                    }
                                }
                                continue;
                            }

                            KRA kra;

                            if (model.SessionId != 0 && model.KRA_IDs.Count > i && model.KRA_IDs[i] != 0)
                            {
                                kra = existingKRAs.FirstOrDefault(k => k.KRA_ID == model.KRA_IDs[i]);
                                if (kra != null)
                                {
                                    kra.KRA1 = model.KRAs[i];
                                    kra.Duration = model.Durations[i];
                                    kra.Updated_date = DateTime.Now;
                                    kra.Updated_by = regId.ToString();
                                }
                            }
                            else
                            {
                                kra = new KRA
                                {
                                    KRA1 = model.KRAs[i],
                                    RegId = regId,
                                    SessionId = NextYrSessionID,
                                    Duration = model.Durations[i],
                                    Created_By = regId.ToString(),
                                    Created_date = DateTime.Now,
                                    Updated_date = DateTime.Now,
                                    Updated_by = regId.ToString()
                                };
                                db.KRAs.Add(kra);
                            }

                            db.SaveChanges();  // Save to ensure KRA has an ID for KPIs

                            if (model.KPIs != null && model.KPIs.Count > i && model.KPIs[i] != null)
                            {
                                var existingKpis = db.KPIs.Where(k => k.KRA_ID == kra.KRA_ID).ToList();

                                // Remove KPIs that are no longer in the model
                                foreach (var existingKpi in existingKpis)
                                {
                                    if (!model.KPIs[i].Any(k => !string.IsNullOrWhiteSpace(k) && k == existingKpi.KPI1))
                                    {
                                        db.KPIs.Remove(existingKpi);
                                    }
                                }

                                for (int j = 0; j < model.KPIs[i].Count; j++)
                                {
                                    if (string.IsNullOrWhiteSpace(model.KPIs[i][j]))
                                    {
                                        continue;
                                    }

                                    var existingKpi = existingKpis.FirstOrDefault(k => k.KPI1 == model.KPIs[i][j]);
                                    if (existingKpi != null)
                                    {
                                        existingKpi.Updated_date = DateTime.Now;
                                        existingKpi.Updated_by = regId.ToString();
                                        db.Entry(existingKpi).State = System.Data.Entity.EntityState.Modified;
                                    }
                                    else
                                    {
                                        KPI kpi = new KPI
                                        {
                                            KPI1 = model.KPIs[i][j],
                                            KRA_ID = kra.KRA_ID,
                                            Created_date = DateTime.Now,
                                            Created_By = regId.ToString(),
                                            Updated_by = regId.ToString(),
                                            Updated_date = DateTime.Now
                                        };
                                        db.KPIs.Add(kpi);
                                    }
                                }
                            }
                            db.SaveChanges();
                        }

                        // remove kra kpi by blank submission
                        var kraIdsInModel = model.KRA_IDs.Where(id => id != 0).ToList();
                        var krasToDelete = existingKRAs.Where(k => !kraIdsInModel.Contains(k.KRA_ID));
                        foreach (var kraToDelete in krasToDelete)
                        {
                            var kpisToDelete = db.KPIs.Where(k => k.KRA_ID == kraToDelete.KRA_ID);
                            db.KPIs.RemoveRange(kpisToDelete);
                            db.KRAs.Remove(kraToDelete);
                        }
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("DisplayAllData", "Employee");
        }



        [CustomAuthorize]
        public ActionResult ViewKraKpi()
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                int regId;
                int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out regId))
                {

                    // Get KRA and KPI data for the logged user
                    var kraKpiData = (from kra in db.KRAs
                                      join kpi in db.KPIs on kra.KRA_ID equals kpi.KRA_ID
                                      where kra.RegId == regId
                                      && kra.SessionId == sessionID
                                      && !string.IsNullOrEmpty(kra.KRA1)
                                      && !string.IsNullOrEmpty(kpi.KPI1)
                                      orderby kra.KRA_ID ascending, kpi.KPI_ID ascending
                                      select new { KRA = kra.KRA1, KPI = kpi.KPI1, KRA_ID = kra.KRA_ID }).ToList();

                    // Grouping KPIs by KRA
                    var groupedData = kraKpiData.GroupBy(x => new { x.KRA, x.KRA_ID })
                                                .Select(g => new KraKpiViewModel
                                                {
                                                    KRA = g.Key.KRA,
                                                    KRA_ID = g.Key.KRA_ID,
                                                    KPIIs = g.Select(x => x.KPI).ToList()
                                                })
                                                .ToList();

                    ViewBag.ApprovalSent = Session["ApprovalSent"];

                    ViewBag.KraKpiData = groupedData;

                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult DeleteKRA(int kraId)
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {

                bool kraExistsInStep = db.STEPs.Any(s => s.KRA_ID == kraId);
                var associatedKpiIds = db.KPIs.Where(k => k.KRA_ID == kraId).Select(k => k.KPI_ID).ToList();
                bool kpiExistsInStep = db.STEPs.Any(s => associatedKpiIds.Contains((int)s.KPI_ID));

/*                if (kraExistsInStep || kpiExistsInStep)
                {
                    TempData["AlertMessage"] = "This KRA is already used in the STEP form. To delete this KRA, first delete it from the STEP form.";
                    return RedirectToAction("ViewKraKpi");
                }*/

                var kra = db.KRAs.Find(kraId);
                var kpis = db.KPIs.Where(k => k.KRA_ID == kraId);
                db.KPIs.RemoveRange(kpis);
                db.KRAs.Remove(kra);
                db.SaveChanges();

                TempData["SuccessMessage"] = "KRA has been successfully deleted.";
            }
            return RedirectToAction("ViewKraKpi");
        }


        [HttpGet]
        public ActionResult Login()
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var companies = db.Company_Information
                                  .OrderBy(c => c.Name)
                                  .Select(c => new SelectListItem
                                  {
                                      Value = c.ID.ToString(),
                                      Text = c.Name
                                  })
                                  .ToList();


                ViewBag.Companies = companies;

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {


                if (ModelState.IsValid)
                {
                    // retrieve the employee information based on the provided RegId
                    var employeeInfo = db.Database.SqlQuery<EmployeeInfo>(
                                   "prc_EmployeeInfoByEmpCode @ComID, @EmpCode",
                                   new SqlParameter("@ComID", model.ComID),
                                   new SqlParameter("@EmpCode", model.EmployeeCode)).FirstOrDefault();


                    if (employeeInfo != null)
                    {
                        var user = db.Database.SqlQuery<User_RegistrationInfo>(
                                   "prc_User_Registration  @RegID",
                                   new SqlParameter("@RegID", employeeInfo.RegId)).FirstOrDefault();

                        if (user != null && PasswordHelper.Decrypt(user.Password.Trim()) == model.Password.Trim())
                        {
                            // insert login history
                            tblUserLogHistory logEntry = new tblUserLogHistory
                            {
                                UserID = (int)employeeInfo.RegId,
                                LoginTime = DateTime.Now,
                                UserIP = GetIPAddress(),
                                ComputerName = GetComputerName(),
                            };
                            db.tblUserLogHistories.Add(logEntry);
                            db.SaveChanges();



                            // Login successful
                            Session["RegID"] = employeeInfo.RegId;
                            Session["ComID"] = employeeInfo.ComID;
                            Session["EmailID"] = employeeInfo.EmailID;
                            Session["MobileNoPerson"] = employeeInfo.MobileNoPerson;
                            Session["DeptHead"] = employeeInfo.DeptHead;
                            Session["Role"] = employeeInfo.Role;
                            Session["SelectedTaxPeriod"] = 17;

                            long regId;

                            if (Session["RegID"] != null && long.TryParse(Session["RegID"].ToString(), out regId))
                            {

                                var SelectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());
                                var kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                                                new SqlParameter("@RegId", regId),
                                                new SqlParameter("@SESSION_ID", SelectedTaxPeriod)).ToList();
                                
                                if (kraKpiOutcomeData.Count > 0)
                                {
                                    Session["ApprovalSent"] = kraKpiOutcomeData[0].ApprovalSent;
                                }
                                /*else
                                {
                                    Session["ApprovalSent"] = false;
                                }*/
                                else
                                {
                                    var approvalSentQuery = @"select top 1 ApprovalSent 
                                                              from tbl_StepMaster 
                                                              where RegId = @RegId and SESSION_ID = @SESSION_ID";

                                    var approvalSent = db.Database.SqlQuery<bool?>(approvalSentQuery,
                                                        new SqlParameter("@RegId", regId),
                                                        new SqlParameter("@SESSION_ID", SelectedTaxPeriod)).FirstOrDefault();

                                    Session["ApprovalSent"] = approvalSent ?? false;
                                }
                            }

                            ModelState.Clear();
                            return RedirectToAction("Dashboard", "Home");

                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Incorrect password!";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Employee not found!";
                    }
                }

                var companies = db.Company_Information
                           .Select(c => new SelectListItem
                           {
                               Value = c.ID.ToString(),
                               Text = c.Name
                           })
                           .ToList();

                ViewBag.Companies = companies;

                return View("Login", model);
            }
        }

        protected string GetIPAddress()
        {
            string ipList = (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                   Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }


        [CustomAuthorize]
        public ActionResult Dashboard()
        {
            if (Session["RegID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            string tagMessage = TempData["Tag"] as string;
            if (!string.IsNullOrEmpty(tagMessage))
            {
                ViewBag.TagMessage = tagMessage;
            }

            var regID = (int)Session["RegID"];
            var roleID = Convert.ToInt32(Session["Role"]);

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                long regId;

                if (Session["RegID"] != null && long.TryParse(Session["RegID"].ToString(), out regId))
                {

                    var userInfo = db.Database.SqlQuery<EmployeeInfo>(
                                  "prc_EmployeeInfoByRegID @RegID",
                                  new SqlParameter("@RegID", Session["RegID"])).FirstOrDefault();



                    if (userInfo != null)
                    {
                        Session["EmployeeCode"] = userInfo.EmployeeCode;
                        Session["Name"] = userInfo.Name;
                        Session["Department"] = userInfo.Department;
                        Session["Section"] = userInfo.Section;
                        Session["Designation"] = userInfo.Designation;
                    }
                }

                /* Fetching unread notifications */
                var userInfo2 = db.Database.SqlQuery<EmployeeInfo>(
                                "prc_User_Registration @RegID",
                                new SqlParameter("@RegID", Session["RegID"])).FirstOrDefault();

                int SupervisorID = userInfo2.ReportSuper;

                var notifications = db.tblNotifications
                                      .Where(n => n.EmployeeRegId == regID && (n.IsRead ?? false) == false)
                                      .ToList();

                ViewBag.UnreadNotificationCount = notifications.Count;
                ViewBag.Notifications = notifications;

                /* get menu by prc */

                List<UserMenu> userMenu = db.Database.SqlQuery<UserMenu>("exec prc_UserMenu @RegId", new SqlParameter("@RegId", regID)).ToList();
                System.Web.HttpContext.Current.Session["AssignedMenuItems"] = userMenu;

                return View();
            }
        }

        [HttpPost]
        public JsonResult MarkNotificationAsRead(int notificationId)
        {
            using (var db = new DB_STEPEntities())
            {
                var notification = db.tblNotifications
                                      .SingleOrDefault(n => n.NotificationId == notificationId);
                if (notification != null)
                {
                    notification.IsRead = true;
                    db.SaveChanges();
                }
            }

            return Json(new { success = true });
        }

        public ActionResult Logout()
        {
            Session.Clear();
            TempData["LoggedOut"] = true;
            return RedirectToAction("Login", "Home");
        }

        [CustomAuthorize]
        public ActionResult DisplayKrasAndKpis()
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                int RegID = int.Parse(Session["RegID"].ToString());

                var last2session = (db.New_Tax_Period
                              .OrderByDescending(t => t.TaxPeriod)
                              .Take(2).ToList());

                ViewBag.TopTaxPeriods = last2session;


                var SelectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());

                ViewBag.SelectedTaxPeriod = SelectedTaxPeriod;

                ViewBag.ApprovalSent = Session["ApprovalSent"];

                // Get KRA and KPI data for the logged user

                var kraKpiData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeEntry @RegId, @SESSION_ID",
                                   new SqlParameter("RegId", RegID),
                                   new SqlParameter("SESSION_ID", SelectedTaxPeriod)).ToList();

                var groupedData = kraKpiData.GroupBy(x => x.KRA_ID)
                                .Select(g => new KraKpiViewModel
                                {
                                    KRA_ID = g.Key,
                                    Section_Name = g.Select(x => x.SESSION_ID).ToList()[0].ToString(),
                                    KRAs = g.Select(x => x.KRA).ToList(),
                                    KPI_IDss = g.Select(x => x.KPI_ID).ToList(),
                                    KPIIs = g.Select(x => x.KPI).ToList(),
                                    KPIOutcomes = g.Select(x => x.Outcome).ToList()
                                })
                                .ToList();

                return View(groupedData);

                /*            return View();*/
            }
        }

        [HttpPost]
        public ActionResult UpdateOutcomeEntry(FormCollection form)
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                int regId;
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out regId))
                {

                    var SelectedSession = form.GetValues("SelectedSession");
                    var selectedKRAs = form.GetValues("SelectedKRA");
                    var selectedKPIs = form.GetValues("SelectedKPI");
                    var kpiOutcomes = form.GetValues("KPIOutcome");

                    for (int i = 0; i < selectedKRAs.Length; i++)
                    {
                        // string selectedKRA = selectedKRAs[i];
                        //  string selectedKPI = selectedKPIs[i];


                        //var kraKpiData = db.Database.SqlQuery<KraKpiOutcomeModel>(
                        //    "prc_GetkrakpiID @regId, @selectedKRA, @selectedKPI",
                        //    new SqlParameter("regId", regId),
                        //    new SqlParameter("selectedKRA", selectedKRA),
                        //    new SqlParameter("selectedKPI", selectedKPI)).FirstOrDefault();
                        if (SelectedSession[i].ToString() == "0")
                        {
                            continue;

                        }
                        int kraId = int.Parse(selectedKRAs[i]);
                        int kpiId = int.Parse(selectedKPIs[i]);
                        string outcome = kpiOutcomes[i];


                        int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
                        var existingRecord = db.STEPs.FirstOrDefault(s => s.REG_ID == regId && s.SESSION_ID == sessionID && s.KRA_ID == kraId && s.KPI_ID == kpiId);

                        if (existingRecord != null)
                        {
                            existingRecord.KPI_OUTCOME = outcome;
                            existingRecord.Created_date = DateTime.Now;
                            existingRecord.Created_By = regId.ToString();
                            existingRecord.Updated_by = regId.ToString();
                            existingRecord.Updated_date = DateTime.Now;
                        }
                        else
                        {
                            var kpiOutcome = new STEP
                            {
                                REG_ID = regId,
                                KRA_ID = kraId,
                                KPI_ID = kpiId,
                                KPI_OUTCOME = outcome,
                                SESSION_ID = sessionID,
                                Created_date = DateTime.Now,
                                Created_By = regId.ToString(),
                                Updated_by = regId.ToString(),
                                Updated_date = DateTime.Now
                            };

                            db.STEPs.Add(kpiOutcome);
                        }
                    }

                    db.SaveChanges();
                    bool success = true;
                    TempData["SuccessMessage"] = success ? "Outcome Data saved successfully!" : "";
                }
            }

            return RedirectToAction("SpecialFactors", "Employee");
        }



        [HttpPost]
        public ActionResult DeleteOutcome(int kraId)
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                int regId;
                int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());

                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out regId))
                {
                    var existingRecord = db.STEPs.Where(s => s.REG_ID == regId && s.SESSION_ID == sessionID && s.KRA_ID == kraId);

                    if (existingRecord != null)
                    {
                        db.STEPs.RemoveRange(existingRecord);
                        db.SaveChanges();
                    }



                }
                return Json(new { success = true });
            }

        }



        [HttpPost]
        public ActionResult DeleteSession(int kraId, int kpiId)
        {
            int regId;
            if (!int.TryParse(Session["RegID"].ToString(), out regId))
            {
                return RedirectToAction("DisplayKrasAndKpis");
            }

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                try
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
                        return RedirectToAction("DisplayKrasAndKpis");
                    }

                    var itemToDelete = db.STEPs.FirstOrDefault(step =>
                       step.REG_ID == regId && step.SESSION_ID == selectedTaxPeriod && step.KRA_ID == kraId && step.KPI_ID == kpiId);

                    if (itemToDelete != null)
                    {
                        db.STEPs.Remove(itemToDelete);
                        db.SaveChanges();
                    }

                    return RedirectToAction("DisplayKrasAndKpis");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("DisplayKrasAndKpis");
                }
            }
        }


        [HttpPost]
        public ActionResult UpdateSelectedSession(string selectedSession)
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                Session["SelectedTaxPeriod"] = selectedSession;
                return Json(new { success = true });
            }

        }

        protected string GetComputerName()
        {
            try
            {
                return System.Environment.MachineName;
            }
            catch (Exception ex)
            {
                return "UnknownComputer";
            }
        }

        [HttpPost]
        public ActionResult UpdateKraKpi(int kraId, string kra, List<string> kpis)
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var kraEntity = db.KRAs.Find(kraId);
                if (kraEntity != null)
                {
                    kraEntity.KRA1 = kra;

                    var kpiEntities = db.KPIs.Where(k => k.KRA_ID == kraId).OrderBy(k => k.KPI_ID).ToList();
                    for (int i = 0; i < kpis.Count && i < kpiEntities.Count; i++)
                    {
                        kpiEntities[i].KPI1 = kpis[i];
                    }

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
        }



    }
}
