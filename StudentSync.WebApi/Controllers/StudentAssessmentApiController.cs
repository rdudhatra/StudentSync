using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.ApiControllers
{
    [Route("api/StudentAssessment")]
    [ApiController]
    [Authorize]
    public class StudentAssessmentApiController : ControllerBase
    {
        private readonly IStudentAssessmentService _studentAssessmentService;
        private readonly ILogger<StudentAssessmentApiController> _logger;

        public StudentAssessmentApiController(IStudentAssessmentService studentAssessmentService, ILogger<StudentAssessmentApiController> logger)
        {
            _studentAssessmentService = studentAssessmentService;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var studentAssessments = await _studentAssessmentService.GetAllStudentAssessments();
                return Ok(studentAssessments);
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
                try
                {
                    await _studentAssessmentService.SaveStudentAssessment(studentAssessment);
                    return Ok(new { success = true, message = "Student assessment added successfully." });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while creating student assessment.");
                    return StatusCode(500, "Internal server error");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
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
                _logger.LogError(ex, "Exception occurred while editing student assessment.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] StudentAssessment studentAssessment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _studentAssessmentService.UpdateStudentAssessment(studentAssessment);
                    return Ok(new { success = true, message = "Student assessment updated successfully." });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while updating student assessment.");
                    return StatusCode(500, "Internal server error");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _studentAssessmentService.DeleteStudentAssessment(id);
                return Ok(new { success = true, message = "Student assessment deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting student assessment.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
 