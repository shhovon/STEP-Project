using System;
using System.Collections.Generic;

    public class KraKpiViewModel
    {
        public List<string> KPIOutcomes { get; set; }
/*    public List<List<string>> KPIOutcomes { get; set; }*/

        public List<string> KPIList { get; set; }
        public string KRA { get; set; }
        public string KRA1 { get; set; }
        public List<DateTime?> Durations { get; set; }
        public int KRA_ID { get; set; }
        public List<KPIViewModel> KPIIIs { get; set; }
        public List<int> KRA_IDs { get; set; }
        public List<List<int>> KPI_IDs { get; set; }
    public int SessionId { get; set; }
    public int KPI_ID { get; set; }
        public int kraId { get; set; }
        public int kpiId { get; set; }
        public string KPI { get; set; }
        public string KPI1 { get; set; }
        public string KPIOutcome { get; set; }
        public string KPI_OUTCOME { get; set; }
        public decimal TotalAverage { get; set; }
        public List<string> KRAs { get; set; }
        public List<string> KPIIs { get; set; }
        public List<int> KPI_IDss { get; set; }
        public List<List<string>> KPIs { get; set; }
        public string Section_Name { get; set; }
        public string Remarks { get; set; }
        public DateTime? Duration { get; set; }
        public List<string> AllRemarks { get; set; }

        public List<string> DefaultKRAs { get; set; }
        public List<List<string>> DefaultKPIs { get; set; }
        public List<DateTime?> DefaultDurations { get; set; }
        public List<KraKpiData> KraKpiData { get; set; }


    public KraKpiViewModel()
        {
            KRAs = new List<string>();
            KPIs = new List<List<string>>();
            KPI_IDs = new List<List<int>>();
            KPIIs = new List<string>();
            //KPIOutcomes = new List<List<string>>();
            KRA_IDs = new List<int>();
            KPI_IDs = new List<List<int>>();
            Durations = new List<DateTime?>();
    }

    public class KPIViewModel
    {
        public int KPI_ID { get; set; }
        public string KPI { get; set; }

    }
}


