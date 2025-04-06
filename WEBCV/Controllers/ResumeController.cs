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
    [Authorize(Roles = "JobSeeker")]
    public class ResumeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResumeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Resume
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resumes = await _context.Resumes
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(resumes);
        }

        // GET: Resume/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Resume/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResumeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var resume = new Resume
                {
                    Title = model.Title,
                    Education = model.Education,
                    Experience = model.Experience,
                    Skills = model.Skills,
                    UserId = userId,
                    CreatedDate = DateTime.Now
                };

                _context.Add(resume);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Resume/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resume = await _context.Resumes
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (resume == null)
            {
                return NotFound();
            }

            var viewModel = new ResumeViewModel
            {
                Id = resume.Id,
                Title = resume.Title,
                Education = resume.Education,
                Experience = resume.Experience,
                Skills = resume.Skills
            };

            return View(viewModel);
        }

        // POST: Resume/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ResumeViewModel model)
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
                    var resume = await _context.Resumes
                        .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

                    if (resume == null)
                    {
                        return NotFound();
                    }

                    resume.Title = model.Title;
                    resume.Education = model.Education;
                    resume.Experience = model.Experience;
                    resume.Skills = model.Skills;
                    resume.UpdatedDate = DateTime.Now;

                    _context.Update(resume);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResumeExists(model.Id))
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

        // GET: Resume/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resume = await _context.Resumes
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // POST: Resume/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resume = await _context.Resumes
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (resume != null)
            {
                _context.Resumes.Remove(resume);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ResumeExists(int id)
        {
            return _context.Resumes.Any(e => e.Id == id);
        }
    }
}