using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interface;
using StudentSync.Core.Services.Interfaces;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentSync.Core.Services
{
    public class InquiryService : IInquiryService
    {
        private readonly StudentSyncDbContext _context;

        public InquiryService(StudentSyncDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetTotalInquiriesAsync()
        {
            return await _context.Inquiries.CountAsync();
        }


        public List<Inquiry> GetAllInquiryno()
        {
            return _context.Inquiries.ToList();
        }

        public async Task<IList<InquiryResponseModel>> GetAllInquiriesAsync()
        {
            var inquiries = await (from inquiry in _context.Inquiries
                                   join course in _context.Courses on inquiry.CourseId equals course.CourseId into courseJoin
                                   from course in courseJoin.DefaultIfEmpty()
                                   select new InquiryResponseModel
                                   {
                                       InquiryNo = inquiry.InquiryNo,
                                       InquiryDate = inquiry.InquiryDate,
                                       Title = inquiry.Title,
                                       FirstName = inquiry.FirstName,
                                       MiddleName = inquiry.MiddleName,
                                       LastName = inquiry.LastName,
                                       ContactNo = inquiry.ContactNo,
                                       EmailId = inquiry.EmailId,
                                       Dob = inquiry.Dob,
                                       Address = inquiry.Address,
                                       Reference = inquiry.Reference,
                                       Job = inquiry.Job,
                                       Business = inquiry.Business,
                                       Study = inquiry.Study,
                                       Other = inquiry.Other,
                                       PrevCompCourse = inquiry.PrevCompCourse,
                                       PrevCompCourseDetails = inquiry.PrevCompCourseDetails,
                                       CourseId = inquiry.CourseId,
                                       CourseName = course.CourseName, // Include CourseName from Courses table
                                       Note = inquiry.Note,
                                       EnquiryType = inquiry.EnquiryType,
                                       Status = inquiry.Status,
                                       IsActive = inquiry.IsActive,
                                       CreatedBy = inquiry.CreatedBy,
                                       CreatedDate = inquiry.CreatedDate,
                                       UpdatedBy = inquiry.UpdatedBy,
                                       UpdatedDate = inquiry.UpdatedDate
                                   })
                                              .ToListAsync();

            return inquiries;
        }



        public async Task<Inquiry> GetInquiryByIdAsync(int id)
        {
            var result = await _context.Inquiries.FromSqlRaw("EXEC GetInquiryById @InquiryNo = {0}", id).ToListAsync();
            return result.Count > 0 ? result[0] : null;
        }

        public async Task AddInquiryAsync(Inquiry inquiry)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC CreateInquiry @InquiryDate = {0}, @Title = {1}, @FirstName = {2}, @MiddleName = {3}, @LastName = {4}, @ContactNo = {5}, @EmailId = {6}, @Dob = {7}, @Address = {8}, @Reference = {9}, @Job = {10}, @Business = {11}, @Study = {12}, @Other = {13}, @PrevCompCourse = {14}, @PrevCompCourseDetails = {15}, @CourseId = {16}, @Note = {17}, @EnquiryType = {18}, @Status = {19}, @IsActive = {20}",
                inquiry.InquiryDate, inquiry.Title, inquiry.FirstName, inquiry.MiddleName, inquiry.LastName, inquiry.ContactNo, inquiry.EmailId, inquiry.Dob, inquiry.Address, inquiry.Reference, inquiry.Job, inquiry.Business, inquiry.Study, inquiry.Other, inquiry.PrevCompCourse, inquiry.PrevCompCourseDetails, inquiry.CourseId, inquiry.Note, inquiry.EnquiryType, inquiry.Status, inquiry.IsActive);
        }

        public async Task UpdateInquiryAsync(Inquiry inquiry)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateInquiry @InquiryNo = {0}, @InquiryDate = {1}, @Title = {2}, @FirstName = {3}, @MiddleName = {4}, @LastName = {5}, @ContactNo = {6}, @EmailId = {7}, @Dob = {8}, @Address = {9}, @Reference = {10}, @Job = {11}, @Business = {12}, @Study = {13}, @Other = {14}, @PrevCompCourse = {15}, @PrevCompCourseDetails = {16}, @CourseId = {17}, @Note = {18}, @EnquiryType = {19}, @Status = {20}, @IsActive = {21}",
                inquiry.InquiryNo, inquiry.InquiryDate, inquiry.Title, inquiry.FirstName, inquiry.MiddleName, inquiry.LastName, inquiry.ContactNo, inquiry.EmailId, inquiry.Dob, inquiry.Address, inquiry.Reference, inquiry.Job, inquiry.Business, inquiry.Study, inquiry.Other, inquiry.PrevCompCourse, inquiry.PrevCompCourseDetails, inquiry.CourseId, inquiry.Note, inquiry.EnquiryType, inquiry.Status, inquiry.IsActive);
        }

        public async Task DeleteInquiryAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC DeleteInquiry @InquiryNo = {0}", id);
        }
    }
}
