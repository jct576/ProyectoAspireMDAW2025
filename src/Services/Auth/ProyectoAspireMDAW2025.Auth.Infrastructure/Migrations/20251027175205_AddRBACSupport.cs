using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProyectoAspireMDAW2025.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRBACSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "Users", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Ver todos los usuarios del sistema", "users.read" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "Users", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Ver solo el propio perfil del usuario", "users.read.own" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "Users", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Crear y actualizar cualquier usuario", "users.write" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "Users", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Actualizar solo el propio perfil del usuario", "users.write.own" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Users", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Realizar soft delete de usuarios", "users.delete" },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "Users", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Realizar hard delete (eliminación permanente) de usuarios", "users.delete.permanent" },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "Users", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Restaurar usuarios eliminados (soft delete)", "users.restore" },
                    { new Guid("20000000-0000-0000-0000-000000000001"), "Roles", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Ver todos los roles del sistema", "roles.read" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "Roles", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Crear, actualizar y eliminar roles", "roles.manage" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "Roles", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Asignar cualquier rol a usuarios", "roles.assign" },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "Roles", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Asignar solo el rol 'User' a usuarios", "roles.assign.user" },
                    { new Guid("30000000-0000-0000-0000-000000000001"), "Permissions", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Ver todos los permisos del sistema", "permissions.read" },
                    { new Guid("30000000-0000-0000-0000-000000000002"), "Permissions", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Gestionar permisos (asignar/remover de roles)", "permissions.manage" },
                    { new Guid("40000000-0000-0000-0000-000000000001"), "Audit", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Ver auditoría limitada (solo eventos propios o de su equipo)", "audit.read" },
                    { new Guid("40000000-0000-0000-0000-000000000002"), "Audit", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Ver toda la auditoría del sistema", "audit.read.all" },
                    { new Guid("40000000-0000-0000-0000-000000000003"), "Audit", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Exportar auditoría", "audit.export" },
                    { new Guid("50000000-0000-0000-0000-000000000001"), "Notifications", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Enviar notificaciones", "notifications.send" },
                    { new Guid("50000000-0000-0000-0000-000000000002"), "Notifications", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Gestionar configuración de notificaciones", "notifications.manage" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "Description", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "admin-seed-stamp-001", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Administrador del sistema con acceso completo a todas las funcionalidades", "Admin", "ADMIN", null },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "manager-seed-stamp-002", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Gestor con permisos para administrar usuarios y contenido", "Manager", "MANAGER", null },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "user-seed-stamp-003", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Usuario estándar con permisos básicos", "User", "USER", null },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "guest-seed-stamp-004", new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Invitado con permisos de solo lectura limitados", "Guest", "GUEST", null }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "GrantedAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("40000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("40000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("40000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("40000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("40000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));
        }
    }
}
