using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(
            DbContextOptions<TodoDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<UserCredential> UserCredentials => Set<UserCredential>();

        public DbSet<TodoTask> Tasks => Set<TodoTask>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(TodoDbContext).Assembly);
        }
    }
}
