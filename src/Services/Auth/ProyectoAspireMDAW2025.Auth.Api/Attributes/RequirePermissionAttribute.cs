using Microsoft.AspNetCore.Authorization;

namespace ProyectoAspireMDAW2025.Auth.Api.Attributes;

/// <summary>
/// Atributo de autorización basado en permisos
/// Requiere que el usuario tenga al menos uno de los permisos especificados
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Permisos requeridos (el usuario debe tener al menos uno)
    /// </summary>
    public string[] Permissions { get; }

    /// <summary>
    /// Si es true, el usuario debe tener TODOS los permisos especificados
    /// Si es false (default), el usuario debe tener AL MENOS UNO de los permisos
    /// </summary>
    public bool RequireAll { get; set; } = false;

    /// <summary>
    /// Constructor que acepta uno o más permisos
    /// </summary>
    /// <param name="permissions">Permisos requeridos</param>
    public RequirePermissionAttribute(params string[] permissions)
    {
        Permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
        
        if (permissions.Length == 0)
        {
            throw new ArgumentException("At least one permission must be specified", nameof(permissions));
        }

        // Configurar la policy basada en los permisos
        // La policy será evaluada por PermissionAuthorizationHandler
        Policy = $"RequirePermission:{string.Join(",", permissions)}:{RequireAll}";
    }
}

