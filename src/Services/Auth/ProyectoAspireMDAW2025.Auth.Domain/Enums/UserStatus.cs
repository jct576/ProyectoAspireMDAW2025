namespace ProyectoAspireMDAW2025.Auth.Domain.Enums;

/// <summary>
/// Estados posibles de un usuario
/// </summary>
public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Locked = 3,
    PendingVerification = 4,
    Deleted = 5
}

