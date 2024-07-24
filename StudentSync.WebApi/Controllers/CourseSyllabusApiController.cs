




using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Threading.Tasks;

namespace StudentSync.ApiControllers
{
    [Route("api/CourseSyllabus")]
    [ApiController]
    [Authorize]
    public class CourseSyllabusApiController : ControllerBase
    {
        private readonly ICourseSyllabusService _courseSyllabusService;

        public CourseSyllabusApiController(ICourseSyllabusService courseSyllabusService)
        {
            _courseSyllabusService = courseSyllabusService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var courseSyllabuses = await _courseSyllabusService.GetAllCourseSyllabusesAsync();
                return Ok(courseSyllabuses);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CourseSyllabus courseSyllabus)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _courseSyllabusService.AddCourseSyllabusAsync(courseSyllabus);
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
                var courseSyllabus = await _courseSyllabusService.GetCourseSyllabusByIdAsync(id);
                if (courseSyllabus == null)
                {
                    return NotFound();
                }
                return Ok(courseSyllabus);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CourseSyllabus courseSyllabus)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _courseSyllabusService.UpdateCourseSyllabusAsync(courseSyllabus);
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
                var courseSyllabus = await _courseSyllabusService.GetCourseSyllabusByIdAsync(id);
                if (courseSyllabus == null)
                {
                    return NotFound();
                }
                await _courseSyllabusService.DeleteCourseSyllabusAsync(id);
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
