using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interface;
using StudentSync.Core.Wrapper;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using StudentSync.Data.ViewModels;
using System.Security.Claims;

namespace StudentSync.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly StudentSyncDbContext _context;

        private const string AdminEmail = "admin@example.com";
        private const string AdminPassword = "Admin@123";

        public AuthService(IHttpContextAccessor httpContextAccessor, StudentSyncDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IResult> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    return Result.Fail("User with this email already exists.");
                }

                var user = new User
                {
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password 
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await SignInAsync(user);

                return Result.Success("Registration successful.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred during registration: {ex.Message}");
            }
        }

        public async Task<IResult> LoginAsync(LoginViewModel model)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

                if (user != null && user.Password == model.Password)
                {
                    if(user.Username ==  model.Username) 
                    {
                        await SignInAsync(user);
                    }
                    else
                    {
                        return Result.Fail("Invalid UserName.");
                    }
                    
                }
                else
                {
                    return Result.Fail("Invalid email or password.");
                }

                return Result.Success("Login successful.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred during login: {ex.Message}");
            }
        }


        public async Task<IResult> LogoutAsync()
        {
            try
            {
                await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Result.Success("Logout successful.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred during logout: {ex.Message}");
            }
        }

        public async Task<IResult> AdminLoginAsync(LoginViewModel model)
        {
            try
            {
                if (model.Email == AdminEmail && model.Password == AdminPassword)
                {
                    var adminClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "Admin"),
                        new Claim(ClaimTypes.Email, AdminEmail),
                        new Claim(ClaimTypes.Role, "Admin")
                    };

                    var adminIdentity = new ClaimsIdentity(adminClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var adminPrincipal = new ClaimsPrincipal(adminIdentity);

                    await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, adminPrincipal);

                    return Result.Success("Admin login successful.");
                }
                return Result.Fail("Invalid admin credentials.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred during admin login: {ex.Message}");
            }
        }

        private async Task SignInAsync(User user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred during sign-in.", ex);
            }
        }
   
    }
}
