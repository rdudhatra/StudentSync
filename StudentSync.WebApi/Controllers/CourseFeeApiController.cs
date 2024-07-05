using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;

namespace StudentSync.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseFeeApiController : ControllerBase
    {
        private readonly ICourseFeeService _courseFeeService;

        public CourseFeeApiController(ICourseFeeService courseFeeService)
        {
            _courseFeeService = courseFeeService;
        }

        [HttpGet]
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
                    return NotFound();

                return Ok(courseFee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CourseFee courseFee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _courseFeeService.AddCourseFeeAsync(courseFee);
                return Ok(new { message = "Course fee added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CourseFee courseFee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _courseFeeService.UpdateCourseFeeAsync(courseFee);
                return Ok(new { message = "Course fee updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _courseFeeService.DeleteCourseFeeAsync(id);
                return Ok(new { message = "Course fee deleted successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
