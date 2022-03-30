using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SimpleAddsAuth.data;

namespace SimpleAddsAuth.web.Controllers
{
    public class Account : Controller
    {
        private readonly string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Auth;Integrated Security=true;";

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var repository = new UserRepository(_connectionString);
            repository.AddUser(user, password);

            return Redirect("/Account/Login");
        }
        public IActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            var repository = new UserRepository(_connectionString);
            var user = repository.Login(Email, Password);
            if (user == null)
            {
                TempData["message"] = "Login Invalid, please try again";
                return RedirectToAction("Login");
            }

            var claims = new List<Claim>
            {
                new Claim("user", Email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/Home/Index");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/Home/Index");
        }
    }
}
