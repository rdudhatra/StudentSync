using StudentSync.Data.Models;

namespace StudentSync.Controllers
{
    public class EmployeeApiResponse
    {
        public int Draw { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
        public List<Employee> Data { get; set; }
    }

}
 