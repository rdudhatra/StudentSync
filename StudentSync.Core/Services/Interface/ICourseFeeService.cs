using System.Collections.Generic;
using System.Threading.Tasks;
using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;

namespace StudentSync.Core.Services.Interface
{
    public interface ICourseFeeService
    {
        Task<IEnumerable<CourseFeeResponseModel>> GetAllCourseFeesAsync();
        Task<CourseFee> GetCourseFeeByIdAsync(int id);
        Task<int> AddCourseFeeAsync(CourseFee courseFee);
        Task<int> UpdateCourseFeeAsync(CourseFee courseFee);
        Task<bool> DeleteCourseFeeAsync(int id);
        List<CourseFee> GetAllCourseExamIds();
        Task<int> GetTotalCourseFeesAsync();

    }
}
