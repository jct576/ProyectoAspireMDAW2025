namespace ProyectoAspireMDAW2025.Common.Enums;

/// <summary>
/// Roles de usuario en el sistema.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Usuario regular sin permisos especiales.
    /// </summary>
    User = 1,

    /// <summary>
    /// Moderador con permisos limitados de administración.
    /// </summary>
    Moderator = 2,

    /// <summary>
    /// Administrador con permisos completos.
    /// </summary>
    Administrator = 3,

    /// <summary>
    /// Super administrador con acceso total al sistema.
    /// </summary>
    SuperAdmin = 4
}

