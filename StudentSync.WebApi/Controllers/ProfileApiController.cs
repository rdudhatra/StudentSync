//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;

//namespace StudentSync.WebApi.Controllers
//{
//    [Route("api/ProfileApiController")]
//    [ApiController]
//    public class ProfileApiController : ControllerBase
//    {
//        private readonly IProfileService _profileService;

//        public ProfileApiController(IProfileService profileService)
//        {
//            _profileService = profileService;
//        }

//        [HttpGet("get-profile/{username}")]
//        public async Task<IActionResult> GetProfile(string username)
//        {
//            var profile = await _profileService.GetProfileAsync(username);
//            if (profile == null)
//            {
//                return NotFound();
//            }
//            return Ok(profile);
//        }
//    }
//}


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

        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileViewModel model)
        {
            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                model.ProfileImage = "/images/" + uniqueFileName;
            }

            await _profileService.UpdateProfileAsync(model);
            return Ok();
        }
    }
}
