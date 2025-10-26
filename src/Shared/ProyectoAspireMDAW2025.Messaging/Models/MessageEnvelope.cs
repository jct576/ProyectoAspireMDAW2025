namespace ProyectoAspireMDAW2025.Messaging.Models;

/// <summary>
/// Envoltura para mensajes que incluye metadata adicional.
/// </summary>
/// <typeparam name="TPayload">Tipo del payload del mensaje.</typeparam>
public class MessageEnvelope<TPayload>
{
    /// <summary>
    /// ID único del mensaje.
    /// </summary>
    public Guid MessageId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// ID de correlación para rastrear el flujo completo.
    /// </summary>
    public Guid CorrelationId { get; init; }

    /// <summary>
    /// Timestamp de creación del mensaje.
    /// </summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Nombre del servicio que envió el mensaje.
    /// </summary>
    public string? Source { get; init; }

    /// <summary>
    /// Tipo del mensaje (nombre del tipo del payload).
    /// </summary>
    public required string MessageType { get; init; }

    /// <summary>
    /// Payload del mensaje.
    /// </summary>
    public required TPayload Payload { get; init; }

    /// <summary>
    /// Metadata adicional del mensaje.
    /// </summary>
    public Dictionary<string, string> Metadata { get; init; } = new();

    /// <summary>
    /// Número de intentos de procesamiento (para retry logic).
    /// </summary>
    public int RetryCount { get; set; }
}

