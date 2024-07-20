

using Microsoft.AspNetCore.Mvc;
using StudentSync.Data.ViewModels;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

                    // Debug output
                    Console.WriteLine($"Token received: {result}");

                    // Parse the JSON response to extract the token
                    using (var document = JsonDocument.Parse(result))
                    {
                        var root = document.RootElement;
                        if (root.TryGetProperty("token", out var tokenElement))
                        {
                            var token = tokenElement.GetString();

                            // Handle null or empty token response
                            if (string.IsNullOrEmpty(token))
                            {
                                ModelState.AddModelError(string.Empty, "Token received from server is null or empty.");
                                return View("~/Views/AuthLogin/Index.cshtml", model);
                            }

                            // Format token as "bearer <token_value>"
                            var formattedToken = $"{token}";

                            // Store token in cookie
                            Response.Cookies.Append("AuthToken", formattedToken, new Microsoft.AspNetCore.Http.CookieOptions
                            {
                                Expires = DateTimeOffset.UtcNow.AddDays(30), // Set cookie expiration as needed
                                HttpOnly = true, // Ensures the cookie is accessible only through HTTP requests
                                Secure = true, // Ensures the cookie is only sent over HTTPS if your site is HTTPS
                                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict // Protects against cross-site request forgery
                            });

                            // Debug output
                            Console.WriteLine("Token stored in cookie.");

                            TempData["SuccessMessage"] = "Login successful.";

                            return RedirectToAction("Index", "Home"); // Replace with your desired redirect action
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Token property not found in the response.");
                            return View("~/Views/AuthLogin/Index.cshtml", model);
                        }
                    }
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout()
        //{
        //    try
        //    {
        //        // Retrieve the token from the cookie or other secure storage
        //        var token = Request.Cookies["AuthToken"];

        //        if (string.IsNullOrEmpty(token))
        //        {
        //            ModelState.AddModelError(string.Empty, "No token found for logout.");
        //            return RedirectToAction("Index", "Home");
        //        }

        //        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "token/logout");
        //        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //        var response = await _httpClient.SendAsync(requestMessage);
        //        response.EnsureSuccessStatusCode();

        //        var result = await response.Content.ReadAsStringAsync();
        //        var message = JsonSerializer.Deserialize<ApiResponse<string>>(result);

        //        // Remove the authentication cookie
        //        Response.Cookies.Delete("AuthToken");

        //        TempData["SuccessMessage"] = message.Data;
        //        return RedirectToAction("Login");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
        //    }

        //    return RedirectToAction("Index", "Home");
        //}


    }
}
