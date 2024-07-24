using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.ApiControllers
{
    [Route("api/StudentAttendance")]
    [ApiController]
    [Authorize]
    public class StudentAttendanceApiController : ControllerBase
    {
        private readonly IStudentAttendanceService _studentAttendanceService;
        private readonly ILogger<StudentAttendanceApiController> _logger;

        public StudentAttendanceApiController(IStudentAttendanceService studentAttendanceService, ILogger<StudentAttendanceApiController> logger)
        {
            _studentAttendanceService = studentAttendanceService;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var studentAttendances = await _studentAttendanceService.GetAllStudentAttendances();
                return Ok(studentAttendances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching all student attendances.");
                return StatusCode(500, "Internal server error");
            } 
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] StudentAttendance studentAttendance)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _studentAttendanceService.AddStudentAttendance(studentAttendance);
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while creating student attendance.");
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
                var studentAttendance = await _studentAttendanceService.GetStudentAttendanceById(id);
                if (studentAttendance == null)
                {
                    return NotFound();
                }
                return Ok(studentAttendance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while editing student attendance.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] StudentAttendance studentAttendance)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _studentAttendanceService.UpdateStudentAttendance(studentAttendance);
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while updating student attendance.");
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
                var studentAttendance = await _studentAttendanceService.GetStudentAttendanceById(id);
                if (studentAttendance == null)
                {
                    return NotFound();
                }
                await _studentAttendanceService.DeleteStudentAttendance(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting student attendance.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
