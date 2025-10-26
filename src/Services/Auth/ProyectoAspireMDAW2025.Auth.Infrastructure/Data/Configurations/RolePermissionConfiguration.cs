using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoAspireMDAW2025.Auth.Domain.Entities;

namespace ProyectoAspireMDAW2025.Auth.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración EF Core para RolePermission (tabla intermedia many-to-many)
/// </summary>
public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");

        // Clave primaria compuesta
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        // Índices
        builder.HasIndex(rp => rp.RoleId);
        builder.HasIndex(rp => rp.PermissionId);

        // Propiedades
        builder.Property(rp => rp.GrantedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // Relaciones (ya configuradas en RoleConfiguration y PermissionConfiguration)
        builder.HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

