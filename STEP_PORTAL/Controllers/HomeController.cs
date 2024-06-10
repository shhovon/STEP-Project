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

                var kraKpiData = (from kra in db.KRAs
                                  join kpi in db.KPIs on kra.KRA_ID equals kpi.KRA_ID
                                  orderby kra.KRA_ID descending, kpi.KPI_ID descending
                                  select new { KRA = kra.KRA1, KPI = kpi.KPI1 }).Take(5).ToList();

                // grouping KPIs by KRA
                var groupedData = kraKpiData.GroupBy(x => x.KRA)
                                            .Select(g => new KraKpiViewModel
                                            {
                                                KRA = g.Key,
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out regId))
                {
                    var kraKpiData = (from kra in db.KRAs
                                      join kpi in db.KPIs on kra.KRA_ID equals kpi.KRA_ID
                                      where kra.RegId == regId && kra.SessionId == 18
                                      orderby kra.KRA_ID
                                      select new { kra.KRA_ID, kra.KRA1, kra.Duration, kpi.KPI_ID, kpi.KPI1 }).ToList();
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430


                    viewModel.DefaultKRAs = Enumerable.Repeat("", 5).ToList();
                    viewModel.DefaultKPIs = Enumerable.Repeat(Enumerable.Repeat("", 3).ToList(), 5).ToList();
                    viewModel.DefaultDurations = Enumerable.Repeat((DateTime?)null, 5).ToList();


<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======

                    viewModel.KRA_IDs = kraKpiData.Select(k => k.KRA_ID).Distinct().ToList();
                    viewModel.KRAs = kraKpiData.Select(k => k.KRA1).Distinct().ToList();
                    viewModel.Durations = kraKpiData.GroupBy(k => k.KRA_ID).Select(g => g.First().Duration).ToList();
                    viewModel.KPIs = kraKpiData.GroupBy(k => k.KRA_ID)
                                               .Select(g => g.Select(k => k.KPI1).ToList())
                                               .ToList();
                    viewModel.KPI_IDs = kraKpiData.GroupBy(k => k.KRA_ID)
                                                  .Select(g => g.Select(k => k.KPI_ID).ToList())
                                                  .ToList();
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
                    //var userInfo = db.Database.SqlQuery<EmployeeInfo>(
                    //              "prc_EmployeeInfoByRegID @RegID",
                    //              new SqlParameter("@RegID", Session["RegID"])).FirstOrDefault();

>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                    ViewBag.ApprovalSent = Session["ApprovalSent"];

                    if (model.KRAs != null && model.Durations != null)
                    {
                        for (int i = 0; i < model.KRAs.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(model.KRAs[i]))
                            {
                                continue;
                            }

                            KRA kra;

                            if( model.SessionId != 0 && model.KRA_IDs.Count > i)
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
                            // if (model.KRA_IDs.Count > i && model.KRA_IDs[i] > 0)
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                            {
                                kra = db.KRAs.Find(model.KRA_IDs[i]);
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
<<<<<<< HEAD
                                    SessionId = NextYrSessionID,
=======
<<<<<<< HEAD
                                    SessionId = NextYrSessionID,
=======
<<<<<<< HEAD
                                    SessionId = NextYrSessionID,
=======
                                    //Section_Name = userInfo.Section,
                                    SessionId = 18,
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                                    Duration = model.Durations[i],
                                    Created_By = regId.ToString(),
                                    Created_date = DateTime.Now,
                                    Updated_date = DateTime.Now,
                                    Updated_by = regId.ToString()
                                };
                                db.KRAs.Add(kra);
                                db.SaveChanges();
                                model.KRA_IDs.Add(kra.KRA_ID);
                            }
<<<<<<< HEAD

=======
<<<<<<< HEAD

=======
<<<<<<< HEAD

=======
                           // if (model.KPIs != null && model.SessionId != 0)
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                             if (model.KPIs != null && model.KPIs.Count > i && model.KPIs[i] != null)
                            {
                                var existingKpis = db.KPIs.Where(k => k.KRA_ID == kra.KRA_ID).ToList();

                                for (int j = 0; j < model.KPIs[i].Count; j++)
                                {
                                    if (string.IsNullOrWhiteSpace(model.KPIs[i][j]))
                                    {
                                        continue;
                                    }

                                    if (existingKpis.Count > j)
                                    {
                                        var kpi = existingKpis[j];
                                        kpi.KPI1 = model.KPIs[i][j];
                                        kpi.Updated_date = DateTime.Now;
                                        kpi.Updated_by = regId.ToString();
                                        db.Entry(kpi).State = System.Data.Entity.EntityState.Modified;
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
                                    db.SaveChanges();
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
                    var userInfo = db.Database.SqlQuery<EmployeeInfo>(
                                  "prc_EmployeeInfoByRegID @RegID",
                                  new SqlParameter("@RegID", Session["RegID"])).FirstOrDefault();

                    ViewBag.ApprovalSent = Session["ApprovalSent"];

                    for (int i = 0; i < model.KRAs.Count; i++)
                    {
                        KRA kra;
                        if (model.KRA_IDs.Count > i && model.KRA_IDs[i] > 0)
                        {
                            kra = db.KRAs.Find(model.KRA_IDs[i]);
                            kra.KRA1 = model.KRAs[i];
                            kra.Duration = model.Durations[i];
                            kra.Updated_date = DateTime.Now;
                            kra.Updated_by = regId.ToString();
                        }
                        else
                        {
                            kra = new KRA
                            {
                                KRA1 = model.KRAs[i],
                                RegId = regId,
                                Section_Name = userInfo.Section,
                                SessionId = 18,
                                Duration = model.Durations[i],
                                Created_By = regId.ToString(),
                                Created_date = DateTime.Now,
                                Updated_date = DateTime.Now,
                                Updated_by = regId.ToString()
                            };
                            db.KRAs.Add(kra);
                            db.SaveChanges();
                            model.KRA_IDs.Add(kra.KRA_ID);
                        }

                        if (model.KPIs != null && model.KPIs.Count > i && model.KPIs[i] != null)
                        {
                            var existingKpis = db.KPIs.Where(k => k.KRA_ID == kra.KRA_ID).ToList();
                            for (int j = 0; j < model.KPIs[i].Count; j++)
                            {
                                if (existingKpis.Count > j)
                                {
                                    var kpi = existingKpis[j];
                                    kpi.KPI1 = model.KPIs[i][j];
                                    kpi.Updated_date = DateTime.Now;
                                    kpi.Updated_by = regId.ToString();
                                    db.Entry(kpi).State = System.Data.Entity.EntityState.Modified;
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
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
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }

            return RedirectToAction("DisplayAllData", "Employee");
        }

<<<<<<< HEAD

=======
<<<<<<< HEAD

=======
<<<<<<< HEAD

=======
<<<<<<< HEAD

=======
<<<<<<< HEAD

=======
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

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
                var kra = db.KRAs.Find(kraId);
                var kpis = db.KPIs.Where(k => k.KRA_ID == kraId);

                db.KPIs.RemoveRange(kpis);
                db.KRAs.Remove(kra);
                db.SaveChanges();
            }

            return RedirectToAction("ViewKraKpi");
        }

        [HttpGet]
        public ActionResult Login()
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var companies = db.Company_Information
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                                  .OrderBy(c => c.Name)
                                  .Select(c => new SelectListItem
                                  {
                                      Value = c.ID.ToString(),
                                      Text = c.Name
                                  })
                                  .ToList();

<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
                                   .Select(c => new SelectListItem
                                   {
                                       Value = c.ID.ToString(),
                                       Text = c.Name
                                   })
                                   .ToList();
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

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
                                else
                                {
                                    Session["ApprovalSent"] = false;
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

                /* get menu by prc */

                List<UserMenu> userMenu = db.Database.SqlQuery<UserMenu>("exec prc_UserMenu @RegId", new SqlParameter("@RegId", regID)).ToList();
                System.Web.HttpContext.Current.Session["AssignedMenuItems"] = userMenu;

                return View();
            }
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
<<<<<<< HEAD

                // Get KRA and KPI data for the logged user
=======
<<<<<<< HEAD

                // Get KRA and KPI data for the logged user
=======
<<<<<<< HEAD

                // Get KRA and KPI data for the logged user
=======
<<<<<<< HEAD

                // Get KRA and KPI data for the logged user
=======
<<<<<<< HEAD
=======

                // Get KRA and KPI data for the logged user
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93

                // Get KRA and KPI data for the logged user
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

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
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
                    /*                    int selectedTaxPeriod = (int)Session["SelectedTaxPeriod"];*/
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
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





    }
}
