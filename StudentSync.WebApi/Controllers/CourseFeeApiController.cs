


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Threading.Tasks;

namespace StudentSync.ApiControllers
{
    [Route("api/CourseFee")]
    [ApiController]
    [Authorize]
    public class CourseFeeApiController : ControllerBase
    {
        private readonly ICourseFeeService _courseFeeService;

        public CourseFeeApiController(ICourseFeeService courseFeeService)
        {
            _courseFeeService = courseFeeService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var courseFees = await _courseFeeService.GetAllCourseFeesAsync();
                return Ok(courseFees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            } 
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var courseFee = await _courseFeeService.GetCourseFeeByIdAsync(id);
                if (courseFee == null)
                {
                    return NotFound();
                }
                return Ok(courseFee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CourseFee courseFee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _courseFeeService.AddCourseFeeAsync(courseFee);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CourseFee courseFee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _courseFeeService.UpdateCourseFeeAsync(courseFee);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var courseFee = await _courseFeeService.GetCourseFeeByIdAsync(id);
                if (courseFee == null)
                {
                    return NotFound();
                }
                await _courseFeeService.DeleteCourseFeeAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
