namespace ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

/// <summary>
/// DTO para representar un permiso del sistema.
/// </summary>
public class PermissionDto
{
    /// <summary>
    /// ID único del permiso.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Nombre del permiso (ej: "users.read", "users.write").
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Descripción del permiso.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Categoría del permiso (ej: "Users", "Roles", "Audit").
    /// </summary>
    public string? Category { get; init; }

    /// <summary>
    /// Fecha de creación del permiso.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Indica si el permiso está asignado al rol actual (usado en contexto de roles).
    /// </summary>
    public bool IsAssigned { get; init; }
}

