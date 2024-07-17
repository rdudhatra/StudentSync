
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Data.Models;
using System.Text;

namespace StudentSync.Web.Controllers
{
    [Route("Batch")]
    public class BatchController : Controller
    {
        private readonly HttpClient _httpClient;


        public BatchController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                var response = await _httpClient.GetAsync("Batch/GetAllBatchesIds");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var batchesIds = JsonConvert.DeserializeObject<List<Batch>>(await response.Content.ReadAsStringAsync());
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




