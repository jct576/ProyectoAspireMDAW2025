using System.Security.Claims;
using System.Text.Json;
using ProyectoAspireMDAW2025.Common.Constants;

namespace ProyectoAspireMDAW2025.Common.Extensions;

/// <summary>
/// Métodos de extensión para ClaimsPrincipal.
/// Facilitan la extracción de información del usuario desde los claims del JWT.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Obtiene el ID del usuario desde los claims.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <returns>GUID del usuario.</returns>
    /// <exception cref="UnauthorizedAccessException">Si el claim no existe o no es un GUID válido.</exception>
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst(ClaimTypes.NameIdentifier)
                    ?? principal.FindFirst(CustomClaimTypes.UserId);

        if (claim == null || !Guid.TryParse(claim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }

        return userId;
    }

    /// <summary>
    /// Intenta obtener el ID del usuario desde los claims.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <param name="userId">GUID del usuario si se encuentra.</param>
    /// <returns>True si se encontró el ID, false en caso contrario.</returns>
    public static bool TryGetUserId(this ClaimsPrincipal principal, out Guid userId)
    {
        userId = Guid.Empty;

        var claim = principal.FindFirst(ClaimTypes.NameIdentifier)
                    ?? principal.FindFirst(CustomClaimTypes.UserId);

        if (claim == null)
        {
            return false;
        }

        return Guid.TryParse(claim.Value, out userId);
    }

    /// <summary>
    /// Obtiene el email del usuario desde los claims.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <returns>Email del usuario.</returns>
    /// <exception cref="UnauthorizedAccessException">Si el claim no existe.</exception>
    public static string GetEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Email)?.Value
               ?? principal.FindFirst(CustomClaimTypes.Email)?.Value
               ?? throw new UnauthorizedAccessException("Email not found in token");
    }

    /// <summary>
    /// Obtiene el nombre de usuario desde los claims.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <returns>Nombre de usuario.</returns>
    /// <exception cref="UnauthorizedAccessException">Si el claim no existe.</exception>
    public static string GetUsername(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(CustomClaimTypes.Username)?.Value
               ?? principal.FindFirst(ClaimTypes.Name)?.Value
               ?? throw new UnauthorizedAccessException("Username not found in token");
    }

    /// <summary>
    /// Obtiene el nombre completo del usuario desde los claims.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <returns>Nombre completo del usuario, o null si no existe.</returns>
    public static string? GetFullName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(CustomClaimTypes.FullName)?.Value;
    }

    /// <summary>
    /// Obtiene todos los roles del usuario desde los claims.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <returns>Lista de roles del usuario.</returns>
    public static List<string> GetRoles(this ClaimsPrincipal principal)
    {
        return principal.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
    }

    /// <summary>
    /// Obtiene todos los permisos del usuario desde los claims.
    /// Los permisos están serializados como JSON array en el claim "permissions".
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <returns>Lista de permisos del usuario.</returns>
    public static List<string> GetPermissions(this ClaimsPrincipal principal)
    {
        var permissionsClaim = principal.FindFirst(CustomClaimTypes.Permissions)?.Value;

        if (string.IsNullOrEmpty(permissionsClaim))
        {
            return new List<string>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<string>>(permissionsClaim)
                   ?? new List<string>();
        }
        catch (JsonException)
        {
            // Si falla la deserialización, retornar lista vacía
            return new List<string>();
        }
    }

    /// <summary>
    /// Verifica si el usuario tiene un permiso específico.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <param name="permission">Permiso a verificar (ej: "users.read").</param>
    /// <returns>True si el usuario tiene el permiso, false en caso contrario.</returns>
    public static bool HasPermission(this ClaimsPrincipal principal, string permission)
    {
        if (string.IsNullOrWhiteSpace(permission))
        {
            return false;
        }

        var permissions = principal.GetPermissions();
        return permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Verifica si el usuario tiene al menos uno de los permisos especificados.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <param name="permissions">Permisos a verificar.</param>
    /// <returns>True si el usuario tiene al menos uno de los permisos, false en caso contrario.</returns>
    public static bool HasAnyPermission(this ClaimsPrincipal principal, params string[] permissions)
    {
        if (permissions == null || permissions.Length == 0)
        {
            return false;
        }

        var userPermissions = principal.GetPermissions();
        return permissions.Any(p => userPermissions.Contains(p, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Verifica si el usuario tiene todos los permisos especificados.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <param name="permissions">Permisos a verificar.</param>
    /// <returns>True si el usuario tiene todos los permisos, false en caso contrario.</returns>
    public static bool HasAllPermissions(this ClaimsPrincipal principal, params string[] permissions)
    {
        if (permissions == null || permissions.Length == 0)
        {
            return true; // Si no se especifican permisos, retornar true
        }

        var userPermissions = principal.GetPermissions();
        return permissions.All(p => userPermissions.Contains(p, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Verifica si el usuario tiene un rol específico.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <param name="role">Rol a verificar (ej: "Admin").</param>
    /// <returns>True si el usuario tiene el rol, false en caso contrario.</returns>
    public static bool HasRole(this ClaimsPrincipal principal, string role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return false;
        }

        return principal.IsInRole(role);
    }

    /// <summary>
    /// Verifica si el usuario tiene al menos uno de los roles especificados.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <param name="roles">Roles a verificar.</param>
    /// <returns>True si el usuario tiene al menos uno de los roles, false en caso contrario.</returns>
    public static bool HasAnyRole(this ClaimsPrincipal principal, params string[] roles)
    {
        if (roles == null || roles.Length == 0)
        {
            return false;
        }

        return roles.Any(role => principal.IsInRole(role));
    }

    /// <summary>
    /// Verifica si el usuario tiene todos los roles especificados.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <param name="roles">Roles a verificar.</param>
    /// <returns>True si el usuario tiene todos los roles, false en caso contrario.</returns>
    public static bool HasAllRoles(this ClaimsPrincipal principal, params string[] roles)
    {
        if (roles == null || roles.Length == 0)
        {
            return true; // Si no se especifican roles, retornar true
        }

        return roles.All(role => principal.IsInRole(role));
    }

    /// <summary>
    /// Verifica si el usuario es administrador (tiene rol Admin).
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <returns>True si el usuario es Admin, false en caso contrario.</returns>
    public static bool IsAdmin(this ClaimsPrincipal principal)
    {
        return principal.IsInRole(Roles.Admin);
    }

    /// <summary>
    /// Verifica si el usuario es manager (tiene rol Manager).
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <returns>True si el usuario es Manager, false en caso contrario.</returns>
    public static bool IsManager(this ClaimsPrincipal principal)
    {
        return principal.IsInRole(Roles.Manager);
    }

    /// <summary>
    /// Verifica si el usuario tiene permisos administrativos (Admin o Manager).
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <returns>True si el usuario es Admin o Manager, false en caso contrario.</returns>
    public static bool IsAdministrative(this ClaimsPrincipal principal)
    {
        return principal.IsInRole(Roles.Admin) || principal.IsInRole(Roles.Manager);
    }

    /// <summary>
    /// Obtiene el ID del tenant/organización desde los claims (para multi-tenancy).
    /// </summary>
    /// <param name="principal">ClaimsPrincipal del usuario autenticado.</param>
    /// <returns>ID del tenant, o null si no existe.</returns>
    public static Guid? GetTenantId(this ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst(CustomClaimTypes.TenantId);

        if (claim == null || !Guid.TryParse(claim.Value, out var tenantId))
        {
            return null;
        }

        return tenantId;
    }
}

