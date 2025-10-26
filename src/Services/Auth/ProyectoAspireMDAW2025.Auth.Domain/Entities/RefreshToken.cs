namespace ProyectoAspireMDAW2025.Auth.Domain.Entities;

/// <summary>
/// Entidad para almacenar refresh tokens
/// </summary>
public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? RevokedByIp { get; set; }
    public string? CreatedByIp { get; set; }
    
    // Navegación
    public virtual ApplicationUser User { get; set; } = null!;
    
    // Métodos de dominio
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
    
    public void Revoke(string? ipAddress = null)
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        RevokedByIp = ipAddress;
    }
}

