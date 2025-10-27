namespace ProyectoAspireMDAW2025.Common.Constants;

/// <summary>
/// Constantes para nombres de topics/routing keys de eventos en RabbitMQ.
/// </summary>
public static class EventTopics
{
    /// <summary>
    /// Topics relacionados con autenticación.
    /// </summary>
    public static class Auth
    {
        public const string UserRegistered = "auth.user.registered";
        public const string UserLoggedIn = "auth.user.loggedin";
        public const string TokenRevoked = "auth.token.revoked";
        public const string PasswordChanged = "auth.password.changed";
    }

    /// <summary>
    /// Topics relacionados con usuarios.
    /// </summary>
    public static class User
    {
        public const string UserUpdated = "user.updated";
        public const string UserDeleted = "user.deleted";
        public const string ProfileCompleted = "user.profile.completed";
    }

    /// <summary>
    /// Exchange principal para eventos.
    /// </summary>
    public const string MainExchange = "proyectoaspire.events";

    /// <summary>
    /// Exchange para eventos de dead letter.
    /// </summary>
    public const string DeadLetterExchange = "proyectoaspire.events.dlx";
}

