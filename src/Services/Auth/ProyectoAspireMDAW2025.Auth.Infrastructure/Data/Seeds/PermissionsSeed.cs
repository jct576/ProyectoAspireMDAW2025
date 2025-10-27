using Microsoft.EntityFrameworkCore;
using ProyectoAspireMDAW2025.Auth.Domain.Entities;
using ProyectoAspireMDAW2025.Common.Constants;

namespace ProyectoAspireMDAW2025.Auth.Infrastructure.Data.Seeds;

/// <summary>
/// Seed data para Permissions
/// </summary>
public static class PermissionsSeed
{
    /// <summary>
    /// IDs fijos para los permisos (para referencias en RolePermissionsSeed)
    /// </summary>
    public static class PermissionIds
    {
        // Users permissions
        public static readonly Guid UsersRead = Guid.Parse("10000000-0000-0000-0000-000000000001");
        public static readonly Guid UsersReadOwn = Guid.Parse("10000000-0000-0000-0000-000000000002");
        public static readonly Guid UsersWrite = Guid.Parse("10000000-0000-0000-0000-000000000003");
        public static readonly Guid UsersWriteOwn = Guid.Parse("10000000-0000-0000-0000-000000000004");
        public static readonly Guid UsersDelete = Guid.Parse("10000000-0000-0000-0000-000000000005");
        public static readonly Guid UsersDeletePermanent = Guid.Parse("10000000-0000-0000-0000-000000000006");
        public static readonly Guid UsersRestore = Guid.Parse("10000000-0000-0000-0000-000000000007");

        // Roles permissions
        public static readonly Guid RolesRead = Guid.Parse("20000000-0000-0000-0000-000000000001");
        public static readonly Guid RolesManage = Guid.Parse("20000000-0000-0000-0000-000000000002");
        public static readonly Guid RolesAssign = Guid.Parse("20000000-0000-0000-0000-000000000003");
        public static readonly Guid RolesAssignUser = Guid.Parse("20000000-0000-0000-0000-000000000004");

        // Permissions management
        public static readonly Guid PermissionsRead = Guid.Parse("30000000-0000-0000-0000-000000000001");
        public static readonly Guid PermissionsManage = Guid.Parse("30000000-0000-0000-0000-000000000002");

        // Audit permissions
        public static readonly Guid AuditRead = Guid.Parse("40000000-0000-0000-0000-000000000001");
        public static readonly Guid AuditReadAll = Guid.Parse("40000000-0000-0000-0000-000000000002");
        public static readonly Guid AuditExport = Guid.Parse("40000000-0000-0000-0000-000000000003");

        // Notifications permissions
        public static readonly Guid NotificationsSend = Guid.Parse("50000000-0000-0000-0000-000000000001");
        public static readonly Guid NotificationsManage = Guid.Parse("50000000-0000-0000-0000-000000000002");
    }

    public static void Seed(ModelBuilder modelBuilder)
    {
        // Usar fecha fija para evitar cambios en el modelo
        var seedDate = new DateTime(2025, 10, 27, 0, 0, 0, DateTimeKind.Utc);

        var permissions = new List<Permission>
        {
            // ==================== USERS PERMISSIONS ====================
            new Permission
            {
                Id = PermissionIds.UsersRead,
                Name = Permissions.Users.Read,
                Description = "Ver todos los usuarios del sistema",
                Category = "Users",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.UsersReadOwn,
                Name = Permissions.Users.ReadOwn,
                Description = "Ver solo el propio perfil del usuario",
                Category = "Users",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.UsersWrite,
                Name = Permissions.Users.Write,
                Description = "Crear y actualizar cualquier usuario",
                Category = "Users",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.UsersWriteOwn,
                Name = Permissions.Users.WriteOwn,
                Description = "Actualizar solo el propio perfil del usuario",
                Category = "Users",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.UsersDelete,
                Name = Permissions.Users.Delete,
                Description = "Realizar soft delete de usuarios",
                Category = "Users",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.UsersDeletePermanent,
                Name = Permissions.Users.DeletePermanent,
                Description = "Realizar hard delete (eliminación permanente) de usuarios",
                Category = "Users",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.UsersRestore,
                Name = Permissions.Users.Restore,
                Description = "Restaurar usuarios eliminados (soft delete)",
                Category = "Users",
                CreatedAt = seedDate
            },

            // ==================== ROLES PERMISSIONS ====================
            new Permission
            {
                Id = PermissionIds.RolesRead,
                Name = Permissions.Roles.Read,
                Description = "Ver todos los roles del sistema",
                Category = "Roles",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.RolesManage,
                Name = Permissions.Roles.Manage,
                Description = "Crear, actualizar y eliminar roles",
                Category = "Roles",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.RolesAssign,
                Name = Permissions.Roles.Assign,
                Description = "Asignar cualquier rol a usuarios",
                Category = "Roles",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.RolesAssignUser,
                Name = Permissions.Roles.AssignUser,
                Description = "Asignar solo el rol 'User' a usuarios",
                Category = "Roles",
                CreatedAt = seedDate
            },

            // ==================== PERMISSIONS MANAGEMENT ====================
            new Permission
            {
                Id = PermissionIds.PermissionsRead,
                Name = Permissions.PermissionsManagement.Read,
                Description = "Ver todos los permisos del sistema",
                Category = "Permissions",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.PermissionsManage,
                Name = Permissions.PermissionsManagement.Manage,
                Description = "Gestionar permisos (asignar/remover de roles)",
                Category = "Permissions",
                CreatedAt = seedDate
            },

            // ==================== AUDIT PERMISSIONS ====================
            new Permission
            {
                Id = PermissionIds.AuditRead,
                Name = Permissions.Audit.Read,
                Description = "Ver auditoría limitada (solo eventos propios o de su equipo)",
                Category = "Audit",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.AuditReadAll,
                Name = Permissions.Audit.ReadAll,
                Description = "Ver toda la auditoría del sistema",
                Category = "Audit",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.AuditExport,
                Name = Permissions.Audit.Export,
                Description = "Exportar auditoría",
                Category = "Audit",
                CreatedAt = seedDate
            },

            // ==================== NOTIFICATIONS PERMISSIONS ====================
            new Permission
            {
                Id = PermissionIds.NotificationsSend,
                Name = Permissions.Notifications.Send,
                Description = "Enviar notificaciones",
                Category = "Notifications",
                CreatedAt = seedDate
            },
            new Permission
            {
                Id = PermissionIds.NotificationsManage,
                Name = Permissions.Notifications.Manage,
                Description = "Gestionar configuración de notificaciones",
                Category = "Notifications",
                CreatedAt = seedDate
            }
        };

        modelBuilder.Entity<Permission>().HasData(permissions);
    }
}

