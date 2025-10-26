using Microsoft.AspNetCore.Identity;
using ProyectoAspireMDAW2025.Auth.Domain.Enums;

namespace ProyectoAspireMDAW2025.Auth.Domain.Entities;

/// <summary>
/// Entidad de usuario para Auth Service - SOLO autenticación y autorización
/// NO contiene datos de perfil personal (nombre, teléfono, avatar, etc.)
/// Esos datos están en User Service (UserDB)
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    // ✅ SOLO datos de autenticación/autorización
    // Email ya viene de IdentityUser
    // PasswordHash ya viene de IdentityUser
    // EmailConfirmed ya viene de IdentityUser
    // PhoneNumber ya viene de IdentityUser (para 2FA)
    // PhoneNumberConfirmed ya viene de IdentityUser
    // TwoFactorEnabled ya viene de IdentityUser
    // LockoutEnd ya viene de IdentityUser
    // LockoutEnabled ya viene de IdentityUser
    // AccessFailedCount ya viene de IdentityUser

    // Propiedades adicionales para Auth
    public UserStatus Status { get; set; } = UserStatus.Active;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    // Soft Delete (para auditoría)
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    // Navegación virtual para EF Core (lazy loading)
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    // ========================================
    // Métodos de dominio - SOLO autenticación
    // ========================================

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        Status = UserStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        IsActive = false;
        Status = UserStatus.Deleted;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Lock()
    {
        IsActive = false;
        Status = UserStatus.Locked;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unlock()
    {
        IsActive = true;
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }
}

