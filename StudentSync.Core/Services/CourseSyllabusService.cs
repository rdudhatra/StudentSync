using StudentSync.Core.Services.Interface;
using StudentSync.Data;
using StudentSync.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentSync.Data.Data;
using StudentSync.Data.ResponseModel;

namespace StudentSync.Core.Services
{
    public class CourseSyllabusService : ICourseSyllabusService
    {
        private readonly StudentSyncDbContext _context;

        public CourseSyllabusService(StudentSyncDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseSyllabusResponseModel>> GetAllCourseSyllabusesAsync()
        {
            var courseSyllabuses = await _context.CourseSyllabi
            .Join(_context.Courses,
                  cs => cs.CourseId,
                  course => course.CourseId,
                  (cs, course) => new CourseSyllabusResponseModel
                  {
                      Id = cs.Id,
                      CourseId = cs.CourseId,
                      CourseName = course.CourseName,
                      ChapterName = cs.ChapterName,
                      TopicName = cs.TopicName,
                      Remarks = cs.Remarks,
                      CreatedBy = cs.CreatedBy,
                      CreatedDate = cs.CreatedDate,
                      UpdatedBy = cs.UpdatedBy,
                      UpdatedDate = cs.UpdatedDate
                  })
            .ToListAsync();

            return courseSyllabuses;
        }

        public async Task<CourseSyllabus> GetCourseSyllabusByIdAsync(int id)
        {
            return await _context.CourseSyllabi.FindAsync(id);
        }

        public async Task<int> AddCourseSyllabusAsync(CourseSyllabus courseSyllabus)
        {
            courseSyllabus.CreatedDate = DateTime.UtcNow;
            _context.CourseSyllabi.Add(courseSyllabus);
            await _context.SaveChangesAsync();
            return courseSyllabus.Id;
        }

        public async Task<int> UpdateCourseSyllabusAsync(CourseSyllabus courseSyllabus)
        {
            var existingCourseSyllabus = await _context.CourseSyllabi.FindAsync(courseSyllabus.Id);
            if (existingCourseSyllabus == null)
                throw new ArgumentException("Course Syllabus not found");

            existingCourseSyllabus.CourseId = courseSyllabus.CourseId;
            existingCourseSyllabus.ChapterName = courseSyllabus.ChapterName;
            existingCourseSyllabus.TopicName = courseSyllabus.TopicName;
            existingCourseSyllabus.Remarks = courseSyllabus.Remarks;
            existingCourseSyllabus.UpdatedBy = courseSyllabus.UpdatedBy;
            existingCourseSyllabus.UpdatedDate = DateTime.UtcNow;

            _context.CourseSyllabi.Update(existingCourseSyllabus);
            await _context.SaveChangesAsync();
            return existingCourseSyllabus.Id;
        }

        public async Task<bool> DeleteCourseSyllabusAsync(int id)
        {
            var courseSyllabus = await _context.CourseSyllabi.FindAsync(id);
            if (courseSyllabus == null)
                throw new ArgumentException("Course Syllabus not found");

            _context.CourseSyllabi.Remove(courseSyllabus);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
