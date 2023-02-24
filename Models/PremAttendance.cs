using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace PunchPad.Models
{
    public partial class PremAttendance
    {
        public int Id { get; set; }
        public int? EmpId { get; set; }
        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }
        [Display(Name = "Date")]
        public DateTime? AttendanceDay { get; set; }
        [Display(Name = "Punch Time")]
        public DateTime? AttendanceTime { get; set; }
        [Display(Name = "Type")]
        public string? AttendanceType { get; set; }
        [Display(Name = "Status")]
        public string? AttendanceStatus { get; set; }

        public virtual PremEmployee? Emp { get; set; }
        public virtual PremEmployee? Manager { get; set; }
    }
}
