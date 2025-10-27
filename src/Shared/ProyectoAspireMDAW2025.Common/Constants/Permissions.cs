namespace ProyectoAspireMDAW2025.Common.Constants;

/// <summary>
/// Constantes para permisos granulares del sistema RBAC.
/// Los permisos se organizan por categorías (Users, Roles, Permissions, Audit, Notifications).
/// </summary>
public static class Permissions
{
    /// <summary>
    /// Permisos relacionados con la gestión de usuarios.
    /// </summary>
    public static class Users
    {
        /// <summary>
        /// Permiso para ver todos los usuarios del sistema.
        /// </summary>
        public const string Read = "users.read";

        /// <summary>
        /// Permiso para ver solo el propio perfil del usuario.
        /// </summary>
        public const string ReadOwn = "users.read.own";

        /// <summary>
        /// Permiso para crear y actualizar cualquier usuario.
        /// </summary>
        public const string Write = "users.write";

        /// <summary>
        /// Permiso para actualizar solo el propio perfil del usuario.
        /// </summary>
        public const string WriteOwn = "users.write.own";

        /// <summary>
        /// Permiso para realizar soft delete de usuarios.
        /// </summary>
        public const string Delete = "users.delete";

        /// <summary>
        /// Permiso para realizar hard delete (eliminación permanente) de usuarios.
        /// Solo Admin debe tener este permiso.
        /// </summary>
        public const string DeletePermanent = "users.delete.permanent";

        /// <summary>
        /// Permiso para restaurar usuarios eliminados (soft delete).
        /// </summary>
        public const string Restore = "users.restore";
    }

    /// <summary>
    /// Permisos relacionados con la gestión de roles.
    /// </summary>
    public static class Roles
    {
        /// <summary>
        /// Permiso para ver todos los roles del sistema.
        /// </summary>
        public const string Read = "roles.read";

        /// <summary>
        /// Permiso para crear, actualizar y eliminar roles.
        /// Solo Admin debe tener este permiso.
        /// </summary>
        public const string Manage = "roles.manage";

        /// <summary>
        /// Permiso para asignar cualquier rol a usuarios.
        /// Solo Admin debe tener este permiso.
        /// </summary>
        public const string Assign = "roles.assign";

        /// <summary>
        /// Permiso para asignar solo el rol "User" a usuarios.
        /// Manager puede tener este permiso.
        /// </summary>
        public const string AssignUser = "roles.assign.user";
    }

    /// <summary>
    /// Permisos relacionados con la gestión de permisos.
    /// </summary>
    public static class PermissionsManagement
    {
        /// <summary>
        /// Permiso para ver todos los permisos del sistema.
        /// </summary>
        public const string Read = "permissions.read";

        /// <summary>
        /// Permiso para gestionar permisos (asignar/remover de roles).
        /// Solo Admin debe tener este permiso.
        /// </summary>
        public const string Manage = "permissions.manage";
    }

    /// <summary>
    /// Permisos relacionados con auditoría.
    /// </summary>
    public static class Audit
    {
        /// <summary>
        /// Permiso para ver auditoría limitada (solo eventos propios o de su equipo).
        /// </summary>
        public const string Read = "audit.read";

        /// <summary>
        /// Permiso para ver toda la auditoría del sistema.
        /// Solo Admin debe tener este permiso.
        /// </summary>
        public const string ReadAll = "audit.read.all";

        /// <summary>
        /// Permiso para exportar auditoría.
        /// Solo Admin debe tener este permiso.
        /// </summary>
        public const string Export = "audit.export";
    }

    /// <summary>
    /// Permisos relacionados con notificaciones.
    /// </summary>
    public static class Notifications
    {
        /// <summary>
        /// Permiso para enviar notificaciones.
        /// </summary>
        public const string Send = "notifications.send";

        /// <summary>
        /// Permiso para gestionar configuración de notificaciones.
        /// Solo Admin debe tener este permiso.
        /// </summary>
        public const string Manage = "notifications.manage";
    }

    /// <summary>
    /// Obtiene todos los permisos del sistema agrupados por categoría.
    /// </summary>
    public static class All
    {
        public static readonly string[] UserPermissions =
        {
            Users.Read,
            Users.ReadOwn,
            Users.Write,
            Users.WriteOwn,
            Users.Delete,
            Users.DeletePermanent,
            Users.Restore
        };

        public static readonly string[] RolePermissions =
        {
            Roles.Read,
            Roles.Manage,
            Roles.Assign,
            Roles.AssignUser
        };

        public static readonly string[] PermissionManagementPermissions =
        {
            PermissionsManagement.Read,
            PermissionsManagement.Manage
        };

        public static readonly string[] AuditPermissions =
        {
            Audit.Read,
            Audit.ReadAll,
            Audit.Export
        };

        public static readonly string[] NotificationPermissions =
        {
            Notifications.Send,
            Notifications.Manage
        };

        /// <summary>
        /// Todos los permisos del sistema (18 permisos).
        /// </summary>
        public static readonly string[] System =
        {
            // Users (7)
            Users.Read,
            Users.ReadOwn,
            Users.Write,
            Users.WriteOwn,
            Users.Delete,
            Users.DeletePermanent,
            Users.Restore,
            
            // Roles (4)
            Roles.Read,
            Roles.Manage,
            Roles.Assign,
            Roles.AssignUser,
            
            // Permissions (2)
            PermissionsManagement.Read,
            PermissionsManagement.Manage,
            
            // Audit (3)
            Audit.Read,
            Audit.ReadAll,
            Audit.Export,
            
            // Notifications (2)
            Notifications.Send,
            Notifications.Manage
        };
    }
}

