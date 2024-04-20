using STEP_DEMO.Models;
using System.Collections.Generic;

public class DisplayAllDataViewModel
{
    public KraKpiOutcomeModel KraKpiOutcome { get; set; } 
    public List<KraKpiOutcomeModel> KraKpiOutcomes { get; set; }
    public tblSpecial_Factor SpecialFactors { get; set; }
    public tblTraining_Need TrainingNeed { get; set; }
}
