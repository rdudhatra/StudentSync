using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSync.Data.Models;

public partial class StudentAttendance
{
    [Key]

    public int Id { get; set; }

    public DateTime? AttendanceDate { get; set; }

    public string? EnrollmentNo { get; set; }

    public int? BatchId { get; set; }

    public string? Remarks { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}


