using STEP_PORTAL.Models;
using System.Collections.Generic;

public class EmployeeSessionViewModelClass
{
    public List<EmployeeInfo> Employees { get; set; }
    public EmployeeInfo EmployeeInfo { get; set; }
    public List<string> TopTaxPeriods { get; set; }
    public List<DesignationModel> Designations { get; set; }
}

