namespace ProyectoAspireMDAW2025.Contracts.Events;

/// <summary>
/// Clase base para todos los eventos de integración del sistema.
/// Proporciona propiedades comunes para trazabilidad y correlación.
/// </summary>
public abstract class BaseEvent
{
    /// <summary>
    /// Identificador único del evento.
    /// </summary>
    public Guid EventId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Identificador de correlación para rastrear el flujo completo de una operación.
    /// </summary>
    public Guid CorrelationId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Fecha y hora UTC en que se generó el evento.
    /// </summary>
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Nombre del servicio que publicó el evento.
    /// </summary>
    public string? PublishedBy { get; init; }

    /// <summary>
    /// Versión del esquema del evento (para compatibilidad hacia atrás).
    /// </summary>
    public string SchemaVersion { get; init; } = "1.0";
}

