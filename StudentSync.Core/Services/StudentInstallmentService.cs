using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interfaces;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace StudentSync.Core.Services
{
    public class StudentInstallmentService : IStudentInstallmentService
    {
        private readonly StudentSyncDbContext _context;

        public StudentInstallmentService(StudentSyncDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentInstallment>> GetAllStudentInstallmentsAsync()
        {
            return await _context.StudentInstallments.ToListAsync();
        }

    
        public async Task<StudentInstallment> GetStudentInstallmentByIdAsync(int id)
        {

            var result =  await _context.StudentInstallments.FromSqlRaw("EXEC GetStudentInstallmentById @Id = {0}", id).ToListAsync();
            return result.Count > 0 ? result[0] : null;
        }
    
        public async Task<int> CreateStudentInstallmentAsync(StudentInstallment studentInstallment)
        {
            var parameters = GetParameters(studentInstallment);
            var result = await _context.Database.ExecuteSqlRawAsync("EXEC CreateStudentInstallment @ReceiptNo, @ReceiptDate, @Amount, @EnrollmentNo, @TransactionMode, @BankName, @Ifsccode, @BranchName, @ChequeTranNo, @Remarks, @CreatedBy, @CreatedDate, @UpdatedBy, @UpdatedDate", parameters);
            return result;
        }

        public async Task<int> UpdateStudentInstallmentAsync(StudentInstallment studentInstallment)
        {
            var parameters = GetParameters(studentInstallment);
            var result = await _context.Database.ExecuteSqlRawAsync("EXEC UpdateStudentInstallment @Id, @ReceiptNo, @ReceiptDate, @Amount, @EnrollmentNo, @TransactionMode, @BankName, @Ifsccode, @BranchName, @ChequeTranNo, @Remarks, @UpdatedBy, @UpdatedDate", parameters);
            return result;
        }

        public async Task DeleteStudentInstallmentAsync(int id)
        {
           
            await _context.Database.ExecuteSqlRawAsync("EXEC DeleteStudentInstallment @Id = {0}", id);
            
        }

        
        private SqlParameter[] GetParameters(StudentInstallment studentInstallment)
        {
            return new[]
            {
                new SqlParameter("@Id", SqlDbType.Int) { Value = studentInstallment.Id },
                new SqlParameter("@ReceiptNo", SqlDbType.NVarChar) { Value = studentInstallment.ReceiptNo ?? (object)DBNull.Value },
                new SqlParameter("@ReceiptDate", SqlDbType.Date) { Value = studentInstallment.ReceiptDate ?? (object)DBNull.Value },
                new SqlParameter("@Amount", SqlDbType.Decimal) { Value = studentInstallment.Amount ?? (object)DBNull.Value },
                new SqlParameter("@EnrollmentNo", SqlDbType.NVarChar) { Value = studentInstallment.EnrollmentNo ?? (object)DBNull.Value },
                new SqlParameter("@TransactionMode", SqlDbType.NVarChar) { Value = studentInstallment.TransactionMode ?? (object)DBNull.Value },
                new SqlParameter("@BankName", SqlDbType.NVarChar) { Value = studentInstallment.BankName ?? (object)DBNull.Value },
                new SqlParameter("@Ifsccode", SqlDbType.NVarChar) { Value = studentInstallment.Ifsccode ?? (object)DBNull.Value },
                new SqlParameter("@BranchName", SqlDbType.NVarChar) { Value = studentInstallment.BranchName ?? (object)DBNull.Value },
                new SqlParameter("@ChequeTranNo", SqlDbType.NVarChar) { Value = studentInstallment.ChequeTranNo ?? (object)DBNull.Value },
                new SqlParameter("@Remarks", SqlDbType.NVarChar) { Value = studentInstallment.Remarks ?? (object)DBNull.Value },
                new SqlParameter("@CreatedBy", SqlDbType.NVarChar) { Value = studentInstallment.CreatedBy ?? (object)DBNull.Value },
                new SqlParameter("@CreatedDate", SqlDbType.DateTime) { Value = studentInstallment.CreatedDate ?? (object)DBNull.Value },
                new SqlParameter("@UpdatedBy", SqlDbType.NVarChar) { Value = studentInstallment.UpdatedBy ?? (object)DBNull.Value },
                new SqlParameter("@UpdatedDate", SqlDbType.DateTime) { Value = studentInstallment.UpdatedDate ?? (object)DBNull.Value }
            };
        }
    }
}
