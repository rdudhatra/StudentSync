using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interfaces;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.ApiControllers
{
    [Route("api/StudentInstallment")]
    [ApiController]
    [Authorize]
    public class StudentInstallmentApiController : ControllerBase
    {
        private readonly IStudentInstallmentService _studentInstallmentService;
        private readonly ILogger<StudentInstallmentApiController> _logger;

        public StudentInstallmentApiController(IStudentInstallmentService studentInstallmentService, ILogger<StudentInstallmentApiController> logger)
        {
            _studentInstallmentService = studentInstallmentService;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var studentInstallments = await _studentInstallmentService.GetAllStudentInstallmentsAsync();
                return Ok(studentInstallments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching all student installments.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] StudentInstallment studentInstallment)
        {
            if (ModelState.IsValid)
            {
                try
                { 
                    await _studentInstallmentService.CreateStudentInstallmentAsync(studentInstallment);
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while creating student installment.");
                    return StatusCode(500, "Internal server error");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var studentInstallment = await _studentInstallmentService.GetStudentInstallmentByIdAsync(id);
                if (studentInstallment == null)
                {
                    return NotFound();
                }
                return Ok(studentInstallment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while editing student installment.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] StudentInstallment studentInstallment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _studentInstallmentService.UpdateStudentInstallmentAsync(studentInstallment);
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while updating student installment.");
                    return StatusCode(500, "Internal server error");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var studentInstallment = await _studentInstallmentService.GetStudentInstallmentByIdAsync(id);
                if (studentInstallment == null)
                {
                    return NotFound();
                }
                await _studentInstallmentService.DeleteStudentInstallmentAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting student installment.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
