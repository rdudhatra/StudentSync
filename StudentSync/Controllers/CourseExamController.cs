//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using StudentSync.Core.Services;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Data.Data;
//using StudentSync.Data.Models;
//using System;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace StudentSync.Controllers
//{
//    [Route("CourseExam")]
//    public class CourseExamController : Controller
//    {
//        private readonly StudentSyncDbContext _context;
//        private readonly ICourseExamServices _courseExamServices;

//        public CourseExamController(StudentSyncDbContext context, ICourseExamServices courseExamServices)
//        {
//            _context = context;
//            _courseExamServices = courseExamServices;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpGet("GetAllExamCourseIds")]
//        public IActionResult GetAllCourseIds()
//        {
//            try
//            {
//                var courseIds = _courseExamServices.GetAllCourseExamIds(); // Implement this method in your service
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
//                var searchValue = Request.Query["search[value]"].FirstOrDefault();
//                var courseExams = await _courseExamServices.GetAllCourseExamsAsync();

//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    courseExams = courseExams
//                        .Where(ce =>
//                            ce.ExamTitle.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
//                            ce.ExamType.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
//                            ce.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
//                        .ToList();
//                }

//                var dataTableResponse = new
//                {
//                    draw = Request.Query["draw"].FirstOrDefault(), // You may adjust this if `draw` is not present in your request
//                    recordsTotal = courseExams.Count(),
//                    recordsFiltered = courseExams.Count(),
//                    data = courseExams
//                };

//                return Ok(dataTableResponse);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, "Internal server error");
//            }
//        }



//        [HttpPost("Create")]
//        public async Task<IActionResult> Create([FromBody] CourseExam courseExam)
//        {
//            if (ModelState.IsValid)
//            {
//                var result = await _courseExamServices.AddCourseExamAsync(courseExam);
//                if (result.Succeeded)
//                {
//                    return Json(new { success = true, message = result.Messages });
//                }
//                return BadRequest(result.Messages);
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpGet("Edit/{id}")]
//        public async Task<IActionResult> Edit(int id)
//        {
//            var courseExam = await _courseExamServices.GetCourseExamByIdAsync(id);
//            if (courseExam == null)
//            {
//                return NotFound();
//            }
//            return Ok(courseExam);


//        }

//        [HttpPost("UpdateCourseExam")]
//        public async Task<IActionResult> UpdateCourseExam([FromBody] CourseExam courseExam)
//            {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);

//            }
//            await _courseExamServices.UpdateCourseExamAsync(courseExam);

//            return Ok(new { message = "Course Exam updated successfully." });

//        }




//        [HttpPost("DeleteConfirmed/{id}")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var result = await _courseExamServices.DeleteCourseExamAsync(id);
//            if (result.Succeeded)
//            {
//                return Json(new { success = true, message = result.Messages });
//            }
//            return BadRequest(result.Messages);
//        }

//        [HttpGet("SearchByExamTitle")]
//        public async Task<IActionResult> SearchByExamTitle(string examTitle)
//        {
//            var result = await _courseExamServices.SearchCourseExamByExamTitleAsync(examTitle);
//            if (result.Succeeded)
//            {
//                return Json(new { data = result.Data });
//            }
//            return BadRequest(result.Messages);
//        }
//    }
//}



using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("CourseExam")]
    public class CourseExamController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ICourseExamServices _courseExamServices;

        public CourseExamController(HttpClient httpClient, ICourseExamServices courseExamServices)
        {
            _courseExamServices = courseExamServices;
            _httpClient = httpClient;
           // _httpClient.BaseAddress = new Uri("https://localhost:7024/api/"); // Adjust as needed
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAllExamCourseIds")]
        public IActionResult GetAllCourseIds()
        {
            try
            {
                var courseIds = _courseExamServices.GetAllCourseExamIds();
                return Json(courseIds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to retrieve Course IDs", error = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _httpClient.GetAsync("CourseExam/GetAll");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var courseExams = JsonConvert.DeserializeObject<List<CourseExam>>(await response.Content.ReadAsStringAsync());

                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    courseExams = courseExams
                        .Where(ce =>
                            ce.ExamTitle.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                            ce.ExamType.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                            ce.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = courseExams.Count(),
                    recordsFiltered = courseExams.Count(),
                    data = courseExams
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CourseExam courseExam)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(courseExam), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("CourseExam/Create", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"CourseExam/Edit/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }

            var courseExam = JsonConvert.DeserializeObject<CourseExam>(await response.Content.ReadAsStringAsync());
            if (courseExam == null)
            {
                return NotFound();
            }
            return Ok(courseExam);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] CourseExam courseExam)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(courseExam), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("CourseExam/Update", content);
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
            var response = await _httpClient.DeleteAsync($"CourseExam/Delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

    }
}