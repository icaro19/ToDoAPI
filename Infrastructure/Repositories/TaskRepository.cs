using Domain.Entities;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TodoDbContext _context;

        public TaskRepository(
            TodoDbContext context)
        {
            _context = context;
        }

        public async Task<TodoTask?> GetByIdAsync(
            Guid id)
        {
            return await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TodoTask>>
            GetAllAsync()
        {
            return await _context.Tasks
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TodoTask>>
            GetByStatusAsync(TaskStatus status)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(
            TodoTask task)
        {
            await _context.Tasks.AddAsync(task);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(
            TodoTask task)
        {
            _context.Tasks.Update(task);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(
            Guid id)
        {
            var task = await _context.Tasks
                .FindAsync(id);

            if (task is null)
                return;

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<TodoTask>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}
