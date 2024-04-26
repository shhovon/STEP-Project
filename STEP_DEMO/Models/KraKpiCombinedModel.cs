using STEP_DEMO.Models;
using System.Collections.Generic;

public class CompositeModel
{
    public CompositeModel()
    {
        KraKpiData = new List<KraKpiOutcomeModel>();
        StepData = new List<KraKpiViewModel>();
    }

    public List<KraKpiOutcomeModel> KraKpiData { get; set; }
    public List<KraKpiViewModel> StepData { get; set; }
}
