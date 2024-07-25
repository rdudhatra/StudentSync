using Microsoft.EntityFrameworkCore;
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
    public class StudentAttendanceService : IStudentAttendanceService
    {
        private readonly StudentSyncDbContext _context;

        public StudentAttendanceService(StudentSyncDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<StudentAttendanceResponseModel>> GetAllStudentAttendances()
        {
            var attendances = await (from attendance in _context.StudentAttendances
                                     join course in _context.Courses on attendance.BatchId equals course.CourseId into courseJoin
                                     from course in courseJoin.DefaultIfEmpty()
                                     select new StudentAttendanceResponseModel
                                     {
                                         Id = attendance.Id,
                                         AttendanceDate = attendance.AttendanceDate,
                                         EnrollmentNo = attendance.EnrollmentNo,
                                         BatchId = attendance.BatchId,
                                         CourseName = course.CourseName,// Include the Course name
                                         Remarks = attendance.Remarks,
                                         CreatedBy = attendance.CreatedBy,
                                         CreatedDate = attendance.CreatedDate,
                                         UpdatedBy = attendance.UpdatedBy,
                                         UpdatedDate = attendance.UpdatedDate
                                     }).ToListAsync();

            return attendances;
        }
        public async Task<int> GetTotalStudentAttendanceAsync()
        {
            return await _context.StudentAttendances.CountAsync(); 
        }
        public async Task<StudentAttendance> GetStudentAttendanceById(int id)
        {
            var result = await _context.StudentAttendances.FromSqlRaw("EXEC GetStudentAttendanceById @Id = {0}", id).ToListAsync();
            return result.Count > 0 ? result[0] : null;
        }

        public async Task AddStudentAttendance(StudentAttendance studentAttendance)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC CreateStudentAttendance @AttendanceDate = {0}, @EnrollmentNo = {1}, @BatchId = {2}, @Remarks = {3}, @CreatedBy = {4}, @CreatedDate = {5}",
                studentAttendance.AttendanceDate, studentAttendance.EnrollmentNo, studentAttendance.BatchId, studentAttendance.Remarks, studentAttendance.CreatedBy, studentAttendance.CreatedDate);
        }

        public async Task UpdateStudentAttendance(StudentAttendance studentAttendance)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateStudentAttendance @Id = {0}, @AttendanceDate = {1}, @EnrollmentNo = {2}, @BatchId = {3}, @Remarks = {4}, @UpdatedBy = {5}, @UpdatedDate = {6}",
                studentAttendance.Id, studentAttendance.AttendanceDate, studentAttendance.EnrollmentNo, studentAttendance.BatchId, studentAttendance.Remarks, studentAttendance.UpdatedBy, studentAttendance.UpdatedDate);
        }

        public async Task DeleteStudentAttendance(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC DeleteStudentAttendance @Id = {0}", id);
        }
    }
}
