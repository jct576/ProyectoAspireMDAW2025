using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ProyectoAspireMDAW2025.Observability.Logging;

/// <summary>
/// Middleware para gestionar el CorrelationId en cada request HTTP.
/// </summary>
public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Obtener o generar CorrelationId
        var correlationId = GetOrCreateCorrelationId(context);

        // Agregar a los headers de respuesta
        context.Response.OnStarting(() =>
        {
            if (!context.Response.Headers.ContainsKey(CorrelationIdHeaderName))
            {
                context.Response.Headers.Append(CorrelationIdHeaderName, correlationId);
            }
            return Task.CompletedTask;
        });

        // Agregar a los items del contexto para uso posterior
        context.Items[CorrelationIdHeaderName] = correlationId;

        // Log del request con CorrelationId
        _logger.LogInformation(
            "Request started: {Method} {Path}. CorrelationId: {CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            correlationId);

        await _next(context);

        // Log del response con CorrelationId
        _logger.LogInformation(
            "Request completed: {Method} {Path}. StatusCode: {StatusCode}. CorrelationId: {CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            correlationId);
    }

    private static string GetOrCreateCorrelationId(HttpContext context)
    {
        // Intentar obtener del header
        if (context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId) &&
            !string.IsNullOrWhiteSpace(correlationId))
        {
            return correlationId.ToString();
        }

        // Generar nuevo CorrelationId
        return Guid.NewGuid().ToString();
    }
}

