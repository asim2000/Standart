using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Web.Extensions;
using Web.Identity;
using Web.Models;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel{ ReturnUrl=ReturnUrl});
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu istifadeci movcud deyil");

                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                TempData.Put<AlertMessage>("message", new AlertMessage
                {
                    Title = "Ugurlu Giris",
                    Message = "Sisteme daxil oldunuz",
                    Type = "success"
                });
                return Redirect(model.ReturnUrl ?? "~/");
            }
            ModelState.AddModelError("", "Girilen istifadeci adi ve ya parol yanlisdir");
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }



            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                ModelState.AddModelError("", "Bu istifadeci movcuddur");

                return View(model);
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("", "Bu Email istifade olunub");

                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Profession = model.Profession
            };
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                user.Image = fileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                TempData.Put<AlertMessage>("message", new AlertMessage
                {
                    Title = "Ugurlu qeydiyyat",
                    Message = "Sistemde qeydiyyata alindiniz,zehmet olmasa daxil olun",
                    Type = "success"
                });
                return Redirect("/account/login");
            }
            ModelState.AddModelError(null, "Yeniden cehd edin");
            return View(model);

        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return Redirect("~/");
        }
    }
}
