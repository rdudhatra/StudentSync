



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
    [Route("StudentAssessment")]
    public class StudentAssessmentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StudentAssessmentController> _logger;

        public StudentAssessmentController(HttpClient httpClient, ILogger<StudentAssessmentController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
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
                var response = await _httpClient.GetAsync("StudentAssessment/GetAll");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var studentAssessments = JsonConvert.DeserializeObject<List<StudentAssessment>>(await response.Content.ReadAsStringAsync());

                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    studentAssessments = studentAssessments
                        .Where(sa => sa.AssessmentDate.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     sa.EnrollmentNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     sa.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = studentAssessments.Count(),
                    recordsFiltered = studentAssessments.Count(),
                    data = studentAssessments
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching all student assessments.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] StudentAssessment studentAssessment)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(studentAssessment), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("StudentAssessment/Create", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true, message = "Student assessment added successfully." });
                }
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"StudentAssessment/Edit/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }

            var studentAssessment = JsonConvert.DeserializeObject<StudentAssessment>(await response.Content.ReadAsStringAsync());
            if (studentAssessment == null)
            {
                return NotFound();
            }
            return Ok(studentAssessment);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] StudentAssessment studentAssessment)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(studentAssessment), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("StudentAssessment/Update", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true, message = "Student assessment updated successfully." });
                }
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"StudentAssessment/Delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true, message = "Student assessment deleted successfully." });
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}
