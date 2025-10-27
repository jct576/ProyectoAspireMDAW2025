namespace ProyectoAspireMDAW2025.Contracts.Events.Auth;

/// <summary>
/// Evento publicado cuando un usuario inicia sesión exitosamente.
/// </summary>
public class UserLoggedInEvent : BaseEvent
{
    /// <summary>
    /// ID del usuario que inició sesión.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Email del usuario.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Dirección IP desde donde se inició sesión.
    /// </summary>
    public string? IpAddress { get; init; }

    /// <summary>
    /// User Agent del navegador/cliente.
    /// </summary>
    public string? UserAgent { get; init; }

    /// <summary>
    /// Fecha y hora del login.
    /// </summary>
    public DateTime LoggedInAt { get; init; }
}

