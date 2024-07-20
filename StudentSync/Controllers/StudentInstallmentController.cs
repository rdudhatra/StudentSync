//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using StudentSync.Data.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//namespace StudentSync.Controllers
//{
//    [Route("StudentInstallment")]
//    public class StudentInstallmentController : Controller
//    {
//        private readonly HttpClient _httpClient;
//        private readonly ILogger<StudentInstallmentController> _logger;

//        public StudentInstallmentController(HttpClient httpClient, ILogger<StudentInstallmentController> logger)
//        {
//            _httpClient = httpClient;
//            _logger = logger;
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
//                var response = await _httpClient.GetAsync("StudentInstallment/GetAll");
//                if (!response.IsSuccessStatusCode)
//                {
//                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
//                }

//                var studentInstallments = JsonConvert.DeserializeObject<List<StudentInstallment>>(await response.Content.ReadAsStringAsync());

//                var searchValue = Request.Query["search[value]"].FirstOrDefault();
//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    studentInstallments = studentInstallments
//                        .Where(si => si.ReceiptNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
//                                     si.EnrollmentNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
//                                     si.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
//                        .ToList();
//                }

//                var dataTableResponse = new
//                {
//                    draw = Request.Query["draw"].FirstOrDefault(),
//                    recordsTotal = studentInstallments.Count(),
//                    recordsFiltered = studentInstallments.Count(),
//                    data = studentInstallments
//                };

//                return Ok(dataTableResponse);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Exception occurred while fetching all student installments.");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpPost("Create")]
//        public async Task<IActionResult> Create([FromBody] StudentInstallment studentInstallment)
//        {
//            if (ModelState.IsValid)
//            {
//                var content = new StringContent(JsonConvert.SerializeObject(studentInstallment), Encoding.UTF8, "application/json");
//                var response = await _httpClient.PostAsync("StudentInstallment/Create", content);
//                if (response.IsSuccessStatusCode)
//                {
//                    return Ok(new { success = true });
//                }
//                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpGet("Edit/{id}")]
//        public async Task<IActionResult> Edit(int id)
//        {
//            var response = await _httpClient.GetAsync($"StudentInstallment/Edit/{id}");
//            if (!response.IsSuccessStatusCode)
//            {
//                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
//            }

//            var studentInstallment = JsonConvert.DeserializeObject<StudentInstallment>(await response.Content.ReadAsStringAsync());
//            if (studentInstallment == null)
//            {
//                return NotFound();
//            }
//            return Ok(studentInstallment);
//        }

//        [HttpPost("Update")]
//        public async Task<IActionResult> Update([FromBody] StudentInstallment studentInstallment)
//        {
//            if (ModelState.IsValid)
//            {
//                var content = new StringContent(JsonConvert.SerializeObject(studentInstallment), Encoding.UTF8, "application/json");
//                var response = await _httpClient.PutAsync("StudentInstallment/Update", content);
//                if (response.IsSuccessStatusCode)
//                {
//                    return Ok(new { success = true });
//                }
//                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpPost("Delete/{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var response = await _httpClient.DeleteAsync($"StudentInstallment/Delete/{id}");
//            if (response.IsSuccessStatusCode)
//            {
//                return Ok(new { success = true });
//            }
//            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Data.Models;
using StudentSync.Service.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("StudentInstallment")]
    public class StudentInstallmentController : Controller
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<StudentInstallmentController> _logger;

        public StudentInstallmentController(IHttpService httpService, ILogger<StudentInstallmentController> logger)
        {
            _httpService = httpService;
            _logger = logger;
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
                var response = await _httpService.Get<List<StudentInstallment>>("StudentInstallment/GetAll");
                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                var studentInstallments = response.Data;

                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    studentInstallments = studentInstallments
                        .Where(si => si.ReceiptNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     si.EnrollmentNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     si.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = studentInstallments.Count(),
                    recordsFiltered = studentInstallments.Count(),
                    data = studentInstallments
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching all student installments.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] StudentInstallment studentInstallment)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Post("StudentInstallment/Create", studentInstallment);
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
            var response = await _httpService.Get<StudentInstallment>($"StudentInstallment/Edit/{id}");
            if (!response.Succeeded)
            {
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }

            var studentInstallment = response.Data;
            if (studentInstallment == null)
            {
                return NotFound();
            }
            return Ok(studentInstallment);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] StudentInstallment studentInstallment)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Put("StudentInstallment/Update", studentInstallment);
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
            var response = await _httpService.Delete($"StudentInstallment/Delete/{id}");
            if (response.Succeeded)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
        }
    }
}

