
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using Microsoft.EntityFrameworkCore;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("CourseFee")]

    public class CourseFeeController : Controller
    {
        private readonly ICourseFeeService _courseFeeService;

        public CourseFeeController(ICourseFeeService courseFeeService)
        {
            _courseFeeService = courseFeeService;
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
                var courseFees = await _courseFeeService.GetAllCourseFeesAsync();
                var dataTableResponse = new
                {
                    draw = Request.Query["draw"],
                    recordsTotal = courseFees.Count(),
                    recordsFiltered = courseFees.Count(),
                    data = courseFees
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
            var courseFee = await _courseFeeService.GetCourseFeeByIdAsync(id);
            if (courseFee == null)
                return NotFound();

            return Ok(courseFee);
        }

        [HttpPost("AddCourseFee")]
        public async Task<IActionResult> AddCourseFee([FromBody] CourseFee courseFee)
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

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           

            courseFee.CreatedBy = userId;
            courseFee.UpdatedBy = userId;

            await _courseFeeService.AddCourseFeeAsync(courseFee);

            return Ok(new { message = "Course fee added successfully." });
        }

        [HttpPost("UpdateCourseFee")]
        public async Task<IActionResult> UpdateCourseFee([FromBody] CourseFee courseFee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           

            courseFee.UpdatedBy = userId;

            await _courseFeeService.UpdateCourseFeeAsync(courseFee);

            return Ok(new { message = "Course fee updated successfully." });
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _courseFeeService.DeleteCourseFeeAsync(id);
                return Json(new { success = true, message = "Course fee deleted successfully" });
            }
            catch (ArgumentException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
