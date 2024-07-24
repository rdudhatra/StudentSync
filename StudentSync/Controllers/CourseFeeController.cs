


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
    [Route("CourseFee")]
    public class CourseFeeController : Controller
    {
        private readonly IHttpService _httpService;

        public CourseFeeController(IHttpService httpService)
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
                var searchValue = Request.Query["search[value]"].FirstOrDefault();

                // Call the HTTP service
                var response = await _httpService.Get<List<CourseFeeResponseModel>>("CourseFee/GetAll");

                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                var courseFees = response.Data;

                if (!string.IsNullOrEmpty(searchValue))
                {
                    courseFees = courseFees
                        .Where(ce =>
                            ce.CourseName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                            ce.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Paginate the result 
                int recordsTotal = courseFees.Count;
                courseFees = courseFees.Skip(start).Take(length).ToList();

                var dataTableResponse = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal, // Assuming no filtering at server-side
                    data = courseFees
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("AddCourseFee")]
        public async Task<IActionResult> Create([FromBody] CourseFee courseFee)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Post("CourseFee/Create", courseFee);

                if (response.Succeeded)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpService.Get<CourseFee>($"CourseFee/GetById/{id}");

            if (!response.Succeeded)
            {
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }

            var courseFee = response.Data;
            if (courseFee == null)
            {
                return NotFound();
            }
            return Ok(courseFee);
        }

        [HttpPost("UpdateCourseFee")]
        public async Task<IActionResult> Update([FromBody] CourseFee courseFee)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Put("CourseFee/Update", courseFee);

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
            var response = await _httpService.Delete($"CourseFee/Delete/{id}");

            if (response.Succeeded)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
        }
    }
}

