using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Data.ResponseModel
{
    public class InquiryResponseModel
    {
        public int InquiryNo { get; set; }
        public DateTime? InquiryDate { get; set; }
        public string? Title { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string? ContactNo { get; set; }
        public string? EmailId { get; set; }
        public DateTime? Dob { get; set; }
        public string? Address { get; set; }
        public string? Reference { get; set; }
        public bool? Job { get; set; }
        public bool? Business { get; set; }
        public bool? Study { get; set; }
        public string? Other { get; set; }
        public bool? PrevCompCourse { get; set; }
        public string? PrevCompCourseDetails { get; set; }
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? Note { get; set; }
        public string? EnquiryType { get; set; }
        public string Status { get; set; } = null!;
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
