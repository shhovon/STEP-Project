using STEP_DEMO.Models;
using STEP_PORTAL.Models;
using System.Collections.Generic;

public class DisplayAllDataViewModel
{
    public KraKpiOutcomeModel KraKpiOutcome { get; set; }
    public List<KraKpiOutcomeModel> KraKpiOutcomes { get; set; }
    public List<KraKpiOutcomeModel> KraKpiData { get; set; }
    public List<KraKpiViewModel> GroupedData { get; set; }
    public List<KraKpiOutcomeModel> KraKpiOutcomeData { get; set; }
    public List<KraKpiViewModel> StepData { get; set; }
    public tblSpecial_Factor SpecialFactors { get; set; }
    public tblTraining_Need TrainingNeed { get; set; }
}
