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
            var userName = Request.Cookies["CurrentUsername"];

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Account");
            }
            // Print the current username for debugging purposes
            System.Diagnostics.Debug.WriteLine($"Current username: {userName}");

            var profile = await _profileService.GetProfileAsync(userName);
            if (profile == null)
            {
                return NotFound();
            }
            // Pass the username to the view
            ViewBag.CurrentUsername = userName;
            return View(profile);
         
        }



     
    }
}
