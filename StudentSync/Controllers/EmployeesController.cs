


using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Web.Controllers
{
    [Route("Employee")]
    public class EmployeesController : Controller
    {
        private readonly HttpClient _httpClient;
       // private readonly IEmployeeService _employeeService;

        public EmployeesController(HttpClient httpClient)
        {
           // _employeeService = employeeService;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(int draw, int start, int length, string searchValue, string sortColumn, string sortColumnDirection)
        {
            try
            {
                var response = await _httpClient.GetAsync("Employee/GetAll");
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var employees = JsonConvert.DeserializeObject<List<Employee>>(jsonResponse);

                var employeeData = employees.AsQueryable();

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    employeeData = employeeData.Where(e => e.FirstName.Contains(searchValue)
                                                    || e.LastName.Contains(searchValue));
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    switch (sortColumn.ToLower())
                    {
                        case "firstname":
                            employeeData = sortColumnDirection == "asc" ?
                                           employeeData.OrderBy(e => e.FirstName) :
                                           employeeData.OrderByDescending(e => e.FirstName);
                            break;
                        case "lastname":
                            employeeData = sortColumnDirection == "asc" ?
                                           employeeData.OrderBy(e => e.LastName) :
                                           employeeData.OrderByDescending(e => e.LastName);
                            break;
                        default:
                            break;
                    }
                }

                // Get total records count
                var recordsTotal = employeeData.Count();

                // Apply pagination
                var data = employeeData.Skip(start).Take(length).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
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
             var response = await _httpClient.GetAsync($"Employee/GetById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var employee = JsonConvert.DeserializeObject<Employee>(await response.Content.ReadAsStringAsync());
                return Ok(employee);
            }
            return NotFound(await response.Content.ReadAsStringAsync());
        }
      


        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Employee/AddEmployee", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
            return BadRequest(ModelState);
        }

        [HttpPost("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("Employee/UpdateEmployee", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
            return BadRequest(ModelState);
        }

        [HttpPost("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var response = await _httpClient.DeleteAsync($"Employee/DeleteEmployee/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { success = true });
            }
            return BadRequest(await response.Content.ReadAsStringAsync());
        }

        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var response = await _httpClient.GetAsync($"Employee/SearchByName?name={name}");
            if (response.IsSuccessStatusCode)
            {
                var employees = JsonConvert.DeserializeObject<List<Employee>>(await response.Content.ReadAsStringAsync());
                return Ok(new { data = employees });
            }
            return BadRequest(await response.Content.ReadAsStringAsync());
        }
    }
}



