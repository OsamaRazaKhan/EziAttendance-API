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
    
    public partial class Attendance
    {
        public int AttendanceID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Status { get; set; }
        public Nullable<bool> LeaveRequest { get; set; }
    
        public virtual User User { get; set; }
    }
}
