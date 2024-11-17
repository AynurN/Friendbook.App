using Friendbook.MVC.Areas.LoginRegister.ViewModels;
using Friendbook.MVC.Services.Implementations;
using Friendbook.MVC.Services.Interfacses;
using Microsoft.AspNetCore.Mvc;
using Friendbook.MVC.HelperTools;
using System.Text.RegularExpressions;

namespace Friendbook.MVC.Areas.LoginRegister.Controllers
{
    [Area("LoginRegister")]
    public class AuthController : Controller
    {
        private readonly ICrudService crudService;
        private readonly IConfiguration configuration;
        private readonly IAuthService authService;

        public AuthController(ICrudService crudService, IConfiguration configuration, IAuthService authService)
        {
            this.crudService = crudService;
            this.configuration = configuration;
            this.authService = authService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM vm)
        {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("Password", "Invalid credentials");
                return View();
            } 

            var data = (await authService.Login(vm)).Data.Entities;
            if(data == null)
            {
                ModelState.AddModelError("Password", "Invalid credentials");
                return View();
            }
            HttpContext.Response.Cookies.Append("token", data.AccessToken, new CookieOptions
            {
                Expires = data.ExpireDate,
                HttpOnly = true
            });

            return RedirectToAction("Index", "Home", new { area = "" });
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (vm.Password != vm.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                return View();
            }


            var response = await authService.Register(vm);

            //if (response.Data == null)
            //{
            //    ModelState.AddModelError("", "An unknown error occurred during registration.");
            //}
            //else
            //{
            if (response == null || !response.IsSuccessful)
            {

                if (!Helper.IsValidPassword(vm.Password)) {
                    ModelState.AddModelError("Password", "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
                }
                if (!Helper.IsValidEmail(vm.Email)) {
                    ModelState.AddModelError("Email", "Invalid email format.");
                }
                return View();
            }
           
            //}

            TempData["Message"] = "You have registered successfully";
            return RedirectToAction("Login","Auth", new { area = "LoginRegister" });
        }


        public IActionResult Logout()
        {
            authService.Logout();

            return RedirectToAction("Login", "Auth", new { area = "LoginRegister" });
        }
    }
    
}
