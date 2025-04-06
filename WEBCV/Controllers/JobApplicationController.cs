using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using WEBCV.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WEBCV.Models;

namespace WEBCV.Controllers
{
    [Authorize(Roles = "JobSeeker")]
    public class JobApplicationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobApplicationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MyApplications
        public async Task<IActionResult> MyApplications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applications = await _context.JobApplications
                .Include(a => a.Job)
                .Include(a => a.Resume)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppliedDate)
                .ToListAsync();

            return View(applications);
        }

        // POST: JobApplication/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(int jobId, int resumeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if already applied
            var existingApplication = await _context.JobApplications
                .AnyAsync(a => a.JobId == jobId && a.UserId == userId);

            if (existingApplication)
            {
                TempData["Error"] = "Bạn đã ứng tuyển vị trí này rồi!";
                return RedirectToAction("Details", "Job", new { id = jobId });
            }

            // Validate job and resume
            var job = await _context.Jobs.FindAsync(jobId);
            var resume = await _context.Resumes.FirstOrDefaultAsync(r => r.Id == resumeId && r.UserId == userId);

            if (job == null || resume == null)
            {
                return NotFound();
            }

            var application = new JobApplication
            {
                JobId = jobId,
                ResumeId = resumeId,
                UserId = userId,
                Status = "Đang xem xét", // Pending
                AppliedDate = DateTime.Now
            };

            _context.Add(application);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Ứng tuyển thành công!";
            return RedirectToAction("Details", "Job", new { id = jobId });
        }

        // POST: JobApplication/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (application == null)
            {
                return NotFound();
            }

            // Only allow cancellation if still pending
            if (application.Status == "Đang xem xét")
            {
                _context.JobApplications.Remove(application);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã hủy đơn ứng tuyển!";
            }
            else
            {
                TempData["Error"] = "Không thể hủy đơn ứng tuyển ở trạng thái hiện tại!";
            }

            return RedirectToAction(nameof(MyApplications));
        }
    }
}