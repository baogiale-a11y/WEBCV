using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEBCV.Models;
using WEBCV.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WEBCV.Models;

namespace WEBCV.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Job
        public async Task<IActionResult> Index(string searchString, string location, string jobType, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentLocation"] = location;
            ViewData["CurrentJobType"] = jobType;
            ViewData["CurrentSort"] = sortOrder;

            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["SalarySortParm"] = sortOrder == "Salary" ? "salary_desc" : "Salary";

            var jobs = from j in _context.Jobs
                       where j.IsActive
                       select j;

            if (!String.IsNullOrEmpty(searchString))
            {
                jobs = jobs.Where(j => j.Title.Contains(searchString) ||
                                        j.Description.Contains(searchString) ||
                                        j.CompanyName.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(location))
            {
                jobs = jobs.Where(j => j.Location.Contains(location));
            }

            if (!String.IsNullOrEmpty(jobType))
            {
                jobs = jobs.Where(j => j.JobType == jobType);
            }

            switch (sortOrder)
            {
                case "title_desc":
                    jobs = jobs.OrderByDescending(j => j.Title);
                    break;
                case "Date":
                    jobs = jobs.OrderBy(j => j.PostedDate);
                    break;
                case "date_desc":
                    jobs = jobs.OrderByDescending(j => j.PostedDate);
                    break;
                case "Salary":
                    jobs = jobs.OrderBy(j => j.Salary);
                    break;
                case "salary_desc":
                    jobs = jobs.OrderByDescending(j => j.Salary);
                    break;
                default:
                    jobs = jobs.OrderByDescending(j => j.PostedDate);
                    break;
            }

            return View(await jobs.ToListAsync());
        }

        // GET: Job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Employer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            var viewModel = new JobDetailsViewModel
            {
                Job = job,
                HasApplied = false,
                UserResumes = new List<Resume>()
            };

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Check if user has already applied
                viewModel.HasApplied = await _context.JobApplications
                    .AnyAsync(a => a.JobId == id && a.UserId == userId);

                // Get user's resumes for applying
                if (User.IsInRole("JobSeeker"))
                {
                    viewModel.UserResumes = await _context.Resumes
                        .Where(r => r.UserId == userId)
                        .ToListAsync();
                }
            }

            return View(viewModel);
        }
    }
}