using System.ComponentModel.DataAnnotations;

namespace ProyectoAspireMDAW2025.Common.DTOs.Requests.Auth;

/// <summary>
/// DTO para solicitud de registro de nuevo usuario.
/// </summary>
public class RegisterRequest
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
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", 
        ErrorMessage = "La contraseña debe contener al menos una mayúscula, una minúscula y un número")]
    public required string Password { get; init; }

    /// <summary>
    /// Confirmación de contraseña.
    /// </summary>
    [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
    public required string ConfirmPassword { get; init; }

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
    public required string FirstName { get; init; }

    /// <summary>
    /// Apellido del usuario.
    /// </summary>
    [Required(ErrorMessage = "El apellido es requerido")]
    [MaxLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
    public required string LastName { get; init; }
}

