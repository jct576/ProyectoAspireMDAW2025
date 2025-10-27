using System.ComponentModel.DataAnnotations;

namespace ProyectoAspireMDAW2025.Common.DTOs.Requests.Roles;

/// <summary>
/// DTO para solicitud de remoción de rol de un usuario.
/// </summary>
public class RemoveRoleRequest
{
    /// <summary>
    /// ID del usuario al que se le removerá el rol.
    /// </summary>
    [Required(ErrorMessage = "El ID del usuario es requerido")]
    public required Guid UserId { get; init; }

    /// <summary>
    /// Nombre del rol a remover (ej: "Admin", "Manager", "User", "Guest").
    /// </summary>
    [Required(ErrorMessage = "El nombre del rol es requerido")]
    [MaxLength(50, ErrorMessage = "El nombre del rol no puede exceder 50 caracteres")]
    public required string RoleName { get; init; }
}

