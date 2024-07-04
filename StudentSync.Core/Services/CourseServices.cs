using StudentSync.Core.Services.Interface;
using StudentSync.Core.Wrapper;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Core.Services
{
    public class CourseServices : ICourseServices
    {
        private readonly StudentSyncDbContext _context;

        public CourseServices(StudentSyncDbContext context)
        {
            _context = context;
        }

        public async Task<IResult<IEnumerable<Course>>> GetAllCourseAsync()
        {
            var courses = await _context.Courses.ToListAsync();
            return Result<IEnumerable<Course>>.Success(courses);
        }
        //public async Task<List<Course>> GetAllCourseAsync()
        //{
        //    return await _context.Courses.ToListAsync();
        //}

        public async Task<IResult<Course>> GetCoursesByIdAsync(int id)
        {
            var courses = await _context.Courses.FindAsync(id);
            if (courses == null)
            {
                return Result<Course>.Fail("Course not found");
            }
            return Result<Course>.Success(courses);
        }

        public async Task<IResult> AddCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return Result.Success("Course added successfully");
        }

        public async Task<IResult> UpdateCourseAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return Result.Success("Course updated successfully");
        }

        public async Task<IResult> DeleteCourseAsync(int id)
        {
            var courses = await _context.Courses.FindAsync(id);
            if (courses == null)
            {
                return Result.Fail("Course not found");
            }

            _context.Courses.Remove(courses);
            await _context.SaveChangesAsync();
            return Result.Success("Course deleted successfully");
        }
        public async Task<IResult<IEnumerable<Course>>> SearchCourseByNameAsync(string name)
        {
            var courses = await _context.Courses
                .Where(e => e.CourseName.Contains(name) || e.Duration.Contains(name))
                .ToListAsync();
            return Result<IEnumerable<Course>>.Success(courses);
        }

        
    }
}
