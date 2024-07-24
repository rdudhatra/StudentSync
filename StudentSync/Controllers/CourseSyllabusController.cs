


using Microsoft.AspNetCore.Mvc;
using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
using StudentSync.Service.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Web.Controllers
{
    [Route("CourseSyllabus")]
    public class CourseSyllabusController : Controller
    {
        private readonly IHttpService _httpService;

        public CourseSyllabusController(IHttpService httpService)
        {
            _httpService = httpService;
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
                // DataTables parameters
                int draw = int.Parse(Request.Query["draw"]);
                int start = int.Parse(Request.Query["start"]);
                int length = int.Parse(Request.Query["length"]);

                var response = await _httpService.Get<List<CourseSyllabusResponseModel>>("CourseSyllabus/GetAll");

                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                var courseSyllabuses = response.Data;
                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    courseSyllabuses = courseSyllabuses
                        .Where(cs =>
                            cs.CourseName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                            cs.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                // Paginate the results
                int recordsTotal = courseSyllabuses.Count;
                courseSyllabuses = courseSyllabuses.Skip(start).Take(length).ToList();

                var dataTableResponse = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    data = courseSyllabuses
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
        public async Task<IActionResult> Create([FromBody] CourseSyllabus courseSyllabus)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Post("CourseSyllabus/Create", courseSyllabus);
                if (response.Succeeded)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpService.Get<CourseSyllabus>($"CourseSyllabus/Edit/{id}");
            if (!response.Succeeded)
            {
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }

            var courseSyllabus = response.Data;
            if (courseSyllabus == null)
            {
                return NotFound();
            }
            return Ok(courseSyllabus);
        }

        [HttpPost("UpdateCourseSyllabus")]
        public async Task<IActionResult> Update([FromBody] CourseSyllabus courseSyllabus)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Put("CourseSyllabus/Update", courseSyllabus);
                if (response.Succeeded)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpService.Delete($"CourseSyllabus/Delete/{id}");
            if (response.Succeeded)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
        }
    }
}

