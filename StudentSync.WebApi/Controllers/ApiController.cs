using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSync.Core.Services.Interface;

namespace StudentSync.WebApi.Controllers
{
    [Route("api/ApiController")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICourseServices _courseService;
        private readonly IInquiryService _inquiryService;
        private readonly IBatchService _batchService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly ICourseFeeService _courseFeeService;
        private readonly IStudentAssessmentService _studentAssessmentService;
        private readonly ICourseExamServices _courseExamService;
        private readonly IStudentAttendanceService _studentAttendanceService;

        public ApiController(IEmployeeService employeeService, ICourseServices courseService, IInquiryService inquiryService, IBatchService batchService,
            IEnrollmentService enrollmentService, ICourseFeeService courseFeeService, IStudentAssessmentService studentAssessmentService,
            ICourseExamServices courseExamService, IStudentAttendanceService studentAttendanceService)
        {
            _employeeService = employeeService;
            _courseService = courseService;
            _inquiryService = inquiryService;
            _batchService = batchService;
            _enrollmentService = enrollmentService;
            _courseFeeService = courseFeeService;
            _studentAssessmentService = studentAssessmentService;
            _courseExamService = courseExamService;
            _studentAttendanceService = studentAttendanceService;
        }

        [HttpGet("total-employees")]
        public async Task<IActionResult> GetTotalEmployees()
        {
            var totalEmployees = await _employeeService.GetTotalEmployeesAsync();
            return Ok(totalEmployees);
        }

        [HttpGet("total-courses")]
        public async Task<IActionResult> GetTotalCourses()
        {
            var totalCourses = await _courseService.GetTotalCoursesAsync();
            return Ok(totalCourses);
        }

        [HttpGet("total-inquiries")]
        public async Task<IActionResult> GetTotalInquiries()
        {
            var totalInquiries = await _inquiryService.GetTotalInquiriesAsync();
            return Ok(totalInquiries);
        }

        [HttpGet("total-batches")]
        public async Task<IActionResult> GetTotalBatches()
        {
            var totalBatches = await _batchService.GetTotalBatchesAsync();
            return Ok(totalBatches);
        }

        [HttpGet("total-enrollments")]
        public async Task<IActionResult> GetTotalEnrollments()
        {
            var totalEnrollments = await _enrollmentService.GetTotalEnrollmentsAsync();
            return Ok(totalEnrollments);
        }

        [HttpGet("total-course-fees")]
        public async Task<IActionResult> GetTotalCourseFees()
        {
            var totalCourseFees = await _courseFeeService.GetTotalCourseFeesAsync();
            return Ok(totalCourseFees);
        }

        [HttpGet("total-student-assessments")]
        public async Task<IActionResult> GetTotalStudentAssessments()
        {
            var totalStudentAssessments = await _studentAssessmentService.GetTotalStudentAssessmentsAsync();
            return Ok(totalStudentAssessments);
        }

        [HttpGet("total-course-exams")]
        public async Task<IActionResult> GetTotalCourseExams()
        {
            var totalCourseExams = await _courseExamService.GetTotalCourseExamsAsync();
            return Ok(totalCourseExams);
        }

        [HttpGet("total-student-attendance")]
        public async Task<IActionResult> GetTotalStudentAttendance()
        {
            var totalStudentAttendance = await _studentAttendanceService.GetTotalStudentAttendanceAsync();
            return Ok(totalStudentAttendance);
        }
    }
}
