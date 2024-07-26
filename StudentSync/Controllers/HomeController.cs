//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Cors.Infrastructure;
//using Microsoft.AspNetCore.Mvc;
//using StudentSync.Core.Services;
//using StudentSync.Core.Services.Interface;
//using StudentSync.Models;
//using System.Diagnostics;

//namespace StudentSync.Controllers
//{
//    [Authorize]

//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;
//        private readonly IEmployeeService _employeeService;
//        private readonly ICourseServices _courseService;
//        private readonly IInquiryService _inquiryService;
//        private readonly IBatchService _batchService;
//        private readonly IEnrollmentService _enrollmentService;
//        private readonly ICourseFeeService _courseFeeService;
//        private readonly IStudentAssessmentService _studentAssessmentService;
//        private readonly ICourseExamServices _courseExamService;
//        private readonly IStudentAttendanceService _studentAttendanceService;

//        public HomeController(ILogger<HomeController> logger , IStudentAttendanceService studentAttendanceService, ICourseExamServices courseExamService,  IStudentAssessmentService studentAssessmentService,IBatchService batchService, 
//            IEmployeeService employeeService, ICourseServices courseService, IInquiryService inquiryService , IEnrollmentService enrollmentService , ICourseFeeService courseFeeService)
//        {
//            _logger = logger;
//            _employeeService = employeeService;
//            _courseService = courseService;
//            _inquiryService = inquiryService;
//            _batchService = batchService;
//            _enrollmentService = enrollmentService;
//            _courseFeeService = courseFeeService;
//            _studentAssessmentService = studentAssessmentService;
//            _courseExamService = courseExamService;
//            _studentAttendanceService = studentAttendanceService;   
//        }

//        //public IActionResult Index()
//        //{
//        //    return View();
//        //}
//        public async Task<IActionResult> Index()
//        {
//            var totalEmployees = await _employeeService.GetTotalEmployeesAsync();
//            var totalCourses = await _courseService.GetTotalCoursesAsync();
//            var totalInquiries = await _inquiryService.GetTotalInquiriesAsync();
//            var totalBatches = await _batchService.GetTotalBatchesAsync();
//            var totalEnrollments = await _enrollmentService.GetTotalEnrollmentsAsync();
//            var totalCourseFees = await _courseFeeService.GetTotalCourseFeesAsync();
//            var totalStudentAssessments = await _studentAssessmentService.GetTotalStudentAssessmentsAsync();
//            var totalCourseExams = await _courseExamService.GetTotalCourseExamsAsync();
//            var totalStudentAttendance = await _studentAttendanceService.GetTotalStudentAttendanceAsync();



//            ViewBag.TotalEmployees = totalEmployees;
//            ViewBag.TotalCourses = totalCourses;
//            ViewBag.TotalInquiries = totalInquiries;
//            ViewBag.TotalBatches = totalBatches;
//            ViewBag.TotalEnrollments = totalEnrollments;
//            ViewBag.TotalCourseFees = totalCourseFees;
//            ViewBag.TotalStudentAssessments = totalStudentAssessments;
//            ViewBag.TotalCourseExams = totalCourseExams;
//            ViewBag.TotalStudentAttendance = totalStudentAttendance;


//            return View();
//        }
//        public IActionResult Privacy()
//        {
//            return View();

//        }


//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error() 
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}


//using Microsoft.AspNetCore.Mvc;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using StudentSync.Models;
//using System.Diagnostics;
//using Microsoft.AspNetCore.Authorization; // Ensure you have this NuGet package installed

//namespace StudentSync.Controllers
//{
//    [Authorize]
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;
//        private readonly HttpClient _httpClient;

//        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
//        {
//            _logger = logger;
//            _httpClient = httpClient;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var totalEmployees = await GetApiDataAsync("ApiController/total-employees");
//            var totalCourses = await GetApiDataAsync("ApiController/total-courses");
//            var totalInquiries = await GetApiDataAsync("ApiController/total-inquiries");
//            var totalBatches = await GetApiDataAsync("ApiController/total-batches");
//            var totalEnrollments = await GetApiDataAsync("ApiController/total-enrollments");
//            var totalCourseFees = await GetApiDataAsync("ApiController/total-course-fees");
//            var totalStudentAssessments = await GetApiDataAsync("ApiController/total-student-assessments");
//            var totalCourseExams = await GetApiDataAsync("ApiController/total-course-exams");
//            var totalStudentAttendance = await GetApiDataAsync("ApiController/total-student-attendance");

//            ViewBag.TotalEmployees = totalEmployees;
//            ViewBag.TotalCourses = totalCourses;
//            ViewBag.TotalInquiries = totalInquiries;
//            ViewBag.TotalBatches = totalBatches;
//            ViewBag.TotalEnrollments = totalEnrollments;
//            ViewBag.TotalCourseFees = totalCourseFees;
//            ViewBag.TotalStudentAssessments = totalStudentAssessments;
//            ViewBag.TotalCourseExams = totalCourseExams;
//            ViewBag.TotalStudentAttendance = totalStudentAttendance;

//            return View();
//        }

//        private async Task<int> GetApiDataAsync(string url)
//        {
//            HttpResponseMessage response = await _httpClient.GetAsync(url);
//            response.EnsureSuccessStatusCode();
//            var jsonResponse = await response.Content.ReadAsStringAsync();
//            return JsonConvert.DeserializeObject<int>(jsonResponse); // Adjust according to the actual data type returned
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StudentSync.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace StudentSync.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var totalEmployees = await GetApiDataAsync("ApiController/total-employees");
            var totalCourses = await GetApiDataAsync("ApiController/total-courses");
            var totalInquiries = await GetApiDataAsync("ApiController/total-inquiries");
            var totalBatches = await GetApiDataAsync("ApiController/total-batches");
            var totalEnrollments = await GetApiDataAsync("ApiController/total-enrollments");
            var totalCourseFees = await GetApiDataAsync("ApiController/total-course-fees");
            var totalStudentAssessments = await GetApiDataAsync("ApiController/total-student-assessments");
            var totalCourseExams = await GetApiDataAsync("ApiController/total-course-exams");
            var totalStudentAttendance = await GetApiDataAsync("ApiController/total-student-attendance");

            var analyticsData = new
            {
                totalEmployees,
                totalCourses,
                totalInquiries,
                totalBatches,
                totalEnrollments,
                totalCourseFees,
                totalStudentAssessments,
                totalCourseExams,
                totalStudentAttendance
            };

            ViewBag.AnalyticsData = JsonConvert.SerializeObject(analyticsData);
            ViewBag.TotalEmployees = totalEmployees;
            ViewBag.TotalCourses = totalCourses;
            ViewBag.TotalInquiries = totalInquiries;
            ViewBag.TotalBatches = totalBatches;
            ViewBag.TotalEnrollments = totalEnrollments;
            ViewBag.TotalCourseFees = totalCourseFees;
            ViewBag.TotalStudentAssessments = totalStudentAssessments;
            ViewBag.TotalCourseExams = totalCourseExams;
            ViewBag.TotalStudentAttendance = totalStudentAttendance;

            return View();
        }

        private async Task<int> GetApiDataAsync(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(jsonResponse);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

