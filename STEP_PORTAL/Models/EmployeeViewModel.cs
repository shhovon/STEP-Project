using System.Collections.Generic;

public class EmployeeViewModel
{
    public string Name { get; set; }
    public string EmployeeCode { get; set; }
    public string Department { get; set; }
    public string Section { get; set; }
    public string Designation { get; set; }
    public string FullNameWithCodeAndDesignation
    {
        get { return $"{EmployeeCode} - {Name} ({Designation})"; }
    }
}
