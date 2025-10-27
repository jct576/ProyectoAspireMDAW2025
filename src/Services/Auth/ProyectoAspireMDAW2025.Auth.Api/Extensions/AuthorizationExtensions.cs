using Microsoft.AspNetCore.Authorization;
using ProyectoAspireMDAW2025.Auth.Api.Authorization;

namespace ProyectoAspireMDAW2025.Auth.Api.Extensions;

/// <summary>
/// Extensiones para configurar autorización basada en permisos
/// </summary>
public static class AuthorizationExtensions
{
    /// <summary>
    /// Agrega servicios de autorización basada en permisos
    /// </summary>
    public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services)
    {
        // Registrar el handler de autorización por permisos
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        // Configurar políticas de autorización
        services.AddAuthorization(options =>
        {
            // Aquí se pueden agregar políticas predefinidas si es necesario
            // Por ahora, las políticas se crean dinámicamente en RequirePermissionAttribute
        });

        return services;
    }
}

