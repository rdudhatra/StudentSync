
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
using StudentSync.Service.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Web.Controllers
{
    [Route("Enrollment")]
    public class EnrollmentController : Controller
    {
        private readonly IHttpService _httpService;

        public EnrollmentController(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public IActionResult Index()
        {
            return View();
        }
         
        [HttpGet("getAllEnrollMentno")]
        public async Task<IActionResult> GetAllEnrollmentNumbers()
        {
            try
            {
                var response = await _httpService.Get<List<Enrollment>>("Enrollment/GetAllEnrollmentNumbers");
                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


  

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _httpService.Get<List<EnrollmentResponseModel>>("Enrollment/GetAll");
                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = response.Data.Count(),
                    recordsFiltered = response.Data.Count(),
                    data = response.Data
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
        public async Task<IActionResult> Create([FromBody] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Post("Enrollment/Create", enrollment);
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
            var response = await _httpService.Get<Enrollment>($"Enrollment/Edit/{id}");
            if (!response.Succeeded)
            {
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }

            var enrollment = response.Data;
            if (enrollment == null)
            {
                return NotFound();
            }
            return Ok(enrollment);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Put("Enrollment/Update", enrollment);
                if (response.Succeeded)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpService.Delete($"Enrollment/Delete/{id}");
            if (response.Succeeded)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
        }
    }
}
