﻿using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System.Threading.Tasks;

namespace StudentSync.Controllers
{
    [Route("Employee")]
    public class EmployeesController : Controller
    {

        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(int draw, int start, int length, string searchValue, string sortColumn, string sortColumnDirection)
        {
            try
            {
                var pageSize = length;
                var skip = start;

                var result = await _employeeService.GetAllEmployeesAsync();
                if (!result.Succeeded)
                {
                    return BadRequest(new { succeeded = false, messages = result.Messages });
                }

                var employeeData = result.Data.AsQueryable(); // Assuming result.Data is IEnumerable<Employee>

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
                var data = employeeData.Skip(skip).Take(pageSize).ToList();

                // Return JSON response
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in GetAll action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { succeeded = false, messages = new[] { "An unexpected error occurred." } });
            }
        }


        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _employeeService.GetEmployeeByIdAsync(id);
            if (result.Succeeded)
            {
                return Json(new { data = result.Data });
            }
            return NotFound(result.Messages);
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var result = await _employeeService.AddEmployeeAsync(employee);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = result.Messages });
                }
                return BadRequest(result.Messages);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var result = await _employeeService.UpdateEmployeeAsync(employee);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = result.Messages });
                }
                return BadRequest(result.Messages);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = result.Messages });
            }
            return BadRequest(result.Messages);
        }

        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var result = await _employeeService.SearchEmployeesByNameAsync(name);
            if (result.Succeeded)
            {
                return Json(new { data = result.Data });
            }
            return BadRequest(result.Messages);
        }
    }
}






//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Data.Models;
//using System.Threading.Tasks;

//namespace StudentSync.Controllers
//{
//    [Route("Employee")]
//    public class EmployeesController : Controller
//    { 

//        private readonly HttpClient _httpClient;

//        public EmployeesController(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<IActionResult> Index()
//        {
//            return View();
//        }


//        [HttpGet("GetAll")]
//        public async Task<IActionResult> GetAll(int draw, int start, int length, string searchValue, string sortColumn, string sortColumnDirection)
//        {
//            var response = await _httpClient.GetAsync("api/employee");
//            if (response.IsSuccessStatusCode)
//            {
//                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Employee>>>();
//                return Json(new { data = result.Data });
//            }
//            return BadRequest(await response.Content.ReadAsStringAsync());
//        }

//        [HttpGet("GetById/{id}")]
//        public async Task<IActionResult> GetById(int id)
//            {
//                var response = await _httpClient.GetAsync($"api/employee/{id}");
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<Employee>>();
//                    return Json(new { data = result.Data });
//                }
//                return NotFound(await response.Content.ReadAsStringAsync());
//            }

//        [HttpPost("AddEmployee")]
//        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
//        {
//            if (ModelState.IsValid)
//            {
//                var response = await _httpClient.PostAsJsonAsync("api/employee", employee);
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
//                    return Json(new { success = true, message = result.Messages });
//                }
//                return BadRequest(await response.Content.ReadAsStringAsync());
//            }
//            return BadRequest(ModelState);
//        }
//        [HttpPut("UpdateEmployee")]
//        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
//        {
//            if (ModelState.IsValid)
//            {
//                var response = await _httpClient.PutAsJsonAsync("api/employee", employee);
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
//                    return Json(new { success = true, message = result.Messages });
//                }
//                return BadRequest(await response.Content.ReadAsStringAsync());
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpDelete("DeleteEmployee/{id}")]
//        public async Task<IActionResult> DeleteEmployee(int id)
//        {
//            var response = await _httpClient.DeleteAsync($"api/employee/{id}");
//            if (response.IsSuccessStatusCode)
//            {
//                var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
//                return Json(new { success = true, message = result.Messages });
//            }
//            return BadRequest(await response.Content.ReadAsStringAsync());
//        }

//        [HttpGet("SearchByName")]
//        public async Task<IActionResult> SearchByName(string name)
//        {
//            var response = await _httpClient.GetAsync($"api/employee/SearchByName?name={name}");
//            if (response.IsSuccessStatusCode)
//            {
//                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Employee>>>();
//                return Json(new { data = result.Data });
//            }
//            return BadRequest(await response.Content.ReadAsStringAsync());
//        }
//    }
//}





