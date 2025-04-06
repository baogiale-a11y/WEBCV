using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBCV.Models;

namespace WEBCV.Controllers
{
    [Authorize]
    public class CVController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CVController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CV
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var cvList = await _context.CVs
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return View(cvList);
        }

        // GET: CV/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var cv = await _context.CVs
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (cv == null)
            {
                return NotFound();
            }

            return View(cv);
        }

        // GET: CV/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CV/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,PhoneNumber,Skills,Education,WorkExperience")] CV cv)
        {
            if (ModelState.IsValid)
            {
                cv.UserId = _userManager.GetUserId(User);
                cv.CreatedDate = DateTime.Now;

                _context.Add(cv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cv);
        }

        // GET: CV/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var cv = await _context.CVs
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (cv == null)
            {
                return NotFound();
            }

            return View(cv);
        }

        // POST: CV/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,PhoneNumber,Skills,Education,WorkExperience")] CV cv)
        {
            if (id != cv.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCV = await _context.CVs.FindAsync(id);
                    if (existingCV == null || existingCV.UserId != _userManager.GetUserId(User))
                    {
                        return NotFound();
                    }

                    existingCV.FullName = cv.FullName;
                    existingCV.Email = cv.Email;
                    existingCV.PhoneNumber = cv.PhoneNumber;
                    existingCV.Skills = cv.Skills;
                    existingCV.Education = cv.Education;
                    existingCV.WorkExperience = cv.WorkExperience;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CVExists(cv.Id))
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
            return View(cv);
        }

        // GET: CV/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var cv = await _context.CVs
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (cv == null)
            {
                return NotFound();
            }

            return View(cv);
        }

        // POST: CV/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var cv = await _context.CVs
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (cv != null)
            {
                _context.CVs.Remove(cv);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CVExists(int id)
        {
            return _context.CVs.Any(e => e.Id == id);
        }
    }
}