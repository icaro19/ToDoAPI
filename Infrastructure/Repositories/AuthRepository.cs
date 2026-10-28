using Domain.Entities;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly TodoDbContext _context;

        public AuthRepository(
            TodoDbContext context)
        {
            _context = context;
        }

        public async Task<UserCredential?>
            GetByUsernameAsync(
                string username)
        {
            return await _context.UserCredentials
                .Include(x => x.User)
                .FirstOrDefaultAsync(
                    x => x.Username == username);
        }

        public async Task AddCredentialAsync(
            UserCredential credential)
        {
            await _context.UserCredentials
                .AddAsync(credential);

            await _context.SaveChangesAsync();
        }

        public async Task<UserCredential?> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserCredentials
                .FirstOrDefaultAsync(
                    x => x.UserId == userId);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.UserCredentials
                .AnyAsync(
                    x => x.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(
                    x => x.Email == email);
        }
    }
}
