using ProyectoAspireMDAW2025.Auth.Domain.Entities;

namespace ProyectoAspireMDAW2025.Auth.Application.Interfaces;

/// <summary>
/// Servicio para gestión de permisos
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Obtiene todos los permisos efectivos de un usuario (combinación de todos sus roles)
    /// </summary>
    Task<IList<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Verifica si un usuario tiene un permiso específico
    /// </summary>
    Task<bool> UserHasPermissionAsync(Guid userId, string permission, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Verifica si un usuario tiene al menos uno de los permisos especificados
    /// </summary>
    Task<bool> UserHasAnyPermissionAsync(Guid userId, params string[] permissions);
    
    /// <summary>
    /// Verifica si un usuario tiene todos los permisos especificados
    /// </summary>
    Task<bool> UserHasAllPermissionsAsync(Guid userId, params string[] permissions);
    
    /// <summary>
    /// Asigna un permiso a un rol
    /// </summary>
    Task<bool> AssignPermissionToRoleAsync(Guid roleId, string permissionName, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Remueve un permiso de un rol
    /// </summary>
    Task<bool> RemovePermissionFromRoleAsync(Guid roleId, string permissionName, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sincroniza los permisos del sistema desde las constantes a la base de datos
    /// Crea permisos que no existen, útil para seed data
    /// </summary>
    Task SyncPermissionsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtiene todos los permisos del sistema
    /// </summary>
    Task<IList<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtiene los permisos de un rol específico
    /// </summary>
    Task<IList<Permission>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default);
}

