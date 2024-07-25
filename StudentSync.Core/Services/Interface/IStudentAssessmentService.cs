using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
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
        Task<IEnumerable<StudentAssessmentResponseModel>> GetAllStudentAssessments();
        Task<StudentAssessment> GetStudentAssessmentById(int id);
        Task<int> GetTotalStudentAssessmentsAsync();

    }

}
