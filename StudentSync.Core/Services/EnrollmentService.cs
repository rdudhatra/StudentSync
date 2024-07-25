using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interface;
using StudentSync.Data;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
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


        public List<Enrollment> GetAllEnrollMentno()
        {
            return _context.Enrollments.ToList();
        }

        public async Task<int> GetTotalEnrollmentsAsync()
        {
            return await _context.Enrollments.CountAsync();
        }

        public async Task<IEnumerable<EnrollmentResponseModel>> GetAllEnrollments()
        {
            var enrollments = await (from enrollment in _context.Enrollments
                                     join course in _context.Courses on enrollment.CourseId equals course.CourseId into courseJoin
                                     from course in courseJoin.DefaultIfEmpty()
                                     select new EnrollmentResponseModel
                                     {
                                         Id = enrollment.Id,
                                         EnrollmentNo = enrollment.EnrollmentNo,
                                         EnrollmentDate = enrollment.EnrollmentDate,
                                         BatchId = enrollment.BatchId,
                                         CourseId = enrollment.CourseId,
                                         CourseName = course.CourseName, 
                                         CourseFeeId = enrollment.CourseFeeId,
                                         InquiryNo = enrollment.InquiryNo,
                                         IsActive = enrollment.IsActive,
                                         Remarks = enrollment.Remarks,
                                         CreatedBy = enrollment.CreatedBy,
                                         CreatedDate = enrollment.CreatedDate,
                                         UpdatedBy = enrollment.UpdatedBy,
                                         UpdatedDate = enrollment.UpdatedDate
                                     })
                                                 .ToListAsync();

            return enrollments;
        }

       
        public async Task<Enrollment> GetEnrollmentById(int id)
        {
            var result = await _context.Enrollments.FromSqlRaw("EXEC GetEnrollmentById @Id = {0}", id).ToListAsync();
            return result.Count > 0 ? result[0] : null;
        }
     
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
