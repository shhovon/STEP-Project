using System;

namespace STEP_DEMO.Models
{
    public class tblMenuNew
    {
        public int ID { get; set; }
        public int Main_Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public string Menu_URL { get; set; }
        public string Menu_Icon { get; set; }
        public bool YsnActive { get; set; }
    }
}
