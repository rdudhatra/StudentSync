//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Data.Models;
//using System;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace StudentSync.Controllers
//{
//    [Route("CourseSyllabus")] 
//    public class CourseSyllabusController : Controller
//    {
//        private readonly ICourseSyllabusService _courseSyllabusService;

//        public CourseSyllabusController(ICourseSyllabusService courseSyllabusService)
//        {
//            _courseSyllabusService = courseSyllabusService;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }


//        [HttpGet("GetAll")]
//        public async Task<IActionResult> GetAll()
//        {
//            try
//            {
//                var searchValue = Request.Query["search[value]"].FirstOrDefault();
//                var courseSyllabuses = await _courseSyllabusService.GetAllCourseSyllabusesAsync();

//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    courseSyllabuses = courseSyllabuses
//                        .Where(cs => cs.ChapterName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
//                                     cs.TopicName.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
//                        .ToList();
//                }

//                var dataTableResponse = new
//                {
//                    draw = Request.Query["draw"],
//                    recordsTotal = courseSyllabuses.Count(),
//                    recordsFiltered = courseSyllabuses.Count(),
//                    data = courseSyllabuses
//                };

//                return Ok(dataTableResponse);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, "Internal server error");
//            }
//        }


//        [HttpGet("Edit/{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var courseSyllabus = await _courseSyllabusService.GetCourseSyllabusByIdAsync(id);
//            if (courseSyllabus == null)
//                return NotFound();

//            return Ok(courseSyllabus);
//        }

//        [HttpPost("Create")]
//        public async Task<IActionResult> Create([FromBody] CourseSyllabus courseSyllabus)
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

//            try
//            {
//                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//                courseSyllabus.CreatedBy = userId;
//                courseSyllabus.UpdatedBy = userId;

//                await _courseSyllabusService.AddCourseSyllabusAsync(courseSyllabus);

//                return Ok(new { message = "Course syllabus added successfully." });
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpPost("UpdateCourseSyllabus")]
//        public async Task<IActionResult> UpdateCourseSyllabus([FromBody] CourseSyllabus courseSyllabus)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//                courseSyllabus.UpdatedBy = userId;

//                await _courseSyllabusService.UpdateCourseSyllabusAsync(courseSyllabus);

//                return Ok(new { message = "Course syllabus updated successfully." });
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpPost("DeleteConfirmed/{id}")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            try
//            {
//                var deleted = await _courseSyllabusService.DeleteCourseSyllabusAsync(id);
//                return Ok(new { success = true, message = "Course syllabus deleted successfully" });
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new { success = false, message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, "Internal server error");
//            }
//        }
//    }
//}



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
    [Route("CourseSyllabus")]
    public class CourseSyllabusController : Controller
    {
        private readonly HttpClient _httpClient;

        public CourseSyllabusController(HttpClient httpClient)
        {
            _httpClient = httpClient;
           // _httpClient.BaseAddress = new Uri("https://localhost:7024/api/"); // Adjust as needed
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _httpClient.GetAsync("CourseSyllabus/GetAll");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var courseSyllabuses = JsonConvert.DeserializeObject<List<CourseSyllabus>>(await response.Content.ReadAsStringAsync());

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = courseSyllabuses.Count,
                    recordsFiltered = courseSyllabuses.Count,
                    data = courseSyllabuses
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
        public async Task<IActionResult> Create([FromBody] CourseSyllabus courseSyllabus)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(courseSyllabus), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("CourseSyllabus/Create", content);
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
            var response = await _httpClient.GetAsync($"CourseSyllabus/Edit/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }

            var courseSyllabus = JsonConvert.DeserializeObject<CourseSyllabus>(await response.Content.ReadAsStringAsync());
            if (courseSyllabus == null)
            {
                return NotFound();
            }
            return Ok(courseSyllabus);
        }

        [HttpPost("UpdateCourseSyllabus")]
        public async Task<IActionResult> Update([FromBody] CourseSyllabus courseSyllabus)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(courseSyllabus), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("CourseSyllabus/Update", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"CourseSyllabus/Delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}
