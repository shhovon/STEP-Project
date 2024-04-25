using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STEP_DEMO.Models
{
    public class SpecialFactorViewModel
    {
        public string Description { get; set; }
        public int Session_Id { get; set; }
        public int Reg_Id { get; set; }
        public List<string> AddedDescriptions { get; set; }
    }
}
