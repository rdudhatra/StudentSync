//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Data.Models;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Core.Services;

//namespace StudentSync.Controllers
//{
//    [Route("Batch")]

//    public class BatchController : Controller
//    {
//        private readonly IBatchService _batchService;

//        public BatchController(IBatchService batchService)
//        {
//            _batchService = batchService;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpGet("GetAllBatchesIds")]
//        public IActionResult GetAllCourseIds()
//        {
//            try
//            {
//                var batchesIds = _batchService.GetAllBatchesIds(); // Implement this method in your service
//                return Json(batchesIds);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { message = "Failed to retrieve Batch IDs", error = ex.Message });
//            }
//        }
//        [HttpGet("GetAll")]
//        public async Task<IActionResult> GetAll()
//        {
//            try
//            {
//                var searchValue = Request.Query["search[value]"].FirstOrDefault();
//                var batches = await _batchService.GetAllBatchesAsync();

//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    batches = batches
//                        .Where(b => b.BatchCode.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
//                                    b.BatchTime.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
//                                    b.FacultyName.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
//                        .ToList();
//                }

//                var dataTableResponse = new
//                {
//                    draw = Request.Query["draw"].FirstOrDefault(),
//                    recordsTotal = batches.Count(),
//                    recordsFiltered = batches.Count(),
//                    data = batches
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
//        public async Task<IActionResult> Create([FromBody] Batch batch)
//        {
//            if (ModelState.IsValid)
//            {
//                await _batchService.CreateBatchAsync(batch);
//                return Json(new { success = true, message = "Batch added successfully." });
//            }
//            return Json(new { success = false, message = "Invalid batch data." });
//        }

//        [HttpGet("Edit/{id}")]
//        public async Task<IActionResult> Edit(int id)
//        {
//            var batch = await _batchService.GetBatchByIdAsync(id);
//            if (batch == null)
//            {
//                return NotFound();
//            }
//            return Json(batch);
//        }

//        [HttpPost("UpdateBatch")]
//        public async Task<IActionResult> UpdateBatch([FromBody] Batch batch)
//        {
//            if (ModelState.IsValid)
//            {
//                await _batchService.UpdateBatchAsync(batch);
//                return Json(new { success = true, message = "Batch updated successfully." });
//            }
//            return Json(new { success = false, message = "Invalid batch data." });
//        }

//        [HttpPost("DeleteConfirmed/{id}")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            await _batchService.DeleteBatchAsync(id);
//            return Json(new { success = true, message = "Batch deleted successfully." });
//        }
//    }
//}



using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Core.Services;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Web.Controllers
{
    [Route("Batch")]
    public class BatchController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IBatchService _batchService;

        public BatchController(HttpClient httpClient , IBatchService batchService)
        {
            _batchService = batchService;
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri("https://localhost:7024/api/"); // Adjust the URL as needed
        }

        public IActionResult Index()
        {
            return View();
        }

       
        [HttpGet("GetAllBatchesIds")]
        public async Task<IActionResult> GetAllBatchIds()
        {
            try
            {
                var batchesIds = _batchService.GetAllBatchesIds(); // Implement this method in your service
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
                var response = await _httpClient.GetAsync("Batch/GetAll");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var batches = JsonConvert.DeserializeObject<List<Batch>>(await response.Content.ReadAsStringAsync());

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = batches.Count,
                    recordsFiltered = batches.Count,
                    data = batches
                };

                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Batch batch)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(batch), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Batch/Create", content);
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
            var response = await _httpClient.GetAsync($"Batch/Edit/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }

            var batch = JsonConvert.DeserializeObject<Batch>(await response.Content.ReadAsStringAsync());
            if (batch == null)
            {
                return NotFound();
            }
            return Ok(batch);
        }

        [HttpPost("UpdateBatch")]
        public async Task<IActionResult> Update([FromBody] Batch batch)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(batch), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("Batch/Update", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"Batch/Delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}
