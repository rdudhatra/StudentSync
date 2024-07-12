//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Data.Models;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace StudentSync.Web.Controllers
//{
//    [Route("Enrollment")]
//    public class EnrollmentController : Controller
//    {
//        private readonly IEnrollmentService _enrollmentService;

//        public EnrollmentController(IEnrollmentService enrollmentService)
//        {
//            _enrollmentService = enrollmentService;
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
//                var enrollments = await _enrollmentService.GetAllEnrollments();

//                var dataTableResponse = new
//                {
//                    draw = Request.Query["draw"].FirstOrDefault(),
//                    recordsTotal = enrollments.Count(),
//                    recordsFiltered = enrollments.Count(),
//                    data = enrollments
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
//        public async Task<IActionResult> Create([FromBody] Enrollment enrollment)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    await _enrollmentService.AddEnrollment(enrollment);
//                    return Ok(new { success = true });
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Exception occurred: {ex.Message}");
//                    return StatusCode(500, "Internal server error");
//                }
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpGet("Edit/{id}")]
//        public async Task<IActionResult> Edit(int id)
//        {
//            var enrollment = await _enrollmentService.GetEnrollmentById(id);
//            if (enrollment == null)
//            {
//                return NotFound();
//            }
//            return Ok(enrollment);
//        }

//        [HttpPost("Update")]
//        public async Task<IActionResult> Update([FromBody] Enrollment enrollment)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    await _enrollmentService.UpdateEnrollment(enrollment);
//                    return Ok(new { success = true });
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Exception occurred: {ex.Message}");
//                    return StatusCode(500, "Internal server error");
//                }
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpPost("Delete/{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var enrollment = await _enrollmentService.GetEnrollmentById(id);
//            if (enrollment == null)
//            {
//                return NotFound();
//            }

//            try
//            {
//                await _enrollmentService.DeleteEnrollment(id);
//                return Ok(new { success = true });
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
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Web.Controllers
{
    [Route("Enrollment")]
    public class EnrollmentController : Controller
    {
        private readonly HttpClient _httpClient;

        public EnrollmentController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7024/api/"); // Adjust as needed
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
                var response = await _httpClient.GetAsync("Enrollment/GetAll");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var enrollments = JsonConvert.DeserializeObject<List<Enrollment>>(await response.Content.ReadAsStringAsync());

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = enrollments.Count(),
                    recordsFiltered = enrollments.Count(),
                    data = enrollments
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
        public async Task<IActionResult> Create([FromBody] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(enrollment), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Enrollment/Create", content);
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
            var response = await _httpClient.GetAsync($"Enrollment/Edit/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }

            var enrollment = JsonConvert.DeserializeObject<Enrollment>(await response.Content.ReadAsStringAsync());
            if (enrollment == null)
            {
                return NotFound();
            }
            return Ok(enrollment);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(enrollment), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Enrollment/Update", content);
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
            var response = await _httpClient.PostAsync($"Enrollment/Delete/{id}", null);
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}


