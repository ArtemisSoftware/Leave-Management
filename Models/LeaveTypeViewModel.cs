using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Models
{
    public class LeaveTypeViewModel
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Default Number Of Days")]
        public int DefaultDays { get; set; }

        [Display(Name="Date Created")]
        public DateTime? DateCreated { get; set; }
    }



}
