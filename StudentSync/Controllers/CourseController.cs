using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services;
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
        //[HttpGet]
        //public IActionResult GetCourseName(int courseId)
        //{
        //    var courseName = _courseServices.GetCourseNameById(courseId);

        //    return Json(new { courseName = courseName });
        //}


        [HttpGet("GetAllCourseIds")]
        public IActionResult GetAllCourseIds()
        {
            try
            {
                var courseIds = _courseServices.GetAllCourseIds(); // Implement this method in your service
                return Json(courseIds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to retrieve Course IDs", error = ex.Message });
            }
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

            // Log the validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { message = "Validation failed", errors });
        }

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


// CourseController.cs

//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using StudentSync.Data.Models;
//using System;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//namespace StudentSync.Controllers
//{
//    [Route("Course")]
//    public class CourseController : Controller
//    {
//        private readonly HttpClient _httpClient;

//        public CourseController(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//            _httpClient.BaseAddress = new Uri("https://localhost:7024"); // Replace with your API base address
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpGet("GetAll")]
//        public async Task<IActionResult> GetAll(int draw, int start, int length, string searchValue, string sortColumn, string sortColumnDirection)
//        {
//            var response = await _httpClient.GetAsync($"/api/CourseApi/GetAll?draw={draw}&start={start}&length={length}&searchValue={searchValue}&sortColumn={sortColumn}&sortColumnDirection={sortColumnDirection}");
//            if (response.IsSuccessStatusCode)
//            {
//                var data = await response.Content.ReadAsStringAsync();
//                return Content(data, "application/json");
//            }
//            return BadRequest("Error retrieving data");
//        }

//        [HttpPost("AddCourse")]
//        public async Task<IActionResult> AddCourse([FromBody] Course course)
//        {
//            var content = new StringContent(JsonConvert.SerializeObject(course), Encoding.UTF8, "application/json");
//            var response = await _httpClient.PostAsync("/api/CourseApi/AddCourse", content);
//            if (response.IsSuccessStatusCode)
//            {
//                var data = await response.Content.ReadAsStringAsync();
//                return Content(data, "application/json");
//            }
//            return BadRequest("Error adding course");
//        }

//        [HttpGet("GetById/{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var response = await _httpClient.GetAsync($"/api/CourseApi/GetById/{id}");
//            if (response.IsSuccessStatusCode)
//            {
//                var data = await response.Content.ReadAsStringAsync();
//                return Content(data, "application/json");
//            }
//            return NotFound();
//        }

//        [HttpPost("UpdateCourse")]
//        public async Task<IActionResult> UpdateCourse([FromBody] Course course)
//        {
//            var content = new StringContent(JsonConvert.SerializeObject(course), Encoding.UTF8, "application/json");
//            var response = await _httpClient.PostAsync("/api/CourseApi/UpdateCourse", content);
//            if (response.IsSuccessStatusCode)
//            {
//                var data = await response.Content.ReadAsStringAsync();
//                return Content(data, "application/json");
//            }
//            return BadRequest("Error updating course");
//        }

//        [HttpPost("DeleteCourse/{id}")]
//        public async Task<IActionResult> DeleteCourse(int id)
//        {
//            var response = await _httpClient.PostAsync($"/api/CourseApi/DeleteCourse/{id}", null);
//            if (response.IsSuccessStatusCode)
//            {
//                var data = await response.Content.ReadAsStringAsync();
//                return Content(data, "application/json");
//            }
//            return BadRequest("Error deleting course");
//        }

//        [HttpGet("SearchByName")]
//        public async Task<IActionResult> SearchByName(string name)
//        {
//            var response = await _httpClient.GetAsync($"/api/CourseApi/SearchByName?name={name}");
//            if (response.IsSuccessStatusCode)
//            {
//                var data = await response.Content.ReadAsStringAsync();
//                return Content(data, "application/json");
//            }
//            return BadRequest("Error searching course");
//        }
//    }
//}

