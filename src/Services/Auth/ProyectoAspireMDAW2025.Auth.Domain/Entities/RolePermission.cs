namespace ProyectoAspireMDAW2025.Auth.Domain.Entities;

/// <summary>
/// Entidad de relación many-to-many entre Roles y Permissions
/// </summary>
public class RolePermission
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    
    // Navegación
    public virtual ApplicationRole Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}

