using STEP_PORTAL.Models;
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
using STEP_PORTAL.Models;
>>>>>>> 9137fd13b8647680fe231d4a419dc66726002065
>>>>>>> 9eef72775c1358dcd3be9836d37cf6dc56b6e5c9
>>>>>>> e23d7850cc7b2ead710a29effff713f83be27a86
>>>>>>> d9006b5ac04096af6a96775f4d6667f2d621d430
>>>>>>> cd15dc3f4cb7dd500e30d1acd2bff531d2316ede
>>>>>>> 5a2d9da693a1e9b71812f71ec6aaa58543fb7baf
>>>>>>> db326d8cd59dceab5db09537dfd01a74afa6f2df
>>>>>>> 16c973996a25fd34500bf630f963ec9cb42136ce
using System.Collections.Generic;

public class DisplayAllDataViewModel
{

    public EmployeeInfo EmployeeInfo { get; set; }
    public tbl_StepMaster StepMaster { get; set; }    
    public KraKpiOutcomeModel KraKpiOutcome { get; set; }
    public List<KraKpiOutcomeModel> KraKpiOutcomes { get; set; }
    public List<DesignationModel> Designations { get; set; }
    public List<KraKpiOutcomeModel> KraKpiData { get; set; }
    public List<KraKpiViewModel> GroupedData { get; set; }
    public List<KraKpiViewModel> NextYearGroupedData { get; set; }
    public List<KraKpiOutcomeModel> KraKpiOutcomeData { get; set; }
    public List<KraKpiOutcomeModel> NextYearKraKpiOutcomeData { get; set; }
    public List<KraKpiViewModel> StepData { get; set; }
    public List<tblSpecial_Factor> SpecialFactors { get; set; }
    public List<tblTraining_Need> TrainingNeed { get; set; }

    public bool ApprovalSent { get; set; }

    public string SupervisorComment { get; set; }
    public string UserComment { get; set; }
    public string HOOcomment { get; set; }
    public string HOOpromotion { get; set; }
    public int HOOincrement { get; set; }
}
