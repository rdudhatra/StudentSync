using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interfaces;
using StudentSync.Data.Models;

namespace StudentSync.Controllers
{
    [Route("StudentInstallment")]
    public class StudentInstallmentController : Controller
    {
        private readonly IStudentInstallmentService _studentInstallmentService;

        public StudentInstallmentController(IStudentInstallmentService studentInstallmentService)
        {
            _studentInstallmentService = studentInstallmentService;
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
                var studentInstallments = await _studentInstallmentService.GetAllStudentInstallmentsAsync();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    studentInstallments = studentInstallments
                        .Where(si => si.ReceiptNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     si.EnrollmentNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     si.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] StudentInstallment studentInstallment)
        {
            if (ModelState.IsValid)
            {
                await _studentInstallmentService.CreateStudentInstallmentAsync(studentInstallment);
                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var studentInstallment = await _studentInstallmentService.GetStudentInstallmentByIdAsync(id);
            if (studentInstallment == null)
            {
                return NotFound();
            }
            return Ok(studentInstallment);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] StudentInstallment studentInstallment)
        {
            if (ModelState.IsValid)
            {
                await _studentInstallmentService.UpdateStudentInstallmentAsync(studentInstallment);
                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var studentInstallment = await _studentInstallmentService.GetStudentInstallmentByIdAsync(id);
            if (studentInstallment == null)
            {
                return NotFound();
            }
            await _studentInstallmentService.DeleteStudentInstallmentAsync(id);
            return Ok(new { success = true });
        }
    }
}
