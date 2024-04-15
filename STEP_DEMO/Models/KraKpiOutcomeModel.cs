using System.Collections.Generic;

public class KraKpiOutcomeModel
    {
        public int KRA_ID { get; set; }
        public int KPI_ID { get; set; }
/*        public string KRA { get; set; }
        public string KPI { get; set; }
        public string KPI_OUTCOME { get; set; }
*/
    public string KRA { get; set; }
/*    public List<string> KPIs { get; set; } = new List<string>();
    public List<string> Outcomes { get; set; } = new List<string>();*/
    public string KPI { get; set; }
    public string EmployeeCode { get; set; }
    public string Name { get; set; }
    public string Designation { get; set; }
    public string Outcome { get; set; }
    public List<string> KPIs { get; set; }
    public List<string> Outcomes { get; set; }
    public string KPI_OUTCOME { get; set; }
    public int Marks_Achieved { get; set; }

}
