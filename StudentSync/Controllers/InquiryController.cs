//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Core.Services.Interfaces;
//using StudentSync.Data.Models;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace StudentSync.Controllers
//{
//    [Route("Inquiry")]
//    public class InquiryController : Controller
//    {
//        private readonly IInquiryService _inquiryService;

//        public InquiryController(IInquiryService inquiryService)
//        {
//            _inquiryService = inquiryService;
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
//                var inquiries = await _inquiryService.GetAllInquiriesAsync();

//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    inquiries = inquiries
//                        .Where(i => i.FirstName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
//                                    i.LastName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
//                                    i.ContactNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
//                        .ToList();
//                }

//                var dataTableResponse = new
//                {
//                    draw = Request.Query["draw"].FirstOrDefault(),
//                    recordsTotal = inquiries.Count(),
//                    recordsFiltered = inquiries.Count(),
//                    data = inquiries
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
//        public async Task<IActionResult> Create([FromBody] Inquiry inquiry)
//        {
//            if (ModelState.IsValid)
//            {
//                if (inquiry.InquiryNo > 0)
//                    await _inquiryService.UpdateInquiryAsync(inquiry);
//                else
//                    await _inquiryService.AddInquiryAsync(inquiry);

//                return Ok(new { success = true });
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpGet("Edit/{id}")]
//        public async Task<IActionResult> Edit(int id)
//        {
//            var inquiry = await _inquiryService.GetInquiryByIdAsync(id);
//            if (inquiry == null)
//            {
//                return NotFound();
//            }
//            return Ok(inquiry);
//        }

//        [HttpPost("Update")]
//        public async Task<IActionResult> Update([FromBody] Inquiry inquiry)
//        {
//            if (ModelState.IsValid)
//            {
//                await _inquiryService.UpdateInquiryAsync(inquiry);
//                return Ok(new { success = true });
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpPost("Delete/{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var inquiry = await _inquiryService.GetInquiryByIdAsync(id);
//            if (inquiry == null)
//            {
//                return NotFound();
//            }
//            await _inquiryService.DeleteInquiryAsync(id);
//            return Ok(new { success = true });
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Core.Services;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("Inquiry")]
    public class InquiryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IInquiryService _inquiryService;

        public InquiryController(HttpClient httpClient, IInquiryService inquiryService)
        {
            _inquiryService = inquiryService;
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri("https://localhost:7024/api/"); // Adjust the base address as needed
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("getAllInquiryno")]
        public IActionResult GetAllCourseIds()
        {
            try
            {
                var batchesIds = _inquiryService.GetAllInquiryno(); // Implement this method in your service
                return Json(batchesIds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to retrieve Batch IDs", error = ex.Message });
            }
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                var response = await _httpClient.GetAsync("Inquiry/GetAll");
                response.EnsureSuccessStatusCode();

                var inquiries = JsonConvert.DeserializeObject<List<Inquiry>>(await response.Content.ReadAsStringAsync());

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
                var content = new StringContent(JsonConvert.SerializeObject(inquiry), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Inquiry", content);
                response.EnsureSuccessStatusCode();

                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"Inquiry/Edit/{id}");
            if (response.IsSuccessStatusCode)
            {
                var inquiry = JsonConvert.DeserializeObject<Inquiry>(await response.Content.ReadAsStringAsync());
                return Ok(inquiry);
            }
            return NotFound();
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(inquiry), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("Inquiry", content);
                response.EnsureSuccessStatusCode();

                return Ok(new { success = true });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"Inquiry/Delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true });
            }
            return NotFound();
        }
    }
}
