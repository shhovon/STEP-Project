using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STEP_PORTAL.Models
{
    public class EmployeeReportModel
    {
        /*        public string Session_Name { get; set; }
                public string Name { get; set; }
                public string EmployeeCode { get; set; }
                public string Designation { get; set; }
                public string Department { get; set; }
                public string Section { get; set; }
                public string KRA { get; set; }
                public string KPI { get; set; }
                public string Outcome { get; set; }
                // public int Marks { get; set; }
                public decimal? KPI_AVG { get; set; }
                public int Marks_Achieved { get; set; }
                public decimal AVG_Marks_Achieved { get; set; }
                public decimal? Rating_Achieved { get; set; }
                public decimal? Attendance { get; set; }
                public decimal? Discipline { get; set; }
                public string Supervisor_Comment { get; set; }
                public string User_Comment { get; set; }
                public string HOD_Comment { get; set; }
                public string HOD_Propose_Promotion { get; set; }
                public int? HOD_Propose_Incr { get; set; }
                public string Final_Comment { get; set; }
                public string Final_Propose_Promotion { get; set; }
                public int? Final_Propose_Incr { get; set; }
                public DateTime? Effect_Date { get; set; }*/
        public int RegId { get; set; }
        public int ComID { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string DeptHead { get; set; }
        public string EmailID { get; set; }
        public string MobileNoPerson { get; set; }
        public int Role { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Service_Length { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public string DesigAddi { get; set; }
        public string WStatus { get; set; }
        public string EmpStatus { get; set; }
        public string StatusValue { get; set; }
    }
}