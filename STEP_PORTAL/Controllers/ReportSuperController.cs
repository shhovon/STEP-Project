
using iTextSharp.text;
using iTextSharp.text.pdf;
using STEP_PORTAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace STEP_PORTAL.Controllers
{
    public class ReportSuperController : Controller
    {
        // GET: ReportSuper
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult AddMarksRS(string regId)
        {
            int RegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
            int deptHeadValue;
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
<<<<<<< HEAD
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();
=======
<<<<<<< HEAD
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();
=======
<<<<<<< HEAD
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();
=======
<<<<<<< HEAD
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();
=======
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

            if (Session["RegID"] != null && int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
            {
                using (DB_STEPEntities db = new DB_STEPEntities())
                {
                    var authResult = db.Database.SqlQuery<StatusResult>(
                                    "exec prc_CheckAuth @RegId, @SESSION_ID, @Type, @EmpRegId",
                                    new SqlParameter("@RegId", deptHeadValue),
                                    new SqlParameter("@SESSION_ID", sessionID),
                                    new SqlParameter("@Type", "AddMarksRS"),
                                    new SqlParameter("@EmpRegId", RegId)
                                ).FirstOrDefault();

                    if (authResult == null || !authResult.Status)
                    {
                        ViewBag.AuthorizationMessage = authResult?.Message ?? "Unauthorized access";
<<<<<<< HEAD
                        TempData["Tag"] = "Unauthorized access";
                        return RedirectToAction("Dashboard", "Home");
=======
                        return RedirectToAction("Dashboard", "Home");
                        TempData["UnauthMessage"] = "Unauthorized access!";
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                    }

                    var last2session = (db.New_Tax_Period
                                 .OrderByDescending(t => t.TaxPeriod)
                                 .Select(t => t.TaxPeriod).Take(2).ToList());

                    ViewBag.TopTaxPeriods = last2session;

                    int nextyearSessionID = sessionID + 1;

                    var EmployeeInfo = DC.GetEmployeeInfoByRegID(RegId);
                    List<KraKpiOutcomeModel> kraKpiOutcomeData = DC.GetKraKpiOutcomeData(RegId, sessionID);
                    List<KraKpiOutcomeModel> nextYearkraKpiOutcomeData = DC.GetKraKpiData(RegId, nextyearSessionID);
                    var StepMaster = DC.GetStepMaster(RegId, sessionID);
<<<<<<< HEAD
                    var SpecialFactors = DC.getSpecialFactors(RegId, sessionID);
                    var TrainingData = DC.getTrainingData(RegId, sessionID);
=======
<<<<<<< HEAD
                    var SpecialFactors = DC.getSpecialFactors(RegId, sessionID);
                    var TrainingData = DC.getTrainingData(RegId, sessionID);
=======
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

                    string selectedTaxPeriod = Session["SelectedTaxPeriod"] as string;

                    string employeeID = Request.Form["employeeCode"];
                    int regID = (int)Session["RegID"];

                    // insert marks history
                    tblMarksEntryHistory logEntry = new tblMarksEntryHistory
                    {
                        SupervisorID = deptHeadValue,
                        EmployeeID = RegId.ToString(),
                        UpdateTime = DateTime.Now,
                        UserIP = GetIPAddress(),
                    };
                    db.tblMarksEntryHistories.Add(logEntry);
                    db.SaveChanges();


                    var userInfo = db.Database.SqlQuery<EmployeeInfo>(
                          "prc_EmployeeInfoByRegID @RegID",
                          new SqlParameter("@RegID", Session["RegID"])).FirstOrDefault();

                    var model = new KraKpiOutcomeModel
                    {
                        EmployeeCode = userInfo.EmployeeCode,
                        Name = userInfo.Name
                    };

                    ViewBag.Designation = userInfo.Designation;

                    /*var kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>
                        ("exec prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                         new SqlParameter("@RegId", RegId),
                         new SqlParameter("@SESSION_ID", Session["SelectedTaxPeriod"])).ToList();*/

                    var nextYeargroupedData = nextYearkraKpiOutcomeData.GroupBy(x => x.KRA_ID)
                           .Select(g => new KraKpiViewModel
                           {
                               KRA = g.First().KRA,
                               KPIIs = g.Where(x => x.KPI != null).Select(x => x.KPI).ToList(),
                               Durations = g.Where(x => x.Duration != null)
                                            .Select(x => x.Duration)
                                            .ToList()
                           })
                           .ToList();

<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

                    ViewBag.KraKpiOutcomeData = kraKpiOutcomeData;
                    ViewBag.RegId = RegId;
                    /*                    return View("KraKpiOutcomeView", kraKpiOutcomeData);*/

                    var approvalSent = db.tbl_StepMaster
                                     .Where(x => x.RegId == RegId)
                                     .Select(x => x.ApprovalSent)
                                     .FirstOrDefault();
<<<<<<< HEAD
=======
=======
<<<<<<< HEAD

                    ViewBag.KraKpiOutcomeData = kraKpiOutcomeData;
                    ViewBag.RegId = RegId;
=======


                    ViewBag.KraKpiOutcomeData = kraKpiOutcomeData;
                    ViewBag.RegId = RegId;
<<<<<<< HEAD
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
/*                    return View("KraKpiOutcomeView", kraKpiOutcomeData);*/
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

                    var viewModel = new DisplayAllDataViewModel
                    {
                        EmployeeInfo = EmployeeInfo,
                        KraKpiOutcomeData = kraKpiOutcomeData,
                        NextYearKraKpiOutcomeData = nextYearkraKpiOutcomeData,
                        /*GroupedData = groupedData,*/
                        NextYearGroupedData = nextYeargroupedData,
                        StepMaster = StepMaster,
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                        SpecialFactors = SpecialFactors,
                        TrainingNeed = TrainingData,
                        ApprovalSent = approvalSent.HasValue ? approvalSent.Value : false
                    };



                    //return View(viewModel);
                    return View("KraKpiOutcomeView", viewModel);
<<<<<<< HEAD
=======
=======
                    };

                    //return View(viewModel);
                    return View("KraKpiOutcomeView", viewModel);
<<<<<<< HEAD
=======
=======

                    return View("KraKpiOutcomeView", kraKpiOutcomeData);
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430

                }
            }

            return null;

            }

        [CustomAuthorize]
        [HttpPost]

        public ActionResult UpdateMarks(List<KraKpiOutcomeModel> model)
        {
            int regId = Convert.ToInt32(Request.Form["regId"]);
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();
            if (model != null)
            {
                using (DB_STEPEntities db = new DB_STEPEntities())
                {
                    foreach (var item in model)
                    {
                        var outcomeEntity = db.STEPs.FirstOrDefault(o => o.KPI_ID == item.KPI_ID && o.REG_ID == regId && o.KPI_OUTCOME == item.Outcome);
                        if (outcomeEntity != null)
                        {
                            outcomeEntity.Marks_Achieved = item.SelectedMarks.Value;
                            outcomeEntity.Remarks = item.Remarks;
                            db.Entry(outcomeEntity).State = EntityState.Modified;
                        }
                    }

                    db.SaveChanges();
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                    var status = DC.UpdateRating(regId, sessionID);

                    var updatedData = db.Database.SqlQuery<KraKpiOutcomeModel>(
                        "exec prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                        new SqlParameter("@RegId", regId),
                        new SqlParameter("@SESSION_ID", sessionID)).ToList();

<<<<<<< HEAD
=======
=======

<<<<<<< HEAD
                    var status = DC.UpdateRating(regId, sessionID);

                    var updatedData = db.Database.SqlQuery<KraKpiOutcomeModel>(
                        "exec prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
=======
                    db.Database.ExecuteSqlCommand(
                        "exec prc_UpdateRating @RegId, @SESSION_ID",
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
                        new SqlParameter("@RegId", regId),
                        new SqlParameter("@SESSION_ID", sessionID)).ToList();

<<<<<<< HEAD
=======
                    var updatedData = db.Database.SqlQuery<KraKpiOutcomeModel>(
                        "exec prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
                        new SqlParameter("@RegId", regId),
                        new SqlParameter("@SESSION_ID", sessionID)).ToList();

>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                    foreach (var updatedItem in updatedData)
                    {
                        var existingItem = model.FirstOrDefault(m => m.KPI_ID == updatedItem.KPI_ID);
                        if (existingItem != null)
                        {
                            existingItem.Remarks = updatedItem.Remarks;
                        }
                    }

                }
            }
            TempData["SuccessMessage"] = "Marks updated successfully!";

            string encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(regId.ToString());
            return RedirectToAction("AddMarksRS", new { regId = encryptedRegId });

            /*return RedirectToAction("AddMarks", new { RegId = regId });*/
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
        [HttpGet]
        public ActionResult ViewMarks()
        {
            int regId = (int)Session["RegId"];
            STEP_DEMO.Controllers.DataController DC = new STEP_DEMO.Controllers.DataController();
            //int EmpRegId = int.Parse(STEP_PORTAL.Helpers.PasswordHelper.Decrypt(regId));
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            int deptHeadValue = int.Parse(Session["RegID"].ToString());

            //List<KraKpiOutcomeModel> kraKpiOutcomeData;
            using (var db = new DB_STEPEntities())
            {
                var last2session = (db.New_Tax_Period
                                   .OrderByDescending(t => t.TaxPeriod)
                                   .Select(t => t.TaxPeriod).Take(2).ToList());

                ViewBag.TopTaxPeriods = last2session;

                int nextyearSessionID = sessionID + 1;
                //int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
                var EmployeeInfo = DC.GetEmployeeInfoByRegID(regId);
                List<KraKpiOutcomeModel> kraKpiOutcomeData = DC.GetKraKpiOutcomeData(regId, sessionID);
                List<KraKpiOutcomeModel> nextYearkraKpiOutcomeData = DC.GetKraKpiData(regId, nextyearSessionID);
                var StepMaster = DC.GetStepMaster(regId, sessionID);
                var designations = DC.GetDesignations();

                kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
               new SqlParameter("@RegId", regId),
               new SqlParameter("@SESSION_ID", sessionID)).ToList();

                var groupedData = kraKpiOutcomeData.GroupBy(x => x.KRA)
                                                .Select(g => new KraKpiViewModel
                                                {
                                                    KRA = g.Key,
                                                    KPIIs = g.Select(x => x.KPI).ToList(),
                                                    KPIOutcomes = g.Select(x => x.KPIOutcome).ToList(),
                                                    AllRemarks = g.Select(x => x.Remarks).ToList()
                                                })
                                                .ToList();


                var viewModel = new DisplayAllDataViewModel
                {
                    EmployeeInfo = EmployeeInfo,
                    KraKpiOutcomeData = kraKpiOutcomeData,
                    StepMaster = StepMaster,
                    GroupedData = groupedData
                };

                return View(viewModel);
            }

        }


        // view marks based on employee code

        [HttpPost]
        public ActionResult ViewMarks(int regId)
        {
            List<MarksData> marksData;
            using (DB_STEPEntities db = new DB_STEPEntities())
            {
/*                if (!string.IsNullOrEmpty(regId))*/
                {
                    var userInfo = db.Database.SqlQuery<EmployeeInfo>(
                                    "prc_EmployeeInfoByRegID @RegID",
                                    new SqlParameter("@RegID", regId)).FirstOrDefault();

                    marksData = (from st in db.STEPs
                                 where st.REG_ID == regId
                                 select new MarksData
                                 {
                                     KPI_OUTCOME = st.KPI_OUTCOME,
                                     Marks_Achieved = st.Marks_Achieved ?? 0
                                 }).ToList();



                    ViewBag.EmployeeCode = userInfo.Name;
                    ViewBag.EmployeeName = userInfo.EmployeeCode;
                }
/*                else
                {
                    marksData = new List<MarksData>();
                }*/

                // fetch employee name based on employeeCode
               // string employeeName = GetEmployeeName(employeeCode);


                var last2session = (db.New_Tax_Period
                                   .OrderByDescending(t => t.TaxPeriod)
                                   .Select(t => t.TaxPeriod)
                                   .Take(2)
                                   .ToList());

                ViewBag.TopTaxPeriods = last2session;
            }



            return View(marksData);
        }

        [HttpPost]
        public ActionResult SaveReportSuperComment(string comment)
        {
            int regId = Convert.ToInt32(Request.Form["regId"]);
            int updatedBy = (int)Session["RegID"];
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            string statusType = "Report Super Comment";
            string statusMessage = comment;
            DateTime updatedDate = DateTime.Now;

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var result = db.Database.SqlQuery<StatusResult>(
                    "EXEC prc_UpdateStatus @RegId, @SESSION_ID, @StatusType, @StatusValue, @StatusMessage, @Updated_date, @Updated_by",
                    new SqlParameter("@RegId", regId),
                    new SqlParameter("@SESSION_ID", sessionID),
                    new SqlParameter("@StatusType", statusType),
                    new SqlParameter("@StatusValue", DBNull.Value),
                    new SqlParameter("@StatusMessage", statusMessage),
                    new SqlParameter("@Updated_date", updatedDate),
                    new SqlParameter("@Updated_by", updatedBy)).FirstOrDefault();

                if (result.Status)
                {
                    return Json(new { success = true, message = result.Message });
                }
                else
                {
                    return Json(new { success = false, message = result.Message });
                }
            }
        }  
        
        [HttpPost]
        public ActionResult SaveUserComment(string comment)
        {
            int updatedBy = (int)Session["RegID"];
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());
            string statusType = "Employee Comment";
            string statusMessage = comment;
            DateTime updatedDate = DateTime.Now;

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var result = db.Database.SqlQuery<StatusResult>(
                    "EXEC prc_UpdateStatus @RegId, @SESSION_ID, @StatusType, @StatusValue, @StatusMessage, @Updated_date, @Updated_by",
                    new SqlParameter("@RegId", updatedBy),
                    new SqlParameter("@SESSION_ID", sessionID),
                    new SqlParameter("@StatusType", statusType),
                    new SqlParameter("@StatusValue", DBNull.Value),
                    new SqlParameter("@StatusMessage", statusMessage),
                    new SqlParameter("@Updated_date", updatedDate),
                    new SqlParameter("@Updated_by", updatedBy)).FirstOrDefault();

                if (result.Status)
                {
                    return Json(new { success = true, message = result.Message });
                }
                else
                {
                    return Json(new { success = false, message = result.Message });
                }
            }
        }

        public ActionResult GetReportSuperComment()
        {

            int regId = (int)Session["RegID"];
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());

            string supervisorComment = "";

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                supervisorComment = db.tbl_StepMaster
                                        .Where(comment => comment.RegId == regId && comment.SESSION_ID == sessionID)
                                        .Select(comment => comment.Supervisor_Comment)
                                        .FirstOrDefault();
            }

            return Json(new { SupervisorComment = supervisorComment }, JsonRequestBehavior.AllowGet);
        } 
        
        
        public ActionResult GetUserComment()
        {

            int regId = (int)Session["RegID"];
            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());

            string userComment = "";

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                userComment = db.tbl_StepMaster
                                        .Where(comment => comment.RegId == regId && comment.SESSION_ID == sessionID)
                                        .Select(comment => comment.User_Comment)
                                        .FirstOrDefault();
            }

            return Json(new { UserComment = userComment }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetReportSuperOwnComment(int regId)
        {

            int sessionID = int.Parse(Session["SelectedTaxPeriod"].ToString());

            string supervisorComment = "";

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                supervisorComment = db.tbl_StepMaster
                                        .Where(comment => comment.RegId == regId && comment.SESSION_ID == sessionID)
                                        .Select(comment => comment.Supervisor_Comment)
                                        .FirstOrDefault();
            }

            return Json(new { SupervisorComment = supervisorComment }, JsonRequestBehavior.AllowGet);
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
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
        /*        [HttpPost]
      public JsonResult CheckAuth(int regId, int empRegId, int sessionId, string type)
               {
                   using (var db = new DB_STEPEntities())
                   {
                       var result = db.Database.SqlQuery<StatusResult>("exec prc_CheckAuth @RegId, @SESSION_ID, @Type, @EmpRegId",
                           new SqlParameter("RegId", regId),
                           new SqlParameter("SESSION_ID", sessionId),
                           new SqlParameter("Type", type),
                           new SqlParameter("EmpRegId", empRegId)
                       ).FirstOrDefault();

                       if (result != null && result.Status)
                       {
                           return Json(new { status = true, encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(empRegId.ToString()) });
                       }
                       else
                       {
                           return Json(new { status = false });
                       }
                   }
               }*/


<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
        [HttpPost]
        public JsonResult CheckAuth(int regId, int empRegId, int sessionId, string type)
        {
            using (var db = new DB_STEPEntities())
            {
                var result = db.Database.SqlQuery<StatusResult>("exec prc_CheckAuth @RegId, @SESSION_ID, @Type, @EmpRegId",
                    new SqlParameter("RegId", regId),
                    new SqlParameter("SESSION_ID", sessionId),
                    new SqlParameter("Type", type),
                    new SqlParameter("EmpRegId", empRegId)
                ).FirstOrDefault();

                if (result != null && result.Status)
                {
                    return Json(new { status = true, encryptedRegId = STEP_PORTAL.Helpers.PasswordHelper.Encrypt(empRegId.ToString()) });
                }
                else
                {
                    return Json(new { status = false });
                }
            }
        }
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9

>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
        public List<EmployeeInfo> GetEmployeeListByDeptHead(int deptHeadValue, int companyId)
        {
            using (DB_STEPEntities db = new DB_STEPEntities())
            {

                var SelectedTaxPeriod = int.Parse(Session["SelectedTaxPeriod"].ToString());


                var employees = db.Database.SqlQuery<EmployeeInfo>("exec prc_GetEmployeeByHR @RegID, @CompID",
                      new SqlParameter("@RegID", deptHeadValue),
                      new SqlParameter("@CompID", companyId)
                    ).ToList();

                return employees;
            }
        }

        // generate pdf

        [HttpPost]
        public ActionResult GeneratePdf(int? companyId)
        {
            if (!companyId.HasValue || Session["RegID"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid request");
            }

            int deptHeadValue;
            if (!int.TryParse(Session["RegID"].ToString(), out deptHeadValue))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid request");
            }

            List<EmployeeInfo> employees = GetEmployeeListByDeptHead(deptHeadValue, companyId.Value);

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 36, 36, 36, 36); 
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                Paragraph title = new Paragraph("Employee List\n\n", FontFactory.GetFont("Arial", 16, Font.BOLD));
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1.5f, 1.5f, 1.5f, 2f, 1.5f, 1.5f }); // Adjust column widths

                table.AddCell(new PdfPCell(new Phrase("Department", FontFactory.GetFont("Arial", 11, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Section", FontFactory.GetFont("Arial", 11, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Designation", FontFactory.GetFont("Arial", 11, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Name", FontFactory.GetFont("Arial", 11, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Employee Code", FontFactory.GetFont("Arial", 11, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Status", FontFactory.GetFont("Arial", 11, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });

                foreach (var employee in employees)
                {
                    table.AddCell(new PdfPCell(new Phrase(employee.Department, FontFactory.GetFont("Arial", 10))) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(employee.Section, FontFactory.GetFont("Arial", 10))) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(employee.Designation, FontFactory.GetFont("Arial", 10))) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(employee.Name, FontFactory.GetFont("Arial", 10))) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(employee.EmployeeCode, FontFactory.GetFont("Arial", 10))) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(employee.EmpStatus, FontFactory.GetFont("Arial", 10))) { HorizontalAlignment = Element.ALIGN_LEFT });
                }

                document.Add(table);
                document.Close();
                writer.Close();

                byte[] file = ms.ToArray();

                return File(file, "application/pdf", "EmployeeList.pdf");
            }
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
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
        // revert 

        [HttpPost]
        public ActionResult RevertEmployee(int regId)
        {
<<<<<<< HEAD
                //Session["ApprovalSent"] = approvalSent;
=======
<<<<<<< HEAD
                //Session["ApprovalSent"] = approvalSent;
=======
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
                var sessionId = int.Parse(Session["SelectedTaxPeriod"].ToString());
                var statusType = "UndoApproval";
                var statusValue = "Reverted";
                var statusMessage = "Approval has been reverted!";
                var updatedDate = DateTime.Now;
                int updatedBy = (int)Session["RegID"];

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var result = db.Database.SqlQuery<StatusResult>(
                    "EXEC prc_UpdateStatus @RegId, @SessionId, @StatusType, @StatusValue, @StatusMessage, @UpdatedDate, @UpdatedBy",
                    new SqlParameter("@RegId", regId),
                    new SqlParameter("@SessionId", sessionId),
                    new SqlParameter("@StatusType", statusType),
                    new SqlParameter("@StatusValue", statusValue),
                    new SqlParameter("@StatusMessage", statusMessage),
                    new SqlParameter("@UpdatedDate", updatedDate),
                    new SqlParameter("@UpdatedBy", updatedBy)
                ).FirstOrDefault();

                if (result.Status)
                {
                    return Json(new { success = true, message = result.Message });
                }
                else
                {
                    return Json(new { success = false, message = result.Message });
                }
            }

        }
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430


    }
}
