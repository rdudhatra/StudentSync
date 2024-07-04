using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Data;
using StudentSync.Data.Models;


namespace StudentSync.Controllers
{
    [Route("Course")]

    public class CourseController : Controller
    {
        private readonly StudentSyncDbContext _context;
        private readonly ICourseServices _courseServices;

    

        public CourseController(StudentSyncDbContext context, ICourseServices courseServices)
        {
            _context = context;
            _courseServices = courseServices;

        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("GetAll")]
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
                return Json(new
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

        //public async Task<IActionResult> GetAll(int draw, int start, int length, string searchValue, string sortColumn, string sortColumnDirection)
        //{
        //    try
        //    {
        //        var pageSize = length;
        //        var skip = start;

        //        var result = await _courseServices.GetAllCourseAsync();
        //        if (!result.Succeeded)
        //        {
        //            return BadRequest(new { succeeded = false, messages = result.Messages });
        //        }

        //        var coursesData = result.Data.AsQueryable(); // Assuming result.Data is IEnumerable<Course>

        //        // Apply search filter
        //        if (!string.IsNullOrEmpty(searchValue))
        //        {
        //            coursesData = coursesData.Where(e => e.CourseName.Contains(searchValue)
        //                                            || e.Duration.Contains(searchValue));
        //        }

        //        // Apply sorting
        //        if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
        //        {
        //            switch (sortColumn.ToLower())
        //            {
        //                case "CourseName":
        //                    coursesData = sortColumnDirection == "asc" ?
        //                                   coursesData.OrderBy(e => e.CourseName) :
        //                                   coursesData.OrderByDescending(e => e.CourseName);
        //                    break;
        //                case "Duration":
        //                    coursesData = sortColumnDirection == "asc" ?
        //                                   coursesData.OrderBy(e => e.Duration) :
        //                                   coursesData.OrderByDescending(e => e.Duration);
        //                    break;

        //                default:
        //                    break;
        //            }
        //        }

        //        // Get total records count
        //        var recordsTotal = coursesData.Count();

        //        // Apply pagination
        //        var data = coursesData.Skip(skip).Take(pageSize).ToList();

        //        // Return JSON response
        //        var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
        //        return Ok(jsonData);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Error.WriteLine($"Error in GetAll action: {ex.Message}");
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { succeeded = false, messages = new[] { "An unexpected error occurred." } });
        //    }
        //}


        [HttpPost("AddCourse")]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            if (ModelState.IsValid)
            {
                var result = await _courseServices.AddCourseAsync(course);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = result.Messages });
                }
                return BadRequest(result.Messages);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] Course course)
        { 
            if (ModelState.IsValid)
            {
                var result = await _courseServices.UpdateCourseAsync(course);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = result.Messages });
                }
                return BadRequest(result.Messages);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseServices.DeleteCourseAsync(id);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = result.Messages });
            }
            return BadRequest(result.Messages);
        }
        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var result = await _courseServices.SearchCourseByNameAsync(name);
            if (result.Succeeded)
            {
                return Json(new { data = result.Data });
            }
            return BadRequest(result.Messages);
        }




    }
}
