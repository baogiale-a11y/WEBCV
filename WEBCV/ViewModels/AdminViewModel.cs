using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WEBCV.Models;
using Microsoft.AspNetCore.Identity;
using WEBCV.Models;

namespace WEBCV.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string UserType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class AdminViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalJobs { get; set; }
        public int TotalApplications { get; set; }

        public IList<ApplicationUser> JobSeekers { get; set; }
        public IList<ApplicationUser> Employers { get; set; }

        public List<Job> RecentJobs { get; set; }
        public List<JobApplication> RecentApplications { get; set; }
    }
}