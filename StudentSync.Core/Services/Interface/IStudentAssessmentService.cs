using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Core.Services.Interface
{
    public interface IStudentAssessmentService
    {
        Task  SaveStudentAssessment(StudentAssessment studentAssessment);
        Task UpdateStudentAssessment(StudentAssessment studentAssessment);
        Task DeleteStudentAssessment(int studentAssessmentId);
        Task<List<StudentAssessment>> GetAllStudentAssessments();
        Task<StudentAssessment> GetStudentAssessmentById(int id);
    }

}
//Task<List<Enrollment>> GetAllEnrollmentsAsync();
//Task<Enrollment> GetEnrollmentByIdAsync(int enrollmentno);
//Task CreateEnrollmentAsync(Enrollment enrollment);
//Task UpdateEnrollmentAsync(Enrollment enrollment);
//Task DeleteEnrollmentAsync(string enrollmentNo);