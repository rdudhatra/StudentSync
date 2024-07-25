using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Core.Services.Interface
{
    public interface IInquiryService
    {
        Task<IList<InquiryResponseModel>> GetAllInquiriesAsync();
        Task<Inquiry> GetInquiryByIdAsync(int id);
        Task AddInquiryAsync(Inquiry inquiry);
        Task UpdateInquiryAsync(Inquiry inquiry);
        Task DeleteInquiryAsync(int id);
        List<Inquiry> GetAllInquiryno();
        Task<int> GetTotalInquiriesAsync();

    }
}
