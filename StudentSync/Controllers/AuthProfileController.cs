using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.ViewModels;

namespace StudentSync.Web.Controllers
{
    public class AuthProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public AuthProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET: AuthProfile/Index
        public async Task<IActionResult> Index()
        {
            var profile = await _profileService.GetProfileAsync(User.Identity.Name); // Assuming username is used as an identifier
            return View(profile);
        }

        // POST: AuthProfile/Update
        [HttpPost]
        public async Task<IActionResult> Update(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _profileService.UpdateProfileAsync(model);
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Index), model);
        }
    }
}
