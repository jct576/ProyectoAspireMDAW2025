using ProyectoAspireMDAW2025.Contracts.Events;

namespace ProyectoAspireMDAW2025.Messaging.Abstractions;

/// <summary>
/// Interfaz principal del message bus que combina publicación y suscripción.
/// </summary>
public interface IMessageBus
{
    /// <summary>
    /// Publica un evento al message broker.
    /// </summary>
    /// <typeparam name="TEvent">Tipo del evento que hereda de BaseEvent.</typeparam>
    /// <param name="event">Instancia del evento a publicar.</param>
    /// <param name="routingKey">Routing key para el evento (opcional, se puede inferir del tipo).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Task que representa la operación asíncrona.</returns>
    Task PublishAsync<TEvent>(TEvent @event, string? routingKey = null, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent;

    /// <summary>
    /// Suscribe un consumer a un tipo de evento específico.
    /// </summary>
    /// <typeparam name="TEvent">Tipo del evento que hereda de BaseEvent.</typeparam>
    /// <param name="queueName">Nombre de la cola para este consumer.</param>
    /// <param name="routingKey">Routing key para filtrar eventos.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Task que representa la operación asíncrona.</returns>
    Task SubscribeAsync<TEvent>(string queueName, string routingKey, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent;
}

