


using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.ViewModels;
using System.IO;
using System.Threading.Tasks;

namespace StudentSync.WebApi.Controllers
{
    [Route("api/ProfileApiController")]
    [ApiController]
    public class ProfileApiController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileApiController(IProfileService profileService, IWebHostEnvironment webHostEnvironment)
        {
            _profileService = profileService;
            _webHostEnvironment = webHostEnvironment;
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
