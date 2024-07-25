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
        Task<List<Course>> GetAllCourseAsync();
        Task<Course> GetCoursesByIdAsync(int courseId);
        Task<IResult> AddCourseAsync(Course course);
        Task<IResult> UpdateCourseAsync(Course course);
        Task<IResult> DeleteCourseAsync(int id);
        Task<IResult<IEnumerable<Course>>> SearchCourseByNameAsync(string name);
        Task<int> GetTotalCoursesAsync();

        List<Course> GetAllCourseIds();

    }
}
