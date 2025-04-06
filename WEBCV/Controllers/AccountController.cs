using WEBCV.Models;
using WEBCV.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using WEBCV.Models;
using WEBCV.ViewModels;

namespace WEBCV.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.UserTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "JobSeeker", Text = "Người tìm việc" },
                new SelectListItem { Value = "Employer", Text = "Nhà tuyển dụng" }
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.UserTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "JobSeeker", Text = "Người tìm việc" },
                new SelectListItem { Value = "Employer", Text = "Nhà tuyển dụng" }
            };

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Address = model.Address,
                    DateOfBirth = model.DateOfBirth,
                    UserType = model.UserType
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Kiểm tra và tạo role nếu chưa tồn tại
                    if (!await _roleManager.RoleExistsAsync(model.UserType))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.UserType));
                    }

                    // Gán role cho user
                    await _userManager.AddToRoleAsync(user, model.UserType);

                    // Đăng nhập user sau khi đăng ký thành công
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đăng nhập không thành công. Vui lòng kiểm tra lại email và mật khẩu.");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var model = new ProfileViewModel
            {
                FullName = "John Doe",
                Email = "johndoe@example.com",
                Role = "User",
                CreatedDate = DateTime.Now // Thay đổi thành giá trị thực tế từ cơ sở dữ liệu của bạn
            };

            return View(model);
        }
    }
}