

using Microsoft.AspNetCore.Mvc;
using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
using StudentSync.Service.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Web.Controllers
{
    [Route("Batch")]
    public class BatchController : Controller
    {
        private readonly IHttpService _httpService;

        public BatchController(IHttpService httpService)
        {
            _httpService = httpService;
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
                var response = await _httpService.Get<List<Batch>>("Batch/GetAllBatchesIds");
                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                return Json(response.Data);
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
                // DataTables parameters
                int draw = int.Parse(Request.Query["draw"]);
                int start = int.Parse(Request.Query["start"]);
                int length = int.Parse(Request.Query["length"]);
                string searchValue = Request.Query["search[value]"];

                var response = await _httpService.Get<List<BatchResponseModel>>("Batch/GetAll");
                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                var batches = response.Data;
                // Apply search filter if searchValue is provided
                if (!string.IsNullOrEmpty(searchValue))
                {
                    batches = batches.Where(b =>
                        b.BatchTime.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                        b.FacultyName.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Paginate the results
                int recordsTotal = batches.Count;
                batches = batches.Skip(start).Take(length).ToList();

                var dataTableResponse = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal, // Assuming no filtering at server-side
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
                var response = await _httpService.Post("Batch/Create", batch);
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
            var response = await _httpService.Get<Batch>($"Batch/Edit/{id}");
            if (!response.Succeeded)
            {
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }

            var batch = response.Data;
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
                var response = await _httpService.Put("Batch/Update", batch);
                if (response.Succeeded)
                {
                    return Ok(new { success = true });
                }
                return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpService.Delete($"Batch/Delete/{id}");
            if (response.Succeeded)
            {
                return Ok(new { success = true });
            }
            return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
        }
    }
}





