namespace ProyectoAspireMDAW2025.Common.DTOs.Responses.Auth;

/// <summary>
/// DTO de respuesta para operaciones de autenticación exitosas.
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Token de acceso JWT.
    /// </summary>
    public required string AccessToken { get; init; }

    /// <summary>
    /// Refresh token para renovar el access token.
    /// </summary>
    public required string RefreshToken { get; init; }

    /// <summary>
    /// Tipo de token (siempre "Bearer").
    /// </summary>
    public string TokenType { get; init; } = "Bearer";

    /// <summary>
    /// Tiempo de expiración del access token en segundos.
    /// </summary>
    public int ExpiresIn { get; init; }

    /// <summary>
    /// ID del usuario autenticado.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Email del usuario autenticado.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Nombre completo del usuario.
    /// </summary>
    public string? FullName { get; init; }
}

