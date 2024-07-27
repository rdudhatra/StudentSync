////using Microsoft.AspNetCore.Mvc;
////using System.Threading.Tasks;
////using StudentSync.Core.Services.Interface;
////using StudentSync.Data.ViewModels;

////namespace StudentSync.Web.Controllers
////{
////    public class AuthProfileController : Controller
////    {
////        private readonly IProfileService _profileService;

////        public AuthProfileController(IProfileService profileService)
////        {
////            _profileService = profileService;
////        }

////        // GET: AuthProfile/Index
////        public async Task<IActionResult> Index()
////        {
////            var userName = Request.Cookies["CurrentUsername"];

////            if (string.IsNullOrEmpty(userName))
////            {
////                return RedirectToAction("Login", "Account");
////            }
////            // Print the current username for debugging purposes
////            System.Diagnostics.Debug.WriteLine($"Current username: {userName}");

////            var profile = await _profileService.GetProfileAsync(userName);
////            if (profile == null)
////            {
////                return NotFound();
////            }
////            // Pass the username to the view
////            ViewBag.CurrentUsername = userName;
////            return View(profile);

////        }




////    }
////}


//using Microsoft.AspNetCore.Mvc;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using StudentSync.Data.ViewModels;

//namespace StudentSync.Web.Controllers
//{
//    public class AuthProfileController : Controller
//    {
//        private readonly HttpClient _httpClient;

//        public AuthProfileController(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        // GET: AuthProfile/Index
//        public async Task<IActionResult> Index()
//        {
//            var userName = Request.Cookies["CurrentUsername"];

//            if (string.IsNullOrEmpty(userName))
//            {
//                return RedirectToAction("Login", "Account");
//            }
//            // Print the current username for debugging purposes
//            System.Diagnostics.Debug.WriteLine($"Current username: {userName}");

//            // Call the API to get the profile
//            var profile = await GetProfileFromApiAsync(userName);
//            if (profile == null)
//            {
//                return NotFound();
//            }
//            // Pass the username to the view
//            ViewBag.CurrentUsername = userName;
//            return View(profile);
//        }

//        private async Task<ProfileViewModel> GetProfileFromApiAsync(string username)
//        {
//            var response = await _httpClient.GetAsync($"ProfileApiController/get-profile/{username}");
//            if (response.IsSuccessStatusCode)
//            {
//                var jsonResponse = await response.Content.ReadAsStringAsync();
//                return JsonConvert.DeserializeObject<ProfileViewModel>(jsonResponse);
//            }
//            return null;
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Data.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;

namespace StudentSync.Web.Controllers
{
    public class AuthProfileController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthProfileController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

            // Call the API to get the profile
            var profile = await GetProfileFromApiAsync(userName);
            if (profile == null)
            {
                return NotFound();
            }
            // Pass the username to the view
            ViewBag.CurrentUsername = userName;
            return View(profile);
        }

        private async Task<ProfileViewModel> GetProfileFromApiAsync(string username)
        {
            var response = await _httpClient.GetAsync($"ProfileApiController/get-profile/{username}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProfileViewModel>(jsonResponse);
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Log the errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine(error);
                }

                // Return the view with the model to show the errors
                return View("Index", model);
            }

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(model.Email), "Email");
                content.Add(new StringContent(model.Username), "Username");
                content.Add(new StringContent(model.Password), "Password");

                if (model.ImageFile != null)
                {
                    var fileContent = new StreamContent(model.ImageFile.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.ImageFile.ContentType);
                    content.Add(fileContent, "ImageFile", model.ImageFile.FileName);
                }

                var response = await _httpClient.PostAsync("ProfileApiController/update-profile", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View("Index", model);
        }
    }
}

