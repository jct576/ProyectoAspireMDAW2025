using ProyectoAspireMDAW2025.Contracts.Events;

namespace ProyectoAspireMDAW2025.Messaging.Abstractions;

/// <summary>
/// Interfaz para publicar eventos de integración.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publica un evento de integración al message broker.
    /// </summary>
    /// <typeparam name="TEvent">Tipo del evento que hereda de BaseEvent.</typeparam>
    /// <param name="event">Instancia del evento a publicar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Task que representa la operación asíncrona.</returns>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent;

    /// <summary>
    /// Publica múltiples eventos en batch.
    /// </summary>
    /// <typeparam name="TEvent">Tipo del evento que hereda de BaseEvent.</typeparam>
    /// <param name="events">Colección de eventos a publicar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Task que representa la operación asíncrona.</returns>
    Task PublishBatchAsync<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent;
}

