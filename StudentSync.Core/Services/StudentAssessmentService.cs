using StudentSync.Core.Services;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
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

        public async Task<List<StudentAssessment>> GetAllStudentAssessments()
        {
            return await _context.GetAllStudentAssessments();
        }

        public async Task<StudentAssessment> GetStudentAssessmentById(int id)
        {
            return await _context.GetStudentAssessmentById(id);
        }
    }

}



