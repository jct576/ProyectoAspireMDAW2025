namespace ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

/// <summary>
/// DTO para representar los roles de un usuario.
/// </summary>
public class UserRolesDto
{
    /// <summary>
    /// ID del usuario.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Email del usuario.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Nombre de usuario.
    /// </summary>
    public string? Username { get; init; }

    /// <summary>
    /// Lista de roles asignados al usuario.
    /// </summary>
    public required List<string> Roles { get; init; }

    /// <summary>
    /// Lista de permisos efectivos del usuario (combinación de todos sus roles).
    /// </summary>
    public List<string> Permissions { get; init; } = new();

    /// <summary>
    /// Fecha de última actualización de roles.
    /// </summary>
    public DateTime? LastUpdated { get; init; }
}

