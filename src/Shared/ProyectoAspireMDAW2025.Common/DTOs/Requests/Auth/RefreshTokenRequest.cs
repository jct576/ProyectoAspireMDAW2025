using System.ComponentModel.DataAnnotations;

namespace ProyectoAspireMDAW2025.Common.DTOs.Requests.Auth;

/// <summary>
/// DTO para solicitud de renovación de token de acceso.
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Refresh token actual.
    /// </summary>
    [Required(ErrorMessage = "El refresh token es requerido")]
    public required string RefreshToken { get; init; }
}

