//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace attendence_sys_api.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LeaveRequest
    {
        public int LeaveRequestID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<System.DateTime> LeaveDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    
        public virtual User User { get; set; }
    }
}
