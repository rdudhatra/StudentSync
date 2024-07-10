using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAttendanceApiController : ControllerBase
    {
        private readonly IStudentAttendanceService _studentAttendanceService;

        public StudentAttendanceApiController(IStudentAttendanceService studentAttendanceService)
        {
            _studentAttendanceService = studentAttendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                var studentAttendances = await _studentAttendanceService.GetAllStudentAttendances();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    studentAttendances = studentAttendances
                        .Where(sa => sa.AttendanceDate.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     sa.EnrollmentNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     sa.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = studentAttendances.Count(),
                    recordsFiltered = studentAttendances.Count(),
                    data = studentAttendances
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentAttendance studentAttendance)
        {
            if (ModelState.IsValid)
            {
                await _studentAttendanceService.AddStudentAttendance(studentAttendance);
                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var studentAttendance = await _studentAttendanceService.GetStudentAttendanceById(id);
            if (studentAttendance == null)
            {
                return NotFound();
            }
            return Ok(studentAttendance);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StudentAttendance studentAttendance)
        {
            if (ModelState.IsValid)
            {
                await _studentAttendanceService.UpdateStudentAttendance(studentAttendance);
                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var studentAttendance = await _studentAttendanceService.GetStudentAttendanceById(id);
            if (studentAttendance == null)
            {
                return NotFound();
            }
            await _studentAttendanceService.DeleteStudentAttendance(id);
            return Ok(new { success = true });
        }
    }
}
