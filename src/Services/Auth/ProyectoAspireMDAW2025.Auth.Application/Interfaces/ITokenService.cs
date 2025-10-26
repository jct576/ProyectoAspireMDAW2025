using ProyectoAspireMDAW2025.Auth.Domain.Entities;

namespace ProyectoAspireMDAW2025.Auth.Application.Interfaces;

/// <summary>
/// Servicio para generación y validación de JWT tokens
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Genera un access token JWT para el usuario
    /// </summary>
    Task<string> GenerateAccessTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Genera un refresh token
    /// </summary>
    Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId, string? ipAddress = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Valida un refresh token
    /// </summary>
    Task<RefreshToken?> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Revoca un refresh token
    /// </summary>
    Task RevokeRefreshTokenAsync(string token, string? ipAddress = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Revoca todos los refresh tokens de un usuario
    /// </summary>
    Task RevokeAllUserTokensAsync(Guid userId, string? ipAddress = null, CancellationToken cancellationToken = default);
}

