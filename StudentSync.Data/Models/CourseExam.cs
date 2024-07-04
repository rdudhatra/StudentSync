using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSync.Data.Models;

public partial class CourseExam
{
    [Key]

    public int Id { get; set; }

    public int? CourseId { get; set; }

    public string? ExamTitle { get; set; }

    public string? ExamType { get; set; }

    public int? TotalMarks { get; set; }

    public int? PassingMarks { get; set; }

    public string? Remarks { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
