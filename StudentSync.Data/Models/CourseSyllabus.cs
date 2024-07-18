using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentSync.Data.Models;

public partial class CourseSyllabus
{
    [Key]

    public int Id { get; set; }

    public int? CourseId { get; set; }


    public string? ChapterName { get; set; }

    public string? TopicName { get; set; }

    public string? Remarks { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
