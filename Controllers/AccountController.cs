using KuaforApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KuaforApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string FullName, string PhoneNumber, string Password)
        {
            if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(Password))
            {
                ViewData["Error"] = "Tüm alanları doldurunuz.";
                return View();
            }

            var user = new ApplicationUser
            {
                FullName = FullName,
                PhoneNumber = PhoneNumber,
                UserName = PhoneNumber,
                Email = null // Email olmadan kayıt
            };

            var result = await _userManager.CreateAsync(user, Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Error"] = string.Join(" ", result.Errors.Select(e => e.Description));
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string PhoneNumber, string Password)
        {
            var result = await _signInManager.PasswordSignInAsync(PhoneNumber, Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["Error"] = "Giriş başarısız. Telefon numarası veya şifre hatalı.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
