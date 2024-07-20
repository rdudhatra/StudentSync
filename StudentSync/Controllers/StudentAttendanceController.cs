﻿
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using StudentSync.Data.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//namespace StudentSync.Web.Controllers
//{
//    [Route("StudentAttendance")]
//    public class StudentAttendanceController : Controller
//    {
//        private readonly HttpClient _httpClient;
//        private readonly ILogger<StudentAttendanceController> _logger;

//        public StudentAttendanceController(HttpClient httpClient, ILogger<StudentAttendanceController> logger)
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
//                var response = await _httpClient.GetAsync("StudentAttendance/GetAll");
//                if (!response.IsSuccessStatusCode)
//                {
//                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
//                }

//                var studentAttendances = JsonConvert.DeserializeObject<List<StudentAttendance>>(await response.Content.ReadAsStringAsync());

//                var searchValue = Request.Query["search[value]"].FirstOrDefault();
//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    studentAttendances = studentAttendances
//                        .Where(sa => sa.AttendanceDate.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
//                                     sa.EnrollmentNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
//                                     sa.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
//                        .ToList();
//                }

//                var dataTableResponse = new
//                {
//                    draw = Request.Query["draw"].FirstOrDefault(),
//                    recordsTotal = studentAttendances.Count(),
//                    recordsFiltered = studentAttendances.Count(),
//                    data = studentAttendances
//                };

//                return Ok(dataTableResponse);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Exception occurred while fetching all student attendances.");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        [HttpPost("Create")]
//        public async Task<IActionResult> Create([FromBody] StudentAttendance studentAttendance)
//        {
//            if (ModelState.IsValid)
//            {
//                var content = new StringContent(JsonConvert.SerializeObject(studentAttendance), Encoding.UTF8, "application/json");
//                var response = await _httpClient.PostAsync("StudentAttendance/Create", content);
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
//            var response = await _httpClient.GetAsync($"StudentAttendance/Edit/{id}");
//            if (!response.IsSuccessStatusCode)
//            {
//                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
//            }

//            var studentAttendance = JsonConvert.DeserializeObject<StudentAttendance>(await response.Content.ReadAsStringAsync());
//            if (studentAttendance == null)
//            {
//                return NotFound();
//            }
//            return Ok(studentAttendance);
//        }

//        [HttpPost("Update")]
//        public async Task<IActionResult> Update([FromBody] StudentAttendance studentAttendance)
//        {
//            if (ModelState.IsValid)
//            {
//                var content = new StringContent(JsonConvert.SerializeObject(studentAttendance), Encoding.UTF8, "application/json");
//                var response = await _httpClient.PutAsync("StudentAttendance/Update", content);
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
//            var response = await _httpClient.DeleteAsync($"StudentAttendance/Delete/{id}");
//            if (response.IsSuccessStatusCode)
//            {
//                return Ok(new { success = true });
//            }
//            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using StudentSync.Data.Models;
using StudentSync.Service.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Web.Controllers
{
    [Route("StudentAttendance")]
    public class StudentAttendanceController : Controller
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<StudentAttendanceController> _logger;

        public StudentAttendanceController(IHttpService httpService, ILogger<StudentAttendanceController> logger)
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
                var response = await _httpService.Get<List<StudentAttendance>>("StudentAttendance/GetAll");
                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                var studentAttendances = response.Data;

                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    studentAttendances = studentAttendances
                        .Where(sa => sa.AttendanceDate.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     sa.EnrollmentNo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                     sa.Remarks.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = studentAttendances.Count(),
                    recordsFiltered = studentAttendances.Count(),
                    data = studentAttendances
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching all student attendances.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] StudentAttendance studentAttendance)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Post("StudentAttendance/Create", studentAttendance);
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
            var response = await _httpService.Get<StudentAttendance>($"StudentAttendance/Edit/{id}");
            if (!response.Succeeded)
            {
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }

            var studentAttendance = response.Data;
            if (studentAttendance == null)
            {
                return NotFound();
            }
            return Ok(studentAttendance);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] StudentAttendance studentAttendance)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Put("StudentAttendance/Update", studentAttendance);
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
            var response = await _httpService.Delete($"StudentAttendance/Delete/{id}");
            if (response.Succeeded)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
        }
    }
}
