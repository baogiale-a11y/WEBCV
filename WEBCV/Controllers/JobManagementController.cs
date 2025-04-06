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
using System.Security.Claims;
using WEBCV.Models;

namespace WEBCV.Controllers
{
    [Authorize(Roles = "Employer")]
    public class JobManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobManagementController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: JobManagement
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobs = await _context.Jobs
                .Where(j => j.EmployerId == userId)
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();

            return View(jobs);
        }

        // GET: JobManagement/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var job = new Job
                {
                    Title = model.Title,
                    Description = model.Description,
                    Requirements = model.Requirements,
                    Location = model.Location,
                    Salary = model.Salary,
                    CompanyName = model.CompanyName,
                    JobType = model.JobType, // Full-time, Part-time, etc.
                    PostedDate = DateTime.Now,
                    DeadlineDate = model.DeadlineDate,
                    IsActive = true,
                    EmployerId = userId
                };

                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: JobManagement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == id && j.EmployerId == userId);

            if (job == null)
            {
                return NotFound();
            }

            var viewModel = new JobViewModel
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Requirements = job.Requirements,
                Location = job.Location,
                Salary = job.Salary,
                CompanyName = job.CompanyName,
                JobType = job.JobType,
                DeadlineDate = job.DeadlineDate
            };

            return View(viewModel);
        }

        // POST: JobManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var job = await _context.Jobs
                        .FirstOrDefaultAsync(j => j.Id == id && j.EmployerId == userId);

                    if (job == null)
                    {
                        return NotFound();
                    }

                    job.Title = model.Title;
                    job.Description = model.Description;
                    job.Requirements = model.Requirements;
                    job.Location = model.Location;
                    job.Salary = model.Salary;
                    job.CompanyName = model.CompanyName;
                    job.JobType = model.JobType;
                    job.DeadlineDate = model.DeadlineDate;
                    job.UpdatedDate = DateTime.Now;

                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: JobManagement/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == id && j.EmployerId == userId);

            if (job == null)
            {
                return NotFound();
            }

            job.IsActive = !job.IsActive;
            job.UpdatedDate = DateTime.Now;

            _context.Update(job);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: JobManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == id && j.EmployerId == userId);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: JobManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == id && j.EmployerId == userId);

            if (job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: JobManagement/Applications/5
        public async Task<IActionResult> Applications(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == id && j.EmployerId == userId);

            if (job == null)
            {
                return NotFound();
            }

            var applications = await _context.JobApplications
                .Include(a => a.User)
                .Include(a => a.Resume)
                .Where(a => a.JobId == id)
                .OrderByDescending(a => a.AppliedDate)
                .ToListAsync();

            ViewData["JobTitle"] = job.Title;
            ViewData["JobId"] = job.Id;

            return View(applications);
        }

        // POST: JobManagement/UpdateApplicationStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApplicationStatus(int id, string status, int jobId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if job belongs to current employer
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == jobId && j.EmployerId == userId);

            if (job == null)
            {
                return NotFound();
            }

            var application = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.Id == id && a.JobId == jobId);

            if (application == null)
            {
                return NotFound();
            }

            application.Status = status;
            application.UpdatedDate = DateTime.Now;

            _context.Update(application);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Applications), new { id = jobId });
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }
    }
}