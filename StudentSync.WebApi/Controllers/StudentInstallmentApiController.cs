using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interfaces;
using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Controllers.Api
{
    [Route("api/studentinstallments")]
    [ApiController]
    public class StudentInstallmentApiController : ControllerBase
    {
        private readonly IStudentInstallmentService _studentInstallmentService;

        public StudentInstallmentApiController(IStudentInstallmentService studentInstallmentService)
        {
            _studentInstallmentService = studentInstallmentService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {   
                    var searchValue = Request.Query["search[value]"].FirstOrDefault();

                var studentInstallments = await _studentInstallmentService.GetAllStudentInstallmentsAsync();

                // Apply search filter if searchValue is provided
                if (!string.IsNullOrEmpty(searchValue))
                {
                    studentInstallments = studentInstallments.Where(si =>
                        si.ReceiptNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                        si.EnrollmentNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                        si.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

             

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = studentInstallments.Count(),
                    recordsFiltered = studentInstallments.Count(),
                    data = studentInstallments
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentInstallment>> GetStudentInstallmentById(int id)
        {
            var studentInstallment = await _studentInstallmentService.GetStudentInstallmentByIdAsync(id);
            if (studentInstallment == null)
            {
                return NotFound();
            }
            return Ok(studentInstallment);
        }

        [HttpPost]
        public async Task<ActionResult<StudentInstallment>> CreateStudentInstallment(StudentInstallment studentInstallment)
        {
            if (ModelState.IsValid)
            {
                await _studentInstallmentService.CreateStudentInstallmentAsync(studentInstallment);
                return Ok(studentInstallment); // Optionally return the created object with Ok()
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudentInstallment(int id, StudentInstallment studentInstallment)
        {
            if (id != studentInstallment.Id)
            {
                return BadRequest();
            }

            try
            {
                await _studentInstallmentService.UpdateStudentInstallmentAsync(studentInstallment);
            }
            catch (Exception ex)
            {
                if (!StudentInstallmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStudentInstallment(int id)
        {
            var studentInstallment = await _studentInstallmentService.GetStudentInstallmentByIdAsync(id);
            if (studentInstallment == null)
            {
                return NotFound();
            }

            try
            {
                await _studentInstallmentService.DeleteStudentInstallmentAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(new { success = true });
        }

        private bool StudentInstallmentExists(int id)
        {
            // Check if the StudentInstallment with the given id exists in your system
            // This is a placeholder method and may vary based on your implementation
            return true; // Replace with your actual implementation
        }
    }
}
