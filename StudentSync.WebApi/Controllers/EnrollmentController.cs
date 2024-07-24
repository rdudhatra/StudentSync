using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.ApiControllers
{
    [Route("api/Enrollment")]
    [ApiController]
    [Authorize]
    public class EnrollmentApiController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentApiController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet("GetAllEnrollmentNumbers")]
        public async Task<IActionResult> GetAllEnrollmentNumbers()
        { 
            try
            {
                var enrollmentNumbers = _enrollmentService.GetAllEnrollMentno();
                return Ok(enrollmentNumbers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to retrieve enrollment numbers", error = ex.Message });
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var enrollments = await _enrollmentService.GetAllEnrollments();
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _enrollmentService.AddEnrollment(enrollment);
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var enrollment = await _enrollmentService.GetEnrollmentById(id);
                if (enrollment == null)
                {
                    return NotFound();
                }
                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _enrollmentService.UpdateEnrollment(enrollment);
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var enrollment = await _enrollmentService.GetEnrollmentById(id);
                if (enrollment == null)
                {
                    return NotFound();
                }
                await _enrollmentService.DeleteEnrollment(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
