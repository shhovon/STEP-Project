//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace STEP_PORTAL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblUserRole
    {
        public int ID { get; set; }
        public Nullable<int> RegId { get; set; }
        public Nullable<int> Role { get; set; }
        public string ComIDs { get; set; }
        public Nullable<bool> YSNActive { get; set; }
        public Nullable<System.DateTime> Created_date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Updated_date { get; set; }
        public string Updated_by { get; set; }
    }
}
