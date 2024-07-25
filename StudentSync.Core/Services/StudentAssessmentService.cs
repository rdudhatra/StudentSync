using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Core.Services
{
    public class StudentAssessmentService : IStudentAssessmentService
    {
        private readonly StudentSyncDbContext _context;

        public StudentAssessmentService(StudentSyncDbContext context)
        {
            _context = context;
        }

        public async Task SaveStudentAssessment(StudentAssessment studentAssessment)
        {
           await _context.SaveStudentAssessment(studentAssessment);
        }

        public async Task UpdateStudentAssessment(StudentAssessment studentAssessment)
        {
            await _context.UpdateStudentAssessment(studentAssessment);
        }

        public async Task DeleteStudentAssessment(int studentAssessmentId)
        {
            await _context.DeleteStudentAssessment(studentAssessmentId);
        }

   

        public async Task<IEnumerable<StudentAssessmentResponseModel>> GetAllStudentAssessments()
        {
            var assessments = await (from assessment in _context.StudentAssessments
                                     join course in _context.Courses on assessment.CourseExamId equals course.CourseId into courseJoin
                                     from course in courseJoin.DefaultIfEmpty()
                                     select new StudentAssessmentResponseModel
                                     {
                                         Id = assessment.Id,
                                         AssessmentDate = assessment.AssessmentDate,
                                         EnrollmentNo = assessment.EnrollmentNo,
                                         CourseExamId = assessment.CourseExamId,
                                         CourseName = course.CourseName ,// Include the Course name
                                         ObtainedMarks = assessment.ObtainedMarks,
                                         Remarks = assessment.Remarks,
                                         CreatedBy = assessment.CreatedBy,
                                         CreatedDate = assessment.CreatedDate,
                                         UpdatedBy = assessment.UpdatedBy,
                                         UpdatedDate = assessment.UpdatedDate
                                     }).ToListAsync();

            return assessments;
        }

        public async Task<StudentAssessment> GetStudentAssessmentById(int id)
        {
            return await _context.GetStudentAssessmentById(id);
        }

        public async Task<int> GetTotalStudentAssessmentsAsync()
        {
            return await _context.StudentAssessments.CountAsync(); 
        }
    }

}



