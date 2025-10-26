using MassTransit;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Contracts.Events;
using ProyectoAspireMDAW2025.Messaging.Abstractions;

namespace ProyectoAspireMDAW2025.Auth.Infrastructure.Messaging;

/// <summary>
/// Implementación de IEventPublisher usando MassTransit con RabbitMQ
/// </summary>
public class RabbitMqEventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RabbitMqEventPublisher> _logger;

    public RabbitMqEventPublisher(
        IPublishEndpoint publishEndpoint,
        ILogger<RabbitMqEventPublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : BaseEvent
    {
        try
        {
            await _publishEndpoint.Publish(@event, cancellationToken);

            _logger.LogInformation("Event published: {EventType}", typeof(TEvent).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing event: {EventType}", typeof(TEvent).Name);
            throw;
        }
    }

    public async Task PublishBatchAsync<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellationToken = default) where TEvent : BaseEvent
    {
        try
        {
            var eventList = events.ToList();

            foreach (var @event in eventList)
            {
                await _publishEndpoint.Publish(@event, cancellationToken);
            }

            _logger.LogInformation("Batch of {Count} events published: {EventType}", eventList.Count, typeof(TEvent).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing batch of events: {EventType}", typeof(TEvent).Name);
            throw;
        }
    }
}

