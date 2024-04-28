using System.Collections.Generic;

public class MenuViewModel
{
    public int RoleID { get; set; }
    public IEnumerable<MenuItem> AssignedMenus { get; set; }
}
