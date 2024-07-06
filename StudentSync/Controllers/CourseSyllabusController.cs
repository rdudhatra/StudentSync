using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("CourseSyllabus")] 
    public class CourseSyllabusController : Controller
    {
        private readonly ICourseSyllabusService _courseSyllabusService;

        public CourseSyllabusController(ICourseSyllabusService courseSyllabusService)
        {
            _courseSyllabusService = courseSyllabusService;
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
                var courseSyllabuses = await _courseSyllabusService.GetAllCourseSyllabusesAsync();
                var dataTableResponse = new
                {
                    draw = Request.Query["draw"],
                    recordsTotal = courseSyllabuses.Count(),
                    recordsFiltered = courseSyllabuses.Count(),
                    data = courseSyllabuses
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var courseSyllabus = await _courseSyllabusService.GetCourseSyllabusByIdAsync(id);
            if (courseSyllabus == null)
                return NotFound();

            return Ok(courseSyllabus);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CourseSyllabus courseSyllabus)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                courseSyllabus.CreatedBy = userId;
                courseSyllabus.UpdatedBy = userId;

                await _courseSyllabusService.AddCourseSyllabusAsync(courseSyllabus);

                return Ok(new { message = "Course syllabus added successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("UpdateCourseSyllabus")]
        public async Task<IActionResult> UpdateCourseSyllabus([FromBody] CourseSyllabus courseSyllabus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                courseSyllabus.UpdatedBy = userId;

                await _courseSyllabusService.UpdateCourseSyllabusAsync(courseSyllabus);

                return Ok(new { message = "Course syllabus updated successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var deleted = await _courseSyllabusService.DeleteCourseSyllabusAsync(id);
                return Ok(new { success = true, message = "Course syllabus deleted successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
