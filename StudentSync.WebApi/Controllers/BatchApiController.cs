//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Data.Models;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace StudentSync.ApiControllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class BatchApiController : ControllerBase
//    {
//        private readonly IBatchService _batchService;

//        public BatchApiController(IBatchService batchService)
//        {
//            _batchService = batchService;
//        }

//        [HttpGet]
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

//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] Batch batch)
//        {
//            if (ModelState.IsValid)
//            {
//                await _batchService.CreateBatchAsync(batch);
//                return Ok(new { success = true, message = "Batch added successfully." });
//            }
//            return BadRequest(new { success = false, message = "Invalid batch data." });
//        }

//        [HttpGet("Edit/{id}")]
//        public async Task<IActionResult> Edit(int id)
//        {
//            var batch = await _batchService.GetBatchByIdAsync(id);
//            if (batch == null)
//            {
//                return NotFound();
//            }
//            return Ok(batch);
//        }

//        [HttpPut]
//        public async Task<IActionResult> UpdateBatch([FromBody] Batch batch)
//        {
//            if (ModelState.IsValid)
//            {
//                await _batchService.UpdateBatchAsync(batch);
//                return Ok(new { success = true, message = "Batch updated successfully." });
//            }
//            return BadRequest(new { success = false, message = "Invalid batch data." });
//        }

//        [HttpDelete]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            await _batchService.DeleteBatchAsync(id);
//            return Ok(new { success = true, message = "Batch deleted successfully." });
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Threading.Tasks;

namespace StudentSync.ApiControllers
{
    [Route("api/Batch")]
    [ApiController]
    public class BatchApiController : ControllerBase
    {
        private readonly IBatchService _batchService;

        public BatchApiController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var batches = await _batchService.GetAllBatchesAsync();
                return Ok(batches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpGet("GetAllBatchesIds")]
        //public IActionResult GetAllBatchIds()
        //{
        //    try
        //    {
        //        var batchIds = _batchService.GetAllBatchesIds(); // Implement this method in your service
        //        return Ok(batchIds);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Batch batch)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _batchService.CreateBatchAsync(batch);
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
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
                var batch = await _batchService.GetBatchByIdAsync(id);
                if (batch == null)
                {
                    return NotFound();
                }
                return Ok(batch);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Batch batch)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _batchService.UpdateBatchAsync(batch);
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
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
                await _batchService.DeleteBatchAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
