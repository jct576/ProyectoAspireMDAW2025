namespace ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

/// <summary>
/// DTO para representar un rol del sistema.
/// </summary>
public class RoleDto
{
    /// <summary>
    /// ID único del rol.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Nombre del rol (ej: "Admin", "Manager", "User", "Guest").
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Nombre normalizado del rol (en mayúsculas).
    /// </summary>
    public string? NormalizedName { get; init; }

    /// <summary>
    /// Descripción del rol.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Fecha de creación del rol.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Fecha de última actualización del rol.
    /// </summary>
    public DateTime? UpdatedAt { get; init; }

    /// <summary>
    /// Número de usuarios que tienen este rol.
    /// </summary>
    public int UserCount { get; init; }

    /// <summary>
    /// Lista de permisos asignados a este rol.
    /// </summary>
    public List<string> Permissions { get; init; } = new();
}

