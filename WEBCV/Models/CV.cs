using System;
using System.ComponentModel.DataAnnotations;

namespace WEBCV.Models
{
    public class CV
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Skills")]
        public string Skills { get; set; }

        [Display(Name = "Education")]
        public string Education { get; set; }

        [Display(Name = "Work Experience")]
        public string WorkExperience { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}