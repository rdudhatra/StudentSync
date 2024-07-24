

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Data.Models;
using StudentSync.Service.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Web.Controllers
{
    [Route("Course")]
    public class CourseController : Controller
    {
        private readonly IHttpService _httpService;

        public CourseController(IHttpService httpService)
        {
            _httpService = httpService;
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
                var response = await _httpService.Get<List<Course>>("Course/GetAllCourseIds");
                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                return Json(response.Data);
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

                var response = await _httpService.Get<List<Course>>("Course/GetAll");
                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                var courses = response.Data;

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
                var response = await _httpService.Post("Course/AddCourse", course);
                if (response.Succeeded)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _httpService.Get<Course>($"Course/GetById/{id}");
            if (!response.Succeeded)
            {
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }

            var course = response.Data;
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
                var response = await _httpService.Put("Course/UpdateCourse", course);
                if (response.Succeeded)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var response = await _httpService.Delete($"Course/DeleteCourse/{id}");
            if (response.Succeeded)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
        }

        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var response = await _httpService.Get<List<Course>>($"Course/SearchByName?name={name}");
            if (response.Succeeded)
            {
                return Json(new { data = response.Response });
            }
            return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
        }
    }
}
