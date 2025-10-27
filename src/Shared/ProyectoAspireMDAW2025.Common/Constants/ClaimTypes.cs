namespace ProyectoAspireMDAW2025.Common.Constants;

/// <summary>
/// Constantes para tipos de claims personalizados en JWT.
/// Estos claims se incluyen en el Access Token para autorización.
/// </summary>
public static class CustomClaimTypes
{
    /// <summary>
    /// Claim para el ID del usuario (GUID).
    /// Corresponde al claim estándar "sub" (subject).
    /// </summary>
    public const string UserId = "sub";

    /// <summary>
    /// Claim para el email del usuario.
    /// Corresponde al claim estándar "email".
    /// </summary>
    public const string Email = "email";

    /// <summary>
    /// Claim para el nombre de usuario (username).
    /// </summary>
    public const string Username = "username";

    /// <summary>
    /// Claim para el nombre completo del usuario.
    /// </summary>
    public const string FullName = "fullname";

    /// <summary>
    /// Claim para los roles del usuario.
    /// Corresponde al claim estándar "role".
    /// Puede haber múltiples claims de este tipo (uno por rol).
    /// </summary>
    public const string Role = "role";

    /// <summary>
    /// Claim para los permisos del usuario.
    /// Se serializa como JSON array con todos los permisos.
    /// Ejemplo: ["users.read", "users.write", "roles.read"]
    /// </summary>
    public const string Permissions = "permissions";

    /// <summary>
    /// Claim para el departamento del usuario (opcional).
    /// </summary>
    public const string Department = "department";

    /// <summary>
    /// Claim para el ID de empleado (opcional).
    /// </summary>
    public const string EmployeeId = "employee_id";

    /// <summary>
    /// Claim para el tenant/organización (para multi-tenancy, opcional).
    /// </summary>
    public const string TenantId = "tenant_id";

    /// <summary>
    /// Claim para el nivel de seguridad del usuario (opcional).
    /// </summary>
    public const string SecurityLevel = "security_level";
}

