namespace ProyectoAspireMDAW2025.Auth.Domain.Entities;

/// <summary>
/// Entidad para permisos granulares del sistema
/// </summary>
public class Permission
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navegación a roles (many-to-many)
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

