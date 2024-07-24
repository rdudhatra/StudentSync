

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Data.Models;
using StudentSync.Service.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Web.Controllers
{
    [Route("Employee")]
    public class EmployeesController : Controller
    {
        private readonly IHttpService _httpService;

        public EmployeesController(IHttpService httpService)
        {
            _httpService = httpService;
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
                // DataTables parameters
                int draw = int.Parse(Request.Query["draw"]);
                int start = int.Parse(Request.Query["start"]);
                int length = int.Parse(Request.Query["length"]);

                var response = await _httpService.Get<List<Employee>>("Employee/GetAll");
                if (!response.Succeeded)
                {
                    return StatusCode((int)response.Response.StatusCode, response.Response.ReasonPhrase);
                }

                var employees = response.Data;

                var searchValue = Request.Query["search[value]"].FirstOrDefault();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    employees = employees
                        .Where(ce =>
                            ce.FirstName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                            ce.LastName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                            ce.Gender.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Paginate the results
                int recordsTotal = employees.Count;
                employees = employees.Skip(start).Take(length).ToList();

                var dataTableResponse = new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = employees
                };
                return Ok(dataTableResponse);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in GetAll action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _httpService.Get<Employee>($"Employee/GetById/{id}");
            if (response.Succeeded)
            {
                return Ok(response.Data);
            }
            return NotFound(response.Response.ReasonPhrase);
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Post("Employee/AddEmployee", employee);
                if (response.Succeeded)
                {
                    return Ok(new { success = true });
                }
                return BadRequest(response.Response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpService.Put("Employee/UpdateEmployee", employee);
                if (response.Succeeded)
                {
                    return Ok(new { success = true });
                }
                return BadRequest(response.Response.ReasonPhrase);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var response = await _httpService.Delete($"Employee/DeleteEmployee/{id}");
            if (response.Succeeded)
            {
                return Ok(new { success = true });
            }
            return BadRequest(response.Response.ReasonPhrase);
        }

        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var response = await _httpService.Get<List<Employee>>($"Employee/SearchByName?name={name}");
            if (response.Succeeded)
            {
                return Ok(new { data = response.Data });
            }
            return BadRequest(response.Response.ReasonPhrase);
        }
    }
}
