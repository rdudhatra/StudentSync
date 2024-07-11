using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("InquiryFollowUp")]
    public class InquiryFollowUpController : Controller
    {
        private readonly IInquiryFollowUpService _inquiryFollowUpService;

        public InquiryFollowUpController(IInquiryFollowUpService inquiryFollowUpService)
        {
            _inquiryFollowUpService = inquiryFollowUpService;
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
                var inquiryFollowUps = await _inquiryFollowUpService.GetAllInquiryFollowUpsAsync();

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
                await _inquiryFollowUpService.AddInquiryFollowUpAsync(inquiryFollowUp);
                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var inquiryFollowUp = await _inquiryFollowUpService.GetInquiryFollowUpByIdAsync(id);
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
                await _inquiryFollowUpService.UpdateInquiryFollowUpAsync(inquiryFollowUp);
                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var inquiryFollowUp = await _inquiryFollowUpService.GetInquiryFollowUpByIdAsync(id);
            if (inquiryFollowUp == null)
            {
                return NotFound();
            }
            await _inquiryFollowUpService.DeleteInquiryFollowUpAsync(id);
            return Ok(new { success = true });
        }
    }
}
