using StudentSync.Core.Services.Interface;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Core.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly StudentSyncDbContext _context;

        public EnrollmentService(StudentSyncDbContext context)
        {
            _context = context;
        }

        public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _context.GetAllEnrollmentsAsync();
        }

        public async Task<Enrollment> GetEnrollmentByIdAsync(string enrollmentNo)
        {
            return await _context.GetEnrollmentByIdAsync(enrollmentNo);
        }

        public async Task CreateEnrollmentAsync(Enrollment enrollment)
        {
            await _context.CreateEnrollmentAsync(enrollment);
        }

        public async Task UpdateEnrollmentAsync(Enrollment enrollment)
        {
            await _context.UpdateEnrollmentAsync(enrollment);
        }

        public async Task DeleteEnrollmentAsync(string enrollmentNo)
        {
            await _context.DeleteEnrollmentAsync(enrollmentNo);
        }
    }
}
