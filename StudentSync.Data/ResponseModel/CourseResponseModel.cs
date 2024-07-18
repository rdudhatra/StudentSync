namespace StudentSync.Data.ResponseModel
{
    public class CourseResponseModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Duration { get; set; }
        public string PreRequisite { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
