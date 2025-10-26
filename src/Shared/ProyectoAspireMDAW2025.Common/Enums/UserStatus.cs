namespace ProyectoAspireMDAW2025.Common.Enums;

/// <summary>
/// Estados posibles de un usuario en el sistema.
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// Usuario activo y con acceso completo.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Usuario inactivo temporalmente.
    /// </summary>
    Inactive = 2,

    /// <summary>
    /// Usuario suspendido por violación de políticas.
    /// </summary>
    Suspended = 3,

    /// <summary>
    /// Usuario eliminado (soft delete).
    /// </summary>
    Deleted = 4,

    /// <summary>
    /// Usuario pendiente de verificación de email.
    /// </summary>
    PendingVerification = 5
}

