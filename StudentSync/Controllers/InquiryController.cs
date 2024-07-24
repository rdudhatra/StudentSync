

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
using StudentSync.Service.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("Inquiry")]
    public class InquiryController : Controller
    {
        private readonly IHttpService _httpService;

        public InquiryController(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("getAllInquiryno")]
        public async Task<IActionResult> GetAllInquiryNumbers()
        {
            try 
            {
                var response = await _httpService.Get<List<Inquiry>>("Inquiry/GetAllInquiryNumbers");

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
                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                var response = await _httpService.Get<List<InquiryResponseModel>>("Inquiry/GetAll");

                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                var inquiries = response.Data;

                if (!string.IsNullOrEmpty(searchValue))
                {
                    inquiries = inquiries
                        .Where(i => i.FirstName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                    i.LastName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                    i.ContactNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = inquiries.Count(),
                    recordsFiltered = inquiries.Count(),
                    data = inquiries
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
        public async Task<IActionResult> Create([FromBody] Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Post("Inquiry", inquiry);

                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpService.Get<Inquiry>($"Inquiry/Edit/{id}");

            if (!response.Succeeded)
            {
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }

            return Ok(response.Data);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Put("Inquiry", inquiry);

                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpService.Delete($"Inquiry/Delete/{id}");

            if (!response.Succeeded)
            {
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }

            return Ok(new { success = true });
        }
    }
}
