<<<<<<< HEAD
﻿using STEP_PORTAL.Models;
=======
<<<<<<< HEAD
﻿using STEP_DEMO.Models;
using STEP_PORTAL.Models;
=======
﻿using STEP_PORTAL.Models;
>>>>>>> 77ac0839bc36eaae374018c3b9f1a7c7b530dd51
>>>>>>> 16c973996a25fd34500bf630f963ec9cb42136ce
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
<<<<<<< HEAD
=======

>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
>>>>>>> cd15dc3f4cb7dd500e30d1acd2bff531d2316ede
>>>>>>> 5a2d9da693a1e9b71812f71ec6aaa58543fb7baf
>>>>>>> db326d8cd59dceab5db09537dfd01a74afa6f2df
>>>>>>> 77ac0839bc36eaae374018c3b9f1a7c7b530dd51
>>>>>>> 16c973996a25fd34500bf630f963ec9cb42136ce
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
=======
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
>>>>>>> cd15dc3f4cb7dd500e30d1acd2bff531d2316ede
>>>>>>> 5a2d9da693a1e9b71812f71ec6aaa58543fb7baf
>>>>>>> db326d8cd59dceab5db09537dfd01a74afa6f2df
>>>>>>> 77ac0839bc36eaae374018c3b9f1a7c7b530dd51
>>>>>>> 16c973996a25fd34500bf630f963ec9cb42136ce
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
>>>>>>> cd15dc3f4cb7dd500e30d1acd2bff531d2316ede
>>>>>>> 5a2d9da693a1e9b71812f71ec6aaa58543fb7baf
>>>>>>> db326d8cd59dceab5db09537dfd01a74afa6f2df
>>>>>>> 77ac0839bc36eaae374018c3b9f1a7c7b530dd51
>>>>>>> 16c973996a25fd34500bf630f963ec9cb42136ce
        }        
        
        public List<tblSpecial_Factor> getSpecialFactors(int regId, int sessionID)
        {

            List<tblSpecial_Factor> getSpecialFactors = new List<tblSpecial_Factor>();

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                getSpecialFactors = db.tblSpecial_Factor.Where(m => m.Reg_Id == regId && m.Session_Id == sessionID).ToList();


/*                if (data != null)
                {
                    getSpecialFactors = data;
                }*/


            }

            return getSpecialFactors;

        }

        public List<tblTraining_Need> getTrainingData(int regId, int sessionID)
        {

            List<tblTraining_Need> getTrainingData = new List<tblTraining_Need>();

            using (DB_STEPEntities db = new DB_STEPEntities())
            {
                getTrainingData = db.tblTraining_Need.Where(m => m.Reg_Id == regId && m.Session_Id == sessionID).ToList();


                /*                if (data != null)
                                {
                                    getSpecialFactors = data;
                                }*/


            }

            return getTrainingData;

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
<<<<<<< HEAD
=======
=======
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
>>>>>>> cd15dc3f4cb7dd500e30d1acd2bff531d2316ede
>>>>>>> 5a2d9da693a1e9b71812f71ec6aaa58543fb7baf
>>>>>>> db326d8cd59dceab5db09537dfd01a74afa6f2df
>>>>>>> 77ac0839bc36eaae374018c3b9f1a7c7b530dd51
>>>>>>> 16c973996a25fd34500bf630f963ec9cb42136ce
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
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
>>>>>>> cd15dc3f4cb7dd500e30d1acd2bff531d2316ede
>>>>>>> 5a2d9da693a1e9b71812f71ec6aaa58543fb7baf
>>>>>>> db326d8cd59dceab5db09537dfd01a74afa6f2df
>>>>>>> 77ac0839bc36eaae374018c3b9f1a7c7b530dd51
>>>>>>> 16c973996a25fd34500bf630f963ec9cb42136ce

    }
}