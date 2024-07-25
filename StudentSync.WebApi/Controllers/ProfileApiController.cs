using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;

namespace StudentSync.WebApi.Controllers
{
    [Route("api/ProfileApiController")]
    [ApiController]
    public class ProfileApiController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileApiController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("get-profile/{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            var profile = await _profileService.GetProfileAsync(username);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }
    }
}
