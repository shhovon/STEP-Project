/*using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STEP_PORTAL.Models
{
    public class KraKpiViewModel
    {
        public int KRA_ID { get; set; }
        public string KRA { get; set; }
        public int KPI_ID { get; set; }
        public string KPI { get; set; }
        public List<string> KPIs { get; set; }
        public List<string> KRAs { get; set; }
    }
}*/

using System.Collections.Generic;

namespace STEP_PORTAL.Models
{
    public class KraKpiViewModel
    {
        public string KRA { get; set; }
        public int KRA_ID { get; set; }
        public int KPI_ID { get; set; }
        public string KPI { get; set; }
        public string KPI_OUTCOME { get; set; }
        public List<string> KRAs { get; set; }
        public List<string> KPIIs { get; set; }
        public List<int> KPI_IDs { get; set; }
        public List<List<string>> KPIs { get; set; }
        public string Section_Name { get; set; }
        public KraKpiViewModel()
        {
            KRAs = new List<string>();
            KPIs = new List<List<string>>();
            KPI_IDs = new List<int>();
        }
    }
}


