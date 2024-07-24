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
    [Route("api/InquiryFollowUp")]
    [ApiController]
    [Authorize]
    public class InquiryFollowUpApiController : ControllerBase
    {
        private readonly IInquiryFollowUpService _inquiryFollowUpService;

        public InquiryFollowUpApiController(IInquiryFollowUpService inquiryFollowUpService)
        {
            _inquiryFollowUpService = inquiryFollowUpService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var inquiryFollowUps = await _inquiryFollowUpService.GetAllInquiryFollowUpsAsync();
                return Ok(inquiryFollowUps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] InquiryFollowUp inquiryFollowUp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _inquiryFollowUpService.AddInquiryFollowUpAsync(inquiryFollowUp); 
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var inquiryFollowUp = await _inquiryFollowUpService.GetInquiryFollowUpByIdAsync(id);
                if (inquiryFollowUp == null)
                {
                    return NotFound();
                }
                return Ok(inquiryFollowUp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] InquiryFollowUp inquiryFollowUp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _inquiryFollowUpService.UpdateInquiryFollowUpAsync(inquiryFollowUp);
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var inquiryFollowUp = await _inquiryFollowUpService.GetInquiryFollowUpByIdAsync(id);
                if (inquiryFollowUp == null)
                {
                    return NotFound();
                }
                await _inquiryFollowUpService.DeleteInquiryFollowUpAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
