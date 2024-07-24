


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Threading.Tasks;

namespace StudentSync.ApiControllers
{
    [Route("api/CourseExam")]
    [ApiController]
    [Authorize]
    public class CourseExamApiController : ControllerBase
    {
        private readonly ICourseExamServices _courseExamServices;

        public CourseExamApiController(ICourseExamServices courseExamServices)
        {
            _courseExamServices = courseExamServices;
        }

        [HttpGet("GetAllCourseExamIds")]
        public async Task<IActionResult> GetAllCourseExamIds()
        {
            try 
            {
                var courseExamIds = _courseExamServices.GetAllCourseExamIds();
                return Ok(courseExamIds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to retrieve Course Exam IDs", error = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var courseExams = await _courseExamServices.GetAllCourseExamsAsync();
                return Ok(courseExams);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CourseExam courseExam)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _courseExamServices.AddCourseExamAsync(courseExam);
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
                var courseExam = await _courseExamServices.GetCourseExamByIdAsync(id);
                if (courseExam == null)
                {
                    return NotFound();
                }
                return Ok(courseExam);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CourseExam courseExam)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _courseExamServices.UpdateCourseExamAsync(courseExam);
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
                var courseExam = await _courseExamServices.GetCourseExamByIdAsync(id);
                if (courseExam == null)
                {
                    return NotFound();
                }
                await _courseExamServices.DeleteCourseExamAsync(id);
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

