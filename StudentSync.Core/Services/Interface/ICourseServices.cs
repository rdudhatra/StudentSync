using StudentSync.Core.Wrapper;
using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Core.Services.Interface
{
    public interface ICourseServices
    {
         Task<IResult<IEnumerable<Course>>> GetAllCourseAsync();
        // Task<List<Course>> GetAllCourseAsync();

        Task<IResult<Course>> GetCoursesByIdAsync(int id);
        Task<IResult> AddCourseAsync(Course course);
        Task<IResult> UpdateCourseAsync(Course course);
        Task<IResult> DeleteCourseAsync(int id);
        Task<IResult<IEnumerable<Course>>> SearchCourseByNameAsync(string name);
    }
}
