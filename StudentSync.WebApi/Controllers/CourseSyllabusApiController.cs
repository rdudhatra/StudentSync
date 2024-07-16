//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Data.Models;
//using System;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace StudentSync.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CourseSyllabusApiController : ControllerBase
//    {
//        private readonly ICourseSyllabusService _courseSyllabusService;

//        public CourseSyllabusApiController(ICourseSyllabusService courseSyllabusService)
//        {
//            _courseSyllabusService = courseSyllabusService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            try
//            {
//                var searchValue = Request.Query["search[value]"].FirstOrDefault();
//                var courseSyllabuses = await _courseSyllabusService.GetAllCourseSyllabusesAsync();

//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    courseSyllabuses = courseSyllabuses
//                        .Where(cs => cs.ChapterName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
//                                     cs.TopicName.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
//                        .ToList();
//                }

//                var dataTableResponse = new
//                {
//                    draw = Request.Query["draw"],
//                    recordsTotal = courseSyllabuses.Count(),
//                    recordsFiltered = courseSyllabuses.Count(),
//                    data = courseSyllabuses
//                };

//                return Ok(dataTableResponse);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpGet("GetById/{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var courseSyllabus = await _courseSyllabusService.GetCourseSyllabusByIdAsync(id);
//            if (courseSyllabus == null)
//                return NotFound();

//            return Ok(courseSyllabus);
//        }

//        [HttpPost("Create")]
//        public async Task<IActionResult> Create([FromBody] CourseSyllabus courseSyllabus)
//        {
//            if (!ModelState.IsValid)
//            {
//                var errors = ModelState.Values.SelectMany(v => v.Errors);
//                foreach (var error in errors)
//                {
//                    Console.WriteLine(error.ErrorMessage);
//                }
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//                courseSyllabus.CreatedBy = userId;
//                courseSyllabus.UpdatedBy = userId;

//                await _courseSyllabusService.AddCourseSyllabusAsync(courseSyllabus);

//                return Ok(new { message = "Course syllabus added successfully." });
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpPut]
//        public async Task<IActionResult> Update([FromBody] CourseSyllabus courseSyllabus)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//                courseSyllabus.UpdatedBy = userId;

//                await _courseSyllabusService.UpdateCourseSyllabusAsync(courseSyllabus);

//                return Ok(new { message = "Course syllabus updated successfully." });
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpDelete]
//        public async Task<IActionResult> Delete(int id)
//        {
//            try
//            {
//                var deleted = await _courseSyllabusService.DeleteCourseSyllabusAsync(id);
//                return Ok(new { success = true, message = "Course syllabus deleted successfully" });
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(new { success = false, message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, "Internal server error");
//            }
//        }
//    }
//}



using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Threading.Tasks;

namespace StudentSync.ApiControllers
{
    [Route("api/CourseSyllabus")]
    [ApiController]
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
