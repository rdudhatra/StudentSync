using StudentSync.Core.Services.Interface;
using StudentSync.Data;
using StudentSync.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentSync.Data.Data;

namespace StudentSync.Core.Services
{
    public class CourseSyllabusService : ICourseSyllabusService
    {
        private readonly StudentSyncDbContext _context;

        public CourseSyllabusService(StudentSyncDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseSyllabus>> GetAllCourseSyllabusesAsync()
        {
            return await _context.CourseSyllabi.ToListAsync();
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
