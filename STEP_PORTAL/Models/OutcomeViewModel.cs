using Newtonsoft.Json;
using System.Collections.Generic;

namespace STEP_PORTAL.Models
{
    public class OutcomeViewModel
    {
/*        public OutcomeViewModel()
        {
            KPI_OUTCOME = new List<string>();
        }*/

        public List<int> KRA_ID { get; set; }
        public List<int> KPI_ID { get; set; }
        public List<string> KPI_OUTCOME { get; set; }
        public int REG_ID { get; set; }
        public int SESSION_ID { get; set; }
        public int Marks_Achieved { get; set; }

        [JsonProperty("formData")]
        public List<FormDataItem> FormData { get; set; }
    }

    public class FormDataItem
    {
        public string KRA { get; set; }
        public string KPI { get; set; }
        public string KPI_OUTCOME { get; set; }
    }
}

