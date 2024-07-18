using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSync.Data.Models;

public partial class Batch
{
    [Key]
    public int Id { get; set; }

    public string? BatchCode { get; set; }

    public string? BatchTime { get; set; }

    public int? BatchCourseId { get; set; }

    public string? FacultyName { get; set; }

    public bool? IsActive { get; set; }

    public string? Remarks { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }


}
