using StudentSync.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentSync.Core.Services.Interface
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<Enrollment>> GetAllEnrollments();
        Task<Enrollment> GetEnrollmentById(int id);
        Task AddEnrollment(Enrollment enrollment);
        Task UpdateEnrollment(Enrollment enrollment);
        Task DeleteEnrollment(int id);

        List<Enrollment> GetAllEnrollMentno();

    }
}
