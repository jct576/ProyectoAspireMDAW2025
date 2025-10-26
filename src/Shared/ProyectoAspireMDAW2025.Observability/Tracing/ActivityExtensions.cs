using System.Diagnostics;

namespace ProyectoAspireMDAW2025.Observability.Tracing;

/// <summary>
/// Extensiones para trabajar con Activities (distributed tracing).
/// </summary>
public static class ActivityExtensions
{
    /// <summary>
    /// Agrega tags comunes a una Activity.
    /// </summary>
    public static Activity? AddCommonTags(this Activity? activity, string serviceName, string operationName, Guid correlationId)
    {
        if (activity == null) return null;

        activity.SetTag("service.name", serviceName);
        activity.SetTag("operation.name", operationName);
        activity.SetTag("correlation.id", correlationId.ToString());
        activity.SetTag("environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production");

        return activity;
    }

    /// <summary>
    /// Agrega información de evento a una Activity.
    /// </summary>
    public static Activity? AddEventTags(this Activity? activity, string eventType, Guid eventId, string? routingKey = null)
    {
        if (activity == null) return null;

        activity.SetTag("event.type", eventType);
        activity.SetTag("event.id", eventId.ToString());
        if (!string.IsNullOrWhiteSpace(routingKey))
        {
            activity.SetTag("event.routing_key", routingKey);
        }

        return activity;
    }

    /// <summary>
    /// Agrega información de usuario a una Activity.
    /// </summary>
    public static Activity? AddUserTags(this Activity? activity, Guid userId, string? email = null)
    {
        if (activity == null) return null;

        activity.SetTag("user.id", userId.ToString());
        if (!string.IsNullOrWhiteSpace(email))
        {
            activity.SetTag("user.email", email);
        }

        return activity;
    }

    /// <summary>
    /// Marca una Activity como exitosa.
    /// </summary>
    public static Activity? MarkAsSuccess(this Activity? activity)
    {
        if (activity == null) return null;

        activity.SetTag("operation.status", "success");
        activity.SetStatus(ActivityStatusCode.Ok);

        return activity;
    }

    /// <summary>
    /// Marca una Activity como fallida con información de error.
    /// </summary>
    public static Activity? MarkAsError(this Activity? activity, Exception exception)
    {
        if (activity == null) return null;

        activity.SetTag("operation.status", "error");
        activity.SetTag("error.type", exception.GetType().Name);
        activity.SetTag("error.message", exception.Message);
        activity.SetStatus(ActivityStatusCode.Error, exception.Message);

        // Agregar evento de excepción
        var tags = new ActivityTagsCollection
        {
            { "exception.type", exception.GetType().FullName },
            { "exception.message", exception.Message },
            { "exception.stacktrace", exception.StackTrace }
        };
        activity.AddEvent(new ActivityEvent("exception", DateTimeOffset.UtcNow, tags));

        return activity;
    }
}

