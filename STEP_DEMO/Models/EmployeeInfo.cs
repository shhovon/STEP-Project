using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STEP_DEMO.Models
{
    public class EmployeeInfo
    {
        public long RegId { get; set; }
        public byte ComID { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string DeptHead { get; set; }
        public string EmailID { get; set; }
        public int MobileNoPerson { get; set; }
        public string Role { get; set; }

    }
}