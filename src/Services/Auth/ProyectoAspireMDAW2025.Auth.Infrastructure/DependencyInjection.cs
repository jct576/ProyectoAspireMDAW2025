using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Auth.Domain.Entities;
using ProyectoAspireMDAW2025.Auth.Infrastructure.Data;
using ProyectoAspireMDAW2025.Auth.Infrastructure.Messaging;
using ProyectoAspireMDAW2025.Auth.Infrastructure.Repositories;
using ProyectoAspireMDAW2025.Auth.Infrastructure.Services;
using ProyectoAspireMDAW2025.Messaging.Abstractions;
using System.Text;

namespace ProyectoAspireMDAW2025.Auth.Infrastructure;

/// <summary>
/// Configuración de Dependency Injection para Infrastructure Layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext con SQL Server
        var connectionString = configuration.GetConnectionString("authdb")
            ?? throw new InvalidOperationException("Connection string 'authdb' not found");

        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });

        // ASP.NET Core Identity
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.RequireUniqueEmail = true;

            // SignIn settings
            options.SignIn.RequireConfirmedEmail = false; // TODO: Cambiar a true cuando se implemente email verification
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();

        // JWT Authentication
        var jwtSecret = configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException("JWT Secret not configured");
        var jwtIssuer = configuration["Jwt:Issuer"] ?? "https://localhost:5000";
        var jwtAudience = configuration["Jwt:Audience"] ?? "https://localhost:5000";

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ClockSkew = TimeSpan.Zero // Eliminar el delay de 5 minutos por defecto
            };
        });

        // Repositories
        services.AddScoped<IAuthRepository, AuthRepository>();

        // Services
        services.AddScoped<ITokenService, TokenService>();

        // MassTransit con RabbitMQ
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqHost = configuration["RabbitMQ:Host"] ?? "localhost";
                var rabbitMqPort = int.Parse(configuration["RabbitMQ:Port"] ?? "5672");
                var rabbitMqUser = configuration["RabbitMQ:Username"] ?? "guest";
                var rabbitMqPassword = configuration["RabbitMQ:Password"] ?? "guest";

                cfg.Host(rabbitMqHost, (ushort)rabbitMqPort, "/", h =>
                {
                    h.Username(rabbitMqUser);
                    h.Password(rabbitMqPassword);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        // Event Publisher
        services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();

        return services;
    }
}

