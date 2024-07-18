

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
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

        public InquiryController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                var response = await _httpClient.GetAsync("Inquiry/GetAllInquiryNumbers");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var inquiryNumbers = JsonConvert.DeserializeObject<List<Inquiry>>(await response.Content.ReadAsStringAsync());
                return Ok(inquiryNumbers);
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
                var response = await _httpClient.GetAsync("Inquiry/GetAll");
                response.EnsureSuccessStatusCode();

                var inquiries = JsonConvert.DeserializeObject<List<InquiryResponseModel>>(await response.Content.ReadAsStringAsync());

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
