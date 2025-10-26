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
}

