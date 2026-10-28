using Domain.Entities;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class UserRepository
    : IUserRepository
    {
        private readonly TodoDbContext _context;

        public UserRepository(
            TodoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(
            Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Id == id);
        }

        public async Task<IEnumerable<User>>
            GetAllAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateAsync(
            User user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(
            Guid id)
        {
            var user =
                await _context.Users.FindAsync(id);

            if (user is null)
                return;

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Email == email);
        }
    }
}
