using System;
using System.ComponentModel.DataAnnotations;

namespace StudentSync.Data.Models
{
    public partial class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course Name is required.")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        public string Duration { get; set; }

        [Required(ErrorMessage = "Prerequisite is required.")]
        public string PreRequisite { get; set; }

        [Required(ErrorMessage = "Remarks are required.")]
        public string Remarks { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
