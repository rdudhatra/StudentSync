
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
using System;
using System.Collections;
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
        public CourseExamController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAllExamCourseIds")]
        public async Task<IActionResult> GetAllCourseExamIds()
        {
            try
            {
                var response = await _httpClient.GetAsync("CourseExam/GetAllCourseExamIds");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var courseExamIds = JsonConvert.DeserializeObject<List<CourseExam>>(await response.Content.ReadAsStringAsync());
                return Json(courseExamIds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to retrieve Course Exam IDs", error = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // DataTables parameters
                int draw = int.Parse(Request.Query["draw"]);
                int start = int.Parse(Request.Query["start"]);
                int length = int.Parse(Request.Query["length"]);

                var response = await _httpClient.GetAsync("CourseExam/GetAll");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var courseExams = JsonConvert.DeserializeObject<List<CourseExamResponseModel>>(await response.Content.ReadAsStringAsync());

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

                // Paginate the results
                int recordsTotal = courseExams.Count;
                courseExams = courseExams.Skip(start).Take(length).ToList();

                var dataTableResponse = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal, // Assuming no filtering at server-side
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

        [HttpPost("UpdateCourseExam")]
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