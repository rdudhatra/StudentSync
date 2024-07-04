
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.ViewModels;
using StudentSync.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Controllers 
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/AuthRegister/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _authService.RegisterAsync(model);
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Registration successful.";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.HttpResponseMessage != null)
                        {
                            var errors = await result.HttpResponseMessage.GetErrors();
                            foreach (var error in errors)
                            {
                                ModelState.AddModelError(string.Empty, error);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Failed to retrieve error details.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            return View("~/Views/AuthRegister/Index.cshtml", model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            return View("~/Views/AuthLogin/Index.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the login is for admin
                    var adminResult = await _authService.AdminLoginAsync(model);
                    if (adminResult.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Admin login successful.";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Check regular user login
                        var result = await _authService.LoginAsync(model);
                        if (result.Succeeded)
                        {
                            TempData["SuccessMessage"] = "Login successful.";
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            if (result.HttpResponseMessage != null)
                            {
                                var errors = await result.HttpResponseMessage.GetErrors();
                                foreach (var error in errors)
                                {
                                    ModelState.AddModelError(string.Empty, error);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Failed to retrieve error details.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            // If ModelState is not valid or login failed, return to the login view with the model
            return View("~/Views/AuthLogin/Index.cshtml", model);
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                var result = await _authService.LogoutAsync();
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Logout successful.";
                    return RedirectToAction("Login");
                }

                // Handle logout failure if needed
                if (result.HttpResponseMessage != null)
                {
                    var errors = await result.HttpResponseMessage.GetErrors();
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to retrieve error details.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}


