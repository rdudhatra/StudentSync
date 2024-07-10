using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Controllers.Api
{
    [Route("api/studentassessments")]
    [ApiController]
    public class StudentAssessmentApiController : ControllerBase
    {
        private readonly IStudentAssessmentService _studentAssessmentService;
        private readonly ILogger<StudentAssessmentApiController> _logger;

        public StudentAssessmentApiController(IStudentAssessmentService studentAssessmentService, ILogger<StudentAssessmentApiController> logger)
        {
            _studentAssessmentService = studentAssessmentService;
            _logger = logger;
        }

        // GET: api/studentassessments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var studentAssessments = await _studentAssessmentService.GetAllStudentAssessments();
                return Ok(studentAssessments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving student assessments.");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/studentassessments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var studentAssessment = await _studentAssessmentService.GetStudentAssessmentById(id);
                if (studentAssessment == null)
                {
                    return NotFound();
                }
                return Ok(studentAssessment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving student assessment.");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/studentassessments
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentAssessment studentAssessment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _studentAssessmentService.SaveStudentAssessment(studentAssessment);
                    return CreatedAtAction(nameof(GetById), new { id = studentAssessment.Id }, studentAssessment);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while saving student assessment.");
                    return StatusCode(500, "Internal server error");
                }
            }
            return BadRequest(ModelState);
        }

        // PUT: api/studentassessments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentAssessment studentAssessment)
        {
            if (id != studentAssessment.Id)
            {
                return BadRequest("ID mismatch between URL and body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _studentAssessmentService.UpdateStudentAssessment(studentAssessment);
                return Ok(new { success = true, message = "Student assessment updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating student assessment.");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/studentassessments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _studentAssessmentService.DeleteStudentAssessment(id);
                return Ok(new { success = true, message = "Student assessment deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting student assessment.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
