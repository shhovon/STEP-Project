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
                                SessionId = 17,
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

            return RedirectToAction("Outcome", "Home");
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

        [CustomAuthorize]
        public ActionResult Outcome()
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                int regId;
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out regId))
                {

                        // Get KRA and KPI data for the logged user
                        var kraKpiData = (from kra in db.KRAs
                                          join kpi in db.KPIs on kra.KRA_ID equals kpi.KRA_ID
                                          where kra.RegId == regId && !string.IsNullOrEmpty(kra.KRA1) && !string.IsNullOrEmpty(kpi.KPI1)
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

            return RedirectToAction("Outcome");
        }


        [CustomAuthorize]
        public ActionResult OutcomeNew()
        {
            OutcomeViewModel model = new OutcomeViewModel();
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                int regId;
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out regId))
                {
                   // var user = db.tblUser_Registration.FirstOrDefault(u => u.RegId == regId);

                   // if (user != null)
                    {

                        // Fetch KRA and KPI data for the logged user
                        var kraKpiData = (from kra in db.KRAs
                                          join kpi in db.KPIs on kra.KRA_ID equals kpi.KRA_ID
                                          where kra.RegId == regId && !string.IsNullOrEmpty(kra.KRA1) && !string.IsNullOrEmpty(kpi.KPI1)
                                          orderby kra.KRA_ID ascending, kpi.KPI_ID ascending
                                          select new
                                          {
                                              KRA_ID = kra.KRA_ID,
                                              KPI_ID = kpi.KPI_ID,
                                              KRA = kra.KRA1,
                                              KPI = kpi.KPI1
                                          }).ToList();


                        var kpiOutcomeData = Enumerable.Repeat("", kraKpiData.Count).ToList();

                        // Grouping KPIs by KRA
                        var groupedData = kraKpiData.GroupBy(x => x.KRA)
                                                    .Select(g => new KeyValuePair<string, List<string>>(g.Key, g.Select(x => x.KPI).ToList()))
                                                    .ToList();
                        model.KRA_ID = kraKpiData.Select(x => x.KRA_ID).Distinct().ToList();
                        model.KPI_ID = kraKpiData.Select(x => x.KPI_ID).ToList();

                        model.KPI_OUTCOME = kpiOutcomeData;
                        ViewBag.KraKpiData = groupedData;
                    }
                }
            }


            return View(model);
        }

        [CustomAuthorize]
        [HttpPost]
        public ActionResult OutcomeNew(OutcomeViewModel model)
        {
            if (model == null || model.REG_ID == 0 || model.FormData == null || model.FormData.Count == 0)
            {
                return RedirectToAction("OutcomeNew");
            }

            try
            {
                using (DB_STEPEntities db = new DB_STEPEntities())
                {
                    foreach (var formDataItem in model.FormData)
                    {
                        // Find the KRA_ID and KPI_ID based on the KRA and KPI strings
                        int kraId = db.KRAs.FirstOrDefault(k => k.KRA1 == formDataItem.KRA)?.KRA_ID ?? 0;
                        int kpiId = db.KPIs.FirstOrDefault(k => k.KPI1 == formDataItem.KPI && k.KRA_ID == kraId)?.KPI_ID ?? 0;

                        if (kraId > 0 && kpiId > 0)
                        {
                            // Insert into the database
                            db.STEPs.Add(new STEP
                            {
                                REG_ID = model.REG_ID,
                                KRA_ID = kraId,
                                KPI_ID = kpiId,
                                KPI_OUTCOME = formDataItem.KPI_OUTCOME
                            });
                        }
                    }

                    db.SaveChanges();
                }

                return RedirectToAction("OutcomeNew", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("OutcomeNew");
            }
        }


        [HttpGet]
        public ActionResult Login()
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var companies = db.Company_Information
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

                List<UserMenu> UserMenu = new List<UserMenu>();
                UserMenu = db.Database.SqlQuery<UserMenu>("exec prc_UserMenu  {0}", regID).ToList();
                System.Web.HttpContext.Current.Session["AssignedMenuItems"] = UserMenu;

                ViewBag.UserMenus = UserMenu;


                /*                Finding Role and Menu             */

                int roleId = Convert.ToInt32(Session["Role"]);
                tblRole role = GetRoleDetails(roleId);
                var assignedMenuItems = GetAssignedMenuItems(role);
                if (assignedMenuItems != null && assignedMenuItems.Any())
                {
                    Session["AssignedMenuItems"] = assignedMenuItems.ToList();
                }

                if (assignedMenuItems != null && assignedMenuItems.Any())
                {
                    ViewBag.AssignedMenuItems = assignedMenuItems;
                }

                var assignedMenuItemsList = assignedMenuItems.ToList();
                ViewBag.Role = role;

                return View();
            }
        }
        private tblRole GetRoleDetails(int roleId)
        {
            using (var db = new DB_STEPEntities())
            {
                var role = db.tblRoles.FirstOrDefault(r => r.ID == roleId);

                if (role != null)
                {
                    var menuIds = role.Menus.Split(',');
                    var menus = db.tblMenus.Where(m => menuIds.Contains(m.ID.ToString())).ToList();
                    var menuNames = string.Join(",", menus.Select(m => m.Menu_Name));
                    role.Menus = menuNames;
                }

                return role;
            }
        }

        /* private IQueryable<tblMenu> GetAssignedMenuItems(tblRole role)
         {
             using (var db = new DB_STEPEntities())
             {
                 if (role == null || string.IsNullOrEmpty(role.Menus))
                 {
                     return (IQueryable<tblMenu>)Enumerable.Empty<tblMenuNew>().AsQueryable();
                 }
                 var menuIds = role.Menus.Split(',');
                 var assignedMenuItems = db.tblMenus.Where(m => menuIds.Contains(m.Menu_Name)).ToList();

                 return assignedMenuItems.AsQueryable();;
             }
         }*/

        private List<tblMenu> GetAssignedMenuItems(tblRole role)
        {
            using (var db = new DB_STEPEntities())
            {
                if (role == null || string.IsNullOrEmpty(role.Menus))
                {
                    return new List<tblMenu>();
                }

                var menuIds = role.Menus.Split(',');
                var assignedMenuItems = db.tblMenus.Where(m => menuIds.Contains(m.Menu_Name)).ToList();

                return assignedMenuItems;
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


                var  SelectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());

                ViewBag.SelectedTaxPeriod = SelectedTaxPeriod;

                ViewBag.ApprovalSent = Session["ApprovalSent"];
           
            // Get KRA and KPI data for the logged user

                var kraKpiData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                                   new SqlParameter("RegId", RegID),
                                   new SqlParameter("SESSION_ID", SelectedTaxPeriod)).ToList();

                var groupedData = kraKpiData.GroupBy(x => x.KRA)
                                .Select(g => new KraKpiViewModel
                                {
                                    KRA = g.Key,
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
                    var selectedKRAs = form.GetValues("SelectedKRA");
                    var selectedKPIs = form.GetValues("SelectedKPI");
                    var kpiOutcomes = form.GetValues("KPIOutcome");

                    for (int i = 0; i < selectedKRAs.Length; i++)
                    {
                        string selectedKRA = selectedKRAs[i];
                        string selectedKPI = selectedKPIs[i];
                        string outcome = kpiOutcomes[i];

                        /* var kraKpiIds = db.KRAs
                                       .Join(db.KPIs, kra => kra.KRA_ID, kpi => kpi.KRA_ID, (kra, kpi) => new { KRA = kra, KPI = kpi })
                                       .Where(x => x.KRA.RegId == regId && x.KRA.KRA1 == selectedKRA && x.KPI.KPI1 == selectedKPI)
                                       .Select(x => new { kraId = x.KRA.KRA_ID, kpiId = x.KPI.KPI_ID })
                                       .FirstOrDefault();*/

                        var kraKpiData = db.Database.SqlQuery<KraKpiOutcomeModel>(
                            "prc_GetkrakpiID @regId, @selectedKRA, @selectedKPI",
                            new SqlParameter("regId", regId),
                            new SqlParameter("selectedKRA", selectedKRA),
                            new SqlParameter("selectedKPI", selectedKPI)
                        ).FirstOrDefault();

                        int kraId = kraKpiData.kraId;
                        int kpiId = kraKpiData.kpiId;


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


        [CustomAuthorize]
        [HttpPost]
        public ActionResult InsertKpiOutcomes(string selectedKRA, string selectedKPI, string kpiOutcomes)
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                int regId;
                if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out regId))
                {
                    var last2session = (db.New_Tax_Period
                             .OrderByDescending(t => t.TaxPeriod)
                             .Select(t => t.TaxPeriod).Take(2).ToList());

                    ViewBag.TopTaxPeriods = last2session;


                    /*  int? sessionId = Session["selectedTaxPeriod"] as int?;*/
                    int sessionId = 17;

                    if (!string.IsNullOrEmpty(selectedKRA) && !string.IsNullOrEmpty(selectedKPI) && !string.IsNullOrEmpty(kpiOutcomes) )
                    {
                            {
                                int kraId = db.KRAs.FirstOrDefault(k => k.KRA1 == selectedKRA)?.KRA_ID ?? 0;
                                int kpiId = db.KPIs.FirstOrDefault(k => k.KPI1 == selectedKPI)?.KPI_ID ?? 0;
  /*                              int? sessionID = db.New_Tax_Period.Where(t => t.TaxPeriod == selectedTaxPeriod).Select(t => t.TaxPerID).FirstOrDefault();*/

                            if (kraId != 0 && kpiId != 0)
                                {
                                    STEP newStep = new STEP
                                    {
                                        REG_ID = regId,
                                        KRA_ID = kraId,
                                        KPI_ID = kpiId,
                                        KPI_OUTCOME = kpiOutcomes,
                                        SESSION_ID = sessionId
                                    };


                                    db.STEPs.Add(newStep);
                                    db.SaveChanges();

                                    var newData = new KraKpiOutcomeModel
                                    {
                                        KRA = selectedKRA,
                                        KPI = selectedKPI,
                                        KPI_OUTCOME = kpiOutcomes
                                    };


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

                                    // Fetch data from the database
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

                                    var model = new CompositeModel
                                    {
                                        KraKpiData = kraKpiData,
                                        StepData = stepData
                                    };

                                    return Json(new { success = true });
                                }
                                else
                                {
                                    return Json(new { success = false, message = "Please select KRA, KPI, provide outcome, and select Session" });
                                }
                            }
                    }
                }
                return View("DisplayKrasAndKpis");
            }
        }

        public ActionResult GetFilteredData(string selectedSession, int regId)
        {

            using (DB_STEPEntities dbContext = new DB_STEPEntities())
            {
                // retrieve the session ID from the selected session name
                var taxPerId = (from st in dbContext.STEPs
                                join tax in dbContext.New_Tax_Period on st.SESSION_ID equals tax.TaxPerID
                                where tax.TaxPeriod == selectedSession && tax.KPI_Enty == true
                                select st.SESSION_ID).FirstOrDefault();

                // filter the data based on the selected session ID
                var kraKpiData = dbContext.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiByRegIdOrSectionName @RegId, @SectionName",
                               new SqlParameter("RegId", regId),
                               new SqlParameter("SectionName", selectedSession)).ToList();

                int? sessionID = dbContext.New_Tax_Period.Where(t => t.TaxPeriod == selectedSession).Select(t => t.TaxPerID).FirstOrDefault();
/*                int selectedTaxPeriod = Convert.ToInt32(Session["SelectedTaxPeriod"]);*/

                var kraKpiOutcomeData = dbContext.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                               new SqlParameter("@RegId", regId),
                               new SqlParameter("@SESSION_ID", sessionID)).ToList();

                var stepData = (from s in dbContext.STEPs
                                join k in dbContext.KRAs on s.KRA_ID equals k.KRA_ID
                                join kp in dbContext.KPIs on s.KPI_ID equals kp.KPI_ID
                                where s.REG_ID == regId
                                select new KraKpiViewModel
                                {
                                    KRA = k.KRA1,
                                    KPI = kp.KPI1,
                                    KPI_OUTCOME = s.KPI_OUTCOME,
                                    KRA_ID = k.KRA_ID,
                                    KPI_ID = kp.KPI_ID
                                }).ToList();

                var result = new
                {
                    KraKpiData = kraKpiData,
                    KraKpiOutcomeData = kraKpiOutcomeData,
                    StepData = stepData
                };

                Session["selectedTaxPeriod"]= taxPerId;


                return Json(result, JsonRequestBehavior.AllowGet);
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
/*                    int selectedTaxPeriod = (int)Session["SelectedTaxPeriod"];*/
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
                       step.REG_ID == regId && step.SESSION_ID == selectedTaxPeriod && step.KRA_ID == kraId && step.KPI_ID == kpiId );

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
