using System.Collections.Generic;

public class KraKpiOutcomeModel
    {
        public int KRA_ID { get; set; }
        public int RegId { get; set; }
        public int KPI_ID { get; set; }
        public int kraId { get; set; }
        public int kpiId { get; set; }
        public int SESSION_ID { get; set; }
        public string KRA { get; set; }
        public string KPI { get; set; }
        public int REG_ID { get; set; }
        public decimal AVG_Marks_Achieved { get; set; }
        public string EmployeeCode { get; set; }
        public string Section_Name { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Outcome { get; set; }
        public List<string> KPIs { get; set; }
        public List<string> Outcomes { get; set; }
        public string KPI_OUTCOME { get; set; }
        public string KPIOutcome { get; set; }
        public int Marks_Achieved { get; set; }
        public bool? ApprovalSent { get; set; }
        public bool? Lock { get; set; }
        public string ReportSuperComment { get; set; }
        public decimal? Attendance { get; set; }
        public decimal? Discipline { get; set; }
        public int Marks { get; set; }
        public int? SelectedMarks { get; set; }

}
