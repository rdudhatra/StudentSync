using Microsoft.AspNetCore.Mvc;
using StudentSync.Data.Models;
using StudentSync.Core.Services.Interface;
using StudentSync.Core.Services;

namespace StudentSync.Controllers
{
    [Route("Batch")]

    public class BatchController : Controller
    {
        private readonly IBatchService _batchService;

        public BatchController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("GetAllBatchesIds")]
        //public IActionResult GetAllCourseIds()
        //{
        //    try
        //    {
        //        var batchesIds = _batchService.GetAllBatchesIds(); // Implement this method in your service
        //        return Json(batchesIds);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = "Failed to retrieve Batch IDs", error = ex.Message });
        //    }
        //}
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                var batches = await _batchService.GetAllBatchesAsync();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    batches = batches
                        .Where(b => b.BatchCode.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                    b.BatchTime.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                    b.FacultyName.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var dataTableResponse = new
                {
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = batches.Count(),
                    recordsFiltered = batches.Count(),
                    data = batches
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
        public async Task<IActionResult> Create([FromBody] Batch batch)
        {
            if (ModelState.IsValid)
            {
                await _batchService.CreateBatchAsync(batch);
                return Json(new { success = true, message = "Batch added successfully." });
            }
            return Json(new { success = false, message = "Invalid batch data." });
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var batch = await _batchService.GetBatchByIdAsync(id);
            if (batch == null)
            {
                return NotFound();
            }
            return Json(batch);
        }

        [HttpPost("UpdateBatch")]
        public async Task<IActionResult> UpdateBatch([FromBody] Batch batch)
        {
            if (ModelState.IsValid)
            {
                await _batchService.UpdateBatchAsync(batch);
                return Json(new { success = true, message = "Batch updated successfully." });
            }
            return Json(new { success = false, message = "Invalid batch data." });
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _batchService.DeleteBatchAsync(id);
            return Json(new { success = true, message = "Batch deleted successfully." });
        }
    }
}
