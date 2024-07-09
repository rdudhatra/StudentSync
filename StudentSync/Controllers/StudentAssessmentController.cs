using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;

namespace StudentSync.Controllers
{
    [Route("StudentAssessment")]
    public class StudentAssessmentController : Controller
    {
        private readonly IStudentAssessmentService _studentAssessmentService;

        public StudentAssessmentController(IStudentAssessmentService studentAssessmentService)
        {
            _studentAssessmentService = studentAssessmentService;
        }

        // GET: StudentAssessment
        public IActionResult Index()
        {
            return View();
        }

        // GET: StudentAssessment/GetAll
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                var studentAssessments = await _studentAssessmentService.GetAllStudentAssessments();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    studentAssessments = studentAssessments
                        .Where(sa => sa.AssessmentDate.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     sa.EnrollmentNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     sa.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = studentAssessments.Count(),
                    recordsFiltered = studentAssessments.Count(),
                    data = studentAssessments
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
      
        // POST: StudentAssessment/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] StudentAssessment studentAssessment)
        {
            if (ModelState.IsValid)
            {
                await _studentAssessmentService.SaveStudentAssessment(studentAssessment);
                return Json(new { success = true, message = "Student assessment added successfully." });
            }
            return Json(new { success = false, message = "Invalid student assessment data." });
        }

        // GET: StudentAssessment/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var studentAssessment = await _studentAssessmentService.GetStudentAssessmentById(id);
                if (studentAssessment == null)
                {
                    return NotFound();
                }
                return Json(studentAssessment);
            }
            catch (Exception ex)
            {
                // Log the detailed exception
                Console.WriteLine($"Exception occurred: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, "Internal server error");
            }
        }



        // POST: StudentAssessment/Update
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] StudentAssessment studentAssessment)
        {
            if (ModelState.IsValid)
            {
                await _studentAssessmentService.UpdateStudentAssessment(studentAssessment);
                return Json(new { success = true, message = "Student assessment updated successfully." });
            }

            // If ModelState is invalid, collect error messages
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }

        // POST: StudentAssessment/Delete/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _studentAssessmentService.DeleteStudentAssessment(id);
            return Json(new { success = true, message = "Student assessment deleted successfully." });
        }
    }
}
