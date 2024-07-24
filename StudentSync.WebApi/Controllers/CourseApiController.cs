

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Threading.Tasks;

namespace StudentSync.Api.Controllers
{
    [Route("api/Course")]
    [ApiController]
    [Authorize]
    public class CourseApiController : ControllerBase
    {
        private readonly ICourseServices _courseServices;

        public CourseApiController(ICourseServices courseServices)
        {
            _courseServices = courseServices;
        }

        [HttpGet("GetAllCourseIds")]
        public async Task<IActionResult> GetAllCourseIds()
        {
            try
            {
                var courseIds =  _courseServices.GetAllCourseIds(); // Implement this method in your service
                return Ok(courseIds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var courses = await _courseServices.GetAllCourseAsync();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var course = await _courseServices.GetCoursesByIdAsync(id);
                if (course == null)
                {
                    return NotFound();
                }
                return Ok(course);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddCourse")]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _courseServices.AddCourseAsync(course);
                    if (result.Succeeded)
                    {
                        return Ok(new { success = true, message = result.Messages });
                    }
                    return BadRequest(result.Messages);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _courseServices.UpdateCourseAsync(course);
                    if (result.Succeeded)
                    {
                        return Ok(new { success = true, message = result.Messages });
                    }
                    return BadRequest(result.Messages);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var result = await _courseServices.DeleteCourseAsync(id);
                if (result.Succeeded)
                {
                    return Ok(new { success = true, message = result.Messages });
                }
                return BadRequest(result.Messages);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

      
    }
}
