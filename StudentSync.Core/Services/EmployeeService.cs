using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interface;
using StudentSync.Core.Wrapper;
using StudentSync.Data;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSync.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly StudentSyncDbContext _context;

        public EmployeeService(StudentSyncDbContext context)
        {
            _context = context;
        }

        
        public async Task<IResult<IEnumerable<Employee>>> GetAllEmployeesAsync()
        {
            var employees = await _context.Employees.ToListAsync();
            return Result<IEnumerable<Employee>>.Success(employees);
        }

        //public async Task<PaginatedResult<Employee>> GetPaginatedEmployeesAsync(int pageNumber, int pageSize)
        //{
        //    try
        //    {
        //        var employees = await _context.Employees
        //            .Skip((pageNumber - 1) * pageSize)
        //            .Take(pageSize)
        //            .ToListAsync();

        //        var totalItems = await _context.Employees.CountAsync();

        //        return new PaginatedResult<Employee>(employees, pageNumber, pageSize, totalItems)
        //        {
        //            Succeeded = true
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging purposes
        //        Console.WriteLine($"Error in GetPaginatedEmployeesAsync: {ex.Message}");
        //        return new PaginatedResult<Employee>(null, pageNumber, pageSize, 0)
        //        {
        //            Succeeded = false,
        //            Messages = new List<string> { "Failed to retrieve paginated employees." }
        //        };
        //    }
        //}




        public async Task<IResult<Employee>> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return Result<Employee>.Fail("Employee not found");
            }
            return Result<Employee>.Success(employee);
        }

        public async Task<IResult> AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return Result.Success("Employee added successfully");
        }

        public async Task<IResult> UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return Result.Success("Employee updated successfully");
        }

        public async Task<IResult> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return Result.Fail("Employee not found");
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return Result.Success("Employee deleted successfully");
        }

        public async Task<IResult<IEnumerable<Employee>>> SearchEmployeesByNameAsync(string name)
        {
            var employees = await _context.Employees
                .Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name))
                .ToListAsync();
            return Result<IEnumerable<Employee>>.Success(employees);
        }
    }
}
