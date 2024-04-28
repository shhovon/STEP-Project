using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STEP_PORTAL.Models
{



    public class HomeData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }



    public partial class New_Month
    {
        public byte SLNo { get; set; }
        public string Month { get; set; }
    }


    public class JobCard
    {
        public Nullable<System.DateTime> prDate { get; set; }
        public string TimeIn { get; set; }
        public string LateHrs { get; set; }
        public string TimeOut { get; set; }
        public string DailyHrs { get; set; }
        public string ValidOT { get; set; }
        public string RestOT { get; set; }
        public string ExtraOT { get; set; }
        public string Status { get; set; }
        public string Shift { get; set; }
        public string ShiftN { get; set; }
        public decimal TransAllow { get; set; }
    }


    public class UserMenu
    {
        public Nullable<System.Int32> ID { get; set; }
        public string Main_Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public string Menu_URL { get; set; }
        public string Menu_Icon { get; set; }
        public bool YsnActive { get; set; }
    }


    public class JobCardParameter
    {
        public string CompID { get; set; }
        public string EmpCode { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
    }


    public class EmailModel
    {
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }


    public class LeaveSummary
    {

        public string LeaveType { get; set; }
        public decimal Total { get; set; }
        public decimal Enjoy { get; set; }
        public decimal Balance { get; set; }

    }
    public class LeaveSummaryParameter
    {
        public string CompID { get; set; }
        public string EmpCode { get; set; }
        public string Year { get; set; }
    }

    public class LeaveEnjoy
    {
        public DateTime FrDate { get; set; }
        public DateTime ToDate { get; set; }
        public string LType { get; set; }
        public decimal LDays { get; set; }
        public string Reasons { get; set; }
        //public string EmpCode { get; set; }

    }

    public class LeaveCardParameter
    {
        public string RegId { get; set; }
        public string Year { get; set; }
    }


    public partial class tbl_EmpAttenApp
    {
        public int RegId { get; set; }
        public string Name { get; set; }
        public System.DateTime AppliedDate { get; set; }
        public System.DateTime prDate { get; set; }
        public string TimeIn { get; set; }
        public string LateHrs { get; set; }
        public string TimeOut { get; set; }
        public string DailyHrs { get; set; }
        public string ValidOT { get; set; }
        public string RestOT { get; set; }
        public string ExtraOT { get; set; }
        public string Status { get; set; }
        public string Shift { get; set; }
        public string ShiftN { get; set; }
        public Nullable<decimal> TransAllow { get; set; }
        public string Reasons { get; set; }

    }


    public class ProcedureReturnStatus
    {
        public bool Status { get; set; }
        public string Message { get; set; }

    }

    public class Album_Insert
    {
        public int AlbumID { get; set; }
        public string Album_Name { get; set; }
        public Nullable<System.DateTime> Created_date { get; set; }
        public string Created_By { get; set; }

    }

    public class UploadedFiles
    {

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public int FileSize { get; set; }

    }
    public partial class AlbumImage_Details
    {
        public int CategoryID { get; set; }
        public int AlbumID { get; set; }
        public string Album_Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int TotalPic { get; set; }
        public int TotalSize { get; set; }
        public int Published { get; set; }
    }



    public partial class AlbumCategory_Details
    {
        public int CategoryID { get; set; }
        public string Category_Name { get; set; }


    }


    public partial class OTRequ
    {
        public int CompID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }

    }

    public class Prc_Get_OTApprove_Summary_Info
    {
        public int CompID { get; set; }
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DailyDate { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public string WStatus { get; set; }
        public string OT { get; set; }
        public string OTReque { get; set; }



    }
    public class OTApproveEditUpdate
    {
        public int CompID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string WStatus { get; set; }
        public string OTRequ { get; set; }
        public string RequstOT { get; set; }
        public string EditReasons { get; set; }




    }
    public class ProcedureOTApproveStatus
    {
        public bool Status { get; set; }
        public string Message { get; set; }

    }

    public class prc_Weekly_OT_Report_Info
    {
        public int CompID { get; set; }

        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }

        public string DEPARTMENT { get; set; }
        public Nullable<int> EMP { get; set; }
        public Nullable<int> EMP60 { get; set; }
        public Nullable<decimal> Percantage60 { get; set; }
        public Nullable<int> EMP66 { get; set; }
        public Nullable<decimal> Percantage66 { get; set; }
        public Nullable<int> EMP72 { get; set; }
        public Nullable<decimal> Percantage72 { get; set; }
        public Nullable<int> EMP80 { get; set; }
        public Nullable<decimal> Percantage80 { get; set; }
        public Nullable<int> EMP100 { get; set; }
        public Nullable<decimal> Percantage100 { get; set; }
        public Nullable<int> EMP100Plus { get; set; }
        public Nullable<decimal> Percantage100Plus { get; set; }
        public Nullable<int> Friday_Work { get; set; }
        public Nullable<decimal> PercantageFwork { get; set; }
        public Nullable<decimal> AVGBuyerWorkHr { get; set; }
        public Nullable<decimal> AVGWorkHr { get; set; }
        public Nullable<decimal> MaxWorkHr { get; set; }




    }

}