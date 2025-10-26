using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoAspireMDAW2025.Auth.Domain.Entities;

namespace ProyectoAspireMDAW2025.Auth.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración EF Core para ApplicationUser
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Tabla ya configurada en AuthDbContext como "Users"

        // Índices
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0"); // Solo emails únicos para usuarios no eliminados

        builder.HasIndex(u => u.UserName)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(u => u.IsDeleted);
        builder.HasIndex(u => u.Status);

        // Propiedades
        builder.Property(u => u.Status)
            .IsRequired()
            .HasConversion<int>(); // Guardar enum como int

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Relaciones
        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Si se elimina el usuario, eliminar sus tokens

        // Query Filter para soft delete
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}

