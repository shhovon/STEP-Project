using System;
using System.Collections.Generic;

    public class KraKpiViewModel
    {
        public List<string> KPIOutcomes { get; set; }
<<<<<<< HEAD
        public List<int> Marks { get; set; }
        public List<decimal> AVG_Marks_List { get; set; }
        public List<decimal?> KPI_AVG_List { get; set; }
    /*    public List<List<string>> KPIOutcomes { get; set; }*/

    public List<string> KPIList { get; set; }
        public string KRA { get; set; }
        public string KRA1 { get; set; }
=======
/*    public List<List<string>> KPIOutcomes { get; set; }*/

        public List<string> KPIList { get; set; }
        public string KRA { get; set; }
<<<<<<< HEAD
        public string KRA1 { get; set; }
=======
<<<<<<< HEAD
        public string KRA1 { get; set; }
=======
<<<<<<< HEAD
        public string KRA1 { get; set; }
=======
<<<<<<< HEAD
        public string KRA1 { get; set; }
=======
<<<<<<< HEAD
        public string KRA1 { get; set; }
=======
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
>>>>>>> cd15dc3f4cb7dd500e30d1acd2bff531d2316ede
        public List<DateTime?> Durations { get; set; }
        public int KRA_ID { get; set; }
        public List<KPIViewModel> KPIIIs { get; set; }
        public List<int> KRA_IDs { get; set; }
        public List<List<int>> KPI_IDs { get; set; }
<<<<<<< HEAD
        public int SessionId { get; set; }
        public int KPI_ID { get; set; }
=======
<<<<<<< HEAD
    public int SessionId { get; set; }
    public int KPI_ID { get; set; }
=======
<<<<<<< HEAD
    public int SessionId { get; set; }
    public int KPI_ID { get; set; }
=======
<<<<<<< HEAD
    public int SessionId { get; set; }
    public int KPI_ID { get; set; }
=======
<<<<<<< HEAD
    public int SessionId { get; set; }
    public int KPI_ID { get; set; }
=======
<<<<<<< HEAD
    public int SessionId { get; set; }
    public int KPI_ID { get; set; }
=======
        public int KPI_ID { get; set; }
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
>>>>>>> cd15dc3f4cb7dd500e30d1acd2bff531d2316ede
        public int kraId { get; set; }
        public int kpiId { get; set; }
        public string KPI { get; set; }
        public string KPI1 { get; set; }
        public string KPIOutcome { get; set; }
        public string KPI_OUTCOME { get; set; }
        public decimal TotalAverage { get; set; }
        public List<decimal?> Attendance { get; set; }
        public List<decimal?> Discipline { get; set; }
        public decimal AVG_Marks_Achieved { get; set; }
        public List<string> KRAs { get; set; }
        public List<string> KPIIs { get; set; }
        public List<int> KPI_IDss { get; set; }
        public List<List<string>> KPIs { get; set; }
        public string Section_Name { get; set; }
        public string Remarks { get; set; }
<<<<<<< HEAD
        public DateTime? Duration { get; set; }
=======
<<<<<<< HEAD
        public DateTime? Duration { get; set; }
=======
<<<<<<< HEAD
        public DateTime? Duration { get; set; }
=======
<<<<<<< HEAD
        public DateTime? Duration { get; set; }
=======
<<<<<<< HEAD
        public DateTime? Duration { get; set; }
=======
<<<<<<< HEAD
        public DateTime? Duration { get; set; }
=======
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
>>>>>>> cd15dc3f4cb7dd500e30d1acd2bff531d2316ede
        public List<string> AllRemarks { get; set; }

        public List<string> DefaultKRAs { get; set; }
        public List<List<string>> DefaultKPIs { get; set; }
        public List<DateTime?> DefaultDurations { get; set; }
<<<<<<< HEAD
        public List<KraKpiData> KraKpiData { get; set; }


=======
<<<<<<< HEAD
        public List<KraKpiData> KraKpiData { get; set; }


=======
<<<<<<< HEAD
        public List<KraKpiData> KraKpiData { get; set; }


=======
<<<<<<< HEAD
        public List<KraKpiData> KraKpiData { get; set; }


=======
<<<<<<< HEAD
        public List<KraKpiData> KraKpiData { get; set; }


=======
<<<<<<< HEAD
        public List<KraKpiData> KraKpiData { get; set; }


=======
>>>>>>> b2b30358692f5e62f581fbf040a7526cf4477f93
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
>>>>>>> cd15dc3f4cb7dd500e30d1acd2bff531d2316ede
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


