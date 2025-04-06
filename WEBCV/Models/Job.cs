using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBCV.Models;

namespace WEBCV.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Requirements { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Salary { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string JobType { get; set; } // Full-time, Part-time, Freelance, etc.

        public DateTime PostedDate { get; set; }

        [Required]
        public DateTime DeadlineDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string EmployerId { get; set; }

        [ForeignKey("EmployerId")]
        public virtual ApplicationUser Employer { get; set; }

        public virtual ICollection<JobApplication> Applications { get; set; }
    }
}