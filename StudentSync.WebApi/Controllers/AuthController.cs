using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.ViewModels;
using System;
using System.Threading.Tasks;

namespace StudentSync.WebApi.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.RegisterAsync(model);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Registration successful." });
                }
                else
                {
                    return BadRequest(result.HttpResponseMessage);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.LoginAsync(model);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Login successful." });
                }
                else
                {
                    return BadRequest(result.HttpResponseMessage);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var result = await _authService.LogoutAsync();
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Logout successful." });
                }
                else
                {
                    return BadRequest(result.HttpResponseMessage);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"An error occurred: {ex.Message}" });
            }
        }

    }
}
