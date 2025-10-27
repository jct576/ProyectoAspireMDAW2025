using Microsoft.EntityFrameworkCore;
using ProyectoAspireMDAW2025.Auth.Domain.Entities;
using ProyectoAspireMDAW2025.Common.Constants;

namespace ProyectoAspireMDAW2025.Auth.Infrastructure.Data.Seeds;

/// <summary>
/// Seed data para Roles
/// </summary>
public static class RolesSeed
{
    /// <summary>
    /// IDs fijos para los roles (para referencias en RolePermissionsSeed)
    /// </summary>
    public static class RoleIds
    {
        public static readonly Guid Admin = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public static readonly Guid Manager = Guid.Parse("00000000-0000-0000-0000-000000000002");
        public static readonly Guid User = Guid.Parse("00000000-0000-0000-0000-000000000003");
        public static readonly Guid Guest = Guid.Parse("00000000-0000-0000-0000-000000000004");
    }

    public static void Seed(ModelBuilder modelBuilder)
    {
        // Usar fecha fija para evitar cambios en el modelo
        var seedDate = new DateTime(2025, 10, 27, 0, 0, 0, DateTimeKind.Utc);

        var roles = new List<ApplicationRole>
        {
            new ApplicationRole
            {
                Id = RoleIds.Admin,
                Name = Roles.Admin,
                NormalizedName = Roles.Admin.ToUpperInvariant(),
                Description = "Administrador del sistema con acceso completo a todas las funcionalidades",
                CreatedAt = seedDate,
                ConcurrencyStamp = "admin-seed-stamp-001"
            },
            new ApplicationRole
            {
                Id = RoleIds.Manager,
                Name = Roles.Manager,
                NormalizedName = Roles.Manager.ToUpperInvariant(),
                Description = "Gestor con permisos para administrar usuarios y contenido",
                CreatedAt = seedDate,
                ConcurrencyStamp = "manager-seed-stamp-002"
            },
            new ApplicationRole
            {
                Id = RoleIds.User,
                Name = Roles.User,
                NormalizedName = Roles.User.ToUpperInvariant(),
                Description = "Usuario estándar con permisos básicos",
                CreatedAt = seedDate,
                ConcurrencyStamp = "user-seed-stamp-003"
            },
            new ApplicationRole
            {
                Id = RoleIds.Guest,
                Name = Roles.Guest,
                NormalizedName = Roles.Guest.ToUpperInvariant(),
                Description = "Invitado con permisos de solo lectura limitados",
                CreatedAt = seedDate,
                ConcurrencyStamp = "guest-seed-stamp-004"
            }
        };

        modelBuilder.Entity<ApplicationRole>().HasData(roles);
    }
}

