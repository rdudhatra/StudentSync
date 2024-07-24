using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Core.Services.Interfaces;
using StudentSync.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.ApiControllers
{
    [Route("api/Inquiry")]
    [ApiController]
    [Authorize]
    public class InquiryApiController : ControllerBase
    {
        private readonly IInquiryService _inquiryService;

        public InquiryApiController(IInquiryService inquiryService)
        {
            _inquiryService = inquiryService;
        }

        [HttpGet("GetAllInquiryNumbers")]
        public async Task<IActionResult> GetAllInquiryNumbers()
        {
            try 
            {
                var inquiryNumbers =  _inquiryService.GetAllInquiryno();
                return Ok(inquiryNumbers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to retrieve inquiry numbers", error = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var inquiries = await _inquiryService.GetAllInquiriesAsync();
                return Ok(inquiries);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                await _inquiryService.UpdateInquiryAsync(inquiry);
                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("Delete/{id}")]
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
