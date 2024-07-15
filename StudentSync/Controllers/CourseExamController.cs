using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("CourseExam")]
    public class CourseExamController : Controller
    {
        private readonly StudentSyncDbContext _context;
        private readonly ICourseExamServices _courseExamServices;

        public CourseExamController(StudentSyncDbContext context, ICourseExamServices courseExamServices)
        {
            _context = context;
            _courseExamServices = courseExamServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAllExamCourseIds")]
        public IActionResult GetAllCourseIds()
        {
            try
            {
                var courseIds = _courseExamServices.GetAllCourseExamIds(); // Implement this method in your service
                return Json(courseIds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to retrieve Course IDs", error = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                var courseExams = await _courseExamServices.GetAllCourseExamsAsync();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    courseExams = courseExams
                        .Where(ce =>
                            ce.ExamTitle.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                            ce.ExamType.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                            ce.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(), // You may adjust this if `draw` is not present in your request
                    recordsTotal = courseExams.Count(),
                    recordsFiltered = courseExams.Count(),
                    data = courseExams
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        //[HttpPost("GetAll")]
        //public async Task<IActionResult> GetAll(int draw, int start, int length, string searchValue, string sortColumn, string sortColumnDirection)
        //{
        //    try
        //    {
        //        var query = _context.CourseExams.AsQueryable();
        //        var courseExam = await _courseExamServices.GetAllCourseExamsAsync();


        //        // Apply search filter if searchValue is provided
        //        if (!string.IsNullOrEmpty(searchValue))
        //        {
        //            query = query.Where(ce =>
        //                ce.ExamTitle.Contains(searchValue) ||
        //                ce.ExamType.Contains(searchValue) ||
        //                ce.Remarks.Contains(searchValue));
        //        }

        //        // Sort data
        //        if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
        //        {
        //            switch (sortColumn.ToLower())
        //            {
        //                case "examtitle":
        //                    query = sortColumnDirection.ToLower() == "asc" ?
        //                        query.OrderBy(ce => ce.ExamTitle) :
        //                        query.OrderByDescending(ce => ce.ExamTitle);
        //                    break;
        //                case "examtype":
        //                    query = sortColumnDirection.ToLower() == "asc" ?
        //                        query.OrderBy(ce => ce.ExamType) :
        //                        query.OrderByDescending(ce => ce.ExamType);
        //                    break;
        //                case "remarks":
        //                    query = sortColumnDirection.ToLower() == "asc" ?
        //                        query.OrderBy(ce => ce.Remarks) :
        //                        query.OrderByDescending(ce => ce.Remarks);
        //                    break;
        //                default:
        //                    // Handle default case or throw an exception for unknown column
        //                    break;
        //            }
        //        }

        //        // Get total count before pagination
        //        var recordsTotal = await query.CountAsync();

        //        // Pagination
        //        var data = await query.Skip(start).Take(length).ToListAsync();

        //        // Return JSON response for DataTables
        //        return Json(new
        //        {
        //            draw = draw,
        //            recordsFiltered = recordsTotal,
        //            recordsTotal = recordsTotal,
        //            data = data
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it appropriately
        //        return BadRequest(new { message = "Error retrieving data from database: " + ex.Message });
        //    }
        //}

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CourseExam courseExam)
        {
            if (ModelState.IsValid)
            {
                var result = await _courseExamServices.AddCourseExamAsync(courseExam);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = result.Messages });
                }
                return BadRequest(result.Messages);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var courseExam = await _courseExamServices.GetCourseExamByIdAsync(id);
            if (courseExam == null)
            {
                return NotFound();
            }
            return Ok(courseExam);


        }

        [HttpPost("UpdateCourseExam")]
        public async Task<IActionResult> UpdateCourseExam([FromBody] CourseExam courseExam)
            {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                
            }
            await _courseExamServices.UpdateCourseExamAsync(courseExam);
          
            return Ok(new { message = "Course Exam updated successfully." });

        }

      


        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _courseExamServices.DeleteCourseExamAsync(id);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = result.Messages });
            }
            return BadRequest(result.Messages);
        }

        [HttpGet("SearchByExamTitle")]
        public async Task<IActionResult> SearchByExamTitle(string examTitle)
        {
            var result = await _courseExamServices.SearchCourseExamByExamTitleAsync(examTitle);
            if (result.Succeeded)
            {
                return Json(new { data = result.Data });
            }
            return BadRequest(result.Messages);
        }
    }
}
