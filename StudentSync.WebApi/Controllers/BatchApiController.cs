


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Threading.Tasks;

namespace StudentSync.ApiControllers
{

    [Route("api/Batch")]
    [ApiController]
    [Authorize]
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

        [HttpGet("GetAllBatchesIds")]
        public async Task<IActionResult> GetAllBatchesIds()
        {
            try
            {
                var batchesIds =  _batchService.GetAllBatchesIdsAsync();
                return Ok(batchesIds);
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
