namespace ProyectoAspireMDAW2025.Common.Constants;

/// <summary>
/// Constantes para claves de caché en Redis.
/// </summary>
public static class CacheKeys
{
    /// <summary>
    /// Claves relacionadas con autenticación.
    /// </summary>
    public static class Auth
    {
        public static string BlacklistedToken(string jti) => $"auth:blacklist:{jti}";
        public static string RefreshToken(Guid userId) => $"auth:refresh:{userId}";
        public static string UserSession(Guid userId) => $"auth:session:{userId}";
    }

    /// <summary>
    /// Claves relacionadas con usuarios.
    /// </summary>
    public static class User
    {
        public static string UserProfile(Guid userId) => $"user:profile:{userId}";
        public static string UserByEmail(string email) => $"user:email:{email.ToLowerInvariant()}";
    }

    /// <summary>
    /// Tiempo de expiración por defecto para caché (en minutos).
    /// </summary>
    public static class Expiration
    {
        public const int ShortTerm = 5;      // 5 minutos
        public const int MediumTerm = 30;    // 30 minutos
        public const int LongTerm = 60;      // 1 hora
        public const int VeryLongTerm = 1440; // 24 horas
    }
}

