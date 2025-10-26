namespace ProyectoAspireMDAW2025.Contracts.Events.Auth;

/// <summary>
/// Evento publicado cuando un nuevo usuario se registra en el sistema.
/// </summary>
public class UserRegisteredEvent : BaseEvent
{
    /// <summary>
    /// ID único del usuario registrado.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Email del usuario.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// Apellido del usuario.
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    /// Fecha y hora de registro.
    /// </summary>
    public DateTime RegisteredAt { get; init; }

    /// <summary>
    /// Indica si el email fue verificado durante el registro.
    /// </summary>
    public bool IsEmailVerified { get; init; }
}

