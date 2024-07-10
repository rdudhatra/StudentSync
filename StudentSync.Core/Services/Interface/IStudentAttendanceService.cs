using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Core.Services.Interface
{
    public interface IStudentAttendanceService
    {
        Task<IList<StudentAttendance>> GetAllStudentAttendances();
        Task<StudentAttendance> GetStudentAttendanceById(int id);
        Task AddStudentAttendance(StudentAttendance studentAttendance);
        Task UpdateStudentAttendance(StudentAttendance studentAttendance);
        Task DeleteStudentAttendance(int id);
    }
}
