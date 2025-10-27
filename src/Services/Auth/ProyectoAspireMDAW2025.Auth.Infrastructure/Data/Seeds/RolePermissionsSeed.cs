using Microsoft.EntityFrameworkCore;
using ProyectoAspireMDAW2025.Auth.Domain.Entities;

namespace ProyectoAspireMDAW2025.Auth.Infrastructure.Data.Seeds;

/// <summary>
/// Seed data para RolePermissions (asignaciones de permisos a roles)
/// </summary>
public static class RolePermissionsSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Usar fecha fija para evitar cambios en el modelo
        var seedDate = new DateTime(2025, 10, 27, 0, 0, 0, DateTimeKind.Utc);
        var rolePermissions = new List<RolePermission>();

        // ==================== ADMIN - TODOS LOS PERMISOS (18) ====================
        rolePermissions.AddRange(new[]
        {
            // Users permissions (7)
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.UsersRead, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.UsersReadOwn, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.UsersWrite, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.UsersWriteOwn, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.UsersDelete, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.UsersDeletePermanent, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.UsersRestore, GrantedAt = seedDate },

            // Roles permissions (4)
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.RolesRead, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.RolesManage, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.RolesAssign, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.RolesAssignUser, GrantedAt = seedDate },

            // Permissions management (2)
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.PermissionsRead, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.PermissionsManage, GrantedAt = seedDate },

            // Audit permissions (3)
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.AuditRead, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.AuditReadAll, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.AuditExport, GrantedAt = seedDate },

            // Notifications permissions (2)
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.NotificationsSend, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Admin, PermissionId = PermissionsSeed.PermissionIds.NotificationsManage, GrantedAt = seedDate }
        });

        // ==================== MANAGER - PERMISOS DE GESTIÓN (10) ====================
        rolePermissions.AddRange(new[]
        {
            // Users permissions (4) - puede ver y editar usuarios, pero no eliminar permanentemente
            new RolePermission { RoleId = RolesSeed.RoleIds.Manager, PermissionId = PermissionsSeed.PermissionIds.UsersRead, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Manager, PermissionId = PermissionsSeed.PermissionIds.UsersReadOwn, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Manager, PermissionId = PermissionsSeed.PermissionIds.UsersWrite, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Manager, PermissionId = PermissionsSeed.PermissionIds.UsersWriteOwn, GrantedAt = seedDate },

            // Roles permissions (2) - puede ver roles y asignar rol User
            new RolePermission { RoleId = RolesSeed.RoleIds.Manager, PermissionId = PermissionsSeed.PermissionIds.RolesRead, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Manager, PermissionId = PermissionsSeed.PermissionIds.RolesAssignUser, GrantedAt = seedDate },

            // Permissions management (1) - solo lectura
            new RolePermission { RoleId = RolesSeed.RoleIds.Manager, PermissionId = PermissionsSeed.PermissionIds.PermissionsRead, GrantedAt = seedDate },

            // Audit permissions (2) - puede ver auditoría limitada
            new RolePermission { RoleId = RolesSeed.RoleIds.Manager, PermissionId = PermissionsSeed.PermissionIds.AuditRead, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.Manager, PermissionId = PermissionsSeed.PermissionIds.AuditExport, GrantedAt = seedDate },

            // Notifications permissions (1) - puede enviar notificaciones
            new RolePermission { RoleId = RolesSeed.RoleIds.Manager, PermissionId = PermissionsSeed.PermissionIds.NotificationsSend, GrantedAt = seedDate }
        });

        // ==================== USER - PERMISOS BÁSICOS (2) ====================
        rolePermissions.AddRange(new[]
        {
            // Users permissions (2) - solo puede ver y editar su propio perfil
            new RolePermission { RoleId = RolesSeed.RoleIds.User, PermissionId = PermissionsSeed.PermissionIds.UsersReadOwn, GrantedAt = seedDate },
            new RolePermission { RoleId = RolesSeed.RoleIds.User, PermissionId = PermissionsSeed.PermissionIds.UsersWriteOwn, GrantedAt = seedDate }
        });

        // ==================== GUEST - PERMISOS MÍNIMOS (1) ====================
        rolePermissions.AddRange(new[]
        {
            // Users permissions (1) - solo puede ver su propio perfil
            new RolePermission { RoleId = RolesSeed.RoleIds.Guest, PermissionId = PermissionsSeed.PermissionIds.UsersReadOwn, GrantedAt = seedDate }
        });

        modelBuilder.Entity<RolePermission>().HasData(rolePermissions);
    }
}

