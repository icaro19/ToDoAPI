using Application.DependencyInjection;
using Infrastructure.DependencyInjection;
using Infrastructure.Security.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Filters;

namespace WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });

            services.AddScoped<ValidationFilter>();

            services.AddOpenApi();

            ConfigureAuthentication(
                services,
                configuration);

            services.AddAuthorization();

            services.AddApplication();

            services.AddInfrastructure(
                configuration);

            return services;
        }

        private static void ConfigureAuthentication(
            IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings =
                configuration
                    .GetSection(JwtSettings.SectionName)
                    .Get<JwtSettings>()
                ?? throw new InvalidOperationException(
                    "Configurações JWT não encontradas.");

            var key =
                Encoding.UTF8.GetBytes(
                    jwtSettings.SecretKey);

            services
                .AddAuthentication(
                    JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,

                            ValidIssuer =
                                jwtSettings.Issuer,

                            ValidAudience =
                                jwtSettings.Audience,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    key),

                            ClockSkew =
                                TimeSpan.Zero
                        };
                });
        }
    }
}
