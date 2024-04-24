using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STEP_DEMO.Models
{
    public class User_RegistrationInfo
    {
        public int ID { get; set; }
        public int RegId { get; set; }
        public string Password { get; set; }
        public Nullable<int> DeptHead { get; set; }
        public Nullable<int> ReportSuper { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string BloodGroup { get; set; }
        public string EmailID { get; set; }
        public string EmailID2 { get; set; }
        public string MobileNoPerson { get; set; }
        public string MobileNoHome { get; set; }
        public string PhoneExt { get; set; }
        public string Picture { get; set; }
        public string Role { get; set; }
        public bool Registerd { get; set; }
        public Nullable<int> LoginReq { get; set; }
        public bool YsnActive { get; set; }
        public Nullable<System.DateTime> last_login_date { get; set; }
        public string last_login_Mac { get; set; }
        public Nullable<System.DateTime> Created_date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Updated_date { get; set; }
        public string Updated_by { get; set; }






    }
}