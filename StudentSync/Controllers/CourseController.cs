

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Web.Controllers
{
    [Route("Course")]
    public class CourseController : Controller
    {
        private readonly HttpClient _httpClient;

        public CourseController(HttpClient httpClient )
        {
            _httpClient = httpClient;

        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("GetAllCourseIds")]
        public async Task<IActionResult> GetAllCourseIds()
        {
            try
            {
                var response = await _httpClient.GetAsync("Course/GetAllCourseIds");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                
                var courseIds = JsonConvert.DeserializeObject<List<Course>>(await response.Content.ReadAsStringAsync());
                return Json(courseIds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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

                var response = await _httpClient.GetAsync("Course/GetAll");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var courses = JsonConvert.DeserializeObject<List<Course>>(await response.Content.ReadAsStringAsync());

                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    courses = courses
                        .Where(ce =>
                            ce.CourseName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                            ce.Duration.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                            ce.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Paginate the results
                int recordsTotal = courses.Count;
                courses = courses.Skip(start).Take(length).ToList();

                var dataTableResponse = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal, // Assuming no filtering at server-side
                    data = courses
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpPost("AddCourse")]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(course), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Course/AddCourse", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"Course/GetById/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }

            var course = JsonConvert.DeserializeObject<Course>(await response.Content.ReadAsStringAsync());
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

    


        [HttpPost("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] Course course)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(course), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("Course/UpdateCourse", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var response = await _httpClient.DeleteAsync($"Course/DeleteCourse/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var response = await _httpClient.GetAsync($"Course/SearchByName?name={name}");
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<List<Course>>(await response.Content.ReadAsStringAsync());
                return Json(new { data = result });
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}

