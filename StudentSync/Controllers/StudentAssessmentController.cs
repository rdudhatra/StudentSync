
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
using StudentSync.Service.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Web.Controllers
{
    [Route("StudentAssessment")]
    public class StudentAssessmentController : Controller
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<StudentAssessmentController> _logger;

        public StudentAssessmentController(IHttpService httpService, ILogger<StudentAssessmentController> logger)
        {
            _httpService = httpService;
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
                // DataTables parameters
                int draw = int.Parse(Request.Query["draw"]);
                int start = int.Parse(Request.Query["start"]);
                int length = int.Parse(Request.Query["length"]);

                var response = await _httpService.Get<List<StudentAssessmentResponseModel>>("StudentAssessment/GetAll");
                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                var studentAssessments = response.Data;

                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    studentAssessments = studentAssessments
                        .Where(sa => sa.AssessmentDate.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     sa.EnrollmentNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     sa.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Paginate the results
                int recordsTotal = studentAssessments.Count;
                studentAssessments = studentAssessments.Skip(start).Take(length).ToList();

                var dataTableResponse = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
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
                var response = await _httpService.Post("StudentAssessment/Create", studentAssessment);
                if (response.Succeeded)
                {
                    return Ok(new { success = true, message = "Student assessment added successfully." });
                }
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpService.Get<StudentAssessment>($"StudentAssessment/Edit/{id}");
            if (!response.Succeeded)
            {
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }

            var studentAssessment = response.Data;
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
                var response = await _httpService.Put("StudentAssessment/Update", studentAssessment);
                if (response.Succeeded)
                {
                    return Ok(new { success = true, message = "Student assessment updated successfully." });
                }
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpService.Delete($"StudentAssessment/Delete/{id}");
            if (response.Succeeded)
            {
                return Ok(new { success = true, message = "Student assessment deleted successfully." });
            }
            return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
        }
    }
}
