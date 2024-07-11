using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interface;
using StudentSync.Data;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentSync.Core.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly StudentSyncDbContext _context;

        public EnrollmentService(StudentSyncDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Enrollment>> GetAllEnrollments()
        {
            return await _context.Enrollments.ToListAsync();
        }
       
        public async Task<Enrollment> GetEnrollmentById(int id)
        {
            var result = await _context.Enrollments.FromSqlRaw("EXEC GetEnrollmentById @Id = {0}", id).FirstOrDefaultAsync();
            return result;
        }
        //public async Task<StudentInstallment> GetStudentInstallmentByIdAsync(int id)
        //{

        //    var result = await _context.StudentInstallments.FromSqlRaw("EXEC GetStudentInstallmentById @Id = {0}", id).ToListAsync();
        //    return result.Count > 0 ? result[0] : null;
        //}
        public async Task AddEnrollment(Enrollment enrollment)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC CreateEnrollment @EnrollmentNo = {0}, @EnrollmentDate = {1}, @BatchId = {2}, @CourseId = {3}, @CourseFeeId = {4}, @InquiryNo = {5}, @IsActive = {6}, @Remarks = {7}, @CreatedBy = {8}, @CreatedDate = {9}",
                enrollment.EnrollmentNo, enrollment.EnrollmentDate, enrollment.BatchId, enrollment.CourseId, enrollment.CourseFeeId, enrollment.InquiryNo, enrollment.IsActive, enrollment.Remarks, enrollment.CreatedBy, enrollment.CreatedDate);
        }

        public async Task UpdateEnrollment(Enrollment enrollment)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateEnrollment @Id = {0}, @EnrollmentNo = {1}, @EnrollmentDate = {2}, @BatchId = {3}, @CourseId = {4}, @CourseFeeId = {5}, @InquiryNo = {6}, @IsActive = {7}, @Remarks = {8}, @UpdatedBy = {9}, @UpdatedDate = {10}",
                enrollment.Id, enrollment.EnrollmentNo, enrollment.EnrollmentDate, enrollment.BatchId, enrollment.CourseId, enrollment.CourseFeeId, enrollment.InquiryNo, enrollment.IsActive, enrollment.Remarks, enrollment.UpdatedBy, enrollment.UpdatedDate);
        }

        public async Task DeleteEnrollment(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC DeleteEnrollment @Id = {0}", id);
        }
    }
}
