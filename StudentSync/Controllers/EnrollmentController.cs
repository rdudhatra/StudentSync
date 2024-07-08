using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("Enrollment")]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
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
                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    enrollments = enrollments
                        .Where(e => e.EnrollmentNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                    e.BatchId.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                    e.CourseId.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                    e.CourseFeeId.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                    e.InquiryNo.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                    e.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = enrollments.Count(),
                    recordsFiltered = enrollments.Count(),
                    data = enrollments
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    enrollment.CreatedDate = DateTime.Now;
                    await _enrollmentService.CreateEnrollmentAsync(enrollment);
                    return Ok(new { success = true, message = "Enrollment added successfully." });
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it appropriately
                    return StatusCode(500, new { success = false, message = "Error saving enrollment." });
                }
            }
            // If ModelState is invalid, return BadRequest with ModelState errors
            return BadRequest(ModelState);
        }


        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
            return Json(enrollment);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                enrollment.UpdatedDate = DateTime.Now;
                await _enrollmentService.UpdateEnrollmentAsync(enrollment);
                return Json(new { success = true, message = "Enrollment updated successfully." });
            }
            return Json(new { success = false, message = "Invalid model state." });
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _enrollmentService.DeleteEnrollmentAsync(id);
            return Json(new { success = true, message = "Enrollment deleted successfully." });
        }
    }
}
