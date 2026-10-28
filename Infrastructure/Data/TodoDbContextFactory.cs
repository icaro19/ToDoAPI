using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class TodoDbContextFactory
    : IDesignTimeDbContextFactory<TodoDbContext>
    {
        public TodoDbContext CreateDbContext(
            string[] args)
        {
            var optionsBuilder =
                new DbContextOptionsBuilder<TodoDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5434;Database=todo_db;Username=postgres;Password=i9o9i998");

            return new TodoDbContext(
                optionsBuilder.Options);
        }
    }
}
