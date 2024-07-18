using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSync.Data.Models;

public partial class Enrollment
{
    [Key]
    public int Id { get; set; }

    public string EnrollmentNo { get; set; } = null!;

    public DateTime? EnrollmentDate { get; set; }

    public int? BatchId { get; set; }

    public int? CourseId { get; set; }

    public int? CourseFeeId { get; set; }

    public int? InquiryNo { get; set; }

    public bool? IsActive { get; set; }

    public string? Remarks { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
