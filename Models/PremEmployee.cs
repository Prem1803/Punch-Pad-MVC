using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace PunchPad.Models
{
    public partial class PremEmployee
    {
        public PremEmployee()
        {
            InverseManager = new HashSet<PremEmployee>();
            PremAttendanceEmps = new HashSet<PremAttendance>();
            PremAttendanceManagers = new HashSet<PremAttendance>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter the name")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Please enter the department")]
        public string? Department { get; set; }
        [Required(ErrorMessage = "Please enter the date of joining")]
        [Display(Name = "Date of Joining")]
        public DateTime? Doj { get; set; }
        [Required(ErrorMessage = "Please enter the username")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Please enter the password")]
        public string? Password { get; set; }
        public int? ManagerId { get; set; }

        public virtual PremEmployee? Manager { get; set; }
        public virtual ICollection<PremEmployee> InverseManager { get; set; }
        public virtual ICollection<PremAttendance> PremAttendanceEmps { get; set; }
        public virtual ICollection<PremAttendance> PremAttendanceManagers { get; set; }
    }
}
