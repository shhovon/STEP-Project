using STEP_PORTAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STEP_DEMO.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        public ActionResult Index()
        {
            return View();
        }


        public StatusResult CheckAuth(int RegId, int sessionID,string Type, int EmpRegId)
        {

            StatusResult StatusResult = new StatusResult();


            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                 var data = db.Database.SqlQuery<StatusResult>(
                        "exec prc_CheckAuth @RegId, @SESSION_ID, @Type, @EmpRegId",
                        new SqlParameter("@RegId", RegId),
                        new SqlParameter("@SESSION_ID", sessionID),
                        new SqlParameter("@Type", Type),
                        new SqlParameter("@EmpRegId", EmpRegId)
                    ).FirstOrDefault();

                if (data != null)
                {
                    StatusResult = data;
                }


            }

            return StatusResult;

        }

        public List<New_Tax_Period> GetTax_Period()
        {

            List<New_Tax_Period> New_Tax_Period = new List<New_Tax_Period>();


            using (DB_STEPEntities db = new DB_STEPEntities())
            {                
                New_Tax_Period = db.Database.SqlQuery<New_Tax_Period>("prc_GetTax_Period").ToList();
            }

            return New_Tax_Period;

        }




        public EmployeeInfo GetEmployeeInfoByRegID(int RegId)
        {

            EmployeeInfo EmployeeInfo = new EmployeeInfo();


            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                var data = db.Database.SqlQuery<EmployeeInfo>(
                            "prc_EmployeeInfoByRegID @RegID",
                            new SqlParameter("@RegID", RegId)).FirstOrDefault();

                if (data != null)
                {
                    EmployeeInfo = data;
                }


            }

            return EmployeeInfo;

        }



        public List<KraKpiOutcomeModel> GetKraKpiOutcomeData(int RegId,int sessionID)
        {

            List<KraKpiOutcomeModel> kraKpiOutcomeData = new List<KraKpiOutcomeModel>();


            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiOutcomeData @RegId, @SESSION_ID",
             new SqlParameter("@RegId", RegId),
             new SqlParameter("@SESSION_ID", sessionID)).ToList();

            }

            return kraKpiOutcomeData;

        }



        public List<KraKpiOutcomeModel> GetKraKpiData(int RegId, int sessionID)
        {

            List<KraKpiOutcomeModel> kraKpiOutcomeData = new List<KraKpiOutcomeModel>();


            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                kraKpiOutcomeData = db.Database.SqlQuery<KraKpiOutcomeModel>("prc_GetKraKpiData @RegId, @SESSION_ID",
             new SqlParameter("@RegId", RegId),
             new SqlParameter("@SESSION_ID", sessionID)).ToList();

            }

            return kraKpiOutcomeData;

        }



        public tbl_StepMaster GetStepMaster(int RegId, int sessionID)
        {

            tbl_StepMaster StepMaster = new tbl_StepMaster();

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
               var data = db.Database.SqlQuery<tbl_StepMaster>("prc_GetStepMaster @RegId, @SESSION_ID",
                        new SqlParameter("@RegId", RegId),
                        new SqlParameter("@SESSION_ID", sessionID)).FirstOrDefault();

                if (data != null)
                {
                    StepMaster = data;
                }
            }           

            return StepMaster;
        }



        public List<DesignationModel> GetDesignations()
        {         

            List<DesignationModel> DesignationModel = new List<DesignationModel>();

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                DesignationModel = db.Database.SqlQuery<DesignationModel>("prc_GetDesignations").ToList();

            }

            return DesignationModel;

        }


<<<<<<< HEAD
        public StatusResult UpdateRating(int regId, int sessionID)
        {

            StatusResult StatusResult = new StatusResult();

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
              var data = db.Database.SqlQuery<StatusResult>(
                           "exec prc_UpdateRating @RegId, @SESSION_ID",
                           new SqlParameter("@RegId", regId),
                           new SqlParameter("@SESSION_ID", sessionID)).FirstOrDefault();


                if (data != null)
                {
                    StatusResult = data;
                }


            }

            return StatusResult;

        }




=======
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9

    }
}