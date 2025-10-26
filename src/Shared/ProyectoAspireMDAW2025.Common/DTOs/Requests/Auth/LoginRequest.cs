using System.ComponentModel.DataAnnotations;

namespace ProyectoAspireMDAW2025.Common.DTOs.Requests.Auth;

/// <summary>
/// DTO para solicitud de inicio de sesión.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Email del usuario.
    /// </summary>
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public required string Email { get; init; }

    /// <summary>
    /// Contraseña del usuario.
    /// </summary>
    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public required string Password { get; init; }

    /// <summary>
    /// Indica si se debe recordar la sesión (refresh token de larga duración).
    /// </summary>
    public bool RememberMe { get; init; }
}

