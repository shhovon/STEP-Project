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
    
    public partial class tblTraining_Need
    {
        public int Train_Id { get; set; }
        public Nullable<int> Session_Id { get; set; }
        public Nullable<int> Reg_Id { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> By_When { get; set; }
        public string Train_Type { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Created_date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Updated_date { get; set; }
        public string Updated_by { get; set; }
    }
}