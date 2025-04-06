using Microsoft.AspNetCore.Identity;
using System;

namespace WEBCV.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime DateOfBirth { get; set; }
        public string UserType { get; set; } // "JobSeeker", "Employer", "Admin"
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}