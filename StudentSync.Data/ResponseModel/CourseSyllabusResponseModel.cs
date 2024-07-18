using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Data.ResponseModel
{
    public class CourseSyllabusResponseModel
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public string? CourseName { get; set; } 

        public string? ChapterName { get; set; }
        public string? TopicName { get; set; }
        public string? Remarks { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
