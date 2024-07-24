
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.WebApi.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public TokenController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _authService.LoginAsync(model); // Implement this method in your IAuthService

                    if (result.Succeeded)
                    {
                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.Email), // Adjust based on your user model
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                        var jwtSecret = _configuration["JWT:Secret"];
                        if (string.IsNullOrEmpty(jwtSecret))
                        {
                            return StatusCode(500, new { message = "JWT secret is not configured correctly." });
                        }

                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

                        var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.UtcNow.AddMonths(12), // Adjust token expiration as needed
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );


                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                    else
                    {
                        return Unauthorized(new { message = "Invalid credentials" });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
                }
            }

            return BadRequest(new { message = "Invalid model" });
        }


        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var result = await _authService.LogoutAsync();

                if (result.Succeeded)
                {
                    return Ok(new { message = "Logout successful" });
                }

                return BadRequest(new { message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }


  
    }
}
