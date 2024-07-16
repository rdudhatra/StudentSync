//// CourseApiController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseApiController : ControllerBase
    {
        private readonly ICourseServices _courseServices;
        private readonly StudentSyncDbContext _context;


        public CourseApiController(ICourseServices courseServices, StudentSyncDbContext context)
        {
            _courseServices = courseServices;
            _context = context;
        }

        // GET: api/CourseApi/GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll(int draw, int start, int length, string searchValue, string sortColumn, string sortColumnDirection)
        {
            try
            {
                var query = _context.Courses.AsQueryable();

                // Apply search filter if searchValue is provided
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(c =>
                        c.CourseName.Contains(searchValue) ||
                        c.Duration.Contains(searchValue) ||
                        c.PreRequisite.Contains(searchValue) ||
                        c.Remarks.Contains(searchValue));
                }

                // Sort data
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    switch (sortColumn.ToLower())
                    {
                        case "courseid":
                            query = sortColumnDirection.ToLower() == "asc" ?
                                query.OrderBy(c => c.CourseId) :
                                query.OrderByDescending(c => c.CourseId);
                            break;
                        case "name":
                            query = sortColumnDirection.ToLower() == "asc" ?
                                query.OrderBy(c => c.CourseName) :
                                query.OrderByDescending(c => c.CourseName);
                            break;
                        case "duration":
                            query = sortColumnDirection.ToLower() == "asc" ?
                                query.OrderBy(c => c.Duration) :
                                query.OrderByDescending(c => c.Duration);
                            break;
                        case "prerequisite":
                            query = sortColumnDirection.ToLower() == "asc" ?
                                query.OrderBy(c => c.PreRequisite) :
                                query.OrderByDescending(c => c.PreRequisite);
                            break;
                        case "remarks":
                            query = sortColumnDirection.ToLower() == "asc" ?
                                query.OrderBy(c => c.Remarks) :
                                query.OrderByDescending(c => c.Remarks);
                            break;
                        default:
                            // Handle default case or throw an exception for unknown column
                            break;
                    }
                }

                // Get total count before pagination
                var recordsTotal = await query.CountAsync();

                // Pagination
                var data = await query.Skip(start).Take(length).ToListAsync();

                // Return JSON response for DataTables
                return Ok(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = data
                });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return BadRequest(new { message = "Error retrieving data from database: " + ex.Message });
            }
        }

        // POST: api/CourseApi/AddCourse
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            if (ModelState.IsValid)
            {
                var result = await _courseServices.AddCourseAsync(course);
                if (result.Succeeded)
                {
                    return Ok(new { success = true, message = result.Messages });
                }
                return BadRequest(result.Messages);
            }
            return BadRequest(ModelState);
        }

        // GET: api/CourseApi/GetById/{id}
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseServices.GetCoursesByIdAsync(id);
            if (course != null)
            {
                return Ok(course);
            }
            return NotFound();
        }

        // POST: api/CourseApi/UpdateCourse
        [HttpPut]
        public async Task<IActionResult> UpdateCourse([FromBody] Course course)
        {
            if (ModelState.IsValid)
            {
                var result = await _courseServices.UpdateCourseAsync(course);
                if (result.Succeeded)
                {
                    return Ok(new { success = true, message = result.Messages });
                }
                return BadRequest(result.Messages);
            }
            return BadRequest(ModelState);
        }

        // POST: api/CourseApi/DeleteCourse/{id}
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseServices.DeleteCourseAsync(id);
            if (result.Succeeded)
            {
                return Ok(new { success = true, message = result.Messages });
            }
            return BadRequest(result.Messages);
        }

        // GET: api/CourseApi/SearchByName
        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var result = await _courseServices.SearchCourseByNameAsync(name);
            if (result.Succeeded)
            {
                return Ok(new { data = result.Data });
            }
            return BadRequest(result.Messages);
        }
    }
}



//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Data.Models;
//using System;
//using System.Threading.Tasks;

//namespace StudentSync.Api.Controllers
//{
//    [Route("api/Course")]
//    [ApiController]
//    public class CourseApiController : ControllerBase
//    {
//        private readonly ICourseServices _courseServices;

//        public CourseApiController(ICourseServices courseServices)
//        {
//            _courseServices = courseServices;
//        }

//        [HttpGet("GetAll")]
//        public async Task<IActionResult> GetAll()
//        {
//            try
//            {
//                var courses = await _courseServices.GetAllCourseAsync();
//                return Ok(courses);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        [HttpGet("GetById/{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            try
//            {
//                var course = await _courseServices.GetCoursesByIdAsync(id);
//                if (course == null)
//                {
//                    return NotFound();
//                }
//                return Ok(course);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        [HttpPost("AddCourse")]
//        public async Task<IActionResult> AddCourse([FromBody] Course course)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    var result = await _courseServices.AddCourseAsync(course);
//                    if (result.Succeeded)
//                    {
//                        return Ok(new { success = true, message = result.Messages });
//                    }
//                    return BadRequest(result.Messages);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Exception occurred: {ex.Message}");
//                    return StatusCode(500, $"Internal server error: {ex.Message}");
//                }
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpPut("UpdateCourse")]
//        public async Task<IActionResult> UpdateCourse([FromBody] Course course)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    var result = await _courseServices.UpdateCourseAsync(course);
//                    if (result.Succeeded)
//                    {
//                        return Ok(new { success = true, message = result.Messages });
//                    }
//                    return BadRequest(result.Messages);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Exception occurred: {ex.Message}");
//                    return StatusCode(500, $"Internal server error: {ex.Message}");
//                }
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpDelete("DeleteCourse/{id}")]
//        public async Task<IActionResult> DeleteCourse(int id)
//        {
//            try
//            {
//                var result = await _courseServices.DeleteCourseAsync(id);
//                if (result.Succeeded)
//                {
//                    return Ok(new { success = true, message = result.Messages });
//                }
//                return BadRequest(result.Messages);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        [HttpGet("SearchByName")]
//        public async Task<IActionResult> SearchByName(string name)
//        {
//            try
//            {
//                var result = await _courseServices.SearchCourseByNameAsync(name);
//                if (result.Succeeded)
//                {
//                    return Ok(new { data = result.Data });
//                }
//                return BadRequest(result.Messages);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }
//    }
//}
