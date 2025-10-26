using Microsoft.AspNetCore.Identity;

namespace ProyectoAspireMDAW2025.Auth.Domain.Entities;

/// <summary>
/// Entidad de rol extendiendo ASP.NET Core Identity
/// </summary>
public class ApplicationRole : IdentityRole<Guid>
{
    // Propiedades adicionales personalizadas
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navegación a permisos (many-to-many)
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

