//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Data.Models;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace StudentSync.Controllers
//{
//    [Route("InquiryFollowUp")]
//    public class InquiryFollowUpController : Controller
//    {
//        private readonly IInquiryFollowUpService _inquiryFollowUpService;

//        public InquiryFollowUpController(IInquiryFollowUpService inquiryFollowUpService)
//        {
//            _inquiryFollowUpService = inquiryFollowUpService;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpGet("GetAll")]
//        public async Task<IActionResult> GetAll()
//        {
//            try
//            {
//                var searchValue = Request.Query["search[value]"].FirstOrDefault();
//                var inquiryFollowUps = await _inquiryFollowUpService.GetAllInquiryFollowUpsAsync();

//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    inquiryFollowUps = inquiryFollowUps
//                        .Where(i => i.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
//                        .ToList();
//                }

//                var dataTableResponse = new
//                {
//                    draw = Request.Query["draw"].FirstOrDefault(),
//                    recordsTotal = inquiryFollowUps.Count(),
//                    recordsFiltered = inquiryFollowUps.Count(),
//                    data = inquiryFollowUps
//                };

//                return Ok(dataTableResponse);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception occurred: {ex.Message}");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpPost("Create")]
//        public async Task<IActionResult> Create([FromBody] InquiryFollowUp inquiryFollowUp)
//        {
//            if (ModelState.IsValid)
//            {
//                await _inquiryFollowUpService.AddInquiryFollowUpAsync(inquiryFollowUp);
//                return Ok(new { success = true });
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpGet("Edit/{id}")]
//        public async Task<IActionResult> Edit(int id)
//        {
//            var inquiryFollowUp = await _inquiryFollowUpService.GetInquiryFollowUpByIdAsync(id);
//            if (inquiryFollowUp == null)
//            {
//                return NotFound();
//            }
//            return Ok(inquiryFollowUp);
//        }

//        [HttpPost("Update")]
//        public async Task<IActionResult> Update([FromBody] InquiryFollowUp inquiryFollowUp)
//        {
//            if (ModelState.IsValid)
//            {
//                await _inquiryFollowUpService.UpdateInquiryFollowUpAsync(inquiryFollowUp);
//                return Ok(new { success = true });
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpPost("Delete/{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var inquiryFollowUp = await _inquiryFollowUpService.GetInquiryFollowUpByIdAsync(id);
//            if (inquiryFollowUp == null)
//            {
//                return NotFound();
//            }
//            await _inquiryFollowUpService.DeleteInquiryFollowUpAsync(id);
//            return Ok(new { success = true });
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("InquiryFollowUp")]
    public class InquiryFollowUpController : Controller
    {
        private readonly HttpClient _httpClient;

        public InquiryFollowUpController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7024/api/"); // Adjust as needed
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
                var response = await _httpClient.GetAsync("InquiryFollowUp/GetAll");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var inquiryFollowUps = JsonConvert.DeserializeObject<List<InquiryFollowUp>>(await response.Content.ReadAsStringAsync());

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
                var content = new StringContent(JsonConvert.SerializeObject(inquiryFollowUp), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("InquiryFollowUp/Create", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"InquiryFollowUp/Edit/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }

            var inquiryFollowUp = JsonConvert.DeserializeObject<InquiryFollowUp>(await response.Content.ReadAsStringAsync());
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
                var content = new StringContent(JsonConvert.SerializeObject(inquiryFollowUp), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("InquiryFollowUp/Update", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.PostAsync($"InquiryFollowUp/Delete/{id}", null);
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}
