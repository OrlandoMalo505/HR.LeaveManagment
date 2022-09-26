﻿using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAuthenticationService _authService;

        public UsersController(IAuthenticationService authService)
        {
            _authService = authService;
        }
        public IActionResult Login(string returnUrl = null)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginVM login, string returnUrl)
        {
            returnUrl ??= Url.Content("~/");

            var isLoggedIn = await _authService.Authenticate(login.Email, login.Password);
            if (isLoggedIn)
            {
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError("", "Log In Attempt Failed. Please try again.");
            return View(login);
        }
    }
}
