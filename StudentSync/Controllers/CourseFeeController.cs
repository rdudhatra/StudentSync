
//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;
//using Microsoft.EntityFrameworkCore;
//using StudentSync.Data.Models;
//using System;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using StudentSync.Core.Services;

//namespace StudentSync.Controllers
//{
//    [Route("CourseFee")]

//    public class CourseFeeController : Controller
//    {
//        private readonly ICourseFeeService _courseFeeService;

//        public CourseFeeController(ICourseFeeService courseFeeService)
//        {
//            _courseFeeService = courseFeeService;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpGet("GetAllfeeCourseIds")]
//        public IActionResult GetAllCourseIds()
//        {
//            try
//            {
//                var courseIds = _courseFeeService.GetAllCourseExamIds(); // Implement this method in your service
//                return Json(courseIds);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { message = "Failed to retrieve Course IDs", error = ex.Message });
//            }
//        }
//        [HttpGet("GetAll")]
//        public async Task<IActionResult> GetAll()
//        {
//            try
//            {
//                var courseFees = await _courseFeeService.GetAllCourseFeesAsync();
//                var dataTableResponse = new
//                {
//                    draw = Request.Query["draw"],
//                    recordsTotal = courseFees.Count(),
//                    recordsFiltered = courseFees.Count(),
//                    data = courseFees
//                };

//                return Ok(dataTableResponse);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpGet("GetById/{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var courseFee = await _courseFeeService.GetCourseFeeByIdAsync(id);
//            if (courseFee == null)
//                return NotFound();

//            return Ok(courseFee);
//        }

//        [HttpPost("AddCourseFee")]
//        public async Task<IActionResult> AddCourseFee([FromBody] CourseFee courseFee)
//        {
//            if (!ModelState.IsValid)
//            {
//                var errors = ModelState.Values.SelectMany(v => v.Errors);
//                foreach (var error in errors)
//                {
//                    Console.WriteLine(error.ErrorMessage);
//                }
//                return BadRequest(ModelState);
//            }

//            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


//            courseFee.CreatedBy = userId;
//            courseFee.UpdatedBy = userId;

//            await _courseFeeService.AddCourseFeeAsync(courseFee);

//            return Ok(new { message = "Course fee added successfully." });
//        }

//        [HttpPost("UpdateCourseFee")]
//        public async Task<IActionResult> UpdateCourseFee([FromBody] CourseFee courseFee)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


//            courseFee.UpdatedBy = userId;

//            await _courseFeeService.UpdateCourseFeeAsync(courseFee);

//            return Ok(new { message = "Course fee updated successfully." });
//        }

//        [HttpPost("Delete/{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            try
//            {
//                var deleted = await _courseFeeService.DeleteCourseFeeAsync(id);
//                return Json(new { success = true, message = "Course fee deleted successfully" });
//            }
//            catch (ArgumentException ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, "Internal server error");
//            }
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Web.Controllers
{
    [Route("CourseFee")]
    public class CourseFeeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ICourseFeeService _courseFeeService;

        public CourseFeeController(HttpClient httpClient, ICourseFeeService courseFeeService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7024/api/"); // Adjust as needed
            //_courseFeeService = courseFeeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("GetAllfeeCourseIds")]
        //public IActionResult GetAllCourseIds()
        //{
        //    try
        //    {
        //        var courseIds = _courseFeeService.GetAllCourseExamIds(); // Implement this method in your service
        //        return Json(courseIds);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = "Failed to retrieve Course IDs", error = ex.Message });
        //    }
        //}

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _httpClient.GetAsync("CourseFee/GetAll");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var courseFees = JsonConvert.DeserializeObject<List<CourseFee>>(await response.Content.ReadAsStringAsync());

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = courseFees.Count,
                    recordsFiltered = courseFees.Count,
                    data = courseFees
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("AddCourseFee")]
        public async Task<IActionResult> Create([FromBody] CourseFee courseFee)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(courseFee), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("CourseFee/Create", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"CourseFee/GetById/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }

            var courseFee = JsonConvert.DeserializeObject<CourseFee>(await response.Content.ReadAsStringAsync());
            if (courseFee == null)
            {
                return NotFound();
            }
            return Ok(courseFee);
        }

        [HttpPost("UpdateCourseFee")]
        public async Task<IActionResult> Update([FromBody] CourseFee courseFee)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(courseFee), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("CourseFee/Update", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"CourseFee/Delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}
