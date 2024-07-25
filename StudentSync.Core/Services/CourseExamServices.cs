using StudentSync.Core.Services.Interface;
using StudentSync.Core.Wrapper;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentSync.Data.ResponseModel;

namespace StudentSync.Core.Services
{
    public class CourseExamServices : ICourseExamServices
    {
        private readonly StudentSyncDbContext _context;

        public CourseExamServices(StudentSyncDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetTotalCourseExamsAsync()
        {
            return await _context.CourseExams.CountAsync();
        }
        public List<CourseExam> GetAllCourseExamIds()
        {
            return _context.CourseExams.ToList();
        }
        public async Task<List<CourseExamResponseModel>> GetAllCourseExamsAsync()
        {
            var courseExams = await _context.CourseExams
            .Join(_context.Courses,
                  ce => ce.CourseId,
                  course => course.CourseId,
                  (ce, course) => new CourseExamResponseModel
                  {
                      Id = ce.Id,
                      CourseId = ce.CourseId,
                      CourseName = course.CourseName,
                      ExamTitle = ce.ExamTitle,
                      ExamType = ce.ExamType,
                      TotalMarks = ce.TotalMarks,
                      PassingMarks = ce.PassingMarks,
                      Remarks = ce.Remarks,
                      CreatedBy = ce.CreatedBy,
                      CreatedDate = ce.CreatedDate,
                      UpdatedBy = ce.UpdatedBy,
                      UpdatedDate = ce.UpdatedDate
                  })
            .ToListAsync();

            return courseExams;
        }



        public async Task<CourseExam> GetCourseExamByIdAsync(int id)
        {
           return await _context.CourseExams.FindAsync(id);
           
        }

        public async Task<IResult> AddCourseExamAsync(CourseExam courseExam)
        {
            _context.CourseExams.Add(courseExam);
            await _context.SaveChangesAsync();
            return Result.Success("Course exam added successfully");
        }

        public async Task<int> UpdateCourseExamAsync(CourseExam courseExam)
        {
            var existingCourseExam = await _context.CourseExams.FindAsync(courseExam.Id);
            if (existingCourseExam == null)
            {
                    throw new ArgumentException("Course Fee not found");
            }

            existingCourseExam.CourseId = courseExam.CourseId;
            existingCourseExam.ExamTitle = courseExam.ExamTitle;
            existingCourseExam.ExamType = courseExam.ExamType;
            existingCourseExam.TotalMarks = courseExam.TotalMarks;
            existingCourseExam.PassingMarks = courseExam.PassingMarks;
            existingCourseExam.Remarks = courseExam.Remarks;
            existingCourseExam.UpdatedBy = courseExam.UpdatedBy;
            existingCourseExam.UpdatedDate = DateTime.UtcNow;

            _context.CourseExams.Update(existingCourseExam);
            await _context.SaveChangesAsync();
            return existingCourseExam.Id;
        }


        public async Task<IResult> DeleteCourseExamAsync(int id)
        {
            var courseExam = await _context.CourseExams.FindAsync(id);
            if (courseExam == null)
            {
                return Result.Fail("Course exam not found");
            }

            _context.CourseExams.Remove(courseExam);
            await _context.SaveChangesAsync();
            return Result.Success("Course exam deleted successfully");
        }

        public async Task<IResult<IEnumerable<CourseExam>>> SearchCourseExamByCourseIdAsync(int courseId)
        {
            var courseExams = await _context.CourseExams
                .Where(ce => ce.CourseId == courseId)
                .ToListAsync();
            return Result<IEnumerable<CourseExam>>.Success(courseExams);
        }

        public async Task<IResult<IEnumerable<CourseExam>>> SearchCourseExamByExamTitleAsync(string examTitle)
        {
            var courseExams = await _context.CourseExams
                .Where(ce => ce.ExamTitle.Contains(examTitle))
                .ToListAsync();
            return Result<IEnumerable<CourseExam>>.Success(courseExams);
        }
    }
}
