
using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using StudentSync.Data.ResponseModel;
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
        public async Task<int> GetTotalBatchesAsync()
        {
            return await _context.Batches.CountAsync();
        }
        public List<Batch> GetAllBatchesIdsAsync()
        {
            return _context.Batches.ToList();
        }


        public async Task<List<BatchResponseModel>> GetAllBatchesAsync()
        {
            var batches = await _context.Batches
                .Join(_context.Courses,
                      batch => batch.BatchCourseId,
                      course => course.CourseId,
                      (batch, course) => new BatchResponseModel
                      {
                          Id = batch.Id,
                          BatchCode = batch.BatchCode,
                          BatchTime = batch.BatchTime,
                          BatchCourseId = batch.BatchCourseId,
                          CourseName = course.CourseName,
                          FacultyName = batch.FacultyName,
                          IsActive = batch.IsActive ?? false,
                          Remarks = batch.Remarks,
                          CreatedBy = batch.CreatedBy,
                          CreatedDate = batch.CreatedDate ?? default,
                          UpdatedBy = batch.UpdatedBy,
                          UpdatedDate = batch.UpdatedDate ?? default
                      })
                .ToListAsync();

            return batches;
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
