//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace STEP_DEMO.Models
{
    using System;
    
    public partial class prc_GetKraKpiOutcomeEntry_Result
    {
        public int SESSION_ID { get; set; }
        public string SESSION_Name { get; set; }
        public Nullable<int> RegId { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public Nullable<int> KRA_ID { get; set; }
        public string KRA { get; set; }
        public Nullable<int> KPI_ID { get; set; }
        public decimal Attendance { get; set; }
        public decimal Discipline { get; set; }
        public string KPI { get; set; }
        public string Outcome { get; set; }
        public int Marks_Achieved { get; set; }
        public decimal AVG_Marks_Achieved { get; set; }
        public bool ApprovalSent { get; set; }
        public bool Lock { get; set; }
        public Nullable<decimal> KPI_AVG { get; set; }
        public Nullable<decimal> Rating_Achieved { get; set; }
    }
}
