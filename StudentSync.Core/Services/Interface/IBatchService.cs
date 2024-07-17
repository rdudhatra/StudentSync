using StudentSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSync.Core.Services.Interface
{
    public interface IBatchService
    {
        Task<List<Batch>> GetAllBatchesAsync();
        Task<Batch> GetBatchByIdAsync(int id);
        Task CreateBatchAsync(Batch batch);
        Task UpdateBatchAsync(Batch batch);
        Task DeleteBatchAsync(int id);
        List<Batch> GetAllBatchesIdsAsync();

    }
}
