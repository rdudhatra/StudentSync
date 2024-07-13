using Microsoft.EntityFrameworkCore;
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
    public class BatchService : IBatchService
    {
        private readonly StudentSyncDbContext _context;

        public BatchService(StudentSyncDbContext context)
        {
            _context = context;
        }

        public List<Batch> GetAllBatchesIds()
        {
            return _context.Batches.ToList();
        }

        public async Task<List<Batch>> GetAllBatchesAsync()
        {
            return await _context.Batches.ToListAsync();
        }

        public async Task<Batch> GetBatchByIdAsync(int id)
        {
            return await _context.Batches.FindAsync(id);
        }

        public async Task CreateBatchAsync(Batch batch)
        {
            await _context.Batches.AddAsync(batch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBatchAsync(Batch batch)
        {
            _context.Batches.Update(batch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBatchAsync(int id)
        {
            var batch = await _context.Batches.FindAsync(id);
            if (batch != null)
            {
                _context.Batches.Remove(batch);
                await _context.SaveChangesAsync();
            }
        }
    }
}
