using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Core.Services.Interfaces;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("Inquiry")]
    public class InquiryController : Controller
    {
        private readonly IInquiryService _inquiryService;

        public InquiryController(IInquiryService inquiryService)
        {
            _inquiryService = inquiryService;
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
                var inquiries = await _inquiryService.GetAllInquiriesAsync();

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
                if (inquiry.InquiryNo > 0)
                    await _inquiryService.UpdateInquiryAsync(inquiry);
                else
                    await _inquiryService.AddInquiryAsync(inquiry);

                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var inquiry = await _inquiryService.GetInquiryByIdAsync(id);
            if (inquiry == null)
            {
                return NotFound();
            }
            return Ok(inquiry);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                await _inquiryService.UpdateInquiryAsync(inquiry);
                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var inquiry = await _inquiryService.GetInquiryByIdAsync(id);
            if (inquiry == null)
            {
                return NotFound();
            }
            await _inquiryService.DeleteInquiryAsync(id);
            return Ok(new { success = true });
        }
    }
}
