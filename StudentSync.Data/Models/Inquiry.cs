using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSync.Data.Models;

public partial class Inquiry
{
    [Key]
    public int InquiryNo { get; set; }

    public DateTime? InquiryDate { get; set; }

    public string? Title { get; set; }

    [Required]
    public string FirstName { get; set; }

    public string? MiddleName { get; set; }

    [Required]
    public string LastName { get; set; }

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


    public string? Note { get; set; }

    public string? EnquiryType { get; set; }

    public string Status { get; set; } = null!;

    public bool? IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
