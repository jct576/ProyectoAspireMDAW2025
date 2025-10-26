using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ProyectoAspireMDAW2025.Auth.Application;

/// <summary>
/// Configuración de Dependency Injection para Application Layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}

