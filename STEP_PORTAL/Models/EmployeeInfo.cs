﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STEP_PORTAL.Models
{
    public class EmployeeInfo
    {
        public int RegId { get; set; }
        public int ComID { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string DeptHead { get; set; }
        public string EmailID { get; set; }
        public string MobileNoPerson { get; set; }
        public int Role { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Designation   { get; set; }
        public string DesigAddi { get; set; }
        public string WStatus { get; set; }
    }
}