using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseExamApiController : ControllerBase
    {
        private readonly ICourseExamServices _courseExamServices;

        public CourseExamApiController(ICourseExamServices courseExamServices)
        {
            _courseExamServices = courseExamServices;
        }

        // GET: api/CourseExamApi
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _courseExamServices.GetAllCourseExamsAsync();
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Messages);
        }

        // GET: api/CourseExamApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _courseExamServices.GetCourseExamByIdAsync(id);
            if (result==null)
            {
                return NotFound();
            }
            return Ok(result);

        }

        // POST: api/CourseExamApi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseExam courseExam)
        {
            if (ModelState.IsValid)
            {
                var result = await _courseExamServices.AddCourseExamAsync(courseExam);
                if (result.Succeeded)
                {
                    return Ok(new { success = true, message = result.Messages });
                }
                return BadRequest(result.Messages);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/CourseExamApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CourseExam courseExam)
        {
            if (id != courseExam.Id)
            {
                return BadRequest("ID mismatch between route parameter and body data.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                 await _courseExamServices.UpdateCourseExamAsync(courseExam);
           
                return Ok(new { success = true, message = "Course Exam updated successfully." });
            
        }

        // DELETE: api/CourseExamApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courseExamServices.DeleteCourseExamAsync(id);
            if (result.Succeeded)
            {
                return Ok(new { success = true, message = result.Messages });
            }
            return BadRequest(result.Messages);
        }

        // Custom endpoint example: Search by Exam Title
        // GET: api/CourseExamApi/SearchByExamTitle?examTitle=sample
        [HttpGet("SearchByExamTitle")]
        public async Task<IActionResult> SearchByExamTitle(string examTitle)
        {
            var result = await _courseExamServices.SearchCourseExamByExamTitleAsync(examTitle);
            if (result.Succeeded)
            {
                return Ok(new { data = result.Data });
            }
            return BadRequest(result.Messages);
        }
    }
}
