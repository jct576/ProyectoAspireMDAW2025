using ProyectoAspireMDAW2025.Contracts.Events;

namespace ProyectoAspireMDAW2025.Messaging.Abstractions;

/// <summary>
/// Interfaz para consumir eventos de integración.
/// </summary>
/// <typeparam name="TEvent">Tipo del evento que hereda de BaseEvent.</typeparam>
public interface IEventConsumer<in TEvent> where TEvent : BaseEvent
{
    /// <summary>
    /// Maneja el evento recibido del message broker.
    /// </summary>
    /// <param name="event">Instancia del evento recibido.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Task que representa la operación asíncrona.</returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}

