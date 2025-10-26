namespace ProyectoAspireMDAW2025.Messaging.Configuration;

/// <summary>
/// Configuración para conexión a RabbitMQ.
/// </summary>
public class RabbitMQSettings
{
    /// <summary>
    /// Nombre de la sección en appsettings.json.
    /// </summary>
    public const string SectionName = "RabbitMQ";

    /// <summary>
    /// Host de RabbitMQ.
    /// </summary>
    public string Host { get; set; } = "localhost";

    /// <summary>
    /// Puerto de RabbitMQ.
    /// </summary>
    public int Port { get; set; } = 5672;

    /// <summary>
    /// Usuario de RabbitMQ.
    /// </summary>
    public string Username { get; set; } = "guest";

    /// <summary>
    /// Contraseña de RabbitMQ.
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Virtual host de RabbitMQ.
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Nombre del exchange principal.
    /// </summary>
    public string ExchangeName { get; set; } = "proyectoaspire.events";

    /// <summary>
    /// Tipo de exchange (topic, direct, fanout, headers).
    /// </summary>
    public string ExchangeType { get; set; } = "topic";

    /// <summary>
    /// Indica si el exchange es durable.
    /// </summary>
    public bool ExchangeDurable { get; set; } = true;

    /// <summary>
    /// Número de reintentos en caso de fallo de conexión.
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Tiempo de espera entre reintentos en segundos.
    /// </summary>
    public int RetryDelaySeconds { get; set; } = 5;
}

