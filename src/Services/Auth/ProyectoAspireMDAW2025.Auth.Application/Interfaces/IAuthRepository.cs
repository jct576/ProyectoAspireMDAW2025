using ProyectoAspireMDAW2025.Auth.Domain.Entities;

namespace ProyectoAspireMDAW2025.Auth.Application.Interfaces;

/// <summary>
/// Repositorio para operaciones de autenticación
/// </summary>
public interface IAuthRepository
{
    /// <summary>
    /// Obtiene un usuario por email
    /// </summary>
    Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtiene un usuario por ID
    /// </summary>
    Task<ApplicationUser?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Valida las credenciales del usuario
    /// </summary>
    Task<bool> ValidateCredentialsAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtiene los roles de un usuario
    /// </summary>
    Task<IList<string>> GetUserRolesAsync(ApplicationUser user, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Guarda un refresh token
    /// </summary>
    Task SaveRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtiene un refresh token por su valor
    /// </summary>
    Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Actualiza un refresh token
    /// </summary>
    Task UpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtiene todos los refresh tokens activos de un usuario
    /// </summary>
    Task<IList<RefreshToken>> GetUserActiveRefreshTokensAsync(Guid userId, CancellationToken cancellationToken = default);

    // ==================== ROLE MANAGEMENT ====================

    /// <summary>
    /// Asigna un rol a un usuario
    /// </summary>
    Task<bool> AssignRoleToUserAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remueve un rol de un usuario
    /// </summary>
    Task<bool> RemoveRoleFromUserAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si un rol existe
    /// </summary>
    Task<bool> RoleExistsAsync(string roleName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea un nuevo rol
    /// </summary>
    Task<ApplicationRole> CreateRoleAsync(string roleName, string? description = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un rol por nombre
    /// </summary>
    Task<ApplicationRole?> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los roles
    /// </summary>
    Task<IList<ApplicationRole>> GetAllRolesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si un usuario tiene un rol específico
    /// </summary>
    Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken = default);

    // ==================== PERMISSION MANAGEMENT ====================

    /// <summary>
    /// Obtiene todos los permisos del sistema
    /// </summary>
    Task<IList<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene los permisos de un rol específico
    /// </summary>
    Task<IList<Permission>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene los permisos de un rol por nombre
    /// </summary>
    Task<IList<Permission>> GetRolePermissionsByNameAsync(string roleName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los permisos efectivos de un usuario (combinación de todos sus roles)
    /// </summary>
    Task<IList<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un permiso por nombre
    /// </summary>
    Task<Permission?> GetPermissionByNameAsync(string permissionName, CancellationToken cancellationToken = default);
}

