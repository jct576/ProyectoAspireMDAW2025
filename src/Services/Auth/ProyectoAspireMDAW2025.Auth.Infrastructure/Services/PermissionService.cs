using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Auth.Domain.Entities;
using ProyectoAspireMDAW2025.Auth.Infrastructure.Data;
using ProyectoAspireMDAW2025.Common.Constants;

namespace ProyectoAspireMDAW2025.Auth.Infrastructure.Services;

/// <summary>
/// Servicio para gestión de permisos
/// </summary>
public class PermissionService : IPermissionService
{
    private readonly AuthDbContext _context;
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<PermissionService> _logger;

    public PermissionService(
        AuthDbContext context,
        IAuthRepository authRepository,
        ILogger<PermissionService> logger)
    {
        _context = context;
        _authRepository = authRepository;
        _logger = logger;
    }

    public async Task<IList<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var permissions = await _authRepository.GetUserPermissionsAsync(userId, cancellationToken);
            _logger.LogInformation("Retrieved {Count} permissions for user {UserId}", permissions.Count, userId);
            return permissions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permissions for user {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> UserHasPermissionAsync(Guid userId, string permission, CancellationToken cancellationToken = default)
    {
        try
        {
            var userPermissions = await GetUserPermissionsAsync(userId, cancellationToken);
            var hasPermission = userPermissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
            
            _logger.LogInformation("User {UserId} has permission {Permission}: {HasPermission}", 
                userId, permission, hasPermission);
            
            return hasPermission;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking permission {Permission} for user {UserId}", permission, userId);
            throw;
        }
    }

    public async Task<bool> UserHasAnyPermissionAsync(Guid userId, params string[] permissions)
    {
        try
        {
            if (permissions == null || permissions.Length == 0)
            {
                return false;
            }

            var userPermissions = await GetUserPermissionsAsync(userId);
            var hasAny = permissions.Any(p => userPermissions.Contains(p, StringComparer.OrdinalIgnoreCase));
            
            _logger.LogInformation("User {UserId} has any of {PermissionCount} permissions: {HasAny}", 
                userId, permissions.Length, hasAny);
            
            return hasAny;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking any permissions for user {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> UserHasAllPermissionsAsync(Guid userId, params string[] permissions)
    {
        try
        {
            if (permissions == null || permissions.Length == 0)
            {
                return true;
            }

            var userPermissions = await GetUserPermissionsAsync(userId);
            var hasAll = permissions.All(p => userPermissions.Contains(p, StringComparer.OrdinalIgnoreCase));
            
            _logger.LogInformation("User {UserId} has all of {PermissionCount} permissions: {HasAll}", 
                userId, permissions.Length, hasAll);
            
            return hasAll;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking all permissions for user {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, string permissionName, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verificar que el rol existe
            var role = await _context.Roles.FindAsync(new object[] { roleId }, cancellationToken);
            if (role == null)
            {
                _logger.LogWarning("Role not found: {RoleId}", roleId);
                throw new InvalidOperationException($"Rol con ID {roleId} no encontrado");
            }

            // Verificar que el permiso existe
            var permission = await _authRepository.GetPermissionByNameAsync(permissionName, cancellationToken);
            if (permission == null)
            {
                _logger.LogWarning("Permission not found: {PermissionName}", permissionName);
                throw new InvalidOperationException($"Permiso '{permissionName}' no encontrado");
            }

            // Verificar si ya existe la asignación
            var existingAssignment = await _context.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id, cancellationToken);

            if (existingAssignment)
            {
                _logger.LogWarning("Permission {PermissionName} already assigned to role {RoleId}", permissionName, roleId);
                return false;
            }

            // Crear la asignación
            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permission.Id,
                GrantedAt = DateTime.UtcNow
            };

            await _context.RolePermissions.AddAsync(rolePermission, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Permission {PermissionName} assigned to role {RoleId}", permissionName, roleId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning permission {PermissionName} to role {RoleId}", permissionName, roleId);
            throw;
        }
    }

    public async Task<bool> RemovePermissionFromRoleAsync(Guid roleId, string permissionName, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verificar que el permiso existe
            var permission = await _authRepository.GetPermissionByNameAsync(permissionName, cancellationToken);
            if (permission == null)
            {
                _logger.LogWarning("Permission not found: {PermissionName}", permissionName);
                throw new InvalidOperationException($"Permiso '{permissionName}' no encontrado");
            }

            // Buscar la asignación
            var rolePermission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id, cancellationToken);

            if (rolePermission == null)
            {
                _logger.LogWarning("Permission {PermissionName} not assigned to role {RoleId}", permissionName, roleId);
                return false;
            }

            // Remover la asignación
            _context.RolePermissions.Remove(rolePermission);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Permission {PermissionName} removed from role {RoleId}", permissionName, roleId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing permission {PermissionName} from role {RoleId}", permissionName, roleId);
            throw;
        }
    }

    public async Task SyncPermissionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting permissions synchronization");

            var allPermissions = Permissions.All.System;
            var createdCount = 0;

            foreach (var permissionName in allPermissions)
            {
                // Verificar si el permiso ya existe
                var existingPermission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.Name == permissionName, cancellationToken);

                if (existingPermission == null)
                {
                    // Determinar categoría del permiso
                    var category = GetPermissionCategory(permissionName);
                    var description = GetPermissionDescription(permissionName);

                    var permission = new Permission
                    {
                        Id = Guid.NewGuid(),
                        Name = permissionName,
                        Description = description,
                        Category = category,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.Permissions.AddAsync(permission, cancellationToken);
                    createdCount++;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Permissions synchronization completed. Created: {CreatedCount}", createdCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error synchronizing permissions");
            throw;
        }
    }

    public async Task<IList<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
    {
        return await _authRepository.GetAllPermissionsAsync(cancellationToken);
    }

    public async Task<IList<Permission>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await _authRepository.GetRolePermissionsAsync(roleId, cancellationToken);
    }

    // ==================== HELPER METHODS ====================

    private string GetPermissionCategory(string permissionName)
    {
        if (permissionName.StartsWith("users.")) return "Users";
        if (permissionName.StartsWith("roles.")) return "Roles";
        if (permissionName.StartsWith("permissions.")) return "Permissions";
        if (permissionName.StartsWith("audit.")) return "Audit";
        if (permissionName.StartsWith("notifications.")) return "Notifications";
        return "General";
    }

    private string GetPermissionDescription(string permissionName)
    {
        return permissionName switch
        {
            "users.read" => "Ver todos los usuarios del sistema",
            "users.read.own" => "Ver solo el propio perfil del usuario",
            "users.write" => "Crear y actualizar cualquier usuario",
            "users.write.own" => "Actualizar solo el propio perfil del usuario",
            "users.delete" => "Realizar soft delete de usuarios",
            "users.delete.permanent" => "Realizar hard delete (eliminación permanente) de usuarios",
            "users.restore" => "Restaurar usuarios eliminados (soft delete)",
            
            "roles.read" => "Ver todos los roles del sistema",
            "roles.manage" => "Crear, actualizar y eliminar roles",
            "roles.assign" => "Asignar cualquier rol a usuarios",
            "roles.assign.user" => "Asignar solo el rol 'User' a usuarios",
            
            "permissions.read" => "Ver todos los permisos del sistema",
            "permissions.manage" => "Gestionar permisos (asignar/remover de roles)",
            
            "audit.read" => "Ver auditoría limitada (solo eventos propios o de su equipo)",
            "audit.read.all" => "Ver toda la auditoría del sistema",
            "audit.export" => "Exportar auditoría",
            
            "notifications.send" => "Enviar notificaciones",
            "notifications.manage" => "Gestionar configuración de notificaciones",
            
            _ => $"Permiso: {permissionName}"
        };
    }
}

