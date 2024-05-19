using STEP_DEMO.Models;
using STEP_PORTAL.Models;
using System.Collections.Generic;

public class DisplayAllDataViewModel
{
    public KraKpiOutcomeModel KraKpiOutcome { get; set; }
    public List<KraKpiOutcomeModel> KraKpiOutcomes { get; set; }
    public List<DesignationModel> Designations { get; set; }
    public List<KraKpiOutcomeModel> KraKpiData { get; set; }
    public List<KraKpiViewModel> GroupedData { get; set; }
    public List<KraKpiOutcomeModel> KraKpiOutcomeData { get; set; }
    public List<KraKpiViewModel> StepData { get; set; }
    public List<tblSpecial_Factor> SpecialFactors { get; set; }
    public List<tblTraining_Need> TrainingNeed { get; set; }

    public string SupervisorComment { get; set; }
    public string UserComment { get; set; }
    public string HOOcomment { get; set; }
    public string HOOpromotion { get; set; }
    public int HOOincrement { get; set; }
}
