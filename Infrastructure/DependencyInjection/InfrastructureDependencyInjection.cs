using Application.Abstractions.Security;
using Domain.Interface;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.Security.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<TodoDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString(
                        "DefaultConnection"));
            });

            services.AddScoped<
                IUserRepository,
                UserRepository>();

            services.AddScoped<
                ITaskRepository,
                TaskRepository>();

            services.AddScoped<
                IAuthRepository,
                AuthRepository>();

            services.AddScoped<
                IPasswordHasher,
                BCryptPasswordHasher>();

            services.Configure<
                JwtSettings>(
                configuration.GetSection(
                JwtSettings.SectionName));

            services.AddScoped<
                IJwtTokenGenerator,
                JwtTokenGenerator>();

            return services;
        }
    }
}
