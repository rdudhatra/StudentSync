using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Core.Services.Interface
{
    public interface IStudentAttendanceService
    {
        Task<IEnumerable<StudentAttendanceResponseModel>> GetAllStudentAttendances();
        Task<StudentAttendance> GetStudentAttendanceById(int id);
        Task AddStudentAttendance(StudentAttendance studentAttendance);
        Task UpdateStudentAttendance(StudentAttendance studentAttendance);
        Task DeleteStudentAttendance(int id);
        Task<int> GetTotalStudentAttendanceAsync();

    }
}
