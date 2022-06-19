using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Auth.Jwt;
using Microsoft.AspNetCore.Http;

namespace Auth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string jwtCookieName = "jwtCoooookie";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "normalUser")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "first, second")]
        [Authorize(Roles = "normalUser")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        public IActionResult getApi(int age) {
            return Ok(new {
                value = $"you age is {age}",
                nane = "Nikhil"
            });
        }
        public IActionResult Login(int userId, string gender, string role, int age) {
            var token = new JwtHandler().GetToken(new Models.User {
                Id = userId,
                Gender = gender,
                Role = role,
                Age = age
            });
            var options = new CookieOptions {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(60)
            };
            HttpContext.Response.Cookies.Append(jwtCookieName, token, options);
            return Ok();
        }
    }
}
