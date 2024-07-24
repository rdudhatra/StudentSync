using Microsoft.AspNetCore.Mvc;
using StudentSync.Data.Models;
using StudentSync.Service.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("InquiryFollowUp")]
    public class InquiryFollowUpController : Controller
    {
        private readonly IHttpService _httpService;

        public InquiryFollowUpController(IHttpService httpService)
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
                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                var response = await _httpService.Get<List<InquiryFollowUp>>("InquiryFollowUp/GetAll");

                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase); 
                }

                var inquiryFollowUps = response.Data;

                if (!string.IsNullOrEmpty(searchValue))
                {
                    inquiryFollowUps = inquiryFollowUps
                        .Where(i => i.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = inquiryFollowUps.Count(),
                    recordsFiltered = inquiryFollowUps.Count(),
                    data = inquiryFollowUps
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
        public async Task<IActionResult> Create([FromBody] InquiryFollowUp inquiryFollowUp)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Post("InquiryFollowUp/Create", inquiryFollowUp);
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
            var response = await _httpService.Get<InquiryFollowUp>($"InquiryFollowUp/Edit/{id}");
            if (!response.Succeeded)
            {
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }

            var inquiryFollowUp = response.Data;
            if (inquiryFollowUp == null)
            {
                return NotFound();
            }
            return Ok(inquiryFollowUp);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] InquiryFollowUp inquiryFollowUp)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Put("InquiryFollowUp/Update", inquiryFollowUp);
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
            var response = await _httpService.Delete($"InquiryFollowUp/Delete/{id}");
            if (response.Succeeded)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
        }
    }
}
