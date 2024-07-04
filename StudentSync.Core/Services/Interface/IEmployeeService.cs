using StudentSync.Core.Wrapper;
using StudentSync.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentSync.Core.Services.Interface
{
    public interface IEmployeeService
    {
        Task<IResult<IEnumerable<Employee>>> GetAllEmployeesAsync();
        Task<IResult<Employee>> GetEmployeeByIdAsync(int id);
        Task<IResult> AddEmployeeAsync(Employee employee);
        Task<IResult> UpdateEmployeeAsync(Employee employee);
        Task<IResult> DeleteEmployeeAsync(int id);
        Task<IResult<IEnumerable<Employee>>> SearchEmployeesByNameAsync(string name);

      //  Task<PaginatedResult<Employee>> GetPaginatedEmployeesAsync(int pageNumber, int pageSize);

    }
}
