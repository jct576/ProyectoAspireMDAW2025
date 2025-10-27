namespace ProyectoAspireMDAW2025.Common.Constants;

/// <summary>
/// Constantes para roles del sistema RBAC.
/// Estos roles definen la jerarquía de permisos en la aplicación.
/// </summary>
public static class Roles
{
    /// <summary>
    /// Administrador con acceso total al sistema.
    /// Puede gestionar roles, permisos, usuarios, y ver auditoría completa.
    /// </summary>
    public const string Admin = "Admin";

    /// <summary>
    /// Manager con permisos de gestión limitados.
    /// Puede gestionar usuarios (CRUD), asignar rol "User", y ver reportes de auditoría.
    /// </summary>
    public const string Manager = "Manager";

    /// <summary>
    /// Usuario regular con permisos básicos.
    /// Puede ver y editar su propio perfil, actualizar preferencias, y eliminar su propia cuenta.
    /// </summary>
    public const string User = "User";

    /// <summary>
    /// Invitado con acceso de solo lectura.
    /// Solo puede ver información pública, no puede modificar nada.
    /// </summary>
    public const string Guest = "Guest";

    /// <summary>
    /// Obtiene todos los roles disponibles en el sistema.
    /// </summary>
    public static readonly string[] All = { Admin, Manager, User, Guest };

    /// <summary>
    /// Roles con permisos administrativos (Admin y Manager).
    /// </summary>
    public static readonly string[] Administrative = { Admin, Manager };

    /// <summary>
    /// Roles con permisos de gestión de usuarios (Admin y Manager).
    /// </summary>
    public static readonly string[] UserManagement = { Admin, Manager };
}

