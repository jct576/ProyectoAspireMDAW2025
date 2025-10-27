using Microsoft.AspNetCore.Authorization;
using ProyectoAspireMDAW2025.Common.Extensions;

namespace ProyectoAspireMDAW2025.Auth.Api.Authorization;

/// <summary>
/// Handler para autorización basada en permisos
/// Evalúa si el usuario tiene los permisos requeridos en sus claims
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly ILogger<PermissionAuthorizationHandler> _logger;

    public PermissionAuthorizationHandler(ILogger<PermissionAuthorizationHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        try
        {
            // Verificar si el usuario está autenticado
            if (context.User?.Identity?.IsAuthenticated != true)
            {
                _logger.LogWarning("User is not authenticated");
                return Task.CompletedTask;
            }

            // Obtener permisos del usuario desde los claims
            var userPermissions = context.User.GetPermissions();

            if (userPermissions == null || !userPermissions.Any())
            {
                _logger.LogWarning("User {UserId} has no permissions", context.User.GetUserId());
                return Task.CompletedTask;
            }

            // Verificar si el usuario es Admin (tiene todos los permisos)
            if (context.User.IsAdmin())
            {
                _logger.LogInformation("User {UserId} is Admin, granting access", context.User.GetUserId());
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Verificar permisos según el modo (RequireAll o RequireAny)
            bool hasPermission;

            if (requirement.RequireAll)
            {
                // El usuario debe tener TODOS los permisos requeridos
                hasPermission = requirement.Permissions.All(p => 
                    userPermissions.Contains(p, StringComparer.OrdinalIgnoreCase));

                if (hasPermission)
                {
                    _logger.LogInformation(
                        "User {UserId} has all required permissions: {Permissions}",
                        context.User.GetUserId(),
                        string.Join(", ", requirement.Permissions));
                    context.Succeed(requirement);
                }
                else
                {
                    _logger.LogWarning(
                        "User {UserId} does not have all required permissions. Required: {Required}, Has: {Has}",
                        context.User.GetUserId(),
                        string.Join(", ", requirement.Permissions),
                        string.Join(", ", userPermissions));
                }
            }
            else
            {
                // El usuario debe tener AL MENOS UNO de los permisos requeridos
                hasPermission = requirement.Permissions.Any(p => 
                    userPermissions.Contains(p, StringComparer.OrdinalIgnoreCase));

                if (hasPermission)
                {
                    _logger.LogInformation(
                        "User {UserId} has at least one required permission: {Permissions}",
                        context.User.GetUserId(),
                        string.Join(", ", requirement.Permissions));
                    context.Succeed(requirement);
                }
                else
                {
                    _logger.LogWarning(
                        "User {UserId} does not have any required permissions. Required: {Required}, Has: {Has}",
                        context.User.GetUserId(),
                        string.Join(", ", requirement.Permissions),
                        string.Join(", ", userPermissions));
                }
            }

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error evaluating permission requirement for user {UserId}", 
                context.User?.GetUserId());
            return Task.CompletedTask;
        }
    }
}

/// <summary>
/// Requirement para autorización basada en permisos
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Permisos requeridos
    /// </summary>
    public string[] Permissions { get; }

    /// <summary>
    /// Si es true, el usuario debe tener TODOS los permisos
    /// Si es false, el usuario debe tener AL MENOS UNO
    /// </summary>
    public bool RequireAll { get; }

    public PermissionRequirement(string[] permissions, bool requireAll = false)
    {
        Permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
        RequireAll = requireAll;

        if (permissions.Length == 0)
        {
            throw new ArgumentException("At least one permission must be specified", nameof(permissions));
        }
    }
}

