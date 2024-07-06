using StudentSync.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentSync.Core.Services.Interface
{
    public interface ICourseSyllabusService
    {
        Task<IEnumerable<CourseSyllabus>> GetAllCourseSyllabusesAsync();
        Task<CourseSyllabus> GetCourseSyllabusByIdAsync(int id);
        Task<int> AddCourseSyllabusAsync(CourseSyllabus courseSyllabus);
        Task<int> UpdateCourseSyllabusAsync(CourseSyllabus courseSyllabus);
        Task<bool> DeleteCourseSyllabusAsync(int id);
    }
}
