using StudentSync.Core.Services.Interface;
using StudentSync.Core.Wrapper;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace StudentSync.Core.Services
{
    public class CourseServices : ICourseServices
    {
        private readonly StudentSyncDbContext _context;

        public CourseServices(StudentSyncDbContext context)
        {
            _context = context;
        }

        public List<Course> GetAllCourseIds()
        {
            return _context.Courses.ToList();
        }
    

        public async Task<List<Course>> GetAllCourseAsync()
        {
            var courses = await _context.Courses.ToListAsync();
            return courses;
        }


        public async Task<Course> GetCoursesByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
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

        public async Task<int> GetTotalCoursesAsync()
        {
            return await _context.Courses.CountAsync();
        }
    }
}
