

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
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AuthController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
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

                    Response.Cookies.Append("CurrentUsername", model.Username, new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddMonths(12), 
                        HttpOnly = true, 
                        Secure = true, 
                        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict 
                    });

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

                    using (var document = JsonDocument.Parse(result))
                    {
                        var root = document.RootElement;
                        if (root.TryGetProperty("token", out var tokenElement))
                        {
                            var token = tokenElement.GetString();

                            if (string.IsNullOrEmpty(token))
                            {
                                ModelState.AddModelError(string.Empty, "Token received from server is null or empty.");
                                return View("~/Views/AuthLogin/Index.cshtml", model);
                            }

                            var formattedToken = $"{token}";

                            Response.Cookies.Append("AuthToken", formattedToken, new Microsoft.AspNetCore.Http.CookieOptions
                            {
                                Expires = DateTimeOffset.UtcNow.AddMonths(12), 
                                HttpOnly = true, 
                                Secure = true,
                                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict 
                            });

                            var username = model.Username;

                            Response.Cookies.Append("CurrentUsername", username, new Microsoft.AspNetCore.Http.CookieOptions
                            {
                                Expires = DateTimeOffset.UtcNow.AddMonths(12), 
                                HttpOnly = true, 
                                Secure = true, 
                                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict 
                            });
                            Console.WriteLine("Token stored in cookie.");

                            TempData["SuccessMessage"] = "Login successful.";

                            return RedirectToAction("Index", "Home"); 
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
                    ModelState.AddModelError(string.Empty, "Invalid credentials"); 
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
                var token = Request.Cookies["AuthToken"];

                if (string.IsNullOrEmpty(token))
                {
                    ModelState.AddModelError(string.Empty, "No token found for logout.");
                    return RedirectToAction("Index", "Home");
                }

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "token/logout");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var message = JsonSerializer.Deserialize<ApiResponse<string>>(result);

                Response.Cookies.Delete("AuthToken");

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
