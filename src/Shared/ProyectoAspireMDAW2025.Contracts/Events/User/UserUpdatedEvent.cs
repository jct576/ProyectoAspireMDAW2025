namespace ProyectoAspireMDAW2025.Contracts.Events.User;

/// <summary>
/// Evento publicado cuando se actualiza la información de un usuario.
/// </summary>
public class UserUpdatedEvent : BaseEvent
{
    /// <summary>
    /// ID del usuario actualizado.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Email del usuario (puede haber cambiado).
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Nombre actualizado.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// Apellido actualizado.
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    /// Campos que fueron modificados.
    /// </summary>
    public List<string> ModifiedFields { get; init; } = new();

    /// <summary>
    /// Fecha y hora de la actualización.
    /// </summary>
    public DateTime UpdatedAt { get; init; }

    /// <summary>
    /// ID del usuario que realizó la actualización (puede ser el mismo usuario o un admin).
    /// </summary>
    public Guid UpdatedBy { get; init; }
}

