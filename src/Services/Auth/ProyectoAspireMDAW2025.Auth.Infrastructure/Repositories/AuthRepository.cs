using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Auth.Domain.Entities;
using ProyectoAspireMDAW2025.Auth.Infrastructure.Data;

namespace ProyectoAspireMDAW2025.Auth.Infrastructure.Repositories;

/// <summary>
/// Repositorio para operaciones de autenticación y gestión de usuarios
/// </summary>
public class AuthRepository : IAuthRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly AuthDbContext _context;
    private readonly ILogger<AuthRepository> _logger;

    public AuthRepository(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        AuthDbContext context,
        ILogger<AuthRepository> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _userManager.FindByEmailAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by email: {Email}", email);
            throw;
        }
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create user {Email}. Errors: {Errors}", user.Email, errors);
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }

            _logger.LogInformation("User created successfully: {UserId}", user.Id);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Email}", user.Email);
            throw;
        }
    }

    public async Task<bool> ValidateCredentialsAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var isValid = await _userManager.CheckPasswordAsync(user, password);

            if (isValid)
            {
                _logger.LogInformation("Credentials validated for user: {UserId}", user.Id);
            }
            else
            {
                _logger.LogWarning("Invalid credentials for user: {UserId}", user.Id);
            }

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating credentials for user: {UserId}", user.Id);
            throw;
        }
    }

    public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation("Retrieved {Count} roles for user: {UserId}", roles.Count, user.Id);

            return roles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting roles for user: {UserId}", user.Id);
            throw;
        }
    }

    public async Task SaveRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Refresh token saved: {TokenId} for user: {UserId}", refreshToken.Id, refreshToken.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving refresh token for user: {UserId}", refreshToken.UserId);
            throw;
        }
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var refreshToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);

            if (refreshToken != null)
            {
                _logger.LogInformation("Refresh token found: {TokenId}", refreshToken.Id);
            }
            else
            {
                _logger.LogWarning("Refresh token not found: {Token}", token);
            }

            return refreshToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting refresh token");
            throw;
        }
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Refresh token updated: {TokenId}", refreshToken.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating refresh token: {TokenId}", refreshToken.Id);
            throw;
        }
    }

    public async Task<IList<RefreshToken>> GetUserActiveRefreshTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} active refresh tokens for user: {UserId}", activeTokens.Count, userId);

            return activeTokens;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active refresh tokens for user: {UserId}", userId);
            throw;
        }
    }

    // ==================== ROLE MANAGEMENT ====================

    public async Task<bool> AssignRoleToUserAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                _logger.LogInformation("Role {RoleName} assigned to user: {UserId}", roleName, user.Id);
                return true;
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to assign role {RoleName} to user {UserId}. Errors: {Errors}", roleName, user.Id, errors);
            throw new InvalidOperationException($"Failed to assign role: {errors}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning role {RoleName} to user: {UserId}", roleName, user.Id);
            throw;
        }
    }

    public async Task<bool> RemoveRoleFromUserAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                _logger.LogInformation("Role {RoleName} removed from user: {UserId}", roleName, user.Id);
                return true;
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to remove role {RoleName} from user {UserId}. Errors: {Errors}", roleName, user.Id, errors);
            throw new InvalidOperationException($"Failed to remove role: {errors}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing role {RoleName} from user: {UserId}", roleName, user.Id);
            throw;
        }
    }

    public async Task<bool> RoleExistsAsync(string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await _roleManager.RoleExistsAsync(roleName);
            _logger.LogInformation("Role {RoleName} exists: {Exists}", roleName, exists);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if role exists: {RoleName}", roleName);
            throw;
        }
    }

    public async Task<ApplicationRole> CreateRoleAsync(string roleName, string? description = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var role = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = roleName,
                NormalizedName = roleName.ToUpperInvariant(),
                Description = description,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create role {RoleName}. Errors: {Errors}", roleName, errors);
                throw new InvalidOperationException($"Failed to create role: {errors}");
            }

            _logger.LogInformation("Role created successfully: {RoleId} - {RoleName}", role.Id, roleName);
            return role;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role: {RoleName}", roleName);
            throw;
        }
    }

    public async Task<ApplicationRole?> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role != null)
            {
                _logger.LogInformation("Role found: {RoleId} - {RoleName}", role.Id, roleName);
            }
            else
            {
                _logger.LogWarning("Role not found: {RoleName}", roleName);
            }

            return role;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting role by name: {RoleName}", roleName);
            throw;
        }
    }

    public async Task<IList<ApplicationRole>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await _roleManager.Roles.ToListAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} roles", roles.Count);
            return roles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all roles");
            throw;
        }
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            _logger.LogInformation("User {UserId} is in role {RoleName}: {IsInRole}", user.Id, roleName, isInRole);
            return isInRole;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if user {UserId} is in role {RoleName}", user.Id, roleName);
            throw;
        }
    }

    // ==================== PERMISSION MANAGEMENT ====================

    public async Task<IList<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var permissions = await _context.Permissions
                .OrderBy(p => p.Category)
                .ThenBy(p => p.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} permissions", permissions.Count);
            return permissions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all permissions");
            throw;
        }
    }

    public async Task<IList<Permission>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        try
        {
            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission)
                .OrderBy(p => p.Category)
                .ThenBy(p => p.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} permissions for role: {RoleId}", permissions.Count, roleId);
            return permissions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permissions for role: {RoleId}", roleId);
            throw;
        }
    }

    public async Task<IList<Permission>> GetRolePermissionsByNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                _logger.LogWarning("Role not found: {RoleName}", roleName);
                return new List<Permission>();
            }

            return await GetRolePermissionsAsync(role.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permissions for role: {RoleName}", roleName);
            throw;
        }
    }

    public async Task<IList<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return new List<string>();
            }

            // Obtener roles del usuario
            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Any())
            {
                _logger.LogInformation("User {UserId} has no roles", userId);
                return new List<string>();
            }

            // Obtener IDs de los roles
            var roleIds = await _context.Roles
                .Where(r => userRoles.Contains(r.Name!))
                .Select(r => r.Id)
                .ToListAsync(cancellationToken);

            // Obtener permisos únicos de todos los roles del usuario
            var permissions = await _context.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} unique permissions for user: {UserId}", permissions.Count, userId);
            return permissions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permissions for user: {UserId}", userId);
            throw;
        }
    }

    public async Task<Permission?> GetPermissionByNameAsync(string permissionName, CancellationToken cancellationToken = default)
    {
        try
        {
            var permission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Name == permissionName, cancellationToken);

            if (permission != null)
            {
                _logger.LogInformation("Permission found: {PermissionId} - {PermissionName}", permission.Id, permissionName);
            }
            else
            {
                _logger.LogWarning("Permission not found: {PermissionName}", permissionName);
            }

            return permission;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permission by name: {PermissionName}", permissionName);
            throw;
        }
    }
}

