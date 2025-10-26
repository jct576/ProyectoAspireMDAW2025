namespace ProyectoAspireMDAW2025.Contracts.Events.User;

/// <summary>
/// Evento publicado cuando un usuario es eliminado (soft delete o hard delete).
/// </summary>
public class UserDeletedEvent : BaseEvent
{
    /// <summary>
    /// ID del usuario eliminado.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Email del usuario eliminado.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Indica si es eliminación lógica (soft delete) o física (hard delete).
    /// </summary>
    public bool IsSoftDelete { get; init; }

    /// <summary>
    /// Razón de la eliminación.
    /// </summary>
    public string? Reason { get; init; }

    /// <summary>
    /// Fecha y hora de la eliminación.
    /// </summary>
    public DateTime DeletedAt { get; init; }

    /// <summary>
    /// ID del usuario que realizó la eliminación (admin).
    /// </summary>
    public Guid DeletedBy { get; init; }
}

