using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentSync.Core.Services
{
    public class InquiryFollowUpService : IInquiryFollowUpService
    {
        private readonly StudentSyncDbContext _context;

        public InquiryFollowUpService(StudentSyncDbContext context)
        {
            _context = context;
        }

        public async Task<IList<InquiryFollowUp>> GetAllInquiryFollowUpsAsync()
        {
            return await _context.InquiryFollowUps.ToListAsync();
        }

        public async Task<InquiryFollowUp> GetInquiryFollowUpByIdAsync(int id)
        {
            var result = await _context.InquiryFollowUps.FromSqlRaw("EXEC GetInquiryFollowUpById @Id = {0}", id).ToListAsync();
            return result.Count > 0 ? result[0] : null;
        }

        public async Task AddInquiryFollowUpAsync(InquiryFollowUp inquiryFollowUp)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC CreateInquiryFollowUp " +
                "@InquiryDate = {0}, @InquiryNo = {1}, @Through = {2}, @Remarks = {3}, @CreatedBy = {4}, @CreatedDate = {5}, @UpdatedBy = {6}, @UpdatedDate = {7}",
                inquiryFollowUp.InquiryDate, inquiryFollowUp.InquiryNo, inquiryFollowUp.Through, inquiryFollowUp.Remarks,
                inquiryFollowUp.CreatedBy, inquiryFollowUp.CreatedDate, inquiryFollowUp.UpdatedBy, inquiryFollowUp.UpdatedDate);
        }

        public async Task UpdateInquiryFollowUpAsync(InquiryFollowUp inquiryFollowUp)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateInquiryFollowUp " +
                "@Id = {0}, @InquiryDate = {1}, @InquiryNo = {2}, @Through = {3}, @Remarks = {4}, @UpdatedBy = {5}, @UpdatedDate = {6}",
                inquiryFollowUp.Id, inquiryFollowUp.InquiryDate, inquiryFollowUp.InquiryNo, inquiryFollowUp.Through,
                inquiryFollowUp.Remarks, inquiryFollowUp.UpdatedBy, inquiryFollowUp.UpdatedDate);
        }

        public async Task DeleteInquiryFollowUpAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC DeleteInquiryFollowUp @Id = {0}", id);
        }
    }
}
