using System.Diagnostics.Metrics;

namespace ProyectoAspireMDAW2025.Observability.Metrics;

/// <summary>
/// Extensiones para métricas personalizadas.
/// </summary>
public static class MetricsExtensions
{
    private static readonly Meter Meter = new("ProyectoAspireMDAW2025", "1.0.0");

    // Contadores
    private static readonly Counter<long> EventsPublishedCounter = Meter.CreateCounter<long>(
        "events.published",
        description: "Número total de eventos publicados");

    private static readonly Counter<long> EventsConsumedCounter = Meter.CreateCounter<long>(
        "events.consumed",
        description: "Número total de eventos consumidos");

    private static readonly Counter<long> EventsFailedCounter = Meter.CreateCounter<long>(
        "events.failed",
        description: "Número total de eventos que fallaron al procesarse");

    // Histogramas
    private static readonly Histogram<double> EventProcessingDuration = Meter.CreateHistogram<double>(
        "events.processing.duration",
        unit: "ms",
        description: "Duración del procesamiento de eventos");

    private static readonly Histogram<double> OperationDuration = Meter.CreateHistogram<double>(
        "operation.duration",
        unit: "ms",
        description: "Duración de operaciones");

    /// <summary>
    /// Incrementa el contador de eventos publicados.
    /// </summary>
    public static void RecordEventPublished(string eventType, string serviceName)
    {
        EventsPublishedCounter.Add(1, new KeyValuePair<string, object?>("event.type", eventType),
                                       new KeyValuePair<string, object?>("service.name", serviceName));
    }

    /// <summary>
    /// Incrementa el contador de eventos consumidos.
    /// </summary>
    public static void RecordEventConsumed(string eventType, string serviceName)
    {
        EventsConsumedCounter.Add(1, new KeyValuePair<string, object?>("event.type", eventType),
                                     new KeyValuePair<string, object?>("service.name", serviceName));
    }

    /// <summary>
    /// Incrementa el contador de eventos fallidos.
    /// </summary>
    public static void RecordEventFailed(string eventType, string serviceName, string errorType)
    {
        EventsFailedCounter.Add(1, new KeyValuePair<string, object?>("event.type", eventType),
                                   new KeyValuePair<string, object?>("service.name", serviceName),
                                   new KeyValuePair<string, object?>("error.type", errorType));
    }

    /// <summary>
    /// Registra la duración del procesamiento de un evento.
    /// </summary>
    public static void RecordEventProcessingDuration(string eventType, string serviceName, double durationMs)
    {
        EventProcessingDuration.Record(durationMs, new KeyValuePair<string, object?>("event.type", eventType),
                                                    new KeyValuePair<string, object?>("service.name", serviceName));
    }

    /// <summary>
    /// Registra la duración de una operación.
    /// </summary>
    public static void RecordOperationDuration(string operationName, string serviceName, double durationMs, bool success)
    {
        OperationDuration.Record(durationMs, new KeyValuePair<string, object?>("operation.name", operationName),
                                             new KeyValuePair<string, object?>("service.name", serviceName),
                                             new KeyValuePair<string, object?>("success", success));
    }
}

