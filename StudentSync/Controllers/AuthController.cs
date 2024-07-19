
//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Data.ViewModels;
//using StudentSync.Extensions;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace StudentSync.Controllers 
//{
//    public class AuthController : Controller
//    {
//        private readonly IAuthService _authService;

//        public AuthController(IAuthService authService)
//        {
//            _authService = authService;
//        }

//        [HttpGet]
//        public IActionResult Register()
//        {
//            return View("~/Views/AuthRegister/Index.cshtml");
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Register(RegisterViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    var result = await _authService.RegisterAsync(model);
//                    if (result.Succeeded)
//                    {
//                        TempData["SuccessMessage"] = "Registration successful.";
//                        return RedirectToAction("Login");
//                    }
//                    else
//                    {
//                        if (result.HttpResponseMessage != null)
//                        {
//                            var errors = await result.HttpResponseMessage.GetErrors();
//                            foreach (var error in errors)
//                            {
//                                ModelState.AddModelError(string.Empty, error);
//                            }
//                        }
//                        else
//                        {
//                            ModelState.AddModelError(string.Empty, "Failed to retrieve error details.");
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
//                }
//            }

//            return View("~/Views/AuthRegister/Index.cshtml", model);
//        }

//        [HttpGet]
//        public IActionResult Login()
//        {
//            var model = new LoginViewModel();
//            return View("~/Views/AuthLogin/Index.cshtml", model);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login(LoginViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    // Check if the login is for admin
//                    var adminResult = await _authService.AdminLoginAsync(model);
//                    if (adminResult.Succeeded)
//                    {
//                        TempData["SuccessMessage"] = "Admin login successful.";
//                        return RedirectToAction("Index", "Home");
//                    }
//                    else
//                    {
//                        // Check regular user login
//                        var result = await _authService.LoginAsync(model);
//                        if (result.Succeeded)
//                        {
//                            TempData["SuccessMessage"] = "Login successful.";
//                            return RedirectToAction("Index", "Home");
//                        }
//                        else
//                        {
//                            if (result.HttpResponseMessage != null)
//                            {
//                                var errors = await result.HttpResponseMessage.GetErrors();
//                                foreach (var error in errors)
//                                {
//                                    ModelState.AddModelError(string.Empty, error);
//                                }
//                            }
//                            else
//                            {
//                                ModelState.AddModelError(string.Empty, "Failed to retrieve error details.");
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
//                }
//            }

//            // If ModelState is not valid or login failed, return to the login view with the model
//            return View("~/Views/AuthLogin/Index.cshtml", model);
//        }

//        public async Task<IActionResult> Logout()
//        {
//            try
//            {
//                var result = await _authService.LogoutAsync();
//                if (result.Succeeded)
//                {
//                    TempData["SuccessMessage"] = "Logout successful.";
//                    return RedirectToAction("Login");
//                }

//                // Handle logout failure if needed
//                if (result.HttpResponseMessage != null)
//                {
//                    var errors = await result.HttpResponseMessage.GetErrors();
//                    foreach (var error in errors)
//                    {
//                        ModelState.AddModelError(string.Empty, error);
//                    }
//                }
//                else
//                {
//                    ModelState.AddModelError(string.Empty, "Failed to retrieve error details.");
//                }
//            }
//            catch (Exception ex)
//            {
//                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
//            }

//            return RedirectToAction("Index", "Home");
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using StudentSync.Data.ViewModels;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                    var json = JsonSerializer.Serialize(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("Auth/register", content);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadAsStringAsync();
                    var message = JsonSerializer.Deserialize<ApiResponse<string>>(result);

                    TempData["SuccessMessage"] = message.Data;
                    return RedirectToAction("Login");
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var json = JsonSerializer.Serialize(model);
        //            var content = new StringContent(json, Encoding.UTF8, "application/json");

        //            var response = await _httpClient.PostAsync("token/token", content);
        //            response.EnsureSuccessStatusCode();

        //            var result = await response.Content.ReadAsStringAsync();
        //            var tokenResponse = JsonSerializer.Deserialize<ApiResponse<string>>(result);

        //            TempData["AuthToken"] = tokenResponse.Data; // Store token in TempData or session as needed
        //            TempData["SuccessMessage"] = "Login successful.";

        //            return RedirectToAction("Index", "Home"); // Replace with your desired redirect action
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
        //        }
        //    }

        //    return View("~/Views/AuthLogin/Index.cshtml", model);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonSerializer.Serialize(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("token/token", content);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonSerializer.Deserialize<ApiResponse<string>>(result);
                    // Debug output
                    Console.WriteLine($"Token received: {result}");
                    // Handle null or empty token response
                    if (string.IsNullOrEmpty(result))
                    {
                        ModelState.AddModelError(string.Empty, "Token received from server is null or empty.");
                        return View("~/Views/AuthLogin/Index.cshtml", model);
                    }

                    // Store token in cookie
                    Response.Cookies.Append("AuthToken", result, new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddHours(1), // Set cookie expiration as needed
                        HttpOnly = true, // Ensures the cookie is accessible only through HTTP requests
                        Secure = true, // Ensures the cookie is only sent over HTTPS if your site is HTTPS
                        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict // Protects against cross-site request forgery
                    });

                    // Debug output
                    Console.WriteLine("Token stored in cookie.");

                    TempData["SuccessMessage"] = "Login successful.";

                    return RedirectToAction("Index", "Home"); // Replace with your desired redirect action
                }
                catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError(string.Empty, "Invalid credentials"); // Show a generic error message for unauthorized access
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            return View("~/Views/AuthLogin/Index.cshtml", model);
        }

        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var json = JsonSerializer.Serialize(model);
        //            var content = new StringContent(json, Encoding.UTF8, "application/json");

        //            var response = await _httpClient.PostAsync("token/token", content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var result = await response.Content.ReadAsStringAsync();
        //                var tokenResponse = JsonSerializer.Deserialize<ApiResponse<string>>(result);

        //                TempData["AuthToken"] = tokenResponse.Data; // Store token in TempData or session as needed
        //                TempData["SuccessMessage"] = "Login successful.";

        //                return RedirectToAction("Index", "Home"); // Replace with your desired redirect action
        //            }
        //            else if (response.StatusCode == HttpStatusCode.Unauthorized)
        //            {
        //                ModelState.AddModelError(string.Empty, "Invalid credentials. Please check your username and password.");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, $"An error occurred: {response.StatusCode}");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
        //        }
        //    }

        //    return View("~/Views/AuthLogin/Index.cshtml", model);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var json = JsonSerializer.Serialize(model);
        //            var content = new StringContent(json, Encoding.UTF8, "application/json");

        //            var response = await _httpClient.PostAsync("Auth/login", content);
        //            response.EnsureSuccessStatusCode();

        //            var result = await response.Content.ReadAsStringAsync();
        //            var message = JsonSerializer.Deserialize<ApiResponse<string>>(result);

        //            TempData["SuccessMessage"] = message.Data;
        //            return RedirectToAction("Index", "Home"); // Replace with your desired redirect action
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
        //        }
        //    }

        //    return View("~/Views/AuthLogin/Index.cshtml", model);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var response = await _httpClient.PostAsync("Auth/logout", null); // No content needed for logout
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var message = JsonSerializer.Deserialize<ApiResponse<string>>(result);

                TempData["SuccessMessage"] = message.Data;
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
