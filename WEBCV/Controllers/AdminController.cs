using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using WEBCV.Models;
using WEBCV.ViewModels;
using Microsoft.EntityFrameworkCore;
using WEBCV.Models;

namespace WEBCV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    UserType = roles.FirstOrDefault() ?? "Unknown",
                    IsActive = user.IsActive,
                    CreatedDate = user.CreatedDate
                });
            }

            return View(userViewModels);
        }

        // GET: Admin/Jobs
        public async Task<IActionResult> Jobs()
        {
            var jobs = await _context.Jobs
                .Include(j => j.Employer)
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();

            return View(jobs);
        }

        // POST: Admin/ToggleJobStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleJobStatus(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            job.IsActive = !job.IsActive;
            job.UpdatedDate = DateTime.Now;

            _context.Update(job);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Jobs));
        }

        // POST: Admin/ToggleUserStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Don't allow deactivating your own account
            if (User.Identity.Name == user.Email)
            {
                TempData["Error"] = "Bạn không thể vô hiệu hóa tài khoản của chính mình!";
                return RedirectToAction(nameof(Users));
            }

            user.IsActive = !user.IsActive;

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Users));
        }

        // GET: Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var dashboardViewModel = new AdminViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalJobs = await _context.Jobs.CountAsync(),
                TotalApplications = await _context.JobApplications.CountAsync(),

                JobSeekers = await _userManager.GetUsersInRoleAsync("JobSeeker"),
                Employers = await _userManager.GetUsersInRoleAsync("Employer"),

                RecentJobs = await _context.Jobs
                    .OrderByDescending(j => j.PostedDate)
                    .Take(5)
                    .ToListAsync(),

                RecentApplications = await _context.JobApplications
                    .Include(a => a.Job)
                    .Include(a => a.User)
                    .OrderByDescending(a => a.AppliedDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboardViewModel);
        }

        // GET: Admin/DashboardSummary
        [HttpGet]
        public IActionResult DashboardSummary()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = 100, // Thay đổi thành giá trị thực tế từ cơ sở dữ liệu của bạn
                TotalJobs = 50,   // Thay đổi thành giá trị thực tế từ cơ sở dữ liệu của bạn
                TotalResumes = 75, // Thay đổi thành giá trị thực tế từ cơ sở dữ liệu của bạn
                TotalApplications = 200 // Thay đổi thành giá trị thực tế từ cơ sở dữ liệu của bạn
            };

            return View(model);
        }
    }
}