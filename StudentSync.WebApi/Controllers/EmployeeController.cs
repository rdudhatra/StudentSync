﻿//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Core.Wrapper;
//using StudentSync.Data.Models;
//using System.Threading.Tasks;

//namespace StudentSync.WebAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmployeeController : ControllerBase
//    {
//        private readonly IEmployeeService _employeeService;
//        private readonly IWebHostEnvironment _environment;


//        public EmployeeController(IEmployeeService employeeService, IWebHostEnvironment environment)
//        {
//            _employeeService = employeeService;
//            _environment = environment;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var result = await _employeeService.GetAllEmployeesAsync();
//            if (result.Succeeded)
//            {
//                return Ok(result);
//            }
//            return BadRequest(result.Messages);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var result = await _employeeService.GetEmployeeByIdAsync(id);
//            if (result.Succeeded)
//            {
//                return Ok(result);
//            }
//            return NotFound(result.Messages);
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var result = await _employeeService.AddEmployeeAsync(employee);
//            if (result.Succeeded)
//            {
//                return Ok(result);
//            }
//            return StatusCode(500, result.Messages);
//        }

//        [HttpPut]
//        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var result = await _employeeService.UpdateEmployeeAsync(employee);
//            if (result.Succeeded)
//            {
//                return Ok(result);
//            }
//            return StatusCode(500, result.Messages);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteEmployee(int id)
//        {
//            var result = await _employeeService.DeleteEmployeeAsync(id);
//            if (result.Succeeded)
//            {
//                return Ok(result);
//            }
//            return NotFound(result.Messages);
//        }

//        [HttpGet("SearchByName")]
//        public async Task<IActionResult> SearchByName(string name)
//        {
//            var result = await _employeeService.SearchEmployeesByNameAsync(name);
//            if (result.Succeeded)
//            {
//                return Ok(result);
//            }
//            return BadRequest(result.Messages);
//        }
//    }
//}














//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Core.Wrapper;
//using StudentSync.Data.Models;
//using System.Threading.Tasks;

//namespace StudentSync.WebAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmployeeController : ControllerBase
//    {
//        private readonly IEmployeeService _employeeService;
//        private readonly IWebHostEnvironment _environment;


//        public EmployeeController(IEmployeeService employeeService, IWebHostEnvironment environment)
//        {
//            _employeeService = employeeService;
//            _environment = environment;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll(int draw, int start, int length, string searchValue, string sortColumn, string sortColumnDirection)
//        {
//            try
//            {
//                var pageSize = length;
//                var skip = start;

//                var result = await _employeeService.GetAllEmployeesAsync();
//                if (!result.Succeeded)
//                {
//                    return BadRequest(new { succeeded = false, messages = result.Messages });
//                }

//                var employeeData = result.Data.AsQueryable(); // Assuming result.Data is IEnumerable<Employee>

//                // Apply search filter
//                if (!string.IsNullOrEmpty(searchValue))
//                {
//                    employeeData = employeeData.Where(e => e.FirstName.Contains(searchValue)
//                                                    || e.LastName.Contains(searchValue));
//                }

//                // Apply sorting
//                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
//                {
//                    switch (sortColumn.ToLower())
//                    {
//                        case "firstname":
//                            employeeData = sortColumnDirection == "asc" ?
//                                           employeeData.OrderBy(e => e.FirstName) :
//                                           employeeData.OrderByDescending(e => e.FirstName);
//                            break;
//                        case "lastname":
//                            employeeData = sortColumnDirection == "asc" ?
//                                           employeeData.OrderBy(e => e.LastName) :
//                                           employeeData.OrderByDescending(e => e.LastName);
//                            break;

//                        default:
//                            break;
//                    }
//                }
//                // Check if employeeData is null or empty before counting
//                if (employeeData == null)
//                {
//                    return StatusCode(StatusCodes.Status500InternalServerError, new { succeeded = false, messages = new[] { "Employee data is null." } });
//                }
//                // Get total records count
//                var recordsTotal = employeeData.Count();

//                // Apply pagination
//                var data = employeeData.Skip(skip).Take(pageSize).ToList();

//                // Return JSON response
//                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
//                return Ok(jsonData);
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine($"Error in GetAll action: {ex.Message}");
//                return StatusCode(StatusCodes.Status500InternalServerError, new { succeeded = false, messages = new[] { "An unexpected error occurred." } });
//            }
//        }



//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var result = await _employeeService.GetEmployeeByIdAsync(id);
//            if (result.Succeeded)
//            {
//                return Ok(result);
//            }
//            return NotFound(result.Messages);
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var result = await _employeeService.AddEmployeeAsync(employee);
//            if (result.Succeeded)
//            {
//                return Ok(result);
//            }
//            return StatusCode(500, result.Messages);
//        }

//        [HttpPut]
//        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var result = await _employeeService.UpdateEmployeeAsync(employee);
//            if (result.Succeeded)
//            {
//                return Ok(result);
//            }
//            return StatusCode(500, result.Messages);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteEmployee(int id)
//        {
//            var result = await _employeeService.DeleteEmployeeAsync(id);
//            if (result.Succeeded)
//            {
//                return Ok(result);
//            }
//            return NotFound(result.Messages);
//        }

//        [HttpGet("SearchByName")]
//        public async Task<IActionResult> SearchByName(string name)
//        {
//            var result = await _employeeService.SearchEmployeesByNameAsync(name);
//            if (result.Succeeded)
//            {
//                return Ok(result);
//            }
//            return BadRequest(result.Messages);
//        }
//    }
//}



using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.ApiControllers
{
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeApiController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.AddEmployeeAsync(employee);
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.UpdateEmployeeAsync(employee);
                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                await _employeeService.DeleteEmployeeAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName(string name)
        {
            try
            {
                var employees = await _employeeService.SearchEmployeesByNameAsync(name);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
