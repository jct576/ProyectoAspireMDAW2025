namespace ProyectoAspireMDAW2025.Contracts.Events.Auth;

/// <summary>
/// Evento publicado cuando un token de acceso es revocado (logout, cambio de contraseña, etc.).
/// </summary>
public class TokenRevokedEvent : BaseEvent
{
    /// <summary>
    /// ID del usuario cuyo token fue revocado.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// JTI (JWT ID) del token revocado.
    /// </summary>
    public required string TokenId { get; init; }

    /// <summary>
    /// Razón de la revocación.
    /// </summary>
    public required string Reason { get; init; }

    /// <summary>
    /// Fecha y hora de expiración original del token.
    /// </summary>
    public DateTime TokenExpiresAt { get; init; }

    /// <summary>
    /// Fecha y hora de la revocación.
    /// </summary>
    public DateTime RevokedAt { get; init; }
}

