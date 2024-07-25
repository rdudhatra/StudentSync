using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Core.Services.Interface
{
    public interface IBatchService
    {
        Task<List<BatchResponseModel>> GetAllBatchesAsync();
        Task<Batch> GetBatchByIdAsync(int id);
        Task CreateBatchAsync(Batch batch);
        Task UpdateBatchAsync(Batch batch);
        Task DeleteBatchAsync(int id);
        List<Batch> GetAllBatchesIdsAsync();
        Task<int> GetTotalBatchesAsync();


    }
}
