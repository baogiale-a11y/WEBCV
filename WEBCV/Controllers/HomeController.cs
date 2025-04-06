using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEBCV.Models;

namespace WEBCV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Set current year for footer copyright
            ViewBag.CurrentYear = DateTime.UtcNow.Year;
            return View();
        }

        public IActionResult Privacy()
        {
            // Set current year for footer copyright
            ViewBag.CurrentYear = DateTime.UtcNow.Year;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}