using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentSync.Core.Services.Interface
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentResponseModel>> GetAllEnrollments();
        Task<Enrollment> GetEnrollmentById(int id);
        Task AddEnrollment(Enrollment enrollment);
        Task UpdateEnrollment(Enrollment enrollment);
        Task DeleteEnrollment(int id);
        Task<int> GetTotalEnrollmentsAsync();

        List<Enrollment> GetAllEnrollMentno();

    }
}
