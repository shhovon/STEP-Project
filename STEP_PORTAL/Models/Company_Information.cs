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
    
    public partial class Company_Information
    {
        public byte ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public Nullable<float> Basic { get; set; }
        public bool BasicM { get; set; }
        public Nullable<float> HRent { get; set; }
        public Nullable<float> WBASIC { get; set; }
        public Nullable<float> WHRent { get; set; }
        public bool HRentM { get; set; }
        public Nullable<float> FoodPay { get; set; }
        public bool FoodPayM { get; set; }
        public Nullable<float> Bonus { get; set; }
        public bool BonusM { get; set; }
        public Nullable<float> PFund { get; set; }
        public bool PFundM { get; set; }
        public bool NZN { get; set; }
        public Nullable<float> Medical { get; set; }
        public Nullable<float> WMedical { get; set; }
        public string NameBangla { get; set; }
        public string AddressBangla { get; set; }
        public string SignedBy { get; set; }
        public string EmpImageLocation { get; set; }
        public string ServerName { get; set; }
        public string DBName { get; set; }
        public string DBPrefix { get; set; }
        public byte IsClose { get; set; }
        public string ReportPath { get; set; }
    }
}
