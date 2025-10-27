using System.ComponentModel.DataAnnotations;

namespace ProyectoAspireMDAW2025.Common.DTOs.Requests.Roles;

/// <summary>
/// DTO para solicitud de creación de un nuevo rol.
/// </summary>
public class CreateRoleRequest
{
    /// <summary>
    /// Nombre del rol (ej: "Admin", "Manager", "User", "Guest").
    /// Debe ser único en el sistema.
    /// </summary>
    [Required(ErrorMessage = "El nombre del rol es requerido")]
    [MaxLength(50, ErrorMessage = "El nombre del rol no puede exceder 50 caracteres")]
    [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]*$", 
        ErrorMessage = "El nombre del rol debe comenzar con una letra y solo puede contener letras, números y guiones bajos")]
    public required string Name { get; init; }

    /// <summary>
    /// Descripción del rol (opcional).
    /// </summary>
    [MaxLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    public string? Description { get; init; }

    /// <summary>
    /// Lista de permisos a asignar al rol (opcional).
    /// Ejemplo: ["users.read", "users.write"]
    /// </summary>
    public List<string>? Permissions { get; init; }
}

