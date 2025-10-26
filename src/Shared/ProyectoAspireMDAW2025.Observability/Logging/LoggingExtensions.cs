using Microsoft.Extensions.Logging;

namespace ProyectoAspireMDAW2025.Observability.Logging;

/// <summary>
/// Extensiones para logging estructurado y consistente.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Log de inicio de operación con CorrelationId.
    /// </summary>
    public static void LogOperationStart(this ILogger logger, string operationName, Guid correlationId, object? additionalData = null)
    {
        logger.LogInformation(
            "Operation {OperationName} started. CorrelationId: {CorrelationId}. Data: {@AdditionalData}",
            operationName,
            correlationId,
            additionalData);
    }

    /// <summary>
    /// Log de finalización exitosa de operación.
    /// </summary>
    public static void LogOperationSuccess(this ILogger logger, string operationName, Guid correlationId, long elapsedMs, object? result = null)
    {
        logger.LogInformation(
            "Operation {OperationName} completed successfully. CorrelationId: {CorrelationId}. Duration: {ElapsedMs}ms. Result: {@Result}",
            operationName,
            correlationId,
            elapsedMs,
            result);
    }

    /// <summary>
    /// Log de fallo de operación.
    /// </summary>
    public static void LogOperationFailure(this ILogger logger, string operationName, Guid correlationId, Exception exception, object? context = null)
    {
        logger.LogError(
            exception,
            "Operation {OperationName} failed. CorrelationId: {CorrelationId}. Context: {@Context}",
            operationName,
            correlationId,
            context);
    }

    /// <summary>
    /// Log de evento publicado.
    /// </summary>
    public static void LogEventPublished(this ILogger logger, string eventType, Guid eventId, Guid correlationId, string? routingKey = null)
    {
        logger.LogInformation(
            "Event published. Type: {EventType}, EventId: {EventId}, CorrelationId: {CorrelationId}, RoutingKey: {RoutingKey}",
            eventType,
            eventId,
            correlationId,
            routingKey);
    }

    /// <summary>
    /// Log de evento recibido.
    /// </summary>
    public static void LogEventReceived(this ILogger logger, string eventType, Guid eventId, Guid correlationId)
    {
        logger.LogInformation(
            "Event received. Type: {EventType}, EventId: {EventId}, CorrelationId: {CorrelationId}",
            eventType,
            eventId,
            correlationId);
    }

    /// <summary>
    /// Log de evento procesado exitosamente.
    /// </summary>
    public static void LogEventProcessed(this ILogger logger, string eventType, Guid eventId, Guid correlationId, long elapsedMs)
    {
        logger.LogInformation(
            "Event processed successfully. Type: {EventType}, EventId: {EventId}, CorrelationId: {CorrelationId}, Duration: {ElapsedMs}ms",
            eventType,
            eventId,
            correlationId,
            elapsedMs);
    }

    /// <summary>
    /// Log de fallo en procesamiento de evento.
    /// </summary>
    public static void LogEventProcessingFailed(this ILogger logger, string eventType, Guid eventId, Guid correlationId, Exception exception)
    {
        logger.LogError(
            exception,
            "Event processing failed. Type: {EventType}, EventId: {EventId}, CorrelationId: {CorrelationId}",
            eventType,
            eventId,
            correlationId);
    }
}

